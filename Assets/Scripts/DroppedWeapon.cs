using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    GameObject player;

    [SerializeField] GameObject PlayerPistol;
    [SerializeField] GameObject CameraPlayerPistol;

    [SerializeField] GameObject PlayerAssaultRiffle;
    [SerializeField] GameObject CameraPlayerAssaultRiffle;

    [SerializeField] GameObject PlayerShotgun;
    [SerializeField] GameObject CameraPlayerShotgun;

    private bool isInDroppedWeaponCollider = false;
    private WeaponInfo currentWeaponInfo;
    private Vector2 playerPosition;
    public GameObject temp;
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (isInDroppedWeaponCollider && Input.GetKeyDown(KeyCode.G))
        {
            PickupWeapon();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("DroppedWeapon"))
        {
            isInDroppedWeaponCollider = true;
            currentWeaponInfo = other.GetComponent<WeaponInfo>();
            temp = other.gameObject;

            if (player != null)
            {
                playerPosition = player.transform.position;
                Debug.Log("Entered DroppedWeapon collider");

                if (currentWeaponInfo != null)
                {
                    Debug.Log("WeaponInfo found: " + currentWeaponInfo.weaponName);

                }
                else
                {
                    Debug.LogWarning("No WeaponInfo component found on the collided object.");
                }
            }
            else
            {
                Debug.LogWarning("Player object not found.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DroppedWeapon"))
        {
            isInDroppedWeaponCollider = false;
            currentWeaponInfo = null;  
            Debug.Log("Exited DroppedWeapon collider");
        }
    }

    private void PickupWeapon()
    {
        if (currentWeaponInfo != null)
        {
            string weaponName = currentWeaponInfo.weaponName;
            Debug.Log("Picked up weapon: " + weaponName);

            switch (weaponName)
            {
                case "Pistol":
                    PlayerPistol.transform.position = playerPosition;
                    PlayerShotgun.SetActive(false);
                    CameraPlayerShotgun.SetActive(false);
                    PlayerAssaultRiffle.SetActive(false);
                    CameraPlayerAssaultRiffle.SetActive(false);

                    PlayerPistol.SetActive(true);
                    CameraPlayerPistol.SetActive(true);
                    Debug.Log("Equipped Pistol");

                    
                    break;
                case "Assault Riffle":
                    PlayerAssaultRiffle.transform.position = playerPosition;
                    PlayerPistol.SetActive(false);
                    CameraPlayerPistol.SetActive(false);
                    PlayerShotgun.SetActive(false);
                    CameraPlayerShotgun.SetActive(false);

                    PlayerAssaultRiffle.SetActive(true);
                    CameraPlayerAssaultRiffle.SetActive(true);
                    Debug.Log("Equipped Assault Riffle");
                    break;
                case "Shotgun":
                    PlayerShotgun.transform.position = playerPosition;
                    PlayerPistol.SetActive(false);
                    CameraPlayerPistol.SetActive(false);
                    PlayerAssaultRiffle.SetActive(false);
                    CameraPlayerAssaultRiffle.SetActive(false);

                    PlayerShotgun.SetActive(true);
                    CameraPlayerShotgun.SetActive(true);
                    Debug.Log("Equipped Shotgun");
                    break;
                default:
                    Debug.LogWarning("Unknown weapon type: " + weaponName);
                    break;
            }
            
        }
        else
        {
            Debug.LogWarning("currentWeaponInfo is null, cannot pick up weapon.");
        }
        Destroy(temp);
    }

    public class WeaponInfo : MonoBehaviour
    {
        public string weaponName;
    }
}
