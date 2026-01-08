using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float ease;


    // Update is called once per frame
    void Update()
    {
        Vector3 desired_position = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired_position, ease);
    }
}
