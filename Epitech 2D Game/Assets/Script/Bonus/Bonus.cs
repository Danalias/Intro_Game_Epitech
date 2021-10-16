using UnityEngine;

public class Bonus : MonoBehaviour
{
    public string bonusType  = "";

    public AudioClip heartSound;

    public AudioClip swordSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11) {
            GiveBonus(other);
            Destroy(gameObject);
        }
    }

    private void GiveBonus(Collider2D other)
    {
        switch (bonusType) {
            case "Heart":
            AudioManager.instance.PlaySoundAt(heartSound, transform.position);
                other.SendMessage("AddHeart");
                break;
            case "Sword":
            AudioManager.instance.PlaySoundAt(swordSound, transform.position);
                other.SendMessage("AddSword");
                break;
            default:
                Debug.Log(bonusType + " is en unvalid bonus");
                break;
        }
    }
}
