﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelViewController : MonoBehaviour {
    public MainPanelView mainView;
    private void Start () {
        mainView.buttonQuit.onClick.AddListener (Quit);
    }
    public void Show () {
        StartCoroutine (DelayShowJoinPanel (.5f));
    }
    public void ShowConnectingIndicate () {
        mainView.connectingIndicateController.gameObject.SetActive (true);
    }
    public void HideConnectingIndicate () {
        mainView.connectingIndicateController.Hide ();
    }
    public void ShowErrorPanel (string text) {
        mainView.errorPanelViewController.SetErrorContent (text);
        mainView.errorPanelViewController.gameObject.SetActive (true);
    }

    public void Hide () {
        mainView.joinPanelViewController.Hide ();
        StartCoroutine (DelayHide (.51f));
    }
    private void Quit () {
        Application.Quit ();
    }
    private IEnumerator DelayShowJoinPanel (float time) {
        yield return new WaitForSeconds (time);
        mainView.joinPanelViewController.gameObject.SetActive (true);
    }
    private IEnumerator DelayHide (float time) {
        yield return new WaitForSeconds (time);
        gameObject.SetActive (false);
    }
}