using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelViewController : MonoBehaviour {
    public MainPanelView mainView;
    private void OnEnable () {
        Show ();
    }
    public void Show () {

        mainView.joinPanelViewController.gameObject.SetActive (true);
    }

    public void Hide () {
        mainView.joinPanelViewController.gameObject.SetActive (false);
        gameObject.SetActive (false);
    }
}