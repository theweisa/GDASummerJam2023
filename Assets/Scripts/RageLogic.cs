using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

public class RageLogic : UnitySingleton<RageLogic>
{
    public GameObject rage;
    public Slider rageMeter;
    public float progress = 0;
    public float rageSpeed = 0.2f;
    [SerializeField]
    private UnityEvent OnCompleted;
    [SerializeField]
    private UnityEvent<float> OnProgress;
    private EventInstance rageSound;
    
    private void Start()
    {
        rageSound = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.TensionRiser);
        rageSound.start();
    }

    public void AddRage(float progress)
    {
        AddRage(progress, rageSpeed);
    }

    public void AddRage(float progress, float speed)
    {
        StartCoroutine(AnimateProgress(progress, speed));
    }

    private IEnumerator AnimateProgress(float progress, float speed)
    {
        float time = 0;
        float initialProgress = rageMeter.value;
        float newProgress = initialProgress + progress;
        Debug.Log($"add {progress} rage");
        LeanTween.value(rageMeter.gameObject, (float val)=>{ rageMeter.value = val; }, initialProgress, newProgress, speed).setEaseOutExpo();

        yield return null;
        /*while (time < 1){
            rageMeter.value = Mathf.Lerp(initialProgress, progress, time);
            time += Time.deltaTime * speed;
            
            OnProgress?.Invoke(rageMeter.value);
            yield return null;
        }*/
    }

    private void Update()
    {
        rageSound.setParameterByName("Rage", rageMeter.value/100);
    }
}