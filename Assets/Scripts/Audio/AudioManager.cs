using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set; }
  
    private List<EventInstance> eventInstances;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Attempted to create second Audio Manager instance.");
        }
        instance = this;
        eventInstances = new List<EventInstance>();
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void CleanUp(bool SoftStop) //false for hard stop, true for soft stop
    {
        if(SoftStop)
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstance.release();
                eventInstances = new List<EventInstance>();
            }            
        }
        else
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
                eventInstances = new List<EventInstance>();
            }
        }

    }
    
    private void OnDestroy()
    {
        CleanUp(true);
    }
}
