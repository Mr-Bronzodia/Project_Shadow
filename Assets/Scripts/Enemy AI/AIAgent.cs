using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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

public class AwareTarget
{
    public AwareTarget(GameObject target)
    {
        Target = target;
    }

    public float LevelFromVision;
    public float LevelFromHearing;

    public GameObject Target;

    public bool vision = false;
    public bool hearing = false;

    public bool IsAware()
    {
        if (vision || hearing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool Contains(GameObject target, List<AwareTarget> list)
    {
        foreach(AwareTarget i in list)
        {
            if (i.Target == target) return true;
        }

        return false;
    }

    public static AwareTarget GetByGameObject(GameObject target, List<AwareTarget> list)
    {
        foreach (AwareTarget i in list)
        {
            if (i.Target == target) return i;
        }

        return null;
    }
}

[RequireComponent(typeof(NavMeshAgent), typeof(ResourceManager), typeof(Attack))]
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
    private List<AwareTarget> _awareForTargets = new List<AwareTarget>();
    private List<bool> _shouldBeSensed = new List<bool>();
    private bool _isDead = false;  
    private ResourceManager _resourceManager;
    private Attack _attackManager;
    private AwareTarget _currentTarget;


    public Action StartSenses;

    public Action OnCombatContinue;

    public AIState CurrentAIState { get { return _currentAIState; } set { _currentAIState = value; } }
    public NavMeshAgent NavMeshAgent { get { return _navAgent; } }
    public AlertState AlertState { get { return _currentAlertLevel; } }
    public GuardShedue GuardShedue { get { return _guardShedue; } }
    //public Transform SuspeciousLocation { get { return _suspeciousLocation; } }
    //public Transform LastSeenTargetLocation { get { return _lastSeenTarget; } }

    public AwareTarget CurrentTarget { get { return _currentTarget; } }

    public List<AwareTarget> AwareForTargets { get { return _awareForTargets; } }

    public bool IsDead { get { return _isDead; } }

    public TMP_Text DebugView;




    public void ChangeAlertState(AlertState newState)
    {
        _currentAlertLevel = newState;

        switch (newState)
        {
            case AlertState.Unalerted:
                _navAgent.speed = 1.7f;
                break;
            case AlertState.Suspecious:
                _navAgent.speed = 1.7f;
                break;
            case AlertState.Alerted:
                _navAgent.speed = 5f;
                break;
        }
    }

    public void OnDeath()
    {
        _isDead = true;
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


    public void UpdateAwarness(GameObject target, float amount, AISense sense, bool sensed)
    {

        if (!AwareTarget.Contains(target, _awareForTargets))
        {
            AwareTarget newTarget = new AwareTarget(target);

            switch (sense)
            {
                case AIVisionSense:
                    newTarget.vision = sensed;
                    newTarget.LevelFromVision = amount;
                    break;
                case AIHearingSense:
                    newTarget.hearing = sensed;
                    newTarget.LevelFromHearing = amount;
                    break;
                default:
                    break;
            }

            _awareForTargets.Add(newTarget);
            return;
        }

        AwareTarget currentTarget = AwareTarget.GetByGameObject(target, _awareForTargets);


        if (sense is AIHearingSense)
        {
            currentTarget.hearing = sensed;
            currentTarget.LevelFromHearing += amount;
        }

        if (sense is AIVisionSense)
        {
            currentTarget.vision = sensed;
            currentTarget.LevelFromVision += amount;
        }


        if (currentTarget.LevelFromVision + currentTarget.LevelFromHearing <= 0)
        {
            _awareForTargets.Remove(currentTarget);
            return;
        }


        //if (currentTarget.LevelFromVision > 1.5 || currentTarget.LevelFromHearing > 1.5)
        //{
        //    AddSuspeciousLocaton(target.transform);
        //}

        //if (currentTarget.LevelFromVision > 2.5 & currentTarget.vision)
        //{
        //    UpdateLastSeen(target.transform);
        //}

        //if (currentTarget.LevelFromHearing > 2.5 & currentTarget.vision)
        //{
        //    UpdateLastSeen(target.transform);
        //}

        //if (currentTarget.LevelFromHearing > 3.5 & currentTarget.hearing)
        //{
        //    UpdateLastSeen(target.transform);
        //}

    }


    public bool IsAware(GameObject target)
    {
        return AwareTarget.GetByGameObject(target, _awareForTargets).IsAware();
    }

    private void Awake()
    {
        GuardShedue.InitStops();
        _AIStateFactory = new AIStateFactory(this);
        _resourceManager = GetComponent<ResourceManager>();
        _attackManager = GetComponent<Attack>();
        _currentAIState = _AIStateFactory.MoveToPost();
        _currentAIState.EnterState();
    }

    private void Start()
    {
        StartSenses?.Invoke();
        DebugView.color = new Color(0.5f, 1f, 0f, 1f);
    }

    private void OnEnable()
    {
        _resourceManager.OnZeroHelath += OnDeath;
    }

    private void OnDisable()
    {
        _resourceManager.OnZeroHelath -= OnDeath;
    }

    private void EvaluateTarget()
    {
        if (_awareForTargets.Count == 0)
        {
            _currentTarget = null;
            return;
        }

        float highestAwarness = 0f;
        foreach(AwareTarget target in _awareForTargets) 
        {
            if (target.LevelFromVision + target.LevelFromHearing > highestAwarness)
            {
                highestAwarness = target.LevelFromVision + target.LevelFromHearing;
                _currentTarget = target;
            }
        }
    }


    void Update()
    {
        EvaluateTarget();
        _currentAIState.UpdateStates();

        DebugView.text = "";

        DebugView.text += gameObject.name + " state: " + _currentAIState + "\n";
        foreach (AwareTarget target in _awareForTargets)
        {
            DebugView.text += target.Target.name + ": " + " Vision: " + target.vision + " Vision Level: " + target.LevelFromVision + " Heraing: " + target.hearing + " Hearing Level: " + target.LevelFromHearing + "\n";
            DebugView.text += "Current Target: " + _currentTarget.Target.name;

        }
    }

}
