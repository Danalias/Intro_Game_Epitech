using UnityEngine;

public class EyeAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11) {
            other.SendMessage("TakeDamage", transform.position.x < other.transform.position.x);
        }
    }
}
