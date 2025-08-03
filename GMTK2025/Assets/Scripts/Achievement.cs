using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private int achievementsCount;

    private List<AchievementSO> achievementList;
    private Dictionary<AchievementSO, bool> achievementCompletions;

    public int AchievementsCount => achievementsCount;

    public int AchievementsGot => achievementCompletions.Count;

    private void Awake()
    {
        Instance = this;

        achievementList = new List<AchievementSO>();
        achievementCompletions = new Dictionary<AchievementSO, bool>();
    }

    private IEnumerator Start()
    {
        int i = 0;

        while (true)
        {


            while (achievementList.Count > i)
            {
                var achievement = achievementList[i];

                i++;

                nameText.text = achievement.name;

                movementAnimator.Move();

                yield return new WaitForSeconds(1f);

                yield return new WaitForSeconds(showTime);

                movementAnimator.Move();

                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
    }

    public void AchievemnetGet(AchievementSO achievement)
    {
        if (achievementCompletions.ContainsKey(achievement))
            return;

        achievementList.Add(achievement);
        achievementCompletions.Add(achievement, true);
    }
}
