using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtony : MonoBehaviour
{
    [SerializeField]
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
            }
            Time.timeScale = 1f;
        } 
    }

    public void ButtonResume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void ButtonBackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}