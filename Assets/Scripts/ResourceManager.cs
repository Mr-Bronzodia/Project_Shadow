using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health = 100;

    [SerializeField] private float _maxMana;
    [SerializeField] private float _mana = 100;

    public float CurrentHealth { get { return _health; } }
    public float CurrentMana { get { return _mana; } }
    public float MaxHealth { get { return _maxHealth; } }
    public float MaxMana { get { return _maxMana; } }

    public Action OnZeroHelath;
    public Action<float> OnHealthDraine;
    public Action<float> OnManaDrained;

    private void OnEnable()
    {
        _health = _maxHealth;
        _mana = _maxMana;
    }

    public void ApplyDamage(float damage)
    {
        _health -= damage;
        OnHealthDraine?.Invoke(damage);

        if (_health <= 0) 
        {
            OnZeroHelath?.Invoke();
        }
    }

    public bool IsManaAvailible(float amout)
    {
        if (_mana > amout)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ApplyManaLose(float manaLose)
    {
        if (_mana - manaLose <= 0 || _mana - manaLose >= _maxMana) return;
        _mana -= manaLose;
        OnManaDrained?.Invoke(manaLose);
    }

}
