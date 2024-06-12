using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunBoxController : MonoBehaviour
{
    bool playerInRange = false;
    GameObject player;
    bool sliderActivated = false;
    Coroutine fillSliderCoroutine;

    /////////////////
    public Canvas GunPicker;
    /////////////////
    [SerializeField]
    Animator animatorAssaultRiffleKolo;
    [SerializeField]
    Animator animatorPistolKolo;
    [SerializeField]
    Animator animatorShotgunKolo;
    [SerializeField]
    Animator animatorShotgunPicture;
    [SerializeField]
    Animator animatorPistolPicture;
    [SerializeField]
    Animator animatorAssaultRifflePicture;
    string GunTypePick;
    public GameObject animObrazekPistol;
    public GameObject animObrazekShotgun;
    public GameObject animObrazekAssaultRiffle;
    public GameObject PistolObrazek;
    public GameObject ShotgunObrazek;
    public GameObject AssaultRiffleObrazek;
    int ammoToAdd;
    int zasobnikyToAdd;


    [SerializeField]
    Slider slider;




    ///////////////////////////////
    [SerializeField] GameObject PlayerPistol;
    [SerializeField] GameObject CameraPlayerPistol;

    [SerializeField] GameObject PlayerAssaultRiffle;
    [SerializeField] GameObject CameraPlayerAssaultRiffle;

    [SerializeField] GameObject PlayerShotgun;
    [SerializeField] GameObject CameraPlayerShotgun;


    private Vector2 playerPosition;
    ///////////////////////////////


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Jsi v collideru");
            playerInRange = true;
            player = collision.gameObject;
            playerPosition = player.transform.position;
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
            fillSliderCoroutine = StartCoroutine(ActivateSlider());
        }

        if (Time.timeScale == 0)
        {

            animatorAssaultRiffleKolo.Update(Time.unscaledDeltaTime);
            animatorAssaultRifflePicture.Update(Time.unscaledDeltaTime);

            animatorPistolKolo.Update(Time.unscaledDeltaTime);
            animatorPistolPicture.Update(Time.unscaledDeltaTime);

            animatorShotgunKolo.Update(Time.unscaledDeltaTime);
            animatorShotgunPicture.Update(Time.unscaledDeltaTime);

        }
    }
    private void Start()
    {
        PlayerPistol.SetActive(true);
        CameraPlayerPistol.SetActive(true);


        GunPicker.enabled = false;
        animObrazekPistol.SetActive(false);
        animObrazekShotgun.SetActive(false);
        animObrazekAssaultRiffle.SetActive(false);
        PistolObrazek.SetActive(false);
        ShotgunObrazek.SetActive(false);
        AssaultRiffleObrazek.SetActive(false);
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
            timer += Time.deltaTime / 2f;
            slider.value = timer;
            yield return null;
        }

        int randomNumber = Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                GunTypePick = "Pistol";
                animObrazekPistol.SetActive(true);
                PistolObrazek.SetActive(true);
                GunPicker.enabled = true;
                animatorPistolKolo.SetBool("IsOpened", true);
                animatorPistolPicture.SetBool("IsOpened", true);
                break;
            case 1:
                GunTypePick = "Shotgun";
                animObrazekShotgun.SetActive(true);
                ShotgunObrazek.SetActive(true);
                GunPicker.enabled = true;
                animatorShotgunKolo.SetBool("IsOpened", true);
                animatorShotgunPicture.SetBool("IsOpened", true);
                break;
            case 2:
                GunTypePick = "Assault Riffle";
                animObrazekAssaultRiffle.SetActive(true);
                AssaultRiffleObrazek.SetActive(true);
                GunPicker.enabled = true;
                animatorAssaultRiffleKolo.SetBool("IsOpened", true);
                animatorAssaultRifflePicture.SetBool("IsOpened", true);
                break;
        }
        Time.timeScale = 0f;
    }

    public void ButtonKeepGun()
    {
        // Obnovení èasu
        Time.timeScale = 1f;

        AddAmmoAndZasobnikyToPlayer(player);

        GunPicker.enabled = false;
        animatorPistolKolo.SetBool("IsOpened", false);
        animatorPistolPicture.SetBool("IsOpened", false);
        animatorShotgunKolo.SetBool("IsOpened", false);
        animatorShotgunPicture.SetBool("IsOpened", false);
        animatorAssaultRiffleKolo.SetBool("IsOpened", false);
        animatorAssaultRifflePicture.SetBool("IsOpened", false);
        Destroy(gameObject);



       
    }

    public void ButtonTakeGun()
    {
        int currentHealth = player.GetComponent<PlayerMovement>().PocetZivotu;

        // Obnovení èasu
        Time.timeScale = 1f;

        if (GunTypePick == "Pistol")
        {
            PlayerPistol.transform.position = playerPosition;
            PlayerShotgun.SetActive(false);
            CameraPlayerShotgun.gameObject.SetActive(false);
            PlayerAssaultRiffle.SetActive(false);
            CameraPlayerAssaultRiffle.gameObject.SetActive(false);

            PlayerPistol.SetActive(true);
            CameraPlayerPistol.gameObject.SetActive(true);
            Debug.Log("Pistol");

            if (player == PlayerPistol)
            {
                AddAmmoAndZasobnikyToPlayer(player);
            }

            PlayerPistol.GetComponent<PlayerMovement>().SetHealth(currentHealth);
        }
        else if (GunTypePick == "Assault Riffle")
        {
            PlayerAssaultRiffle.transform.position = playerPosition;
            PlayerPistol.SetActive(false);
            CameraPlayerPistol.SetActive(false);
            PlayerShotgun.SetActive(false);
            CameraPlayerShotgun.gameObject.SetActive(false);

            PlayerAssaultRiffle.SetActive(true);
            CameraPlayerAssaultRiffle.gameObject.SetActive(true);
            Debug.Log("AssaultRiffle");

            if (player == PlayerAssaultRiffle)
            {
                AddAmmoAndZasobnikyToPlayer(player);
            }

            PlayerAssaultRiffle.GetComponent<PlayerMovement>().SetHealth(currentHealth);
        }
        else if (GunTypePick == "Shotgun")
        {
            PlayerShotgun.transform.position = playerPosition;
            PlayerPistol.SetActive(false);
            CameraPlayerPistol.gameObject.SetActive(false);
            PlayerAssaultRiffle.SetActive(false);
            CameraPlayerAssaultRiffle.gameObject.SetActive(false);

            PlayerShotgun.SetActive(true);
            CameraPlayerShotgun.gameObject.SetActive(true);
            Debug.Log("Shotgun");

            if (player == PlayerShotgun)
            {
                AddAmmoAndZasobnikyToPlayer(player);
            }

            PlayerShotgun.GetComponent<PlayerMovement>().SetHealth(currentHealth);
        }

        GunPicker.enabled = false;
        animatorPistolKolo.SetBool("IsOpened", false);
        animatorPistolPicture.SetBool("IsOpened", false);
        animatorShotgunKolo.SetBool("IsOpened", false);
        animatorShotgunPicture.SetBool("IsOpened", false);
        animatorAssaultRiffleKolo.SetBool("IsOpened", false);
        animatorAssaultRifflePicture.SetBool("IsOpened", false);
        Destroy(gameObject);
    }

    private void AddAmmoAndZasobnikyToPlayer(GameObject player)
    {
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (playerMovement.GunType == "Pistol" && GunTypePick == "Pistol")
                {
                    ammoToAdd = 7;
                    zasobnikyToAdd = 2;
                }
                else if (playerMovement.GunType == "Assault Riffle" && GunTypePick == "Assault Riffle")
                {
                    ammoToAdd = 10;
                    zasobnikyToAdd = 2;

                }
                else if (playerMovement.GunType == "Shotgun" && GunTypePick == "Shotgun")
                {
                    ammoToAdd = 5;
                    zasobnikyToAdd = 2;

                }
                playerMovement.AddAmmo(ammoToAdd);
                playerMovement.AddZasobnik(zasobnikyToAdd);
            }
        }
    }
}