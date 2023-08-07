using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { Start, Wait, FeelingRage, PanicStart, Rage, PostRage };
public class GameManager : UnitySingleton<GameManager>
{
    public Transform cinematicBars;
    public GameState gameState;
    public RectTransform logo;
    private EventInstance rageMusic;
    private EventInstance rumble;
    public TMP_Text tutorialText;
    public GameObject explosion;
    public RectTransform gameOver;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
        //StartCoroutine(PostRage());
        //PlayerManager.Instance.controller.StopPlayer();
        //StartCoroutine(FeelingRagePhase());
    }

    // Update is called once per frame
    void Update()
    {
        if (RageLogic.Instance.rageMeter.value >= 100f && gameState == GameState.Wait && PlayerManager.Instance.controller.canInteract) {
            StartCoroutine(FeelingRagePhase());
        } 
    }

    public IEnumerator StartGame() {
        PlayerManager.Instance.cameraPosition.transform.position = Vector3.zero;
        logo.gameObject.SetActive(true);
        RageLogic.Instance.gameObject.SetActive(false);
        PlayerManager.Instance.controller.StopPlayer();
        yield return new WaitUntil(()=>Input.GetMouseButtonDown(0));
        RuntimeManager.PlayOneShot(FMODEventReferences.instance.MouseClick);
        LeanTween.moveY(logo.gameObject, logo.transform.position.y+100f, 1.5f).setEaseInBack().setOnComplete(()=>logo.gameObject.SetActive(false));
        LeanTween.value(PlayerManager.Instance.cameraPosition.gameObject, (float val) => {
            PlayerManager.Instance.cameraPosition.transform.localPosition = new Vector2(0, val);
        }, PlayerManager.Instance.cameraPosition.transform.localPosition.y, 0, 2.5f).setEaseInOutCirc();
        yield return new WaitForSeconds(2.5f);
        PlayerManager.Instance.controller.canInteract = true;
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
        QueueNumber.Instance.SetRectPos(new Vector2(0.5f, 0f));
        QueueNumber.Instance.text.text = "numberidfk";

        StartCoroutine(EndCinematicEdges(0.2f, LeanTweenType.easeOutQuart, 1.13f));
        CameraManager.Instance.Zoom(prevZoom, 0.3f);
        LeanTween.value(PlayerManager.Instance.gameObject, (float val)=>{
            PlayerManager.Instance.controller.SetRed(val);
        }, 100f, 0f, 0.3f);
        LeanTween.value(PlayerManager.Instance.gameObject, (float val)=>{
            RageLogic.Instance.rageMeter.value = val;
        }, 100f, 0f, 0.5f).setEaseOutQuart();
        CameraManager.Instance.StopShake(0f);
        
        yield return new WaitForSeconds(2f);
        yield return QueueNumber.Instance.AnimateFadeOut();
        yield return EndCinematicEdges(0.2f);
        PlayerManager.Instance.controller.ResumePlayer();
    }

    public IEnumerator EnterRagePhase() {
        PlayerManager.Instance.controller.StopPlayer();
        gameState = GameState.Rage;
        yield return new WaitForSeconds(2f);
        RageLogic.Instance.rageMeter.value = 100f;
        LeanTween.value(RageLogic.Instance.fill.gameObject, (float val)=>{RageLogic.Instance.fill.localScale = new Vector2(val,1);}, 0, 5, 1.5f);
        yield return new WaitForSeconds(0.7f);
        RuntimeManager.StudioSystem.setParameterByName("RageEnd", 1);
        yield return new WaitForSeconds(2f);
        AudioManager.instance.CleanUp(true);
        rumble = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.Rumble);
        rumble.start();
        // cock shotgun sound idk
        PlayerManager.Instance.controller.fists.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        PlayerManager.Instance.controller.rage = true;
        PlayerManager.Instance.controller.canPunch = true;
        PlayerManager.Instance.controller.punchForce = 0f;
        tutorialText.text = "Click to punch";
        tutorialText.gameObject.SetActive(true);
        // TODO: tutorial to punch the guy
    }

    public IEnumerator StartPanic() {
        gameState = GameState.PanicStart;
        rumble.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        PlayerManager.Instance.controller.StopPlayer();
        PlayerManager.Instance.controller.rage = false;
        StartCoroutine(StartCinematicEdges());
        PlayerManager.Instance.cameraPosition.transform.position = Vector3.zero;
        yield return new WaitForSeconds(3.5f);
        RuntimeManager.PlayOneShot(FMODEventReferences.instance.Panic);
        EnvironmentManager.Instance.UnFreezeAllObjects();
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("NPC");
        foreach(GameObject go in gos){
            go.SendMessage("StartPanic");
        }
        gameState = GameState.Rage;
        yield return new WaitForSeconds(2f);
        rageMusic = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.RageModeMusic);
        rageMusic.start();
        PlayerManager.Instance.cameraPosition.transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(2f);
        PlayerManager.Instance.controller.rage = true;
        PlayerManager.Instance.controller.ResumePlayer();
        yield return EndCinematicEdges();
    }

    public IEnumerator PostRage() {
        gameState = GameState.PostRage;
        PlayerManager.Instance.controller.StopPlayer();
        rageMusic.setParameterByName("Music_End", 1);
        StartCoroutine(StartCinematicEdges());//2f, LeanTweenType.easeOutQuart, 1.4f
        yield return new WaitForSeconds(2f);
        rageMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        yield return QueueNumber.Instance.AnimatePopIn();
        QueueNumber.Instance.SetRectPos(new Vector2(0.5f, 0f));
        QueueNumber.Instance.text.text = "fucking dumbass";
        yield return new WaitForSeconds(2f);
        yield return QueueNumber.Instance.AnimateFadeOut();
        yield return new WaitForSeconds(2f);
        var explode = Instantiate(explosion, PlayerManager.Instance.transform.position, Quaternion.identity);
        CameraManager.Instance.StartShake(50f, 0.7f, 20f);
        PlayerManager.Instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        Destroy(explode);
        Vector3 prevPos = gameOver.localPosition;
        gameOver.localPosition = new Vector2(prevPos.x, prevPos.y+350f);
        gameOver.gameObject.SetActive(true);
        LeanTween.moveLocalY(gameOver.gameObject, prevPos.y, 0.75f).setEaseOutElastic();
        yield return new WaitUntil(()=>Input.GetMouseButtonDown(0));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ///StartCoroutine(EndCinematicEdges());
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
