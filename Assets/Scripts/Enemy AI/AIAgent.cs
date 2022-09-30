using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AlertState
{
    Unalerted,
    Suspecious,
    Alerted
}

[Serializable]
public class GuardShedue
{
    public List<Transform> Targets = new List<Transform>();
    public List<float> WaitTime = new List<float>();
    public int CurrentStop = 0;

    public void GoToNextStop()
    {
        CurrentStop = CurrentStop + 1 < Targets.Count ? CurrentStop + 1 : 0;
    }
}


public class AIAgent : MonoBehaviour
{
    
    public AlertState CurrentAlertLevel { get; private set; }

    [SerializeField] private NavMeshAgent _navAgent;
    [SerializeField] private GuardShedue _guardShedue;
    private Transform _lastKnowPlayerLocation;
    private AIState _currentAIState;
    private AIStateFactory _AIStateFactory;

    public AIState CurrentAIState { get { return _currentAIState; } set { _currentAIState = value; } }
    public NavMeshAgent NavMeshAgent { get { return _navAgent; } }

    public GuardShedue GuardShedue { get { return _guardShedue; } }



    public void ChangeAlertState(AlertState newState)
    {
        CurrentAlertLevel = newState;
    }

    private void Awake()
    {
        _AIStateFactory = new AIStateFactory(this);
        _currentAIState = _AIStateFactory.MoveToPost();
        _currentAIState.EnterState();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _currentAIState.UpdateState();
    }
}
