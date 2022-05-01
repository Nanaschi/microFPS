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
    
    
    

    private void Update()
    {
        if(_enemyStates == EnemyStates.Patrolling) Patrolling(_destinationPoints);
    }

    private void Patrolling(Transform[] destinationPoints)
    {

        _navMeshAgent.destination = destinationPoints[_currentPositionNumber].position;

        var reachedTheNextPoint = 
            Math.Abs(transform.position.x - destinationPoints[_currentPositionNumber].position.x) < .1;
        if (reachedTheNextPoint)
        {
            _currentPositionNumber += 1;

            var reachedLastPoint = _currentPositionNumber == destinationPoints.Length;
            if (reachedLastPoint) _currentPositionNumber = 0;
        }
    }
    
}


enum EnemyStates
{
    Patrolling,
    Aggressive
}
