using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveText : MonoBehaviour
{
    private TMP_Text text;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActivateObjective(string objectiveText)
    {
        text.text = objectiveText;
        gameObject.SetActive(true);
    }

    public void DeactivateObjective()
    {
        text.text = "";
        gameObject.SetActive(false);
    }
}
