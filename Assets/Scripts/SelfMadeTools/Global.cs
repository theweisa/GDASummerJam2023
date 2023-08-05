using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.U2D;

public static class Global {
    #nullable enable
    public static T FindComponent<T>(GameObject obj) {
        T? returnVal = obj.GetComponent<T>();
        if (returnVal != null && !returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInChildren<T>();
        if (returnVal != null && !returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInParent<T>();
        if (returnVal != null && !returnVal.Equals(null)) {
            return returnVal;
        }
        Debug.Log("ERROR: Could not Find Component");
        return returnVal;
        
    }
}