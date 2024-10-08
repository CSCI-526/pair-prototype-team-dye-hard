using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public int scoreCount = 100;
    public int rewardScore = 10;
    public TextMeshProUGUI scoreText;

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScoreUI()
    {
        scoreText.text = scoreCount.ToString();
    }

    public void ModifyScore(int count)
    {
        scoreCount = Math.Max(0, scoreCount + count);
        UpdateScoreUI();

        if (scoreCount >= levelManager.levelMilestones[levelManager.currentLevel])
        {
            levelManager.LevelUp();
        }
    }
}
