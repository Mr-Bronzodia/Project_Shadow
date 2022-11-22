using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterKillScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ResourceManager colliderResourceManager;
        if (other.gameObject.TryGetComponent<ResourceManager>(out colliderResourceManager))
        {
            colliderResourceManager.ApplyDamage(99999f);
        }
    }
}
