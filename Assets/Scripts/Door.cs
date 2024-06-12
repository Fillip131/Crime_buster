using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer black;
    [SerializeField] GameObject door;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(FindObjectOfType<PlayerMovement>().transform.position, transform.position) < 5 && Input.GetKey(KeyCode.F))
        {
            black.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            door.SetActive(false);
        }
    }
}
