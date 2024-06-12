using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RukojmiController : MonoBehaviour
{
    [SerializeField]
    Canvas DeathMenu;
    [SerializeField]
    TextMeshProUGUI MainText;
    [SerializeField]
    Slider slider;

    bool playerInRange = false;
    GameObject player;
    bool sliderActivated = false;
    Coroutine fillSliderCoroutine;

    ///////
    public static int zachranenychRukojmi = 0;
    ///////

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Klávesa E zmáèknuta");
            fillSliderCoroutine = StartCoroutine(ActivateSlider());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Jsi v collideru");
            playerInRange = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Nejsi v collideru");
            playerInRange = false;
            player = null;

            if (fillSliderCoroutine != null)
            {
                Debug.Log("Opustil jsi v prùbìhu");
                StopCoroutine(fillSliderCoroutine);
                slider.value = 0f;
                slider.gameObject.SetActive(false);
                sliderActivated = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            DeathMenu.enabled = true;
            MainText.fontSize = 50;
            MainText.text = "YOU KILLED THE HOSTAGE";
        }
    }
    IEnumerator ActivateSlider()
    {
        slider.gameObject.SetActive(true);
        slider.value = 0f;
        sliderActivated = true;

        // Naplnìní slideru
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime / 2.5f;
            slider.value = timer;
            yield return null;
        }

        Destroy(gameObject);
        zachranenychRukojmi++;
    }
}
