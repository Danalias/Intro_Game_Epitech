using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11) {
            transform.parent.SendMessage("SwitchPattern", Pattern.CHASE);
        }
    }
}
