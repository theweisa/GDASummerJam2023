using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QueueNumber : MonoBehaviour
{
    private float interval = 10f;
    private int currentNum = 0;
    public List<string> numberQueue = new List<string>();
    public TMP_Text text;
    private List<int> bag = new List<int>(){1, 2, 3};

    public Coroutine queueCountdown;
    public RectTransform boxPos;

    void Start()
    {
        queueCountdown = StartCoroutine(queueNumTimer());
        boxPos = GetComponent<RectTransform>();
    }

    private IEnumerator queueNumTimer(){
        //Replace this later with environment variable to stop iteration
        while(true){
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
                    SetRectPos(new Vector2(0,0));
                    break;
            }
            text.text = numberQueue[currentNum];
            currentNum += 1;
            yield return new WaitForSeconds(interval);
        }
    }

    void SetRectPos(Vector2 pivot) {
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

    public void SetAndStretchToParentSize(RectTransform _mRect, RectTransform _parent)
    {
        _mRect.anchoredPosition = _parent.position;
        _mRect.anchorMin = new Vector2(1, 0);
        _mRect.anchorMax = new Vector2(0, 1);
        _mRect.pivot = new Vector2(0.5f, 0.5f);
        _mRect.sizeDelta = _parent.rect.size;
        _mRect.transform.SetParent(_parent);
    }
}
