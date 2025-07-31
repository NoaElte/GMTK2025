using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float loopTime;

    private float timer;
    private bool gameBeaten;

    private IEnumerator Start()
    {
        // first time stuff

        while (!gameBeaten)
        {
            // loop stuff

            timer = 0;

            while (timer < loopTime)
            {
                timer += Time.deltaTime;

                if (gameBeaten)
                    break;

                yield return null;
            }

            // restart
        }
    }

    public void BeatGame()
    {
        gameBeaten = true;
    }
}
