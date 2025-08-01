using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    private float walkingSpeed = 7.5f;
    [SerializeField]
    private float runningSpeed = 11.5f;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField]
    private float flySpeed = 8.0f;
    [SerializeField]
    private float gravity = 20.0f;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float lookSpeed = 2.0f;
    [SerializeField]
    private float lookXLimit = 45.0f;
    [SerializeField]
    private float interactionSphereRadius = .5f;
    [SerializeField]
    private float maxInteractionDistance = 5.0f;
    [SerializeField]
    private Transform camFollow;
    [SerializeField]
    private float headBobStrength = 0.1f;
    [SerializeField]
    private float walkBobFrequency = 1.25f;
    [SerializeField]
    private float runBobFrequency = 1.75f;
    [SerializeField]
    private GameObject interactIndicator;
    [SerializeField]
    private TMP_Text interactText;
    [SerializeField]
    private LayerMask interactableMask;
    [SerializeField]
    private bool lockCursor;
    [SerializeField]
    private LayerMask FlyZoneMask;
    [SerializeField, Range(-1, 1)]
    private float footStepTimer;
    [SerializeField]
    private UnityEvent onStep;
    [SerializeField]
    private Transform grabPoint;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;
    private bool allowedToMove = true;
    private bool isFlying = false;
    private float headBobTimer;
    private Vector3 camFollowPos;
    private Vector3 lastHeadBobOffset = Vector3.zero;
    private Grabbable currentGrabbed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        camFollowPos = camFollow.localPosition;

        if(lockCursor )
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (!allowedToMove)
        {
            interactText.text = "";
            return;
        }

        if (isFlying)
            FlyMovement();
        else
            GroundMovement();

        Interraction();
    }

    public void SetAllowedToMove(bool allowedToMove) => this.allowedToMove = allowedToMove;

    public void SetCanMove(bool canMove) => this.canMove = canMove;

    private void GroundMovement()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = new Vector3(playerCamera.transform.TransformDirection(Vector3.forward).x, 0, playerCamera.transform.TransformDirection(Vector3.forward).z).normalized;
        Vector3 right = new Vector3(playerCamera.transform.TransformDirection(Vector3.right).x, 0, playerCamera.transform.TransformDirection(Vector3.right).z).normalized;
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButtonDown("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // head bobbing
        if (Mathf.Abs(moveDirection.x) > .1f || Mathf.Abs(moveDirection.z) > .1f)
        {
            headBobTimer += Time.deltaTime * (isRunning ? runBobFrequency : walkBobFrequency);
            Vector3 headBobOffset = new Vector3(0, Mathf.Sin(headBobTimer) * headBobStrength, 0);
            camFollow.localPosition = camFollowPos + headBobOffset;

            //Debug.Log(headBobOffset);

            if (lastHeadBobOffset.y > headBobOffset.y && lastHeadBobOffset.y > footStepTimer * headBobStrength && headBobOffset.y <= footStepTimer * headBobStrength)
            {
                onStep.Invoke();
            }

            lastHeadBobOffset = headBobOffset;
        }
        else
        {
            camFollow.localPosition = Vector3.Lerp(camFollow.localPosition, camFollowPos, Time.deltaTime * 5f);
        }

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void FlyMovement()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = new Vector3(playerCamera.transform.TransformDirection(Vector3.forward).x, 0, playerCamera.transform.TransformDirection(Vector3.forward).z).normalized;
        Vector3 right = new Vector3(playerCamera.transform.TransformDirection(Vector3.right).x, 0, playerCamera.transform.TransformDirection(Vector3.right).z).normalized;
        // Press Left Shift to run
        float curSpeedX = canMove ? flySpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? flySpeed * Input.GetAxis("Horizontal") : 0;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove)
        {
            moveDirection.y = flySpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            moveDirection.y = -flySpeed;
        }
        else
        {
            moveDirection.y = 0;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void Interraction()
    {
        interactIndicator.SetActive(false);
        interactText.text = "";
        RaycastHit hit;
        if(Physics.SphereCast(playerCamera.transform.position, interactionSphereRadius, playerCamera.transform.TransformDirection(Vector3.forward), out hit, maxInteractionDistance, interactableMask))
        {

            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if (interactable == null) 
            {
                interactable = hit.transform.GetComponentInParent<Interactable>();
                if (interactable == null)
                    return;
            }

            interactIndicator.SetActive(true);
            interactText.text = interactable.UseText;

            if (Input.GetKeyDown(KeyCode.E))
                interactable.Interact(transform);
        }
        else if(currentGrabbed != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentGrabbed.Drop();
                currentGrabbed = null;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == FlyZoneMask)
            isFlying = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == FlyZoneMask)
            isFlying = false;
    }

    public void SetIsFlying(bool isFlying)
    {
        this.isFlying = isFlying;
    }

    public Transform GetGrabPoint(Grabbable grabbed)
    {
        currentGrabbed = grabbed;
        return grabPoint;
    }
}
