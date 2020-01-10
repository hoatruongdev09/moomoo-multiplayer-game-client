using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewController : MonoBehaviour, IGameViewController, IPanelUpgradeItemDelegate {
    public ItemTrayController itemTrayController;
    public GameView gameView;
    public MapViewController mapView;
    public PanelUpgradeItem panelUpgradeItem;
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
        itemTrayController.RegisterDelegateForButton (controllerDelegate.OnChangeItem);
    }
    public void Show () {

    }
    public void Hide () {
        gameObject.SetActive (false);
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
}
public interface IGameViewController {
    IGameViewControllerDatasource Datasource { get; set; }
    IGameViewControllerDelegate Delegate { get; set; }
}
public interface IGameViewControllerDelegate {
    void OnChangeItem (string item);
    void OnUpgradeItem (string code);
}
public interface IGameViewControllerDatasource {

}