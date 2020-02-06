using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinClanPopupController : MonoBehaviour {
    public Text textPlayerName;
    public Button buttonDeny;
    public Button buttonAccept;
    public CanvasGroup canvasGroup;
    public IJoinClanPopupDelegate Delegate { get; set; }

    private void OnEnable () {
        Show ();
    }
    private void Start () {
        buttonAccept.onClick.AddListener (Accept);
        buttonDeny.onClick.AddListener (Deny);
    }
    private void Accept () {
        Hide ();
        if (Delegate != null) {
            Delegate.OnAccept ();
        }
    }
    private void Deny () {
        Hide ();
        if (Delegate != null) {
            Delegate.OnDeny ();
        }
    }
    public void SetRequestName (string name) {
        textPlayerName.text = name;
    }
    public void Show () {
        if (LeanTween.isTweening (canvasGroup.gameObject)) {
            LeanTween.cancel (canvasGroup.gameObject);
        }
        canvasGroup.LeanAlpha (1, .5f);
        (transform as RectTransform).anchoredPosition = new Vector2 (580, -350);
        (transform as RectTransform).LeanMoveX (0, .5f).setEaseInOutBack ();
    }
    public void Hide () {
        canvasGroup.LeanAlpha (0, .5f);
        (transform as RectTransform).LeanMoveX (580, .5f).setEaseInOutBack ().setOnComplete (() => {
            gameObject.SetActive (false);
        });
    }
}

public interface IJoinClanPopupDelegate {
    void OnAccept ();
    void OnDeny ();
}