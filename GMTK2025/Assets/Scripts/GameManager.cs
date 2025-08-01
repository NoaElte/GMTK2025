using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private float loopTime;
    [SerializeField]
    private MovementAnimator blackHoleAnimator;

    private float timer;
    private bool gameBeaten;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private IEnumerator Start()
    {
        // first time stuff

        while (!gameBeaten)
        {
            // loop stuff

            timer = 0;

            blackHoleAnimator.Move();

            while (timer < loopTime)
            {
                timer += Time.deltaTime;

                if (gameBeaten)
                    break;

                yield return null;
            }

            yield return null;

            // restart
        }
    }

    public void BeatGame()
    {
        gameBeaten = true;
    }
}
