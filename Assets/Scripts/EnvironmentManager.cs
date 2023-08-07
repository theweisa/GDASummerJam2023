using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class EnvironmentManager : UnitySingleton<EnvironmentManager>
{
    private EventInstance ambience;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(AudioManager.instance);
        Debug.Log(FMODEventReferences.instance.Ambience);
        ambience = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.Ambience);
        //ambience = FMODUnity.RuntimeManager.CreateInstance("event:/World_Ambience");
        ambience.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnFreezeAllObjects() {
        UnFreezeObject(transform);
    }

    void UnFreezeObject(Transform obj) {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        if (obj.childCount <= 0) return;
        foreach (Transform ob in obj.transform) {
            UnFreezeObject(ob);
        }
        return;
    }
}
