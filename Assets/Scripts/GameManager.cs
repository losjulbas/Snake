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
    public TMP_Text firstScoreText;
    public TMP_Text secondScoreText;
    public TMP_Text thirdScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameOverScreen.SetActive(false);
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        firstScoreText.text = "1ST: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);  // Show game over screen
        int finalScore = score;
        finalScoreText.text = "YOUR SCORE: " + finalScore.ToString();
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
        if (PlayerPrefs.HasKey("HighScore1"))
        {
            if (score > PlayerPrefs.GetInt("HighScore1"))
            {
                //set new highscore
                PlayerPrefs.SetInt("HighScore1", score);
                PlayerPrefs.Save();  // Save the high score
            }
        }
        else
        {
            // if there is no highscore...
            PlayerPrefs.SetInt("HighScore1", score);
            PlayerPrefs.Save();  // Save the high score
        }
        firstScoreText.text = "1ST: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();

        // is it new high score?
        if (PlayerPrefs.HasKey("HighScore2"))
        {
            if (score > PlayerPrefs.GetInt("HighScore2") && score < PlayerPrefs.GetInt("HighScore1"))
            {
                //set new highscore
                PlayerPrefs.SetInt("HighScore2", score);
                PlayerPrefs.Save();  // Save the high score
            }
        }
        else
        {
            // if there is no highscore...
            PlayerPrefs.SetInt("HighScore2", score);
            PlayerPrefs.Save();  // Save the high score
        }
        secondScoreText.text = "2ND: " + PlayerPrefs.GetInt("HighScore2", 0).ToString();

        // is it new high score?
        if (PlayerPrefs.HasKey("HighScore2"))
        {
            if (score > PlayerPrefs.GetInt("HighScore3") && score < PlayerPrefs.GetInt("HighScore2"))
            {
                //set new highscore
                PlayerPrefs.SetInt("HighScore3", score);
                PlayerPrefs.Save();  // Save the high score
            }
        }
        else
        {
            // if there is no highscore...
            PlayerPrefs.SetInt("HighScore3", score);
            PlayerPrefs.Save();  // Save the high score
        }
        thirdScoreText.text = "3RD: " + PlayerPrefs.GetInt("HighScore3", 0).ToString();
    }



    public void ClearDataButton()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All saved data has been cleared.");
    }

}
