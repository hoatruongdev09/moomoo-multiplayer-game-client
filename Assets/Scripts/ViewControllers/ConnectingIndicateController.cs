using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingIndicateController : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public Image rotatingImage;
    public Text textInfo;

    private void OnEnable () {
        canvasGroup.LeanAlpha (1, .2f);
        LeanTween.value (rotatingImage.gameObject, 0, 360, 1).setOnUpdate ((float value) => {
            rotatingImage.transform.rotation = Quaternion.Euler (0, 0, value);
        }).setLoopClamp ();
        LeanTween.value (rotatingImage.gameObject, 0, .3f, 1).setOnUpdate ((float value) => {
            rotatingImage.fillAmount = value;
        });
    }

    public void Hide () {
        if (LeanTween.isTweening (canvasGroup.gameObject)) {
            LeanTween.cancel (canvasGroup.gameObject);
        }
        canvasGroup.LeanAlpha (0, .2f).setOnComplete (() => {
            LeanTween.cancel (rotatingImage.gameObject);
            gameObject.SetActive (false);
        });
    }
}