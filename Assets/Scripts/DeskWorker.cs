using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DeskWorker : NPC
{
    public List<string> initialScript = new List<string>();
    public List<string> waitScript = new List<string>();
    public List<string> rageScript = new List<string>();

    public override void Start()
    {
        base.Start();
        script = initialScript;
    }

    void Update()
    {
        if(RageLogic.Instance.fullRage)
        {
            script = new List<string>(){"I'm sorry, you're boned"};
        }
    }
}
