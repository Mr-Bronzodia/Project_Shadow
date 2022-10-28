using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShadowController : MonoBehaviour
{
    private StarterAssetsInputs _inputs;
    private ShadowAbility _shadowAbility;

    private Animator _animator;
    private int _attackInputHash;
    private int _Executing;
    private Attack _attackManager;
    private ResourceManager _resourceManager;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _attackManager = GetComponent<Attack>();
        _attackInputHash = Animator.StringToHash("IsAttacking");
        _Executing = Animator.StringToHash("Executing");
        _resourceManager = GetComponent<ResourceManager>();

        _attackManager.OnExecuting += OnExcuteEnemy;
        _resourceManager.OnZeroHelath += Dead;
}
    private void OnDisable()
    {
        _inputs.OnAttackTrigger -= Attack; 
        _attackManager.OnExecuting -= OnExcuteEnemy;
        _resourceManager.OnZeroHelath -= Dead;
    }

    private void OnExcuteEnemy(Transform executePosition)
    {
        Debug.Log(executePosition.position);
        Debug.Log(transform.position);
        _animator.SetTrigger(_Executing);
        transform.position = executePosition.position;
        transform.rotation = executePosition.rotation;

    }
    private void Dead()
    {
        _shadowAbility.ShadowInstance = null;
    }

    private void Attack(bool value)
    {
        _animator.SetBool(_attackInputHash, value);
    }

    public void AssignInputs(StarterAssetsInputs inputs, ShadowAbility owner)
    {
        _inputs = inputs;
        _shadowAbility = owner;
        _inputs.OnAttackTrigger += Attack;
        
    }

    private void Update()
    {
 
    }
}
