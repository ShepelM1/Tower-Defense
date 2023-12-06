using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionMenu()
    {
        if (mainMenu.activeInHierarchy)
        {
            mainMenu.SetActive(false);
            optionMenu.SetActive(true);
        }
        else
        {
            optionMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("The exit was completed successfully");
    }
}
