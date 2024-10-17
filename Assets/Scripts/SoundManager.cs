using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Image soundOnIcon;
    [SerializeField] private Image soundOffIcon;
    [SerializeField] private Button muteButton;  // Reference to the mute button

    public AudioClip buttonClick;
    public AudioClip eatingFood;
    private AudioSource soundManageraudioSource;

    private bool muted = false;

    public void PlayButtonSound()
    {
        Debug.Log("Clicking sound!");
        soundManageraudioSource.clip = buttonClick;  // Use the gameManagerAudioSource instead
        soundManageraudioSource.Play();
    }

    public void EatingFoodSound()
    {
        Debug.Log("Food sound!");
        soundManageraudioSource.PlayOneShot(eatingFood);
    }

    void Start()
    {
        soundManageraudioSource = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        else
        {
            Load();
        }

        UpdateButtonIcon();
        AudioListener.pause = muted;

        // Assign the OnButtonPress method to the mute button on scene load
        if (muteButton != null)
        {
            muteButton.onClick.AddListener(OnButtonPress);  // Dynamically assign the click event
        }
    }

    public void OnButtonPress()
    {
        Debug.Log("Sound Button is pressed!");  // Debug log to ensure it's working
        muted = !muted;
        AudioListener.pause = muted;

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (muted)
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
        else
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}
