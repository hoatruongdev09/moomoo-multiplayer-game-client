using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IJoinPanelViewControllerDelegate, IJoinPanelViewControllerDatasource, IGameViewControllerDatasource, IGameViewControllerDelegate {
    public GameController gameController;
    public ClanManager clanManager;
    public MainPanelViewController mainPanelViewController;
    public GameViewController gameViewController;
    public JoinPanelViewController joinPanelViewController;

    private void Start () {
        joinPanelViewController.Delegate = this;
        joinPanelViewController.Datasource = this;

        gameViewController.Delegate = this;
        gameViewController.Datasource = this;
        mainPanelViewController.ShowConnectingIndicate ();
    }

    public void OnJoinGame () {
        mainPanelViewController.Hide ();
        gameViewController.gameObject.SetActive (true);
    }
    public void OnConnectFailed (string text) {
        mainPanelViewController.ShowErrorPanel (text);
    }
    public void OnGameOver () {
        mainPanelViewController.gameObject.SetActive (true);
        gameViewController.Hide ();
        StartCoroutine (AnimateStartup (.5f));
    }

    public void OnConnect () {
        mainPanelViewController.HideConnectingIndicate ();
        StartCoroutine (AnimateStartup (.5f));
    }
    public void UpdatePlayerInfo (PlayerStatusModel model) {
        gameViewController.UpdatePlayerStatus (model);
    }
    public void ShowUpgradeItem (UpgradeItemModel model) {
        gameViewController.OpenUpgradeItem (model.items);
    }
    public void SyncItemTray (SyncItemModel model) {
        gameViewController.SyncItemTray (model);
    }
    private IEnumerator AnimateStartup (float time) {
        yield return new WaitForSeconds (time);
        mainPanelViewController.Show ();
    }

    public void ResetClanPanel (List<ClanInfoModel> listClan) {
        gameViewController.panelClanViewController.SetClansInfo (listClan.ToArray ());
        gameViewController.panelClanViewController.ResetList ();
    }
    public void ResetClanMemberPanel (VisualClanMemberModel[] listMember) {
        gameViewController.panelClanMemberController.SetClanMemberInfo (listMember);
        gameViewController.panelClanMemberController.ResetList ();
    }
    public void OpenClanPanel () {
        if (IsClanMemberPanelOpened ()) {
            gameViewController.HideClanMemberPanel ();
        }
        if (IsClanPanelOpened ()) {
            gameViewController.HideClanPanel ();
        } else {
            gameViewController.panelClanViewController.SetClansInfo (clanManager.listClan.ToArray ());
            gameViewController.ShowClanPanel ();
        }
    }
    public void OpenClanMemberPanel () {
        if (IsClanPanelOpened ()) {
            gameViewController.HideClanPanel ();
        }
        if (IsClanMemberPanelOpened ()) {
            gameViewController.HideClanMemberPanel ();
        } else {
            gameViewController.panelClanMemberController.SetClanMemberInfo (clanManager.GetListMemberLocalClan ());
            gameViewController.panelClanMemberController.SetClanName (clanManager.GetLocalClan ().name);
            gameViewController.panelClanMemberController.SetPermission (clanManager.IsMasterLocalClan ());
            gameViewController.ShowClanMemberPanel ();
        }
    }
    public void ShowRequestJoinClan (int id) {
        if (gameController.playerManager.players[id] != null) {
            gameViewController.ShowPopupJoinClan (gameController.playerManager.players[id].character.name);
        }
    }
    public void HideRequestJoinClan () {
        gameViewController.HidePopupJoinClan ();
    }
    public bool IsClanMemberPanelOpened () {
        return gameViewController.panelClanMemberController.gameObject.activeSelf;
    }
    public bool IsClanPanelOpened () {
        return gameViewController.panelClanViewController.gameObject.activeSelf;
    }
    public void AddPlayerToMap (int id) {
        gameViewController.AddPlayerToMap (id);
    }
    public void RemovePlayerFromMap (int id) {
        gameViewController.RemovePlayerFromMap (id);
    }
    public void AddWoodToMap (Vector3 position) {
        gameViewController.AddWoodToMap (position);
    }

    public void AddFoodToMap (Vector3 position) {
        gameViewController.AddFoodToMap (position);
    }
    public void AddStoneToMap (Vector3 position) {
        gameViewController.AddStoneToMap (position);
    }
    public void AddGoldToMap (Vector3 position) {
        gameViewController.AddGoldToMap (position);
    }
    public void UpdatePositionPlayer (int id, Vector3 position) {
        gameViewController.UpdatePositionPlayer (id, position);
    }
    public void SetServerMapSize (Vector2 size) {
        // Debug.Log ($"set server mapSize: {size}");
        gameViewController.SetServerMapSize (size);
    }
    public void ReceiveScoreInfo (ScoreInfo[] infos) {
        gameViewController.ReceiveScoreboard (infos);
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

    public void OnUpgradeItem (string code) {
        gameController.RequestUpgradeItem (code);
    }

    public void OnSendChat (string text) {
        gameController.SendChat (text);
    }

    public void OnOpenScoreboard () {
        gameController.RequestScore ();
    }

    public void OnOpenClan () {
        if (clanManager.localClanId == -1) {
            OpenClanPanel ();
        } else {
            OpenClanMemberPanel ();
        }
    }

    public void CreateClan (string text) {
        gameController.RequestCreateClan (text);
    }

    public void LeaveClan () {
        gameController.RequestLeaveClan ();
    }

    public void RequestJoinClan (int id) {
        gameController.RequestJoinClan (id);
    }

    public void RequestKick (int id) {
        gameController.RequestKickMember (id);
    }

    public void AcceptRequest () {
        gameController.AcceptJoinRequest ();
    }

    public void DenyRequest () {
        gameController.DenyJoinRequest ();
    }

    #endregion
}