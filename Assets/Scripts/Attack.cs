using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform boxCenter;
    [SerializeField] private LayerMask characterMask;
    [SerializeField] private float halfExtends;


   public void CheckForColisions()
   {
        if(boxCenter == null) return;

        Collider[] collidersDetected = Physics.OverlapBox(boxCenter.position, new Vector3(halfExtends, halfExtends, halfExtends), Quaternion.identity, characterMask);

        foreach(Collider collider in collidersDetected)
        {
            if(collider.gameObject.tag != gameObject.tag)
            {
                Debug.Log(gameObject.tag + "Attacked " + collider.gameObject.tag + " : " + collider.name);
            }
        }
   }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(boxCenter.position ,new Vector3(halfExtends * 2, halfExtends * 2, halfExtends * 2));
    }
}
