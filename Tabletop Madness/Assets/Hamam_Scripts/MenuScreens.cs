using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreens : MonoBehaviour
{
    public GameObject mainMenuPanel, pauseMenuPanel, winPanel, losePanel;

    public void SetMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
    }

    public void SetPauseMenuPanel()
    {
        pauseMenuPanel.SetActive(true);
    }

    public void SetWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void SetLosePanel()
    {
        losePanel.SetActive(true);
    }

    public void ResetPanels()
    {
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);        
    }
}
