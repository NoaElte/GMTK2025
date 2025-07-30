using UnityEngine;

public class TV : Interactable
{
    [SerializeField]
    private GameObject errorScreen;
    [SerializeField]
    private GameObject pongScreen;
    [SerializeField]
    private PongController pongController;

    private Transform player;

    public new string UseText => "Play";

    public override void Interact(Transform player)
    {
        base.Interact(player);

        this.player = player;
        errorScreen.SetActive(false);
        pongScreen.SetActive(true);

        player.GetComponent<FirstPersonController>().SetAllowedToMove(false);
        pongController.StartPong(this);
    }

    public void EndOfPong()
    {
        errorScreen.SetActive(true);
        pongScreen.SetActive(false);

        player.GetComponent<FirstPersonController>().SetAllowedToMove(true);
    }
}
