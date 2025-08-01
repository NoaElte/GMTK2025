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

    [Header("Rotation Settings")]
    [SerializeField]
    private float rotationalSpeed = 50f;

    [Header("Debug")]
    public float minForce;
    public float minTorque;

    private Rigidbody rb;
    private Transform grabPoint;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnEnable()
    {
        GameManager.OnReset += ResetPosition;
    }

    private void OnDisable()
    {
        GameManager.OnReset -= ResetPosition;
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
        ApplyRotationSpring();
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

    void ApplyRotationSpring()
    {
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, grabPoint.rotation, rotationalSpeed * Time.deltaTime));
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

    private void ResetPosition()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
    }
}
