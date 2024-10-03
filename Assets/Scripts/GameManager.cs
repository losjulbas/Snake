using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverScreen;

    public int score = 0;


    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameOverScreen.SetActive(false);
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("SavedHighScore", 0).ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);  // Show game over screen
        int finalScore = score;
        finalScoreText.text = "Score: " + finalScore.ToString();
        HighScore();
    }

    public void RestartButton()
    {
        Debug.Log("Restart button clicked!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Exits play mode in the editor
#else
        Application.Quit();  // Exits the application when running as a build
#endif
    }

    public void HighScore()
    {
        // is it new high score?
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            if (score > PlayerPrefs.GetInt("SavedHighScore"))
            {
                //set new highscore
                PlayerPrefs.SetInt("SavedHighScore", score);
                PlayerPrefs.Save();  // Save the high score
            }
        }
        else
        {
            // if there is no highscore...
            PlayerPrefs.SetInt("SavedHighScore", score);
            PlayerPrefs.Save();  // Save the high score
        }
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("SavedHighScore", 0).ToString();

    }



}
