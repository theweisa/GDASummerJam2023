using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : UnitySingleton<GameManager>
{
    public Transform cinematicBars;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerManager.Instance.controller.StopPlayer();
        PlayerManager.Instance.controller.canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator EnterRagePhase() {
        StartCoroutine(StartCinematicEdges());
        PlayerManager.Instance.controller.StopPlayer();
        CameraManager.Instance.StartShake(1f, 1, 1, true);
        LeanTween.value(CameraManager.Instance.playerCamera.gameObject, (float val)=>{
            CameraManager.Instance.SetShakeStrength(val);
        }, 0f, 3f, 5f);
        yield return new WaitForSeconds(5f);
        // TBD: play cutscene showing bs idk
        CameraManager.Instance.StopShake(0f);

    }

    public IEnumerator StartCinematicEdges() {
        foreach (Transform child in cinematicBars.transform) {
            LeanTween.scaleY(child.gameObject, 2, 2f).setEase(LeanTweenType.easeOutQuart);
        }
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator EndCinematicEdges() {
        foreach (Transform child in cinematicBars.transform) {
            LeanTween.scaleY(child.gameObject, 1, 2f).setEase(LeanTweenType.easeOutQuart);
        }
        yield return new WaitForSeconds(2f);
    }
}
