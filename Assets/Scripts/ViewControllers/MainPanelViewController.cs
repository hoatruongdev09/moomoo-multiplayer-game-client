using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelViewController : MonoBehaviour {
    public MainPanelView mainView;
    public void Show () {
        StartCoroutine (DelayShowJoinPanel (.5f));
    }

    public void Hide () {
        mainView.joinPanelViewController.gameObject.SetActive (false);
        gameObject.SetActive (false);
    }
    private IEnumerator DelayShowJoinPanel (float time) {
        yield return new WaitForSeconds (time);
        mainView.joinPanelViewController.gameObject.SetActive (true);
    }
}