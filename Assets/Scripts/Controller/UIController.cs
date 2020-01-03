using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IJoinPanelViewControllerDelegate, IJoinPanelViewControllerDatasource, IGameViewControllerDatasource, IGameViewControllerDelegate {
    public GameController gameController;
    public MainPanelViewController mainPanelViewController;
    public GameViewController gameViewController;
    public JoinPanelViewController joinPanelViewController;

    private void Start () {
        joinPanelViewController.Delegate = this;
        joinPanelViewController.Datasource = this;

        gameViewController.Delegate = this;
        gameViewController.Datasource = this;
    }

    public void OnJoinGame () {
        mainPanelViewController.Hide ();
        gameViewController.gameObject.SetActive (true);
    }
    public void OnGameOver () {
        mainPanelViewController.gameObject.SetActive (true);
        gameViewController.Hide ();
    }

    public void OnConnect () {
        StartCoroutine (AnimateStartup (2f));
    }
    private IEnumerator AnimateStartup (float time) {
        yield return new WaitForSeconds (time);
        mainPanelViewController.Show ();
    }

    #region PANEL VIEW
    public void OnJoinClick (string name, int gameId, int skinId) {
        gameController.RequestJoinGame (name, gameId, skinId);
    }

    public GameStatusModel[] ListGame () {
        return gameController.RequestConnectInfo ().listGame;
    }

    #endregion
    #region GAME VIEW
    public void OnChangeItem (string item) {
        gameController.RequestSwitchItem (item);
    }
    #endregion
}