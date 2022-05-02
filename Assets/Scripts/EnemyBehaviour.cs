using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] Transform[] _destinationPoints;
    [SerializeField] private DetectionLogic _detectionLogic;
    [SerializeField] private float _faceRotationSpeed;
    private EnemyStates _enemyStates = EnemyStates.Patrolling;

    [SerializeField] private float _shootingFrequency;
    private int _currentPositionNumber;


    [SerializeField] private GameObject _projectilePrefab;
    
    

    private bool ReachedTheNextPoint =>
        Math.Abs(transform.position.x - _destinationPoints[_currentPositionNumber].position.x) < .1;
    
    private bool ReachedLastPoint =>
        Math.Abs(transform.position.x - _destinationPoints[^1].position.x) < .1;
    
    
    

    private void OnEnable()
    {
        _detectionLogic.OnDetected += AggressiveBehaviourStarted;
        _detectionLogic.OnDetectionLost += AggressiveBehaviourEnded;
    }




    private void AggressiveBehaviourStarted(Collider collider)
    {
        _enemyStates = EnemyStates.Aggressive;
        _navMeshAgent.destination = transform.position;

        StartCoroutine(RotationLogic(collider));
        StartCoroutine(ShootingLogic());
    }

    private IEnumerator ShootingLogic()
    {
        while (_enemyStates == EnemyStates.Aggressive)
        {
            yield return new WaitForSeconds(_shootingFrequency);
            _projectilePrefab.transform.position = transform.position;
            _projectilePrefab.SetActive(true);
            StartCoroutine(ProjectileFly());
            print("shoot performed");
            Shoot();
        }
    }


    IEnumerator ProjectileFly()
    {
        while (true)
        {
            _projectilePrefab.transform.Translate(Vector3.forward);
            yield return null;
        }
    }
    private void Shoot()
    {
        RaycastHit raycastHit;
       if(Physics.Raycast(transform.position, transform.forward, out raycastHit))  
        {
            print(raycastHit.transform.name);
        }
    }

    private void AggressiveBehaviourEnded()
    {
        _enemyStates = EnemyStates.Patrolling;
        StartMovingTowardsPosition(_currentPositionNumber);
    }



    IEnumerator RotationLogic(Collider collider)
    {
        while (_enemyStates == EnemyStates.Aggressive)
        {
            FaceTarget(collider.transform.position);
            yield return null;
        }
    }
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }


    
    private void Start()
    {
        StartMovingTowardsPosition(_currentPositionNumber);
    }
    

    private void Update()
    {
        
        if (_enemyStates == EnemyStates.Patrolling
            &&
            ReachedTheNextPoint) ChangeThePointToMoveTowards();
    }

    private void ChangeThePointToMoveTowards()
    {
        if (ReachedLastPoint) _currentPositionNumber = 0;
            else _currentPositionNumber += 1;



        StartMovingTowardsPosition(_currentPositionNumber);
    }
    
    private void StartMovingTowardsPosition(int indexOfTransformToMoveTo)
    {
        _navMeshAgent.destination = _destinationPoints[indexOfTransformToMoveTo].position;
    }
    
    
    
    private void OnDisable()
    {
        _detectionLogic.OnDetected -= AggressiveBehaviourStarted;
        _detectionLogic.OnDetectionLost -= AggressiveBehaviourEnded;
    }
    
    
    private Quaternion FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _faceRotationSpeed);
        return rotation;
    }
    
}


enum EnemyStates
{
    Patrolling,
    Aggressive
}
