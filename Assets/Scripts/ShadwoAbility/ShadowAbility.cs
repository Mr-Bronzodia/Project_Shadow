using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowAbility : MonoBehaviour
{
    [SerializeField] private GameObject _shadowPrefab;
    [SerializeField] private GameObject _fxPrefab;
    [SerializeField] private float _range;
    private ResourceManager _resources;
    private StarterAssetsInputs _inputs;
    private ShadowState _currentState;
    private ShadowFactory _factory;
    private GameObject _fxInstance;
    private GameObject _shadowInstance;
    [SerializeField] private LayerMask _layerMask;

    public ShadowState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public StarterAssetsInputs Inputs { get { return _inputs; } }
    public float AbilityRange { get { return _range; } }
    public GameObject FxInstance { get { return _fxInstance; } set { _fxInstance = value; } }

    public LayerMask AbilityLayerMask { get { return _layerMask; } }

    public GameObject ShadowInstance { get { return _shadowInstance; } set { _shadowInstance = value; } }


    private void Awake()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
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
