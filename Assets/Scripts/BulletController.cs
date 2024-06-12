using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    int PocetZivotu = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string otherTag = collision.gameObject.tag;

        if (gameObject.CompareTag("PlayerBullet") && otherTag == "EnemyBullet")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Debug.Log("PlayerBullet collided with EnemyBullet: Both destroyed");
        }
        else if (gameObject.CompareTag("EnemyBullet") && otherTag == "PlayerBullet")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Debug.Log("EnemyBullet collided with PlayerBullet: Both destroyed");
        }
        else if ((gameObject.CompareTag("PlayerBullet") || gameObject.CompareTag("EnemyBullet")) && !(otherTag == "PlayerBullet" || otherTag == "EnemyBullet"))
        {
            Destroy(gameObject); 
        }
    }
}