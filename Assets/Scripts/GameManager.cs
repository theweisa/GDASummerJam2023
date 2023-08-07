using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum GameState { Start, Wait, FeelingRage, Rage, PostRage };
public class GameManager : UnitySingleton<GameManager>
{
    public Transform cinematicBars;
    public GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerManager.Instance.controller.StopPlayer();
        PlayerManager.Instance.controller.canInteract = true;
        //StartCoroutine(EnterRagePhase());
    }

    // Update is called once per frame
    void Update()
    {
        if (RageLogic.Instance.rageMeter.value >= 100f && gameState == GameState.Wait) {
            StartCoroutine(FeelingRagePhase());
        } 
    }

    public IEnumerator FeelingRagePhase() {
        gameState = GameState.FeelingRage;
        StartCoroutine(StartCinematicEdges(5f, LeanTweenType.linear));
        QueueNumber.Instance.StopQueue();
        PlayerManager.Instance.controller.StopPlayer();
        PlayerManager.Instance.controller.makeRed = false;
        CameraManager.Instance.StartShake(1f, 1, 1, true);
        LeanTween.value(CameraManager.Instance.playerCamera.gameObject, (float val)=>{
            CameraManager.Instance.SetShakeStrength(val);
        }, 0f, 3f, 5f);
        LeanTween.value(CameraManager.Instance.playerCamera.gameObject, (float val)=>{
            CameraManager.Instance.SetShakeFrequency(val);
        }, 0.5f, 1.75f, 5f);
        float prevZoom = CameraManager.Instance.playerCamera.m_Lens.OrthographicSize;
        CameraManager.Instance.Zoom(4f, 5f, LeanTweenType.linear);
        yield return new WaitForSeconds(5f);
        yield return QueueNumber.Instance.AnimatePopIn();

        StartCoroutine(EndCinematicEdges(0.2f, LeanTweenType.easeOutQuart, 1.13f));
        CameraManager.Instance.Zoom(prevZoom, 0.3f);
        LeanTween.value(PlayerManager.Instance.gameObject, (float val)=>{
            PlayerManager.Instance.controller.SetRed(val);
        }, 100f, 0f, 0.3f);
        LeanTween.value(PlayerManager.Instance.gameObject, (float val)=>{
            RageLogic.Instance.rageMeter.value = val;
        }, 100f, 0f, 0.5f).setEaseOutQuart();
        CameraManager.Instance.StopShake(0f);
        QueueNumber.Instance.SetRectPos(new Vector2(0.5f, 0f));
        QueueNumber.Instance.text.text = "numberidfk";
        
        yield return new WaitForSeconds(2f);
        yield return QueueNumber.Instance.AnimateFadeOut();
        yield return EndCinematicEdges(0.2f);
        PlayerManager.Instance.controller.ResumePlayer();
    }

    public IEnumerator EnterRagePhase() {
        yield return null;
        gameState = GameState.Rage;
    }

    public IEnumerator StartPanic() {
        gameState = GameState.Rage;
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("NPC");

        foreach(GameObject go in gos){
            go.SendMessage("StartPanic");
        }
        yield return true;
    }

    public IEnumerator StartCinematicEdges(float dur=2f, LeanTweenType ease=LeanTweenType.easeOutQuart, float endScale=2f) {
        foreach (Transform child in cinematicBars.transform) {
            LeanTween.scaleY(child.gameObject, endScale, dur).setEase(ease);
        }
        yield return new WaitForSeconds(dur);
    }
    public IEnumerator EndCinematicEdges(float dur=2f, LeanTweenType ease=LeanTweenType.easeOutQuart, float endScale=1f) {
        foreach (Transform child in cinematicBars.transform) {
            LeanTween.scaleY(child.gameObject, endScale, dur).setEase(ease);
        }
        yield return new WaitForSeconds(dur);
    }
}
