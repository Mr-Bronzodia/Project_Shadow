using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAgent))]
public abstract class AISense : MonoBehaviour
{
    public LayerMask TargetMask;
    public LayerMask ObsticleLayer;
    private List<GameObject> SensedTargets = new List<GameObject>();
    private List<GameObject> SensedThisFrame = new List<GameObject>();

    protected AIAgent _agent;
    [SerializeField] protected float _senseInterval;


    private void Awake()
    {
        _agent = GetComponent<AIAgent>();
        _agent.StartSenses += RegisterSense;
    }

    private void OnDisable()
    {
        _agent.StartSenses -= RegisterSense;
    }

    protected abstract IEnumerator BeginSense();

    private void RegisterSense()
    {
        StartCoroutine(BeginSense());
    }



}
