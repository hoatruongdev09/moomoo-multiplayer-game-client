using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TextPopup : MonoBehaviour {
    public TextMeshPro text;

    private void OnEnable () {
        Show ();
    }
    public void SetText (string text, Vector3 position, Color color) {
        this.text.text = text;
        this.text.color = color;
        this.transform.position = position;
    }

    private void Show () {
        text.transform.LeanMoveLocal (new Vector3 (0, 2), .5f);
        LeanTween.textAlpha (text.rectTransform, 1, .1f);
        LeanTween.textAlpha (text.rectTransform, 0, .1f).setDelay (.5f).setOnComplete (() => {
            Destroy (gameObject);
        });
    }
}