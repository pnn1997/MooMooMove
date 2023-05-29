using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public HerdManager herd;
    public InvasionManager invasion;

    public TMP_Text scoreText, highScoreText, cowCountText;
    private int score, highscore;
    private float elapsedTime;
    private const int MAX_SCORE = 999999;

    private void Start()
    {
        elapsedTime = 0;
        Load();
    }

    public void Update()
    {
        scoreText.text = score.ToString("D6");
        highScoreText.text = highscore.ToString("D6");
        cowCountText.text = herd.GetHerdSize().ToString("D2");

        // Scoring is added every second for each cow still alive in the herd
        if (elapsedTime > 1.0f)
        {
            elapsedTime = 0;
            AddScore(herd.GetHerdSize() * invasion.GetDifficulty());
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }

        if (score > highscore)
        {
            highscore = score;
            Save();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("highscore", highscore);
    }

    public void Load()
    {
        highscore = PlayerPrefs.GetInt("highscore", highscore);
    }
}
