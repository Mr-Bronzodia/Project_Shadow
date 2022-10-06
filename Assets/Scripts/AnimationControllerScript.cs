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

    private int _velocityParHash;
    private int _shouldRunParHash;


    private Vector3 _velocity;
    private bool _shouldRun;

    //walk 1
    //run 


    private void Awake()
    {
        _navComponent = GetComponent<NavMeshAgent>();
        _aiComponent = GetComponent<AIAgent>();
        _animator = GetComponent<Animator>();

        _velocityParHash = Animator.StringToHash("Velocity");
        _shouldRunParHash = Animator.StringToHash("ShouldRun");
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

    private void Update()
    {
        UpdateAnimatorVars();
    }
}
