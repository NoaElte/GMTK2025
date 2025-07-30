using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongController : MonoBehaviour
{
    [SerializeField]
    private Ball ball;
    [SerializeField]
    private Paddle playerPaddle;
    [SerializeField]
    private Paddle aiPaddle;
    [SerializeField]
    private List<GameObject> lightOfLosses;

    private int lossCount = 0;
    private TV tv;

    public void StartPong(TV tv)
    {
        this.tv = tv;
        ball.Launch();
        playerPaddle.SetIsPlaying(true);
        aiPaddle.SetIsPlaying(true);
    }

    public void Score(bool isPlayerGoal)
    {
        ball.Reset();
        if (!isPlayerGoal) return;

        lightOfLosses[lossCount].SetActive(true);
        lossCount++;

        if (lossCount >= lightOfLosses.Count)
        {
            FinishPong();
            return;
        }

        ball.Launch();
    }

    private void FinishPong()
    {
        ball.Reset();
        playerPaddle.SetIsPlaying(false);
        aiPaddle.SetIsPlaying(false);

        tv.EndOfPong();
    }
}
