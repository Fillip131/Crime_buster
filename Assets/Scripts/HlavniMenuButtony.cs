using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HlavniMenuButtony : MonoBehaviour
{

    [SerializeField]
    Canvas canvasMenu;
    [SerializeField]
    Canvas canvasSelectLevel;


    public void Start()
    {
        canvasSelectLevel.enabled = false;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SelectLevel()
    {
        canvasMenu.enabled = false;
        canvasSelectLevel.enabled = true;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SelectLevelButton1()
    {
        SceneManager.LoadScene(1);
    }
    public void SelectLevelButton2()
    {
        SceneManager.LoadScene(2);
    }
    public void SelectLevelButton3()
    {
        SceneManager.LoadScene(3);
    }
    public void SelectLevelButton4()
    {
        SceneManager.LoadScene(4);
    }
    public void SelectLevelButtonBack()
    {
        canvasMenu.enabled = true;
        canvasSelectLevel.enabled = false;
    }
}

