using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAgent))]
public abstract class AISense : MonoBehaviour
{
    public bool Enable = true;
    public LayerMask TargetMask;
    public LayerMask ObsticleLayer;

    protected AIAgent _agent;
    [SerializeField] protected float _senseInterval;


    private void Awake()
    {
        if (Enable)
        {
            _agent = GetComponent<AIAgent>();
            _agent.StartSenses += RegisterSense;
        } 
    }

    private void OnDisable()
    {
        if (Enable)
        {
            _agent.StartSenses -= RegisterSense;
        }
    }

    protected abstract IEnumerator BeginSense();

    private void RegisterSense()
    {
        StartCoroutine(BeginSense());
    }



}
