using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInfoText : MonoBehaviour {
    public string label;
    public Text textInfo;

    public void SetText (string info) {
        textInfo.text = info;
    }
}