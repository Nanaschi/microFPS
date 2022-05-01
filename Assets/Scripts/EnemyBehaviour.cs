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

    private EnemyStates _enemyStates = EnemyStates.Patrolling;

    
    private int _currentPositionNumber;

    private bool ReachedTheNextPoint =>
        Math.Abs(transform.position.x - _destinationPoints[_currentPositionNumber].position.x) < .1;
    
    private bool ReachedLastPoint =>
        Math.Abs(transform.position.x - _destinationPoints[^1].position.x) < .1;
    
    
    

    private void OnEnable()
    {
        _detectionLogic.OnDetected += AggressiveBehaviourStarted;
        _detectionLogic.OnDetectionLost += AggressiveBehaviourEnded;
    }

    private void OnDisable()
    {
        _detectionLogic.OnDetected -= AggressiveBehaviourStarted;
        _detectionLogic.OnDetectionLost -= AggressiveBehaviourEnded;
    }


    private void AggressiveBehaviourStarted()
    {
        _enemyStates = EnemyStates.Aggressive;
        _navMeshAgent.destination = transform.position;
        print(EnemyStates.Aggressive);
    }    
    
    private void AggressiveBehaviourEnded()
    {
        _enemyStates = EnemyStates.Patrolling;
        print(EnemyStates.Patrolling);
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
            ReachedTheNextPoint)
        {
            print("unnecessary update");
            ChangeThePointToMoveTowards();
        }
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
    
    
}


enum EnemyStates
{
    Patrolling,
    Aggressive
}
