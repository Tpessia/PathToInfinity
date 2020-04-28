using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform PlayerTransform;
    public float RestartDelay = 1.5f;
    private bool _gameEnded = false;

    void Start()
    {
        FindObjectOfType<MaxScoreUpdate>().MaxScoreText.text = MaxScore.Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTransform.position.y < -1)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (!_gameEnded)
        {
            _gameEnded = true;
            Invoke("Restart", RestartDelay);
        }

        var score = FindObjectOfType<Score>().ScoreText.text;
        if (Int32.Parse(score) > Int32.Parse(MaxScore.Load()))
        {
            MaxScore.Save(FindObjectOfType<Score>().ScoreText.text);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
