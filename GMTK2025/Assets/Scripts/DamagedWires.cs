using UnityEngine;

public class DamagedWires : Interactable
{
    [SerializeField]
    private float distanceForAchievement = 5f;
    [SerializeField]
    private AchievementSO achievementBuzzerBeater;

    public override void Interact(Transform player)
    {
        if (Vector3.Distance(transform.position, player.position) >= distanceForAchievement)
            Achievement.Instance.AchievemnetGet(achievementBuzzerBeater);

        base.Interact(player);
    }
}
