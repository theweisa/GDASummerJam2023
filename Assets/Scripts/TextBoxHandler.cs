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

    public bool canContinue = true;
    void Start()
    {

    }

    public void Activate()
    {
        PlayerManager.Instance.controller.canMove = false;
        PlayerManager.Instance.controller.moveAnim.Stop();
        canContinue = false;
        textBox.SetActive(true);
        StartCoroutine(DisplayLine(NPC.script[currentLine]));
    }

    public void Continue()
    {
        canContinue = false;
        StartCoroutine(DisplayLine(NPC.script[currentLine]));
    }

    public void Deactivate()
    {
        PlayerManager.Instance.cameraPosition.localPosition = Vector2.zero;
        PlayerManager.Instance.controller.canMove = true;
        textBox.SetActive(false);
        currentLine = 0;
        if(!hasActivated)
        {
            RageLogic.Instance.AddRage(2.5f);
        }
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
                Deactivate();
            }
            else
            {
                Continue();
            }
        }
    }
}
