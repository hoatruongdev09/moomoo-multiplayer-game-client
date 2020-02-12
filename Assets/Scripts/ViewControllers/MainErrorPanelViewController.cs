using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainErrorPanelViewController : MonoBehaviour {
    public Text textErrorContent;
    public Button buttonOkay;

    public CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private void OnEnable () {
        originalPosition = (transform as RectTransform).anchoredPosition;
        Show ();
    }
    private void Start () {
        buttonOkay.onClick.AddListener (() => {
            Hide ();
        });
    }
    public void SetErrorContent (string text) {
        textErrorContent.text = text;
    }
    public void Show () {
        (transform as RectTransform).anchoredPosition = originalPosition - new Vector2 (0, 50);
        canvasGroup.LeanAlpha (1, .4f);
        (transform as RectTransform).LeanMoveY (0, .4f);
    }
    public void Hide () {
        canvasGroup.LeanAlpha (0, .4f);
        (transform as RectTransform).LeanMoveY (-50, .4f).setOnComplete (() => {
            gameObject.SetActive (false);
        });
    }
}