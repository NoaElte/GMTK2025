using System.Collections;
using TMPro;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    public static Achievement Instance;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private MovementAnimator movementAnimator;
    [SerializeField]
    private float showTime;

    [HideInInspector]
    public bool IsFinished;

    private void Awake()
    {
        Instance = this;
    }

    public void AchievemnetGet(AchievementSO achievement)
    {
        if (IsFinished)
            return;

        nameText.text = achievement.name;

        IsFinished = true;

        StartCoroutine(ShowAchievement());
    }

    private IEnumerator ShowAchievement()
    {
        movementAnimator.Move();

        yield return new WaitForSeconds(showTime);

        movementAnimator.Move();
    }
}
