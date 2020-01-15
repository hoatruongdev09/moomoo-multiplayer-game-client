using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatViewController : MonoBehaviour {
    public Button buttonSend;
    public InputField textChatInput;
    public CanvasGroup canvasGroup;
    public IChatViewControllerDelegate Delegate {
        get { return controllerDelegate; }
        set { controllerDelegate = value; }
    }

    private IChatViewControllerDelegate controllerDelegate;
    private void OnEnable () {
        Show ();
    }
    private void Start () {
        buttonSend.onClick.AddListener (ButtonChat);
    }
    private void ButtonChat () {
        if (string.IsNullOrEmpty (textChatInput.text) || string.IsNullOrWhiteSpace (textChatInput.text)) {
            Hide ();
            return;
        }
        if (controllerDelegate != null) {
            Hide ();
            controllerDelegate.OnButtonSendClick (textChatInput.text);
        }
    }
    public void Show () {
        if (LeanTween.isTweening (canvasGroup.gameObject)) {
            return;
        }
        canvasGroup.LeanAlpha (1, .3f).setEaseInQuint ();
    }
    public void Hide () {
        if (LeanTween.isTweening (canvasGroup.gameObject)) {
            return;
        }
        canvasGroup.LeanAlpha (0, .3f).setEaseOutQuint ().setOnComplete (() => {
            gameObject.SetActive (false);
            textChatInput.text = "";
        });
    }
}
public interface IChatViewControllerDelegate {
    void OnButtonSendClick (string text);
}