using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerExperience : MonoBehaviour
{
    public int experience = 0;
    public int playerLevel = 1;
    public int experienceCap;

    public static event Action<int> OnLevelUp;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }
    public List<LevelRange> levelRanges;

    private void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    public void AddExperience(GameObject collector, int amount)
    {
        experience += amount;

        string playerName = collector.name;
        Debug.Log($"Exp Collected by {playerName}! EXP level: {playerLevel}");

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (experience >= experienceCap)
        {
            experience -= experienceCap;
            playerLevel++;

            int capIncrease = 0;
            foreach (var range in levelRanges)
            {
                if (playerLevel >= range.startLevel && playerLevel <= range.endLevel)
                {
                    capIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += capIncrease;
            OnLevelUp?.Invoke(playerLevel);
        }
    }
}
