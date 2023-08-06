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

    public Coroutine queueCountdown;

    void Start()
    {
        text.text = numberQueue[currentNum];
        queueCountdown = StartCoroutine(queueNumTimer());
    }

    private IEnumerator queueNumTimer(){
        //Replace this later with environment variable to stop iteration
        while(true){
            yield return new WaitForSeconds(interval);
            currentNum += 1;
            text.text = numberQueue[currentNum];
        }
    }

    void Update()
    {

    }
}
