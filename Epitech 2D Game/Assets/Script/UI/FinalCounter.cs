using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalCounter : MonoBehaviour
{
    public Text txt;
    void Start()
    {
        txt.text = ((int)Timer.globalTime).ToString() + " seconds";
    }
}
