using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    [SerializeField]
    private Collider collider;
    [SerializeField]
    private Interactable canInteractWith;

    [Header("Position Settings")]
    [SerializeField]
    private float springForce;
    [SerializeField]
    private float positionalDamping = 50f;
    [SerializeField]
    private float maxForce = 1000f;

    [Header("Debug")]
    public float minForce;

    private Rigidbody rb;
    private Transform grabPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact(Transform player)
    {
        //base.Interact(player);
        grabPoint = player.GetComponent<FirstPersonController>().GetGrabPoint(this);
        rb.useGravity = false;
        transform.SetPositionAndRotation(grabPoint.position, grabPoint.rotation);
        //collider.isTrigger = true;
    }

    public void Drop()
    {
        grabPoint = null;
        rb.useGravity = true;
        //collider.isTrigger = false;
    }

    private void FixedUpdate()
    {
        if (grabPoint == null)
            return;

        ApplyPositionSpring();
    }

    void ApplyPositionSpring()
    {
        Vector3 toTarget = grabPoint.position - transform.position;
        Vector3 desiredVelocity = toTarget * springForce;
        Vector3 force = desiredVelocity - rb.velocity * positionalDamping;

        if (force.sqrMagnitude < minForce)
        {
            force -= rb.velocity * positionalDamping;
        }

        force = Vector3.ClampMagnitude(force, maxForce);
        rb.AddForce(force, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision == null || canInteractWith == null) return;

        if(collision.gameObject == canInteractWith.gameObject)
        {
            canInteractWith.Interact(transform);
            onInteract.Invoke();
        }
    }
}
