using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _mana = 100;

    public Action OnZeroHelath;
    public Action<float> OnHealthDraine;
    public Action<float> OnManaDrained;

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
        _mana -= manaLose;
        OnManaDrained?.Invoke(manaLose);
    }

}
