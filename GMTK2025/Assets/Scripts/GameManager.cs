using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Intro")]
    [SerializeField]
    private float textTime;
    [SerializeField]
    private UnityEngine.UI.Image storyBackGround;
    [SerializeField]
    private List<GameObject> storyTexts;
    [SerializeField]
    private AudioSource introMusic;

    [Header("Loop")]
    [SerializeField]
    private float loopTime;
    [SerializeField]
    private RotateByAngle podOpener;
    [SerializeField]
    private MovementAnimator blackHoleAnimator;
    [SerializeField]
    private float resetWaitTime;
    [SerializeField]
    private float backgroundFadeoutTime;
    [SerializeField]
    private AnimationCurve backgroundFadeoutAnimationCurve;
    [SerializeField]
    private AudioSource loopMusic;
    [SerializeField]
    private AnimationCurve cameraShakeStrengthOverTime;

    [Header("Ending")]
    [SerializeField]
    private AudioSource endingMusic;
    [SerializeField]
    private GameObject warpDriveEffect;
    [SerializeField]
    private MeshRenderer warpdriveBackgroundRenderer;
    [SerializeField]
    private GameObject endingScreen;
    [SerializeField]
    private TMP_Text notesStatText;
    [SerializeField]
    private string notesStatTemplate;
    [SerializeField]
    private TMP_Text collectiblesStatText;
    [SerializeField]
    private string collectiblesStatTemplate;
    [SerializeField]
    private TMP_Text achievementsStatText;
    [SerializeField]
    private string achievementsStatTemplate;

    [Header("Extras")]
    [SerializeField]
    private int collectibleCount;
    [SerializeField]
    private AchievementSO collectibleAchievement;
    [SerializeField]
    private int noteCount;
    [SerializeField]
    private AchievementSO noteAchievement;
    [SerializeField]
    private Transform radio;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private AchievementSO radioAchievement;
    [SerializeField]
    private AchievementSO blackHoleAchievement;
    
    [Header("Debug")]
    [SerializeField]
    private bool skipIntro;

    public delegate void ResetEvent();
    public static event ResetEvent OnReset;

    public delegate void CameraShakeStrengthSetterEvent(float strength);
    public static event CameraShakeStrengthSetterEvent OnShakeStrengthSet;

    private bool gameBeaten;
    private int collectiblesCollected;
    private List<GameObject> notesRead;
    private bool isPlayerDead = false;
    private bool endingSequence;
    

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        notesRead = new List<GameObject>();
    }

    private IEnumerator Start()
    {
        if (!skipIntro)
        {
            // first time stuff

            storyBackGround.color = Color.black;

            introMusic.Play();

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

            StartCoroutine(FadeMusicOut(introMusic, textTime));

            yield return new WaitForSeconds(textTime);

        }

        OnReset();

        while (!gameBeaten)
        {
            // loop stuff

            isPlayerDead = false;
            
            //start music
            StartCoroutine(FadeMusicIn(loopMusic, backgroundFadeoutTime));

            podOpener.Rotate();

            float storyBackgroundFadeoutTimer = 0;

            while (storyBackgroundFadeoutTimer <= backgroundFadeoutTime)
            {
                Color color = storyBackGround.color;

                float a = Mathf.Lerp(1, 0, backgroundFadeoutAnimationCurve.Evaluate(storyBackgroundFadeoutTimer / backgroundFadeoutTime));

                color.a = a;
                storyBackGround.color = color;

                storyBackgroundFadeoutTimer += Time.deltaTime;

                yield return null;
            }

            storyBackGround.color = Color.clear;

            float timer = 0;

            blackHoleAnimator.Move();

            while (timer <= loopTime - backgroundFadeoutTime)
            {
                timer += Time.deltaTime;

                float shakeStregth = cameraShakeStrengthOverTime.Evaluate(timer / (loopTime - backgroundFadeoutTime));
                
                OnShakeStrengthSet?.Invoke(shakeStregth);

                if (gameBeaten || isPlayerDead)
                    break;

                yield return null;
            }

            if (gameBeaten)
            {
                endingSequence = true;

                StartCoroutine(EndingSequence());
            }

            while (endingSequence)
                yield return null;

            yield return null;

            storyBackGround.color = Color.black;

            OnReset();

            yield return new WaitForSeconds(resetWaitTime);

            loopMusic.Stop();
        }
    }

    private IEnumerator EndingSequence()
    {
        yield return new WaitForSeconds(3);

        StartCoroutine(FadeMusicOut(loopMusic, 3f));

        warpDriveEffect.SetActive(true);

        float warpdriveBackgoundFadeInTimer = 0;

        while (warpdriveBackgoundFadeInTimer <= 1f)
        {
            Color color = Color.clear;

            float a = Mathf.Lerp(0, 1, backgroundFadeoutAnimationCurve.Evaluate(warpdriveBackgoundFadeInTimer / 1f));

            color.a = a;

            float shakeStregth = cameraShakeStrengthOverTime.Evaluate(warpdriveBackgoundFadeInTimer / 1f);

            OnShakeStrengthSet?.Invoke(shakeStregth);

            warpdriveBackgroundRenderer.material.SetColor("_BaseColor", color);

            warpdriveBackgoundFadeInTimer += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(2f);


        if (Vector3.Distance(player.position, radio.position) <= 6f)
            Achievement.Instance.AchievemnetGet(radioAchievement);
        else
            StartCoroutine(FadeMusicIn(endingMusic, backgroundFadeoutTime));

        yield return new WaitForSeconds(3f);

        float storyBackgroundFadeinTimer = 0;

        while (storyBackgroundFadeinTimer <= backgroundFadeoutTime)
        {
            Color color = storyBackGround.color;

            float a = Mathf.Lerp(0, 1, backgroundFadeoutAnimationCurve.Evaluate(storyBackgroundFadeinTimer / backgroundFadeoutTime));

            color.a = a;
            storyBackGround.color = color;

            storyBackgroundFadeinTimer += Time.deltaTime;

            yield return null;
        }

        storyBackGround.color = Color.black;

        yield return new WaitForSeconds(1);

        endingScreen.SetActive(true);

        notesStatText.text = string.Format(notesStatTemplate, notesRead.Count, noteCount);

        collectiblesStatText.text = string.Format(collectiblesStatTemplate, collectiblesCollected, collectibleCount);

        achievementsStatText.text = string.Format(achievementsStatTemplate, Achievement.Instance.AchievementsGot, Achievement.Instance.AchievementsCount);

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void Continue()
    {
        endingScreen.SetActive(false);
        gameBeaten = false;
        endingSequence = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;


        StartCoroutine(FadeMusicOut(endingMusic, 1f));

        // stop the drive effects
        warpdriveBackgroundRenderer.material.SetColor("_BaseColor", Color.clear);
        warpDriveEffect.SetActive(false);
    }

    private void Update()
    {
        loopMusic.volume = Mathf.Clamp01((Vector3.Distance(player.position, radio.position) - 2f) / 6f);

        if(Vector3.Distance(player.position, blackHoleAnimator.transform.position) < 5f)
        {
            Achievement.Instance.AchievemnetGet(blackHoleAchievement);

            PlayerDied();
        }
    }

    public void PlayerDied()
    {
        isPlayerDead = true;
    }

    private IEnumerator FadeMusicOut(AudioSource audioSource, float time)
    {
        float startVolume = audioSource.volume;

        float timer = 0;

        while (timer < time)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / time);

            timer += Time.deltaTime;

            yield return null;
        }

        audioSource.Stop();
    }

    private IEnumerator FadeMusicIn(AudioSource audioSource, float time)
    {
        audioSource.volume = 0;
        audioSource.Play();

        float timer = 0;

        while (timer < time)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / time);

            timer += Time.deltaTime;

            yield return null;
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

    public void ReadNote(GameObject gameObject)
    {
        if (notesRead.Contains(gameObject))
            return;

        notesRead.Add(gameObject);

        if(notesRead.Count >= noteCount)
            Achievement.Instance.AchievemnetGet(noteAchievement);
    }
}
