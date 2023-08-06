using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEventReferences : MonoBehaviour
{
    // Start is called before the first frame update
    [field: Header("Music")]
    [field: SerializeField] public EventReference RageModeMusic {get; private set;}

    [field: Header("UI SFX")]

    [field: Header("Player SFX")] 
    [field: SerializeField] public EventReference Walking {get; private set;}
    [field: SerializeField] public EventReference PunchWhiff {get; private set;}

    [field: Header ("World SFX")]
    [field: SerializeField] public EventReference Ambience {get; private set;}
    [field: SerializeField] public EventReference TensionRiser {get; private set;}

    [field: Header ("NPC SFX")]    
    [field: SerializeField] public EventReference DialogueBlip {get; private set;}
    [field: SerializeField] public EventReference QueueNumber {get; private set;} 
    public static FMODEventReferences instance {get; private set;}
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Attempted to create second FMODEventReferences instance.");
        }
        instance = this;
    }
}
