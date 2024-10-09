using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject leaderboardScreen;
    public VisualEffect fireworksVFXRight;
    public VisualEffect fireworksVFXLeft;

    public int snakeScore = 0;
    public int wormScore = 0;

    public TMP_Text snakeScoreText;
    public TMP_Text wormScoreText;
    public TMP_Text winnerText;
    public TMP_Text highScoreText;
    public TMP_Text firstScoreText;
    public TMP_Text secondScoreText;
    public TMP_Text thirdScoreText;

    public bool headToHeadCollision = false;
    public bool snakeWonCollision = false;
    public bool wormWonCollision = false;

    void Awake()
    {
        gameOverScreen.SetActive(false);
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        firstScoreText.text = "1ST: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        if (fireworksVFXRight != null)
        {
            fireworksVFXRight.Stop();  // Ensure it doesn't play on start
        }
        if (fireworksVFXLeft != null)
        {
            fireworksVFXLeft.Stop();  // Ensure it doesn't play on start
        }
    }

    public void UpdateSnakeScoreText()
    {
        snakeScoreText.text = "Snake: " + snakeScore.ToString();  // Use snakeScore here
    }

    public void UpdateWormScoreText()
    {
        wormScoreText.text = "Worm: " + wormScore.ToString();  // Use wormScore here
    }

    public void GameOver()
    {
        if (headToHeadCollision)
        {
            winnerText.text = "ITS A TIE!";
        }
        else if (snakeWonCollision)
        {
            winnerText.text = "SNAKE WON!";
        }
        else if (wormWonCollision)
        {
            winnerText.text = "WORM WON!";
        }

        // Calculate final score based on individual scores if needed
        if (snakeScore > PlayerPrefs.GetInt("HighScore1"))
        {
            gameOverScreen.SetActive(true);
            HighScore(snakeScore);  // Pass snakeScore or wormScore depending on who wins
            fireworksVFXRight.gameObject.SetActive(true);
            fireworksVFXRight.Play();
            fireworksVFXLeft.gameObject.SetActive(true);
            fireworksVFXLeft.Play();
        }
        else
        {
            gameOverScreen.SetActive(true);  // Show game over screen
            HighScore(snakeScore);  // Example, adjust based on who wins
        }
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

    public void LeaderBoardButton()
    {
        leaderboardScreen.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    public void BackToMainMenu()
    {
        leaderboardScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void HighScore(int playerScore)
    {
        // Get current high scores
        int highScore1 = PlayerPrefs.GetInt("HighScore1", 0);
        int highScore2 = PlayerPrefs.GetInt("HighScore2", 0);
        int highScore3 = PlayerPrefs.GetInt("HighScore3", 0);

        // Check if the new score is greater than HighScore1
        if (playerScore > highScore1)
        {
            // Shift down the scores
            PlayerPrefs.SetInt("HighScore3", highScore2); // Move 2nd to 3rd
            PlayerPrefs.SetInt("HighScore2", highScore1); // Move 1st to 2nd
            PlayerPrefs.SetInt("HighScore1", playerScore); // Set new 1st place
        }
        // Check if the new score is greater than HighScore2 but less than HighScore1
        else if (playerScore > highScore2 && playerScore < highScore1)
        {
            // Shift down HighScore2 to HighScore3
            PlayerPrefs.SetInt("HighScore3", highScore2); // Move 2nd to 3rd
            PlayerPrefs.SetInt("HighScore2", playerScore); // Set new 2nd place
        }
        // Check if the new score is greater than HighScore3 but less than HighScore2
        else if (playerScore > highScore3 && playerScore < highScore2)
        {
            PlayerPrefs.SetInt("HighScore3", playerScore); // Set new 3rd place
        }

        // Save PlayerPrefs
        PlayerPrefs.Save();

        // Update the text display
        firstScoreText.text = "1ST: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        secondScoreText.text = "2ND: " + PlayerPrefs.GetInt("HighScore2", 0).ToString();
        thirdScoreText.text = "3RD: " + PlayerPrefs.GetInt("HighScore3", 0).ToString();
    }

    public void ClearDataButton()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All saved data has been cleared.");
    }
}

