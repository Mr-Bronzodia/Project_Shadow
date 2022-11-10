using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private static MenuController _instance;

    private bool _enabled = false;

    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _hud;
    [SerializeField] private StarterAssetsInputs _inputs;
    [SerializeField] private FirstPersonController _firstPersonController;

    public static MenuController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void OnPause(bool value)
    {
        switch (value)
        {
            case true:
                EnableUI();
                break;
            case false:
                DisableUI();
                break;
        }
    }

    public void EnableUI()
    {
        _hud.SetActive(false);
        _pause.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        _firstPersonController.LockControls = true;
        Time.timeScale = 0f;
    }

    public void DisableUI()
    {
        _hud.SetActive(true);
        _pause.SetActive(false);
        _inputs.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        _firstPersonController.LockControls = false;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        _firstPersonController.LockControls = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        _inputs.OnPauseTrigger += OnPause;
    }

    private void OnDisable()
    {
        _inputs.OnPauseTrigger -= OnPause;
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
