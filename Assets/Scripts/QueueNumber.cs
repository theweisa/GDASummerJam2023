using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QueueNumber : UnitySingleton<QueueNumber>
{
    private float interval = 10f;
    private int currentNum = 0;
    public List<string> numberQueue = new List<string>();
    public TMP_Text text;
    private List<int> bag = new List<int>(){1, 2, 3};

    public Coroutine queueCountdown;
    public RectTransform boxPos;
    private Vector2 initScale;

    void Start()
    {
        gameObject.SetActive(false);
        initScale = transform.localScale;
        boxPos = GetComponent<RectTransform>();
    }

    public void StartQueue() {
        gameObject.SetActive(true);
        transform.localScale = new Vector2(0,0);
        queueCountdown = StartCoroutine(queueNumTimer());
    }

    private IEnumerator queueNumTimer(){
        yield return new WaitForSeconds(interval * 0.5f);
        //Replace this later with environment variable to stop iteration
        while(!RageLogic.Instance.fullRage){
            switch(SelectPosition())
            {
                case 1:
                    SetRectPos(new Vector2(1,1));
                    //gameObject.transform.localPosition = new Vector3(240, 160, 0);
                    break;
                case 2:
                    SetRectPos(new Vector2(1, 0));
                    //gameObject.transform.localPosition = new Vector3(-240, -160, 0);
                    break;
                case 3:
                    SetRectPos(new Vector2(0, 0));
                    break;
            }
            text.text = numberQueue[currentNum];
            StartCoroutine(AnimatePopIn());
            currentNum = currentNum+1 < numberQueue.Count ? currentNum+1 : 0;
            yield return new WaitForSeconds(interval * 0.7f);
            StartCoroutine(AnimateFadeOut());
            yield return new WaitForSeconds(interval * 0.3f);
        }
    }

    public void StopQueue() {
        StopAllCoroutines();
        queueCountdown = null;
        StartCoroutine(AnimateFadeOut());
    }

    public void SetRectPos(Vector2 pivot) {
        boxPos.anchorMin = pivot;
        boxPos.anchorMax = pivot;
        boxPos.pivot = pivot;
    }

    private int SelectPosition(){
        int index = Random.Range(0, bag.Count);
        int selected = bag[index];
        bag.RemoveAt(index);
        if(bag.Count == 0)
        {
            bag = new List<int>(){1, 2, 3};
        }
        return selected;
    }

    void Update()
    {

    }

    public IEnumerator AnimatePopIn()
    {
        transform.localScale = new Vector2(0, 0);
        LeanTween.scale(gameObject, initScale, 0.2f);
        yield return null;
    }

    public IEnumerator AnimateFadeOut()
    {
        LeanTween.scale(gameObject, new Vector2(0,0), 0.2f);
        yield return new WaitForSeconds(0.2f);
    }
}
