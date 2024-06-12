using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBoxController : MonoBehaviour
{
    int ammoToAdd;
    int zasobnikToAdd;
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
            fillSliderCoroutine = StartCoroutine(ActivateSliderAndAddAmmo());
        }
    }

    IEnumerator ActivateSliderAndAddAmmo()
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

        // Pøidání nábojù
        AddAmmoToPlayer(player);
        AddZasobniky(player);
        Destroy(gameObject);
    }

    private void AddAmmoToPlayer(GameObject player)
    {
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (playerMovement.GunType == "Pistol")
                {
                    ammoToAdd = 5;
                }
                else if (playerMovement.GunType == "Assault Riffle")
                {
                    ammoToAdd = 6;
                }
                else if (playerMovement.GunType == "Shotgun")
                {
                    ammoToAdd = 3;
                }
                Debug.Log(ammoToAdd + " Náboje pøidány");
                playerMovement.AddAmmo(ammoToAdd);
            }
        }
    }

    private void AddZasobniky(GameObject player)
    {
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (playerMovement.GunType == "Pistol")
                {
                    zasobnikToAdd = 2;
                }
                else if (playerMovement.GunType == "Assault Riffle")
                {
                    zasobnikToAdd = 2;
                }
                else if (playerMovement.GunType == "Shotgun")
                {
                    zasobnikToAdd = 2;
                }
                Debug.Log(zasobnikToAdd + " Zásobníky pøidány");
                playerMovement.AddZasobnik(zasobnikToAdd);
            }
        }
    }
}