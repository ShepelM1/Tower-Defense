using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionMenu;

    public void BackToMainMenu()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


}
