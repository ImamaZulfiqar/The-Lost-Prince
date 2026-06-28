using UnityEngine;

public class ArrowFly : MonoBehaviour
{
    public float arrowSpeed = 40f;
    public float lifeTime = 5f;

    void Start()
    {
        // Teer ko seedha aage ki taraf bhagana
        GetComponent<Rigidbody>().linearVelocity = transform.forward * arrowSpeed;
        Destroy(gameObject, lifeTime); // 5 second baad khud tabah ho jaye
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Dushman ko teer laga!");
            Destroy(gameObject); // Takrate hi teer gayb
        }
    }
}