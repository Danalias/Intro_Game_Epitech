using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    private Transform Hearts;
    private Transform Swords;
    private Player player;

    void Awake()
    {
        Hearts = transform.Find("Hearts");
        Swords = transform.Find("Swords");
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        CheckLife();
        CheckSpecial();
    }

    private void CheckLife()
    {
        int i;

        for (i = 0; i < player.nbLife && i < 3; i++)
            Hearts.GetChild(i).gameObject.SetActive(true);
        
        for (; i < 3; i++)
            Hearts.GetChild(i).gameObject.SetActive(false);
    }

    private void CheckSpecial()
    {
        int i;

        for (i = 0; i < player.nbSpecial && i < 3; i++)
            Swords.GetChild(i).gameObject.SetActive(true);
        
        for (; i < 3; i++)
            Swords.GetChild(i).gameObject.SetActive(false);
    }
}
