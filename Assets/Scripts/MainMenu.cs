using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject musicSource;

    public Slider musicSlider;
    public TextMeshProUGUI musicSliderText;
    
    public Slider sfxSlider;
    public TextMeshProUGUI sfxSliderText;

    private void Awake()
    {
        // If the player has just started the game for the first time
        if (!PlayerPrefs.HasKey("MusicToggledOn"))
        {
            // Set the toggle variables to "On" (1)
            PlayerPrefs.SetInt("MusicToggledOn", 1);
            PlayerPrefs.SetInt("SfxToggledOn", 1);

            // Set the float variables to full values (1.0f)
            PlayerPrefs.SetFloat("MusicVolumeValue", 1f);
            PlayerPrefs.SetFloat("SfxVolumeValue", 1f);
        }

        // Set the music volume and play
        musicSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolumeValue", 1f);
        if (PlayerPrefs.GetInt("MusicToggledOn") == 1)
        {
            musicSource.GetComponent<AudioSource>().Play();
            EnableSlider(musicSlider);
        }
        else
        {
            musicSliderText.text = "Music: Off";
            DisableSlider(musicSlider);
        }
        
        // If sfx is off
        print(PlayerPrefs.GetInt("SfxToggledOn"));
        if (PlayerPrefs.GetInt("SfxToggledOn") == 1)
        {
            sfxSliderText.text = "Sfx: " + PlayerPrefs.GetFloat("SfxVolumeValue");
            EnableSlider(sfxSlider);
        }
        else
        {
            sfxSliderText.text = "Sfx: Off";
            DisableSlider(sfxSlider);
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        LoadValues();
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay Test");
    }

    public void MusicVolumeSlider(float volume)
    {
        var musicVolumeValue = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolumeValue", musicVolumeValue);

        musicSliderText.text =  musicSlider.value == 0 || PlayerPrefs.GetInt("MusicToggledOn") == 0 ? 
            "Music: Off" : 
            "Music: " + musicSlider.value.ToString("0.0");
        LoadValues();
    }
    
    public void SfxVolumeSlider(float volume)
    {
        var sfxVolumeValue = sfxSlider.value;
        PlayerPrefs.SetFloat("SfxVolumeValue", sfxVolumeValue);
        
        sfxSliderText.text =  sfxSlider.value == 0 || PlayerPrefs.GetInt("SfxToggledOn") == 0 ? 
            "Sfx: Off" : 
            "Sfx: " + sfxSlider.value.ToString("0.0");
        LoadValues();
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("MusicToggledOn", 1) == 1 && PlayerPrefs.GetFloat("MusicVolumeValue") != 0.0f)
        {
            PlayerPrefs.SetInt("MusicToggledOn", 0);
            musicSource.GetComponent<AudioSource>().Pause();
            musicSliderText.text = "Music: Off";
            DisableSlider(musicSlider);
        }
        else
        {
            PlayerPrefs.SetInt("MusicToggledOn", 1);
            musicSource.GetComponent<AudioSource>().Play();
            musicSliderText.text = musicSlider.value == 0 ? "Music: Off" : "Music: " + musicSlider.value.ToString("0.0");
            EnableSlider(musicSlider);
        }
        // PlayerPrefs.Save();
    }
    
    public void ToggleSfx()
    {
        if (PlayerPrefs.GetInt("SfxToggledOn", 1) == 1 && PlayerPrefs.GetFloat("SfxVolumeValue") != 0.0f)
        {
            PlayerPrefs.SetInt("SfxToggledOn", 0);
            sfxSliderText.text = "Sfx: Off";
            DisableSlider(sfxSlider);
        }
        else
        {
            PlayerPrefs.SetInt("SfxToggledOn", 1);
            sfxSliderText.text =  sfxSlider.value == 0 ? "Sfx: Off" : "Sfx: " + sfxSlider.value.ToString("0.0");
            EnableSlider(sfxSlider);
        }
    }

    private static void DisableSlider(Component slider)
    {
        // Set the opacity to 50%
        var sliderColor = slider.GetComponentInChildren<Image>().color;
        sliderColor.a = 0.5f;
        slider.GetComponentInChildren<Image>().color = sliderColor;
    }

    private static void EnableSlider(Component slider)
    {
        // Set the opacity to 100%
        var sliderColor = slider.GetComponentInChildren<Image>().color;
        sliderColor.a = 1f;
        slider.GetComponentInChildren<Image>().color = sliderColor;
    }

    private void LoadValues()
    {
        var musicVolumeValue = PlayerPrefs.GetFloat("MusicVolumeValue");
        musicSlider.value = musicVolumeValue;
        musicSource.GetComponent<AudioSource>().volume = musicVolumeValue;

        var sfxVolumeValue = PlayerPrefs.GetFloat("SfxVolumeValue");
        sfxSlider.value = sfxVolumeValue;
    }
}

