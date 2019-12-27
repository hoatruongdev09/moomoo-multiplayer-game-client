using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IJoinPanelViewControllerDelegate, IJoinPanelViewControllerDatasource {
    public GameController gameController;
    public MainPanelViewController mainPanelViewController;
    public GameViewController gameViewController;
    public JoinPanelViewController joinPanelViewController;

    private void Start () {
        joinPanelViewController.Delegate = this;
        joinPanelViewController.Datasource = this;
    }

    public void OnJoinGame () {
        mainPanelViewController.Hide ();
        gameViewController.gameObject.SetActive (true);
    }

    public void OnConnect () {
        StartCoroutine (AnimateStartup (.5f));
    }
    private IEnumerator AnimateStartup (float time) {
        yield return new WaitForSeconds (time);
        joinPanelViewController.gameObject.SetActive (true);
    }

    #region PANEL VIEW
    public void OnJoinClick (string name, int gameId, int skinId) {
        gameController.RequestJoinGame (name, gameId, skinId);
    }

    public GameStatusModel[] ListGame () {
        return gameController.RequestConnectInfo ().listGame;
    }
    #endregion
}