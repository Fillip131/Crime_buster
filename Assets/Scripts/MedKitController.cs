using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MedKitController : MonoBehaviour
{
    [SerializeField]
    int HealthToAdd;

    bool playerInRange = false;
    GameObject player;
    bool sliderActivated = false;
    Coroutine fillSliderCoroutine;

    [SerializeField]
    Slider slider;

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

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !sliderActivated)
        {
            Debug.Log("Klávesa E zmáèknuta");
            fillSliderCoroutine = StartCoroutine(ActivateSliderAndAddHealth());
        }
    }

    IEnumerator ActivateSliderAndAddHealth()
    {
        slider.gameObject.SetActive(true);
        slider.value = 0f;
        sliderActivated = true;

        // Naplnìní slideru
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime / 2f;
            slider.value = timer;
            yield return null;
        }

        // Pøidání životù
        AddHealthToPlayer(player);
        Destroy(gameObject);
    }

    private void AddHealthToPlayer(GameObject player)
    {
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                Debug.Log(HealthToAdd + " Životy pøidány");
                playerMovement.AddHealth(HealthToAdd);
            }
        }
    }
}