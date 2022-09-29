using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHead : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Vector3 _viewOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = _head.position + _viewOffset; 
    }
}
