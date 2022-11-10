using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowAbility : MonoBehaviour
{
    [SerializeField] private GameObject _shadowPrefab;
    [SerializeField] private GameObject _fxPrefab;
    [SerializeField] private float _range;
    private FirstPersonController _firstPersonController;
    private ResourceManager _resources;
    private StarterAssetsInputs _inputs;
    private ShadowState _currentState;
    private ShadowFactory _factory;
    private GameObject _fxInstance;
    private GameObject _shadowInstance;
    private bool _isExecuting = false;
    [SerializeField] private LayerMask _layerMask;


    public Action<ShadowState> OnStateChanged;
    public ShadowState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public StarterAssetsInputs Inputs { get { return _inputs; } }
    public float AbilityRange { get { return _range; } }
    public GameObject FxInstance { get { return _fxInstance; } set { _fxInstance = value; } }

    public LayerMask AbilityLayerMask { get { return _layerMask; } }

    public GameObject ShadowInstance { get { return _shadowInstance; } set { _shadowInstance = value; } }

    public FirstPersonController FirstPersonController { get { return _firstPersonController; } }

    public ResourceManager PlayerReourceManager { get { return _resources; } }

    public bool IsExecuting { get { return _isExecuting; } }


    private void Awake()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
        _firstPersonController = GetComponent<FirstPersonController>();
        _resources = GetComponent<ResourceManager>();
        _factory = new ShadowFactory(this);
        _currentState = _factory.Inactive();
        _currentState.EnterState();
    }

    public void InstantiateFX(Vector3 pos, Quaternion rot)
    {
        _fxInstance = Instantiate(_fxPrefab, pos, rot);
    }

    public void DestroyFX()
    {
        Destroy(_fxInstance);
        _fxInstance = null; 
    }

    public void CreateShadow(Transform tra)
    {
        _shadowInstance = Instantiate(_shadowPrefab, tra.position, Quaternion.identity);
        _shadowInstance.GetComponent<ShadowController>().AssignInputs(_inputs, this);
    }

    public void OnExecute()
    {
        _isExecuting = true;
        StartCoroutine(WaitForExecutingAnim(5f));
    }

    private IEnumerator WaitForExecutingAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isExecuting = false;
    }

    public void MovePlayer(Vector3 dir)
    {
        _firstPersonController.CharacterController.Move(dir);
    }

    public void DestroyShadow()
    {
        Destroy(_shadowInstance);
        _shadowInstance = null;
    }

    private void Update()
    {
        _currentState.UpdateState();
    }
}
