using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveText : MonoBehaviour
{
    public TMP_Text text;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActivateObjective(string objectiveText)
    {
        text.text = objectiveText;
        gameObject.SetActive(true);
        StartCoroutine(AnimatePopIn());
    }

    public void DeactivateObjective()
    {
        text.text = "";
        gameObject.SetActive(false);
    }

    private IEnumerator AnimatePopIn(){
        var targetScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        LeanTween.scale(gameObject, targetScale, 0.4f);
        yield return null;
    }
}
