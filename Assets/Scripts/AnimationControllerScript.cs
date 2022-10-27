using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AIAgent), typeof(Animator))]
public class AnimationControllerScript : MonoBehaviour
{
    private NavMeshAgent _navComponent;
    private AIAgent _aiComponent;
    private Animator _animator;
    private Attack _attackManager;
    private ResourceManager _resourceManager;
    private Rigidbody[] _rbs;


    private int _velocityParHash;
    private int _shouldRunParHash;
    private int _attack1ParHash;
    private int _attack2ParHash;
    private int _attack3ParHash;
    private int _slash1ParHash;
    private int _slash2ParHash;
    private int _slash3ParHash;
    private int _slash4ParHash;
    private int _onBeingExecutedParHash;


    private Vector3 _velocity;
    private bool _shouldRun;

    private float _animcooldown = 0;
    private float _currentCooldown = 0;

    private void Awake()
    {
        _navComponent = GetComponent<NavMeshAgent>();
        _aiComponent = GetComponent<AIAgent>();
        _animator = GetComponent<Animator>();
        _attackManager = GetComponent<Attack>();   
        _resourceManager = GetComponent<ResourceManager>();

        HashStrings();   
    }

    private void Start()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();
    }

    private void HashStrings()
    {
        _velocityParHash = Animator.StringToHash("Velocity");
        _shouldRunParHash = Animator.StringToHash("ShouldRun");

        _attack1ParHash = Animator.StringToHash("Attack1");
        _attack2ParHash = Animator.StringToHash("Attack2");
        _attack3ParHash = Animator.StringToHash("Attack3");

        _slash1ParHash = Animator.StringToHash("Slash1");
        _slash2ParHash = Animator.StringToHash("Slash2");
        _slash3ParHash = Animator.StringToHash("Slash3");
        _slash4ParHash = Animator.StringToHash("Slash4");
        _onBeingExecutedParHash = Animator.StringToHash("OnBegginExecution");
    }

    private void OnBeingExecuted()
    {
        _animator.SetTrigger(_onBeingExecutedParHash);
        _aiComponent.OnDeath();
        StartCoroutine(TurnOnRagdoll(5f));
    }

    private void OnZeroHalth()
    {
        StartCoroutine(TurnOnRagdoll(0.1f));
    }

    private IEnumerator TurnOnRagdoll(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.enabled = false;
        
        foreach(Rigidbody rb in _rbs)
        {
            rb.maxDepenetrationVelocity = 1f;
            rb.velocity = Vector3.zero;
        }

    }

    private void OnEnable()
    {
        _aiComponent.OnCombatContinue += UpdateCombatAnims;
        _attackManager.OnBeingExecuted += OnBeingExecuted;
        _resourceManager.OnZeroHelath += OnZeroHalth;
    }

    private void OnDisable()
    {
        _aiComponent.OnCombatContinue -= UpdateCombatAnims;
        _attackManager.OnBeingExecuted -= OnBeingExecuted;
        _resourceManager.OnZeroHelath -= OnZeroHalth;
    }


    private void UpdateAnimatorVars()
    {
        _velocity = _navComponent.velocity;
        _animator.SetFloat(_velocityParHash, _velocity.magnitude);

        if(_aiComponent.AlertState == AlertState.Alerted)
        {
            _shouldRun = true;
        }
        else
        {
            _shouldRun = false;
        }

        _animator.SetBool(_shouldRunParHash, _shouldRun);
    }

    private void UpdateCombatAnims()
    {
        _currentCooldown += Time.deltaTime;

        if (_currentCooldown >= _animcooldown)
        {
            if (_velocity.magnitude > 3.0f)
            {
                int rool = Random.Range(0, 3);

                switch(rool) 
                {
                    case 0:
                        _animator.SetTrigger(_attack1ParHash);
                        _animcooldown = 1.4f;
                        break;

                    case 1:
                        _animator.SetTrigger(_attack2ParHash);
                        _animcooldown = 1.8f;
                        break;

                    case 2:
                        _animator.SetTrigger(_attack3ParHash);
                        _animcooldown = 2.4f;
                        break;
                }

                _currentCooldown = 0f;
            }
            else if(_velocity.magnitude < 3.0f)
            {
                int rool = Random.Range(0, 4);

                switch (rool)
                {
                    case 0:
                        _animator.SetTrigger(_slash1ParHash);
                        _animcooldown = 3.6f;
                        break;

                    case 1:
                        _animator.SetTrigger(_slash2ParHash);
                        _animcooldown = 1.6f;
                        break;

                    case 2:
                        _animator.SetTrigger(_slash3ParHash);
                        _animcooldown = 2.4f;
                        break;
                    case 3:
                        _animator.SetTrigger(_slash4ParHash);
                        _animcooldown = 1.6f;
                        break;
                }

                _currentCooldown = 0f;
            }
        }
        
    }

    private void Update()
    {
        UpdateAnimatorVars();
    }
}
