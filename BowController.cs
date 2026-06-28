using UnityEngine;
using Unity.Cinemachine; // Unity 6 ke Cinemachine cameras ko control karne ke liye laazmi hai

public class BowController : MonoBehaviour
{
    public Animator anim;
    public GameObject arrowPrefab;    
    public Transform arrowSpawnPoint; 
    
    [Header("UI Elements")]
    public GameObject crosshairUI; 

    [Header("Camera Zoom Effect")]
    public CinemachineCamera aimCameraObject; // Ab hum direct Cinemachine Camera component lenge!

    private bool isAiming = false;

    void Update()
    {
        // 1. RIGHT CLICK dabatay hi sab se pehle back se teer nikalne ka trigger chalay
        if (Input.GetMouseButtonDown(1)) 
        {
            if (anim != null) anim.SetTrigger("PullFromB");
        }

        // 2. RIGHT CLICK DABA KAR RAKHNA HAI (Hold): Character aim state mein rahay ga
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
            if (anim != null) anim.SetBool("IsAiming", true);
            
            // Crosshair nishana show kar dein
            if (crosshairUI != null) crosshairUI.SetActive(true);

            // Unity 6 Priority Switch: Aim Camera ki priority up kar dein
            if (aimCameraObject != null) 
            {
                aimCameraObject.Priority = 15; 
            }

            // 3. SHOOT: Aim ke dauran LEFT CLICK dabane par shot chalay
            if (Input.GetMouseButtonDown(0)) 
            {
                if (anim != null) anim.SetTrigger("ReleaseArrow");
                ShootArrow();
            }
        }
        // 4. RIGHT CLICK CHHORTAY HI: Aiming, Crosshair aur Zoom sab khatam
        else
        {
            if (isAiming)
            {
                isAiming = false;
                if (anim != null) anim.SetBool("IsAiming", false);
                
                // Crosshair nishana chhupa dein
                if (crosshairUI != null) crosshairUI.SetActive(false);

                // Unity 6 Priority Switch: Aim Camera ki priority wapas kam kar dein (Normal camera auto chal parega)
                if (aimCameraObject != null) 
                {
                    aimCameraObject.Priority = 5; 
                }
            }
        }
    }

    void ShootArrow()
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            // SCREEN CENTER RAYCAST LOGIC: Screen ke center (crosshair) se laser guzaarna
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            
            Vector3 targetPoint;

            // Agar samne dushman ya terrain ho, toh teer exact us hit point par jaye
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                // Agar samne bilkul khali aasmaan hai, toh 100 meters door hawa mein point lock kare
                targetPoint = ray.GetPoint(100);
            }

            // Kaman se le kar us targeted point tak ka direction rukh calculate karna
            Vector3 direction = (targetPoint - arrowSpawnPoint.position).normalized;
            Quaternion shootRotation = Quaternion.LookRotation(direction);

            // Teer ko paida karna jo perfect crosshair par hit karega
            Instantiate(arrowPrefab, arrowSpawnPoint.position, shootRotation);
        }
    }
}