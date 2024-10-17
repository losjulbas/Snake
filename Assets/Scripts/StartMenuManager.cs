using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    public GameObject leaderboardScreen;
    public GameObject startMenuScreen;
    public GameObject creditsScreen;

    public TMP_Text highScoreText;
    public TMP_Text firstScoreText;
    public TMP_Text secondScoreText;
    public TMP_Text thirdScoreText;

    void Awake()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        firstScoreText.text = "1ST: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        secondScoreText.text = "2ST: " + PlayerPrefs.GetInt("HighScore2", 0).ToString();
        thirdScoreText.text = "3ST: " + PlayerPrefs.GetInt("HighScore3", 0).ToString();
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("Snake");  // Make sure to use your actual gameplay scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LeaderBoardButton()
    {
        leaderboardScreen.SetActive(true);
        startMenuScreen.SetActive(false);
    }

    public void BackToMainMenu()
    {
        leaderboardScreen.SetActive(false);
        startMenuScreen.SetActive(true);
    }

    public void CreditsButton()
    {
        creditsScreen.SetActive(true);
        startMenuScreen.SetActive(false);
    }

    public void BackToMainMenuFromCredits()
    {
        creditsScreen.SetActive(false);
        startMenuScreen.SetActive(true);
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

}
