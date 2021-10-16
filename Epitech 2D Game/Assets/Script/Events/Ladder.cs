using UnityEngine;

public class Ladder : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11) {
            Victory.isWin = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.layer == 11) {
            Victory.isWin = false;
        }
    }
}
