using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;
    [SerializeField] Canvas PauseMenu;
    //[SerializeField] GameObject picturePistol;
    //[SerializeField] GameObject pictureAssaultRiffle;
    //[SerializeField] GameObject pictureShotgun;
    [SerializeField] Canvas DeathMenu;
    [SerializeField] Animator animator;
    public GameObject prefabStrely;
    public Transform[] vystrelovaciBodShotgun; // Pole transformaèních bodù pro brokovnici
    public Transform vystrelovaciBod; // Transformaèní bod pro ostatní zbranì
    [SerializeField] public float rychlostStrely;
    [SerializeField] public ScreenShake screenShake;
    private bool muzeStrelit = true;

    [SerializeField] public string GunType;
    int Naboje;
    int Maxnaboje;
    int PocetZasobniku;
    float delayMeziStrelami;
    float delayMeziReloadem;
    public Image healthBar;
    ////////////////////////////////////
    public int PocetZivotu = 100;
    ////////////////////////////////////

    public TextMeshProUGUI Guninfo;
    public TextMeshProUGUI Zivoty;

    public Slider reloadTimerSlider;
    private bool isReloading = false;

    void Start()
    {
        healthBar = GameObject.Find("Health").GetComponent<Image>();
        rb = GetComponent<Rigidbody2D>();
        switch (GunType)
        {
            case "Pistol":
                PocetZasobniku = 2;
                Naboje = 7;
                delayMeziStrelami = 0.5f;
                delayMeziReloadem = 2f;
                break;

            case "Assault Riffle":
                PocetZasobniku = 2;
                Naboje = 10;
                delayMeziStrelami = 0.2f;
                delayMeziReloadem = 3f;
                break;

            case "Shotgun":
                PocetZasobniku = 2;
                Naboje = 5;
                delayMeziStrelami = 2f;
                delayMeziReloadem = 3f;
                break;
        }
        Maxnaboje = Naboje;
        if (Zivoty != null)
        {
            Zivoty.text = "Health " + PocetZivotu.ToString();
        }
        Guninfo.text = GunType + " " + PocetZasobniku + "/" + Naboje;
        DeathMenu.enabled = false;

        reloadTimerSlider.minValue = 0f;
        reloadTimerSlider.maxValue = delayMeziReloadem;
        reloadTimerSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        healthBar.fillAmount = PocetZivotu / 100f;
        if (Zivoty != null)
            Zivoty.text = PocetZivotu.ToString() + " / 100";

        if (PauseMenu.gameObject.activeSelf || Time.timeScale == 0f)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            LookAtMouse();
            Move();

            if (Input.GetMouseButton(0) && muzeStrelit == true && Naboje > 0)
            {
                if (GunType == "Shotgun")
                {
                    StartCoroutine(ShotgunStrelbaWithDelay());
                }
                else
                {
                    StartCoroutine(StrelbaWithDelay());
                }
            }

            if (isReloading)
            {
                reloadTimerSlider.value += Time.deltaTime;
                if (reloadTimerSlider.value >= delayMeziReloadem)
                {
                    isReloading = false;
                    reloadTimerSlider.value = 0f;
                    Naboje = Maxnaboje;
                    PocetZasobniku--;
                    Guninfo.text = GunType + " " + PocetZasobniku + "/" + Naboje;
                    reloadTimerSlider.gameObject.SetActive(false);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R) && PocetZasobniku > 0 && Naboje < Maxnaboje)
        {
            StartCoroutine(ReloadWeapon());
        }

        CheckDeath();
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = mousePos - new Vector2(transform.position.x, transform.position.y);
    }

    private void Move()
    {
        var PohybHrace = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (PohybHrace.magnitude > 0.1f)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        rb.velocity = PohybHrace.normalized * playerSpeed;
    }

    private IEnumerator StrelbaWithDelay()
    {
        muzeStrelit = false;

        GameObject NovaStrela = Instantiate(prefabStrely, vystrelovaciBod.position, Quaternion.identity);
        NovaStrela.transform.up = transform.up;
        animator.SetTrigger("Shoot");

        Rigidbody2D rbStrely = NovaStrela.GetComponent<Rigidbody2D>();
        rbStrely.velocity = NovaStrela.transform.up * rychlostStrely;
        screenShake.ShakeScreen();

        Naboje--;
        Guninfo.text = GunType + " " + PocetZasobniku + "/" + Naboje;

        if (Naboje == 0 && PocetZasobniku > 0)
        {
            isReloading = true;
            reloadTimerSlider.value = 0f;
            reloadTimerSlider.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayMeziReloadem);
        }

        yield return new WaitForSeconds(delayMeziStrelami);

        muzeStrelit = true;
    }

    private IEnumerator ShotgunStrelbaWithDelay()
    {
        muzeStrelit = false;

        float[] angles = { 15f, 7.5f, 0f, -7.5f, -15f };

        for (int i = 0; i < vystrelovaciBodShotgun.Length; i++)
        {
            Transform bod = vystrelovaciBodShotgun[i];

            GameObject NovaStrela = Instantiate(prefabStrely, bod.position, Quaternion.identity);
            NovaStrela.transform.up = transform.up;

            NovaStrela.transform.Rotate(Vector3.forward, angles[i]);

            animator.SetTrigger("Shoot");

            Rigidbody2D rbStrely = NovaStrela.GetComponent<Rigidbody2D>();
            rbStrely.velocity = NovaStrela.transform.up * rychlostStrely;
            screenShake.ShakeScreen();

            Destroy(NovaStrela, 0.1f);
        }

        Naboje--;
        Guninfo.text = GunType + " " + PocetZasobniku + "/" + Naboje;

        if (Naboje == 0 && PocetZasobniku > 0)
        {
            isReloading = true;
            reloadTimerSlider.value = 0f;
            reloadTimerSlider.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayMeziReloadem);
        }

        yield return new WaitForSeconds(delayMeziStrelami);

        muzeStrelit = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (PocetZivotu > 0)
            {
                Debug.Log("Ty kurvo a máš jí");
                PocetZivotu -= 10;
            }
        }
    }
    void CheckDeath()
    {
        if (PocetZivotu <= 0)
        {
            DeathMenu.enabled = true;
        }
    }
    public void AddAmmo(int ammoToAdd)
    {
        if (ammoToAdd > 0)
        {
            Naboje += ammoToAdd;
            if (Naboje > Maxnaboje)
            {
                Naboje = Maxnaboje;
            }
            Guninfo.text = GunType + " " + PocetZasobniku + "/" + Naboje;
        }
    }
    public void AddZasobnik(int zasobnikToAdd)
    {
        if (PocetZasobniku < 2)
        {
            PocetZasobniku += zasobnikToAdd;
            if (PocetZasobniku > 2)
            {
                PocetZasobniku = 2;
            }
            Guninfo.text = GunType + " " + PocetZasobniku + "/" + Naboje;
        }
    }
    public void AddHealth(int HealthToAdd)
    {
        if (PocetZivotu < 100)
        {
            PocetZivotu += HealthToAdd;
            if (PocetZivotu > 100)
            {
                PocetZivotu = 100;
            }
            Zivoty.text = "Health " + PocetZivotu.ToString();
        }
    }
    public void SetHealth(int health)
    {
        PocetZivotu = health;
        if (Zivoty != null)
        {
            Zivoty.text = "Health " + PocetZivotu.ToString();
        }
    }
    private IEnumerator ReloadWeapon()
    {
        muzeStrelit = false;

        reloadTimerSlider.value = 0f;
        reloadTimerSlider.maxValue = delayMeziReloadem;
        reloadTimerSlider.gameObject.SetActive(true);

        float reloadTimer = 0f; // Novì pøidaný kód pro sledování èasu reloadu

        while (reloadTimer < delayMeziReloadem)
        {
            reloadTimer += Time.deltaTime; // Aktualizovat èas reloadu
            reloadTimerSlider.value = reloadTimer; // Aktualizovat hodnotu slideru
            yield return null;
        }

        Naboje = Maxnaboje;
        PocetZasobniku--;
        Guninfo.text = GunType + " " + PocetZasobniku + "/" + Naboje;

        reloadTimerSlider.gameObject.SetActive(false);

        muzeStrelit = true;
    }
}
