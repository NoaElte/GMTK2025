using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private bool isPlayerGoal;
    [SerializeField]
    private PongController controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.GetComponent<Ball>();
        if(ball != null)
        {
            controller.Score(isPlayerGoal);
        }
    }
}
