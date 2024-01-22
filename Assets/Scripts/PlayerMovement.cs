using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    Canvas PauseMenu;


    public GameObject prefabStrely;
    public Transform vystrelovaciBod;
    [SerializeField]
    public float rychlostStrely;
    [SerializeField]
    public float delayMeziStrelami = 0.5f; // delka delay v sekundach

    private bool muzeStrelit = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            LookAtMouse();
            Move();


            if (Input.GetMouseButtonDown(0) && muzeStrelit)
            {
                StartCoroutine(StrelbaWithDelay());
            }
        }
    }

    private void LookAtMouse()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = mousePos - new Vector2(transform.position.x, transform.position.y);

    }

    private void Move()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = input.normalized * playerSpeed;
    }

    private IEnumerator StrelbaWithDelay()
    {
        muzeStrelit = false; 

        if (prefabStrely != null && vystrelovaciBod != null)
        {
            GameObject novaStrela = Instantiate(prefabStrely, vystrelovaciBod.position, Quaternion.identity);
            novaStrela.transform.up = transform.up;
            Rigidbody2D rbStrely = novaStrela.GetComponent<Rigidbody2D>();

            if (rbStrely != null)
            {
                rbStrely.velocity = novaStrela.transform.up * rychlostStrely;
            }
        }

        yield return new WaitForSeconds(delayMeziStrelami); 

        muzeStrelit = true; 


    }
}