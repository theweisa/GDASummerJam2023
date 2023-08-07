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

    }

    public override IEnumerator ShowTextbox() {
        switch (GameManager.Instance.gameState) {
            case GameState.Start: {
                StartCoroutine(GameManager.Instance.StartCinematicEdges());
                script = initialScript;
                break;
            }
            case GameState.Wait: {
                script = waitScript;
                break;
            }
            case GameState.FeelingRage: {
                script = rageScript;
                break;
            }
            default: 
                break;
        }
        StartCoroutine(base.ShowTextbox());
        PlayerManager.Instance.cameraPosition.position = transform.position;
        yield return null;
    }

    public override IEnumerator TextboxFinished()
    {
        yield return base.TextboxFinished();
        switch (GameManager.Instance.gameState) {
            case GameState.Start: {
                yield return GameManager.Instance.EndCinematicEdges();
                PlayerManager.Instance.controller.ResumePlayer();
                QueueNumber.Instance.StartQueue();
                GameManager.Instance.gameState = GameState.Wait;
                break;
            }
            case GameState.Wait: {
                break;
            }
            case GameState.FeelingRage: {
                // do some shit
                break;
            }
            default:
                break;
        }
    }
}
