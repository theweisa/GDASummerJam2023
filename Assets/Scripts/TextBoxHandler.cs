using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class TextBoxHandler : MonoBehaviour
{
    public GameObject textBox;
    public TMP_Text text;
    public NPC NPC;
    public int currentLine = 0;
    public bool hasActivated = false;
    public bool finished = false;

    public bool canContinue = true;
    private Vector3 initScale;

    void Awake() {
        initScale = transform.localScale;
    }
    /*void Start()
    {
        initScale = transform.localScale;
    }*/

    public IEnumerator Activate()
    {
        finished = false;
        canContinue = false;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, initScale, 0.5f).setEaseOutQuart();
        //yield return new WaitForSeconds(0.5f);
        StartCoroutine(DisplayLine(NPC.script[currentLine]));
        yield return new WaitUntil(()=>finished);
        Destroy(gameObject);
    }

    public void Continue()
    {
        canContinue = false;
        StartCoroutine(DisplayLine(NPC.script[currentLine]));
    }

    public IEnumerator Deactivate()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseOutQuart();
        yield return new WaitForSeconds(0.5f);
        finished = true;
        currentLine = 0;
        hasActivated = true;
    }

    private IEnumerator DisplayLine(string line)
    {
        text.text = "";
        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            RuntimeManager.PlayOneShot(FMODEventReferences.instance.DialogueBlip);
            yield return new WaitForSeconds(NPC.textSpeed);
        }
        canContinue = true;
        currentLine += 1;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canContinue && gameObject.activeSelf)
        {
            if(currentLine >= NPC.script.Count){
                StartCoroutine(Deactivate());
            }
            else
            {
                Continue();
            }
        }
    }
}
