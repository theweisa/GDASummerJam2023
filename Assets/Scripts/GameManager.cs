using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : UnitySingleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayEvents());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        
    }

    /*
    for now, fucking uh
    play like, a shit ton of events
    ie.) call a number every x seconds
    */
    IEnumerator PlayEvents() {
        for (int i = 0; i < 10; i++) {
            RageLogic.Instance.AddRage(10f);
            yield return new WaitForSeconds(3f);
        }
    }
}
