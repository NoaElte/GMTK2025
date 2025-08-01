using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Intro")]
    [SerializeField]
    private float textTime;
    [SerializeField]
    private GameObject storyBackGround;
    [SerializeField]
    private List<GameObject> storyTexts;

    [Header("Loop")]
    [SerializeField]
    private float loopTime;
    [SerializeField]
    private MovementAnimator blackHoleAnimator;

    public delegate void ResetEvent();
    public static event ResetEvent OnReset;

    private bool gameBeaten;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private IEnumerator Start()
    {
        // first time stuff

        int textIndex = 0;

        while (textIndex < storyTexts.Count && storyTexts != null)
        {
            storyTexts.ForEach(t => t.SetActive(false));

            storyTexts[textIndex].SetActive(true);

            textIndex++;

            yield return null;
        }

        storyTexts.ForEach(t => t.SetActive(false));

        while (!gameBeaten)
        {
            // loop stuff

            float timer = 0;

            blackHoleAnimator.Move();

            while (timer <= loopTime)
            {
                timer += Time.deltaTime;

                if (gameBeaten)
                    break;

                yield return null;
            }

            if (gameBeaten)
                break;

            yield return null;

            OnReset();
        }
    }

    public void BeatGame()
    {
        gameBeaten = true;
    }
}
