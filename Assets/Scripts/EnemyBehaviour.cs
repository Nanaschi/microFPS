using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] Transform[] _destinationPoints;

    private int _currentPositionNumber;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        PatrolLogic(_destinationPoints);
    }

    private void PatrolLogic(Transform[] destinationPoints)
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
