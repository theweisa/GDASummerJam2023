using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;

public class DeskWorker : NPC
{
    public List<string> initialScript = new List<string>();
    public List<string> waitScript = new List<string>();
    public List<string> rageScript = new List<string>();

    public override void Start()
    {
        base.Start();
        //hitstun = 1f;
        hitstun = 0.3f;
        speakRange = 6f;
        script = initialScript;
    }

    public override IEnumerator OnHit()
    {
        yield return base.OnHit();
        //hitstun = 0.1f;
        GameManager.Instance.tutorialText.gameObject.SetActive(false);
        GetComponent<Collider2D>().isTrigger = false;
        PlayerManager.Instance.controller.punchForce = PlayerManager.Instance.controller.basePunchForce;
    }

    public override void OnMouseOver()
    {
        if (GameManager.Instance.gameState != GameState.Start && GameManager.Instance.gameState != GameState.FeelingRage && GameManager.Instance.gameState != GameState.Wait) return;
        bool inDistance = Vector3.Distance(PlayerManager.Instance.transform.position, transform.position) <= speakRange;
        if (interactPrompt != null && script.Count != 0 && inDistance && PlayerManager.Instance.controller.canInteract) {
            interactPrompt.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0) 
            && PlayerManager.Instance.controller.canInteract
            && script.Count != 0
            && inDistance)
        {
            if (textBox == null)
            {
                StartCoroutine(ShowTextbox());
                GameManager.Instance.tutorialText.gameObject.SetActive(false);
                //textBox.Activate();
            }
        }
    }

    public override void Update() {
        base.Update();
        if (GameManager.Instance.followDeskWorker) {
            PlayerManager.Instance.cameraPosition.position = transform.position;
        }
    }

    public override IEnumerator SporadicMovement() {
        yield return null;
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
                RageLogic.Instance.gameObject.SetActive(true);
                Vector3 prevPos = RageLogic.Instance.transform.localPosition;
                RageLogic.Instance.transform.localPosition = new Vector2(RageLogic.Instance.transform.localPosition.x, RageLogic.Instance.transform.localPosition.y+75f);
                LeanTween.moveLocalY(RageLogic.Instance.gameObject, prevPos.y, 1.4f).setEaseOutCirc();
                GameManager.Instance.ticketText.text = "Ticket #F7158";
                break;
            }
            case GameState.Wait: {
                break;
            }
            case GameState.FeelingRage: {
                StartCoroutine(GameManager.Instance.EnterRagePhase());
                GameManager.Instance.ticketText.text = "Ticket #X9999999...";
                break;
            }
            default:
                break;
        }
    }
}
