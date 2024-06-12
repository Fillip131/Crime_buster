using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    [SerializeField]
    public GameObject deathMenu;
    int sceneIndex;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

    }
    // Update is called once per frame
    void Update()
    {
    }

    public void ButtonBackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        deathMenu.SetActive(false);
    }
    public void ButtonRestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
        deathMenu.SetActive(false);
    }
}
