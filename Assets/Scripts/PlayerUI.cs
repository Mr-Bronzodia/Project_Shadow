using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _manaSlider;
    [SerializeField] private Image _abilityIcon;

    private ResourceManager _resources;
    private ShadowAbility _shadowAbility;

    [SerializeField] private Sprite _shadowActiveSprite;
    [SerializeField] private Sprite _shadowInActiveSprite;

    private float _maxHealth;
    private float _currentHealth;

    private float _currentMana;
    private float _maxMana;

    private void OnEnable()
    {
        _resources = GetComponent<ResourceManager>();
        _shadowAbility = GetComponent<ShadowAbility>();

        _maxHealth = _resources.MaxHealth;
        _maxMana = _resources.MaxMana;

        _currentHealth = _maxHealth;
        _currentMana = _maxMana;

        UpdateUI();

        _resources.OnHealthDraine += OnHealthDown;
        _resources.OnManaDrained += OnManaDown;
        _shadowAbility.OnStateChanged += OnAbilityStatusChanged;

    }

    private void OnDisable()
    {
        _resources.OnHealthDraine -= OnHealthDown;
        _resources.OnManaDrained -= OnManaDown;
        _shadowAbility.OnStateChanged -= OnAbilityStatusChanged;
    }

    private void UpdateUI()
    {
        _hpSlider.value = _currentHealth / _maxHealth;
        _manaSlider.value = _currentMana / _maxMana;
    }

    private void OnAbilityStatusChanged(ShadowState currentstate)
    {
        if (currentstate is ShadowActiveState) _abilityIcon.sprite = _shadowActiveSprite;

        if (currentstate is ShadowInactiveState) _abilityIcon.sprite = _shadowInActiveSprite;
    }

    private void OnHealthDown(float amount)
    {
        _currentHealth -= amount;
        UpdateUI();
    }

    private void OnManaDown(float amount) 
    {
        _currentMana -= amount;
        UpdateUI();
    }


}
