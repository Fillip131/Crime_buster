using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform hrac;

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }
}
