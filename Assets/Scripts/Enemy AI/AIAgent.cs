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


public class AIAgent : MonoBehaviour
{
    
    public AlertState CurrentAlertLevel { get; private set; }

    [SerializeField] private NavMeshAgent _navAgent;
    [SerializeField] private Transform[] _guardPosts;
    private Transform _lastKnowPlayerLocation;
    private AIState _currentAIState;
    private AIStateFactory _AIStateFactory;

    public AIState CurrentAIState { get { return _currentAIState; } set { _currentAIState = value; } }



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
        
    }
}
