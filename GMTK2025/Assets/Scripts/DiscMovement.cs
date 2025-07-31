using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscMovement : MonoBehaviour
{
    public GameObject viewPosition;
    public Vector3 addedRotation;

    void Update()
    {
        // Make the disc face the camera, but keep its up vector (optional: Vector3.up for world up)
        transform.rotation = Quaternion.LookRotation(transform.position - viewPosition.transform.position, Vector3.up) * Quaternion.Euler(addedRotation);
    }
}
