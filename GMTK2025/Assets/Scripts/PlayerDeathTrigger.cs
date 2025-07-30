using UnityEngine;

public class PlayerDeathTrigger : MonoBehaviour
{
    public bool killPlayer;

    private void Update()
    {
        if (killPlayer)
            Die();
    }

    public void Die()
    {
        Debug.Log("player died");
    }
}
