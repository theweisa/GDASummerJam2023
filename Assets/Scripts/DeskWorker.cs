using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DeskWorker : NPC
{
    public List<string> initialScript = new List<string>();
    public List<string> waitScript = new List<string>();
    public List<string> rageScript = new List<string>();
    public int mode = 0;

    public override void Start()
    {
        base.Start();
        script = initialScript;
    }

    void Update()
    {

    }

    public override IEnumerator ShowTextbox() {
        StartCoroutine(GameManager.Instance.StartCinematicEdges());
        StartCoroutine(base.ShowTextbox());
        PlayerManager.Instance.cameraPosition.position = transform.position;
        yield return null;
    }

    public override IEnumerator TextboxFinished()
    {
        yield return base.TextboxFinished();
        if (mode == 0) {
            yield return GameManager.Instance.EndCinematicEdges();
            PlayerManager.Instance.controller.ResumePlayer();
            script = waitScript;
        }
        
    }
}
