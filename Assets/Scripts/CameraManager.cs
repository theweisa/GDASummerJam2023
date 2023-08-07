using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : UnitySingleton<CameraManager>
{
    public CinemachineVirtualCamera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShake(float str=1f, float dur=1f, float freq=1f, bool perma=false) {
        playerCamera.GetComponent<CameraShake>().StartShake(str,dur,freq,perma);
    }
    public void StopShake(float dur=0.75f) {
        playerCamera.GetComponent<CameraShake>().StopShake(dur);
    }
    public void SetShakeStrength(float str) {
        playerCamera.GetComponent<CameraShake>().strength = str;
    }
    public void SetShakeFrequency(float freq) {
        playerCamera.GetComponent<CameraShake>().frequency = freq;
    }

    public virtual IEnumerator PanToTarget(Transform target, float dur) {
        Vector2 initEase = CameraManager.Instance.GetEase();
        CameraManager.Instance.SetEase(1.5f);
        CameraManager.Instance.playerCamera.m_Follow = target;
        yield return new WaitForSeconds(dur);
        CameraManager.Instance.SetEase(initEase);
    }

    public void Zoom(float newVal, float dur=1f, LeanTweenType ease=LeanTweenType.easeOutQuart) {
        LeanTween.value(gameObject, (float val) => {
            playerCamera.m_Lens.OrthographicSize = val;
        }, playerCamera.m_Lens.OrthographicSize, newVal, dur).setEase(ease);
    }

    public void SetEase(float x, float y) {
        CinemachineTransposer trans = playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        trans.m_XDamping = x;
        trans.m_YDamping = y;
    }
    public void SetEase(Vector2 ease) {
        SetEase(ease.x, ease.y);
    }
    public void SetEase(float val) {
        SetEase(val, val);
    }
    public Vector2 GetEase() {
        CinemachineTransposer trans = playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        return (new Vector2(trans.m_XDamping, trans.m_YDamping));
    }
}
