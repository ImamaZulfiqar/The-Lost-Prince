using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // Yahan Erika Archer set rahegi
    public float mouseSensitivity = 200f;
    public float distanceFromPlayer = 4f; // Player se doori
    public float heightOffset = 2f;       // Player se unchai (Upar se view theek karne ke liye)

    private float xRotation = 15f; // Shuru ka thoda sa jhookao
    private float yRotation = 0f;

    void Start()
    {
        // Mouse cursor ko game screen ke andar lock karne ke liye
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Mouse ka input lena
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        
        // Camera ko boht zyada upar ya bilkul zameen ke andar janay se rokna
        xRotation = Mathf.Clamp(xRotation, -10f, 60f); 

        // 2. Camera ki Rotation calculation
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // 3. Camera ki Position set karna (Player ke hamesha peeche aur upar rahega)
        Vector3 targetPosition = player.position - (rotation * Vector3.forward * distanceFromPlayer) + (Vector3.up * heightOffset);
        
        transform.position = targetPosition;

        // 4. Camera ka rukh player ki taraf rakhna
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}