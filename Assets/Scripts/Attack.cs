using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform boxCenter;
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private float halfExtends;
    [SerializeField] Transform _executePosition;

    public Action<Transform> OnExecuting;
    public Action OnBeingExecuted;


   public void CheckForColisions()
   {
        if(boxCenter == null) return;

        Collider[] collidersDetected = Physics.OverlapBox(boxCenter.position, new Vector3(halfExtends, halfExtends, halfExtends), transform.rotation, characterLayer);

        foreach(Collider collider in collidersDetected)
        {
            if (collider.gameObject.tag == gameObject.tag || collider.gameObject.tag == "Untagged") continue;

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

    public Transform GetExecutePosition()
    {
        return _executePosition;
    }

    public void CheckForExecution()
    {
        if (boxCenter == null) return;

        Collider[] collidersDetected = Physics.OverlapBox(boxCenter.position, new Vector3(halfExtends, halfExtends, halfExtends), transform.rotation, characterLayer);

        foreach (Collider collider in collidersDetected)
        {
            if (collider.gameObject.tag == gameObject.tag || collider.tag == "Untagged") continue;

            if (Vector3.Angle(gameObject.transform.forward, collider.transform.forward) <= 60f)
            {

                Attack enemyAttackManager;
                if (!collider.gameObject.TryGetComponent<Attack>(out enemyAttackManager))
                {
                    Debug.LogError("Coudnt get enemy  attack Script from " + collider.name);
                    return;
                }

                OnExecuting?.Invoke(enemyAttackManager.GetExecutePosition());

                enemyAttackManager.OnBeingExecuted?.Invoke();
            }


        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(boxCenter.position ,new Vector3(halfExtends * 2, halfExtends * 2, halfExtends * 2));
    }
}
