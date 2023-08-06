using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        while (time < 1){
            rageMeter.value = Mathf.Lerp(initialProgress, progress, time);
            time += Time.deltaTime * speed;
            
            OnProgress?.Invoke(rageMeter.value);
            yield return null;
        }
    }
}
