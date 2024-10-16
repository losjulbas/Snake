using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject leaderboardScreen;
    public GameObject creditsScreen;
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

    private bool isGameOver = false;

    public Image logoImage;
    public Sprite normalLogo;
    public Sprite snakeGreyLogo;
    public Sprite wormGreyLogo;

    public AudioClip inGameMusic;  // In-game music clip
    public AudioClip gameOverMusic;  // Game over music clip
    public AudioClip fireWorks;
    private AudioSource audioSource;  // Audio source on the camera or other object
    private AudioSource gameManagerAudioSource;

    void Awake()
    {
        gameOverScreen.SetActive(false);
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        firstScoreText.text = "1ST: " + PlayerPrefs.GetInt("HighScore1", 0).ToString();
        secondScoreText.text = "2ST: " + PlayerPrefs.GetInt("HighScore2", 0).ToString();
        thirdScoreText.text = "3ST: " + PlayerPrefs.GetInt("HighScore3", 0).ToString();

        if (fireworksVFXRight != null)
        {
            fireworksVFXRight.Stop();  // Ensure it doesn't play on start
        }
        if (fireworksVFXLeft != null)
        {
            fireworksVFXLeft.Stop();  // Ensure it doesn't play on start
        }

        audioSource = Camera.main.GetComponent<AudioSource>(); // Assuming the AudioSource is on the main camera
        gameManagerAudioSource = GetComponent<AudioSource>();
        PlayInGameMusic();  // Start with the in-game music
    }


    // Method to switch to in-game music
    public void PlayInGameMusic()
    {
        if (audioSource.clip != inGameMusic)  // Only change if not already playing
        {
            audioSource.clip = inGameMusic;
            audioSource.Play();
        }
    }

    public void PlayFireworks()
    {
        Debug.Log("Playing fireworks sound!");
        gameManagerAudioSource.clip = fireWorks;  // Use the gameManagerAudioSource instead
        gameManagerAudioSource.Play();
    }

    // Method to switch to game-over music
    public void PlayGameOverMusic()
    {
        if (audioSource.clip != gameOverMusic)  // Only change if not already playing
        {
            audioSource.clip = gameOverMusic;
            audioSource.Play();
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
        Time.timeScale = 0;
        // Check if the game is already over to avoid multiple executions
        if (isGameOver) return;

        // Set game over flag to true so the method doesn't execute again
        isGameOver = true;

        PlayGameOverMusic();  // Play game-over music when the game ends

        if (headToHeadCollision)
        {
            winnerText.text = "ITS A TIE!";
            logoImage.sprite = normalLogo;
            winnerText.color = Color.white;
        }
        else if (snakeWonCollision)
        {
            winnerText.text = "SNAKE WON!";
            fireworksVFXRight.gameObject.SetActive(true);
            fireworksVFXRight.Play();
            fireworksVFXLeft.gameObject.SetActive(true);
            fireworksVFXLeft.Play();
            logoImage.sprite = wormGreyLogo;
            winnerText.color = Color.green;
            PlayFireworks();
        }
        else if (wormWonCollision)
        {
            winnerText.text = "WORM WON!";
            fireworksVFXRight.gameObject.SetActive(true);
            fireworksVFXRight.Play();
            fireworksVFXLeft.gameObject.SetActive(true);
            fireworksVFXLeft.Play();
            logoImage.sprite = snakeGreyLogo;
            winnerText.color = new Color(199f / 255f, 124f / 255f, 44f / 255f);
            PlayFireworks();
        }

        // Show game over screen and handle high score logic
        gameOverScreen.SetActive(true);
        HighScore(snakeScore);
        HighScore(wormScore);
    }

    public void RestartButton()
    {
        // Reset game over flag on restart
        isGameOver = false;
        gameOverScreen.SetActive(false);   // Hide game over screen if it's shown
        PlayInGameMusic();  // Switch back to in-game music when restarting

        Debug.Log("Restart button clicked!");
        Time.timeScale = 1.0f;  // Reset the game speed if you have paused it
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
    }

    void Update()
    {
        if (isGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            isGameOver = false;
            Debug.Log("Restart button clicked!");
            PlayInGameMusic();  // Switch back to in-game music when restarting
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
            Time.timeScale = 1.0f;
        }

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

    public void CreditsButton()
    {
        creditsScreen.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    public void BackToMainMenuFromCredits()
    {
        creditsScreen.SetActive(false);
        gameOverScreen.SetActive(true); ;
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