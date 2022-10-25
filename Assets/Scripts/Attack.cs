using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform boxCenter;
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private float halfExtends;


   public void CheckForColisions()
   {
        if(boxCenter == null) return;

        Collider[] collidersDetected = Physics.OverlapBox(boxCenter.position, new Vector3(halfExtends, halfExtends, halfExtends), Quaternion.identity, characterLayer);

        foreach(Collider collider in collidersDetected)
        {
            if (collider.gameObject.tag == gameObject.tag) return;

            ResourceManager resourceManager;
            if (collider.gameObject.TryGetComponent<ResourceManager>(out resourceManager))
            {
                resourceManager.ApplyDamage(30f);
            }
            else
            {
                Debug.LogError("Object: " + collider.gameObject.name + " doesn't have resource manager but should");
            }

        }
   }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(boxCenter.position ,new Vector3(halfExtends * 2, halfExtends * 2, halfExtends * 2));
    }
}
