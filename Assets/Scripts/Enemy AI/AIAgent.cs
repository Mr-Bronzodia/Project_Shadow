using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public enum AlertState
{
    Unalerted,
    Suspecious,
    Alerted
}

[Serializable]
public class GuardShedue
{
    public Transform GuardPath = null;
    public float MinWaitTime;
    public float MaxWaitTime;
    public int CurrentStop = 0;
    public List<float> WaitTime { get; private set; } = new List<float>();
    public List<Transform> Targets { get; private set; } = new List<Transform>();


    public void InitStops()
    {
        if (GuardPath != null)
        {
            foreach (Transform childTransform in GuardPath.GetComponentsInChildren<Transform>())
            {
                Targets.Add(childTransform);
                WaitTime.Add(UnityEngine.Random.Range(MinWaitTime, MaxWaitTime));
            }
        }
    }

    public void GoToNextStop()
    {
        CurrentStop = CurrentStop + 1 < Targets.Count ? CurrentStop + 1 : 0;
    }
}

[RequireComponent(typeof(NavMeshAgent))]
public class AIAgent : MonoBehaviour
{

    [SerializeField] private NavMeshAgent _navAgent;
    [SerializeField] private GuardShedue _guardShedue;
    private Transform _lastKnowPlayerLocation;
    private AIState _currentAIState;
    private AIStateFactory _AIStateFactory;
    [SerializeField] private AlertState _currentAlertLevel;
    private Transform _suspeciousLocation = null;
    private Transform _lastSeenTarget = null;
    public Action StartSenses;

    public AIState CurrentAIState { get { return _currentAIState; } set { _currentAIState = value; } }
    public NavMeshAgent NavMeshAgent { get { return _navAgent; } }
    public AlertState AlertState { get { return _currentAlertLevel; } }
    public GuardShedue GuardShedue { get { return _guardShedue; } }
    public Transform SuspeciousLocation { get { return _suspeciousLocation; } }
    public Transform LastSeenTargetLocation { get { return _lastSeenTarget; } }

    public bool IsAwareOfTarget = false;



    public void ChangeAlertState(AlertState newState)
    {
        _currentAlertLevel = newState;

        switch (newState)
        {
            case AlertState.Unalerted:
                _navAgent.speed = 1.5f;
                break;
            case AlertState.Suspecious:
                _navAgent.speed = 3.5f;
                break;
            case AlertState.Alerted:
                _navAgent.speed = 5.0f;
                break;
        }
    }

    public AIState GetCurrentState()
    {
        return _currentAIState;
    }

    public void AddSuspeciousLocaton(Transform loaction)
    {
        _suspeciousLocation = loaction;
    }

    public void UpdateLastSeen(Transform target) 
    {
        _lastSeenTarget = target;
    }

    private void Awake()
    {
        GuardShedue.InitStops();
        _AIStateFactory = new AIStateFactory(this);
        _currentAIState = _AIStateFactory.MoveToPost();
        _currentAIState.EnterState();
    }

    private void Start()
    {
        StartSenses?.Invoke();
    }


    void Update()
    {
        _currentAIState.UpdateState();
    }
}
