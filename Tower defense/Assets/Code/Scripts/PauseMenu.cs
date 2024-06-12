using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    public Button pauseButton;    
    public Button resumeButton;    
    public Button muteButton;      
    public Button sfxMuteButton;   
    public Button continueButton;  
    public Button mainMenuButton;  

    private bool isGamePaused = false;
    private bool isMuted = false;
    private bool isSfxMuted = false;

    void Start()
    {
        pauseButton.onClick.AddListener(TogglePauseMenu);
        muteButton.onClick.AddListener(ToggleMute);
        sfxMuteButton.onClick.AddListener(ToggleSfxMute);
        continueButton.onClick.AddListener(ResumeGame); 
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void TogglePauseMenu()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }

    void ToggleSfxMute()
    {
        isSfxMuted = !isSfxMuted;
        AudioManager.Instance.MuteSFX(isSfxMuted);
    }

    void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }
}
