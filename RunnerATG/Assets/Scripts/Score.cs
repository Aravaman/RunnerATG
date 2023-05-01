using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text recordText;
    [SerializeField] private TMP_Text scorFinalText;

    [SerializeField]
    float speedBust;

    private float score;
    int highScore;
    bool startScore;

    void Start()
    {
        startScore = false;
    }

    void Update()
    {
        if (startScore)
        {
            speedBust = 0.1f * Time.deltaTime;
            score += Time.deltaTime + speedBust;
            highScore = (int)score;
            scoreText.text = highScore.ToString();

            if (PlayerPrefs.GetInt("score") <= highScore)
                PlayerPrefs.SetInt("score", highScore);

            scorFinalText.text = highScore.ToString();
        }

        recordText.text = PlayerPrefs.GetInt("score").ToString();
    }

    public void StartLevle()
    {
        startScore = true;
    }
}
