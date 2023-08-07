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
    public RectTransform fill;
    public float progress = 0;
    public float rageSpeed = 0.2f;
    [SerializeField]
    private UnityEvent OnCompleted;
    [SerializeField]
    private UnityEvent<float> OnProgress;
    private EventInstance rageSound;
    public bool fullRage = false;
    
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
        float initialProgress = rageMeter.value;
        float newProgress = initialProgress + progress;
        LeanTween.value(rageMeter.gameObject, (float val)=>{ rageMeter.value = val; }, initialProgress, newProgress, speed).setEaseOutExpo();
        yield return null;
    }

    private void Update()
    {
        rageSound.setParameterByName("Rage", rageMeter.value/100);
        if(rageMeter.value >= 100)
        {
            fullRage = true;
        }
    }
}
