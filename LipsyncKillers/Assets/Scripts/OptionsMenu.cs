using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioMixer audioMixer;

    public void Start()
    {
        audioMixer.SetFloat("volumeMaster", 0.5f);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volumeMaster", volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
