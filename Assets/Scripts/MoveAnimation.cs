using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
public class MoveAnimation : MonoBehaviour
{
    public float rotationDegree = 13f;
    public float rotationOffset = 3f;
    public float rotateTimer;
    public bool moving = false;
    private Vector3 initScale;
    private EventInstance walkingSFX;
    
    public void Awake() {
        initScale = transform.localScale;
    }
    public void Move() {
        if (moving) return;
        walkingSFX = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.Walking);
        walkingSFX.start();
        moving = true;
        Turn();
    }
    public void Stop() {
        if (!moving) return;
        walkingSFX.stop(STOP_MODE.ALLOWFADEOUT);
        walkingSFX.release();
        moving = false;
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, initScale, rotateTimer*0.3f).setEaseOutQuad();
        LeanTween.moveLocalY(gameObject, 0f, rotateTimer*0.3f);
        LeanTween.rotateLocal(gameObject, Vector3.zero, rotateTimer*0.3f).setEaseOutQuad();
    }

    public void Turn(int right=1) {
        //LeanTween.value(gameObject, (float val)=>{transform.localScale=new Vector3(initScale.x,val,initScale.z);},transform.localScale.y,initScale.y*0.85f, 0.1f).setLoopPingPong(2).setEaseOutExpo();
        LeanTween.scaleY(gameObject, initScale.y*0.85f, rotateTimer*0.3f).setLoopPingPong(1);
        LeanTween.moveLocalY(gameObject, 0.2f, rotateTimer*0.4f).setLoopPingPong(1);
        //LeanTween.rotateLocal(gameObject, new Vector3(0f,0f,rotationDegree*right+Random.Range(-rotationOffset, rotationOffset)), rotateTimer).setEaseOutQuad().setOnComplete(()=>Turn(-right));
        LeanTween.rotateZ(gameObject, rotationDegree*right+Random.Range(-rotationOffset, rotationOffset), rotateTimer).setEaseOutQuad().setOnComplete(()=>Turn(-right));
    }

    public void Flip() {
        LeanTween.value(gameObject, (float val)=>{transform.localScale=new Vector3(val, transform.localScale.y, transform.localScale.z);}, initScale.x, 0f, 0.1f).setEaseInQuad().setLoopPingPong(1);
        /*LeanTween.scaleX(gameObject, 0f, 0.15f).setEaseInQuad().setOnComplete(()=>{
            LeanTween.scaleX(gameObject, initScale.x, 0.035f).setEaseInQuad();
        });*/
    }
}
