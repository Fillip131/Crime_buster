using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartCount : MonoBehaviour
{
    public TextMeshProUGUI pocetRukojmich;
    public TextMeshProUGUI pocetEnemies;
    public GameObject obrazekEnemyDone; // Ujistìte se, že tyto objekty jsou pøiøazeny v editoru
    public GameObject obrazekRukojmiDone;
    public Image fadeImage; // UI Image pro fade efekt
    public float fadeDuration = 1.0f;

    int ruk;
    int ene;

    void Start()
    {
        Time.timeScale = 1f;
        UpdateCounts();
        obrazekEnemyDone.SetActive(false); // Zajistíme, že obrázky jsou na zaèátku skryté
        obrazekRukojmiDone.SetActive(false);

        // Startovní fade-in efekt
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        pocetRukojmich.text = "Hostages saved: " + RukojmiController.zachranenychRukojmi.ToString() + "/" + ruk.ToString();
        pocetEnemies.text = "Enemies killed: " + EnemyController.zabityEnemy.ToString() + "/" + ene.ToString();

        CheckEndCondition();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] bloodPrefabs = GameObject.FindGameObjectsWithTag("Blood");
        foreach (GameObject bloodPrefab in bloodPrefabs)
        {
            Destroy(bloodPrefab);
        }

        UpdateCounts();
    }

    private void UpdateCounts()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] hostages = GameObject.FindGameObjectsWithTag("Rukojmi");

        int hostageCount = hostages.Length;
        int enemyCount = enemies.Length;
        ruk = hostageCount;
        ene = enemyCount;

        RukojmiController.zachranenychRukojmi = 0;
        EnemyController.zabityEnemy = 0;

        Debug.Log("Poèet objektù s tagem 'Enemy' v této scénì: " + enemyCount);
        Debug.Log("Poèet objektù s tagem 'Rukojmi' v této scénì: " + hostageCount);
    }

    private void CheckEndCondition()
    {
        if (RukojmiController.zachranenychRukojmi >= ruk)
        {
            obrazekRukojmiDone.SetActive(true);
        }

        if (EnemyController.zabityEnemy >= ene)
        {
            obrazekEnemyDone.SetActive(true);
        }

        if (RukojmiController.zachranenychRukojmi >= ruk && EnemyController.zabityEnemy >= ene)
        {
            StartCoroutine(FadeOutAndLoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}