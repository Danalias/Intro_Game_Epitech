using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11) {
            other.SendMessage("LoseWithDeath");
        }
    }
}
