using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class EnvironmentAudio : MonoBehaviour
{
    private EventInstance ambience;
    // Start is called before the first frame update
    void Start()
    {
        ambience = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.Ambience);
        ambience.start();
        Debug.Log("Started Ambience");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
