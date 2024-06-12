using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using static DroppedWeapon;

public class EnemyController : MonoBehaviour
{
    public float hp;
    public ParticleSystem blood;
    public Transform player;
    public Transform playerAssaultRiffle;
    public Transform playerShotgun;
    public float moveSpeed;
    public float ReloadMoveSpeed = 2f;
    public float DelayMeziReloadem;
    float OriginalMoveSpeed;
    public float detectionRange;
    public LayerMask obstacleMask;
    public float lookAroundInterval;
    public float lookAroundRange = 90f;
    public float lookAroundDuration = 1.5f;
    public float maxMovementDistance = 2f;
    public float playerLostTime;
    public float stoppingDistance = 0.1f; // Pøidáno: Vzdálenost, na kterou se nepøítel zastaví

    [SerializeField] string GunName;
    private int PocetNaboju;
    private int MaxPocetNaboju;
    private int PocetZasobniku;
    public Transform[] vystrelovaciBodShotgun;
    public Transform vystrelovaciBod;
    public GameObject prefabStrely;
    private bool muzeStrelit = true;
    public float delayMeziStrelami = 0.5f;
    public float rychlostStrely;

    private Rigidbody2D rb;
    private float lookTimer;
    private bool isLookingAround;
    private Vector2 randomDirection;
    private Vector2 lastKnownPlayerPosition;
    private float playerLostTimer;
    private bool hasSeenPlayer;

    public static int zabityEnemy = 0;
    [SerializeField] Animator animator;

    [SerializeField]
    public GameObject bloodPrefab;

    [SerializeField]
    GameObject Pistol;
    [SerializeField]
    GameObject Ar;
    [SerializeField]
    GameObject Shotgun;

    GameObject gundrop;

    public float directionRotation;
    private bool directionOneTime;
    public Quaternion directionHolder;
    public Quaternion startDirectionHolder;
    public float directionChangeSpeed;
    public bool hasSeen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            hp -= 25;
            blood.Play();
            Destroy(collision.gameObject);
        }
    }

    void Start()
    {
        directionChangeSpeed = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        startDirectionHolder = transform.rotation;
        lookTimer = lookAroundInterval;
        isLookingAround = false;
        randomDirection = Vector2.zero;
        lastKnownPlayerPosition = player.position;
        OriginalMoveSpeed = moveSpeed;

        playerLostTimer = playerLostTime;
        hasSeenPlayer = false;
        StartCoroutine(ChangeIdleDirection());
        SetWeaponParameters();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (hp <= 0)
        {
            GameObject droppedWeapon = Instantiate(gundrop, transform.position, Quaternion.identity);
            droppedWeapon.AddComponent<WeaponInfo>().weaponName = GunName;


            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            zabityEnemy++;
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            player = players[0].transform;
        }
        else
        {
            player = null;
        }
        if (directionOneTime)
        {
            directionHolder = Quaternion.Euler(startDirectionHolder.eulerAngles + new Vector3(0f, 0f, directionRotation));
            directionOneTime = false;
        }
     
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, distanceToPlayer, obstacleMask);
              
                if (hit.collider == null)
                {
                    hasSeen = true;
                    Vector2 direction = (player.position - transform.position).normalized;
                    rb.velocity = direction * moveSpeed;

                    transform.up = direction;

                    if (isLookingAround)
                    {
                        rb.velocity = Vector2.zero;
                        transform.rotation = Quaternion.Lerp(transform.rotation, directionHolder, Time.deltaTime * directionChangeSpeed);
                    }
                    else
                    {
                        rb.velocity = direction * moveSpeed;

                        lookTimer -= Time.deltaTime;
                        if (lookTimer <= 0f)
                        {
                            isLookingAround = true;
                            randomDirection = Quaternion.Euler(0, 0, Random.Range(-lookAroundRange / 2f, lookAroundRange / 2f)) * transform.up;
                            lookTimer = lookAroundInterval;
                            Invoke("StopLookingAround", lookAroundDuration);
                        }

                        if (muzeStrelit && PocetZasobniku > 0 && PocetNaboju > 0)
                        {
                            if (GunName == "Shotgun")
                            {
                                StartCoroutine(ShotgunStrelbaWithDelay());
                                animator.SetTrigger("Shoot");
                            }
                            else
                            {
                                StartCoroutine(StrelbaWithDelay());
                                animator.SetTrigger("Shoot");
                            }

                        }
                    }

                    lastKnownPlayerPosition = player.position;

                    playerLostTimer = playerLostTime;
                    hasSeenPlayer = true;
                }
                else
                {
                    if(hasSeen==false)
                    transform.rotation = Quaternion.Lerp(transform.rotation, directionHolder, Time.deltaTime * directionChangeSpeed);
                    rb.velocity = Vector2.zero;
                    if (hasSeenPlayer && playerLostTimer <= 0)
                    {
                        MoveToLastKnownPosition();
                    }
                    else
                    {
                        playerLostTimer -= Time.deltaTime;
                    }
                }
            }
            else
            {
               
                rb.velocity = Vector2.zero;
                if (hasSeenPlayer && playerLostTimer <= 0)
                {
                  
                    MoveToLastKnownPosition();
                }
                else
                {
                    playerLostTimer -= Time.deltaTime;
                }
            }
        }

        if (rb.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }
    IEnumerator ChangeIdleDirection() {
        directionRotation = Random.Range(-40, 40);
  
        yield return new WaitForSeconds(Random.Range(2, 3));
        directionOneTime = true;
        StartCoroutine(ChangeIdleDirection());
    }

    void MoveToLastKnownPosition()
    {
        float distanceToLastKnownPosition = Vector2.Distance(transform.position, lastKnownPlayerPosition);
        if (distanceToLastKnownPosition > stoppingDistance)
        {
            Vector2 directionToLastKnownPosition = (lastKnownPlayerPosition - (Vector2)transform.position).normalized;
            rb.velocity = directionToLastKnownPosition * moveSpeed;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, directionHolder, Time.deltaTime * directionChangeSpeed);
            rb.velocity = Vector2.zero;
        }
    }

    void StopLookingAround()
    {
        isLookingAround = false;
        randomDirection = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (isLookingAround)
        {
            rb.velocity = randomDirection * moveSpeed;
        }
    }

    private IEnumerator StrelbaWithDelay()
    {
        muzeStrelit = false;

        GameObject NovaStrela = Instantiate(prefabStrely, vystrelovaciBod.position, Quaternion.identity);
        NovaStrela.transform.up = transform.up;
        Rigidbody2D rbStrely = NovaStrela.GetComponent<Rigidbody2D>();
        rbStrely.velocity = NovaStrela.transform.up * rychlostStrely;

        PocetNaboju--;
        yield return new WaitForSeconds(delayMeziStrelami);

        if (PocetNaboju == 0)
        {
            moveSpeed = ReloadMoveSpeed;
            yield return new WaitForSeconds(DelayMeziReloadem);
            if (PocetZasobniku > 0)
            {
                PocetZasobniku--;
                PocetNaboju = MaxPocetNaboju;
                moveSpeed = OriginalMoveSpeed;
            }
        }

        muzeStrelit = true;
    }

    private IEnumerator ShotgunStrelbaWithDelay()
    {
        muzeStrelit = false;

        float[] angles = { 15f, 7.5f, 0f, -7.5f, -15f };

        for (int i = 0; i < vystrelovaciBodShotgun.Length; i++)
        {
            GameObject NovaStrela = Instantiate(prefabStrely, vystrelovaciBodShotgun[i].position, Quaternion.identity);
            NovaStrela.transform.up = transform.up;

            NovaStrela.transform.Rotate(Vector3.forward, angles[i]);

            Rigidbody2D rbStrely = NovaStrela.GetComponent<Rigidbody2D>();
            rbStrely.velocity = NovaStrela.transform.up * rychlostStrely;

            Destroy(NovaStrela, 0.1f);

        }
        PocetNaboju--;
        yield return new WaitForSeconds(delayMeziStrelami);

        if (PocetNaboju == 0)
        {
            moveSpeed = ReloadMoveSpeed;
            yield return new WaitForSeconds(DelayMeziReloadem);
            if (PocetZasobniku > 0)
            {
                PocetZasobniku--;
                PocetNaboju = MaxPocetNaboju;
                moveSpeed = OriginalMoveSpeed;
            }
        }

        muzeStrelit = true;
    }

    private void SetWeaponParameters()
    {
        if (GunName == "Pistol")
        {
            PocetZasobniku = 2;
            MaxPocetNaboju = 5;
            PocetNaboju = MaxPocetNaboju;
            DelayMeziReloadem = 2f;

            gundrop = Pistol;
        }
        else if (GunName == "Assault Riffle")
        {
            PocetZasobniku = 2;
            MaxPocetNaboju = 10;
            PocetNaboju = MaxPocetNaboju;
            DelayMeziReloadem = 3f;

            gundrop = Ar;
        }
        else if (GunName == "Shotgun")
        {
            PocetZasobniku = 2;
            MaxPocetNaboju = 3;
            PocetNaboju = MaxPocetNaboju;
            DelayMeziReloadem = 5f;

            gundrop = Shotgun;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject blood in GameObject.FindGameObjectsWithTag("Blood"))
        {
            Destroy(blood);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
