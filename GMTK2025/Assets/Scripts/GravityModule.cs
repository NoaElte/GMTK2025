using System.Collections;
using UnityEngine;

public class GravityModule : Interactable
{
    [SerializeField]
    private float gravityOffTime = 30f;

    [SerializeField]
    private RotateByAngle leverRotate;

    [SerializeField]
    private Material redLightMat;
    [SerializeField] 
    private Material greenLightMat;
    [SerializeField]
    private GameObject redLight;
    [SerializeField]
    private GameObject greenLight;


    private FirstPersonController playerController;
    private MeshRenderer renderer;


    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        GameManager.OnReset += ResetGravity;
    }

    private void OnDisable()
    {
        GameManager.OnReset -= ResetGravity;
    }

    public override void Interact(Transform player)
    {
        base.Interact(player);

        playerController = player.GetComponent<FirstPersonController>();

        StartCoroutine(TurnGravityOffWithTimer());
    }

    private IEnumerator TurnGravityOffWithTimer()
    {
        leverRotate.Rotate();

        yield return new WaitForSeconds(1);

        playerController.SetIsFlying(true);

        renderer.materials[3] = redLightMat;

        greenLight.SetActive(false);
        redLight.SetActive(true);

        yield return new WaitForSeconds(gravityOffTime);

        playerController.SetIsFlying(false);

        ResetGravity();
    }

    private void ResetGravity()
    {
        renderer.materials[3] = greenLightMat;

        redLight.SetActive(false);
        greenLight.SetActive(true);

        leverRotate.Rotate();
    }
}
