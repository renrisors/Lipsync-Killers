using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGStop : MonoBehaviour
{
    public GameObject[] bg;
    
    // Start is called before the first frame update
    void Start()
    {
       if (bg == null)
        {
            bg = GameObject.FindGameObjectsWithTag("Note");
        }
        
       foreach (GameObject bg in bg)
        {
            bg.GetComponent<AudioSource>().Stop();
        }

    }
}
