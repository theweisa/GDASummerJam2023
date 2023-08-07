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

    void Start()
    {
        queueCountdown = StartCoroutine(queueNumTimer());
    }

    private IEnumerator queueNumTimer(){
        //Replace this later with environment variable to stop iteration
        while(!RageLogic.Instance.fullRage){
            switch(SelectPosition())
            {
                case 1:
                    gameObject.transform.localPosition = new Vector3(240, 160, 0);
                    break;
                case 2:
                    gameObject.transform.localPosition = new Vector3(-240, -160, 0);
                    break;
                case 3:
                    gameObject.transform.localPosition = new Vector3(240, -160, 0);
                    break;
            }
            text.text = numberQueue[currentNum];
            currentNum += 1;
            yield return new WaitForSeconds(interval);
        }
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
}
