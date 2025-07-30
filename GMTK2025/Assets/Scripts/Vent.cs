using UnityEngine;

public class Vent : Interactable
{
    private bool hasScrewdriver;

    public override string UseText => "Open" + (!hasScrewdriver ? " (No screwdriver)" : "");

    public override void Interact(Transform player)
    {
        if (!hasScrewdriver)
            return;

        base.Interact(player);

        gameObject.SetActive(false);
    }

    public void ReceiveScrewdriver() => hasScrewdriver = true;
    public void LostScrewdriver() => hasScrewdriver = false;
}
