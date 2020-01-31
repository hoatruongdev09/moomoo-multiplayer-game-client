using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewController : MonoBehaviour, IGameViewController, IPanelUpgradeItemDelegate, IChatViewControllerDelegate {
    public ItemTrayController itemTrayController;
    public ChatViewController chatViewController;
    public GameView gameView;
    public MapViewController mapView;
    public PanelUpgradeItem panelUpgradeItem;
    public PanelScoreBoard panelScoreBoard;

    public IGameViewControllerDatasource Datasource {
        get { return controllerDatasource; }
        set { controllerDatasource = value; }
    }
    public IGameViewControllerDelegate Delegate {
        get { return controllerDelegate; }
        set { controllerDelegate = value; }
    }
    private IGameViewControllerDatasource controllerDatasource;
    private IGameViewControllerDelegate controllerDelegate;

    private void OnEnable () {
        Show ();
    }
    private void Start () {

        panelUpgradeItem.Delegate = this;
        chatViewController.Delegate = this;
        itemTrayController.RegisterDelegateForButton (controllerDelegate.OnChangeItem);

        gameView.buttonChat.onClick.AddListener (OpenChatPanel);
        gameView.scoreBoard.onClick.AddListener (() => {
            OpenScoreboard ();
            controllerDelegate.OnOpenScoreboard ();
        });

#if UNITY_ANDROID || UNITY_IOS
        Debug.Log ("Unity android");
        gameView.virtualGamePad.SetActive (true);
#endif
#if UNITY_EDITOR || UNITY_WEBGL
        Debug.Log ("Unity editor");
        gameView.virtualGamePad.SetActive (false);
#endif
    }
    public void Show () {
        gameView.canvasGroup.LeanAlpha (1, .5f);
    }
    public void Hide () {
        gameView.canvasGroup.LeanAlpha (0, .5f).setOnComplete (() => {
            gameObject.SetActive (false);
        });
    }
    public void UpdatePlayerStatus (PlayerStatusModel model) {
        gameView.UpdateInfo (model);
    }
    public void SyncItemTray (SyncItemModel model) {
        itemTrayController.SyncItemTray (model.items);
    }
    public void OpenUpgradeItem (string[] codes) {
        panelUpgradeItem.upgradeCodes = codes;
        panelUpgradeItem.Show ();
    }

    public void OnChooseCode (string code) {
        controllerDelegate.OnUpgradeItem (code);
    }
    public void AddPlayerToMap (int id) {
        mapView.AddPlayerStatue (id);
    }
    public void AddWoodToMap (Vector3 position) {
        mapView.AddWoodResource (position);
    }
    public void AddFoodToMap (Vector3 position) {
        mapView.AddFoodResource (position);
    }
    public void AddStoneToMap (Vector3 position) {
        mapView.AddStoneResource (position);
    }
    public void AddGoldToMap (Vector3 position) {
        mapView.AddGoldResource (position);
    }
    public void UpdatePositionPlayer (int id, Vector3 position) {
        mapView.UpdatePlayerPosition (id, position);
    }
    public void SetServerMapSize (Vector2 mapSize) {
        mapView.serverMapSize = mapSize;
    }
    public void InitPlayerMapCount (int count) {
        mapView.InitPlayerCount (count);
    }

    public void OnButtonSendClick (string text) {
        if (controllerDelegate != null) {
            controllerDelegate.OnSendChat (text);
        }
    }
    public void OpenChatPanel () {
        if (chatViewController.gameObject.activeSelf) {
            chatViewController.Hide ();
        } else {
            chatViewController.gameObject.SetActive (true);
        }
    }
    public void OpenScoreboard () {
        if (panelScoreBoard.gameObject.activeSelf) {
            panelScoreBoard.Hide ();
        } else {
            panelScoreBoard.gameObject.SetActive (true);
        }
    }
    public void ReceiveScoreboard (ScoreInfo[] infos) {
        panelScoreBoard.SetScoreInfo (infos);
    }
}
public interface IGameViewController {
    IGameViewControllerDatasource Datasource { get; set; }
    IGameViewControllerDelegate Delegate { get; set; }
}
public interface IGameViewControllerDelegate {
    void OnChangeItem (string item);
    void OnUpgradeItem (string code);
    void OnSendChat (string text);
    void OnOpenScoreboard ();
}
public interface IGameViewControllerDatasource {

}