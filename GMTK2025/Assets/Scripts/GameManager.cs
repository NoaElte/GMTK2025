using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Intro")]
    [SerializeField]
    private float textTime;
    [SerializeField]
    private Image storyBackGround;
    [SerializeField]
    private List<GameObject> storyTexts;

    [Header("Loop")]
    [SerializeField]
    private float loopTime;
    [SerializeField]
    private MovementAnimator blackHoleAnimator;
    [SerializeField]
    private float resetWaitTime;
    [SerializeField]
    private float backgroundFadeoutTime;
    [SerializeField]
    private AnimationCurve backgroundFadeoutAnimationCurve;

    [Header("Extras")]
    [SerializeField]
    private int collectibleCount;
    [SerializeField]
    private AchievementSO collectibleAchievement;
    [SerializeField]
    private Achievement achievement;

    public delegate void ResetEvent();
    public static event ResetEvent OnReset;

    private bool gameBeaten;
    private int collectiblesCollected;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private IEnumerator Start()
    {
        // first time stuff

        storyBackGround.color = Color.black;

        yield return new WaitForSeconds(3f);

        int textIndex = 0;

        while (textIndex < storyTexts.Count && storyTexts != null)
        {
            storyTexts.ForEach(t => t.SetActive(false));

            storyTexts[textIndex].SetActive(true);

            textIndex++;

            yield return new WaitForSeconds(textTime);
        }

        storyTexts.ForEach(t => t.SetActive(false));

        yield return new WaitForSeconds(textTime);

        while (!gameBeaten)
        {
            // loop stuff

            float backgroundFadeoutTimer = 0;

            while (backgroundFadeoutTimer <= backgroundFadeoutTime)
            {
                Color color = storyBackGround.color;

                float a = Mathf.Lerp(1, 0, backgroundFadeoutAnimationCurve.Evaluate(backgroundFadeoutTimer / backgroundFadeoutTime));

                color.a = a;
                storyBackGround.color = color;

                backgroundFadeoutTimer += Time.deltaTime;

                yield return null;
            }

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

            storyBackGround.color = Color.black;

            yield return new WaitForSeconds(resetWaitTime);

            OnReset();
        }
    }

    public void BeatGame()
    {
        gameBeaten = true;
    }

    public void OnCollectibleCollected()
    {
        collectiblesCollected++;

        if (collectiblesCollected >= collectibleCount)
            Achievement.Instance.AchievemnetGet(collectibleAchievement);
    }
}
