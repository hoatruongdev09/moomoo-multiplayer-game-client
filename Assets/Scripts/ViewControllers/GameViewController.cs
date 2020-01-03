using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewController : MonoBehaviour, IGameViewController {
    public GameObject virtualGamePad;
    public ItemTrayController itemTrayController;

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
        itemTrayController.RegisterDelegateForButton (controllerDelegate.OnChangeItem);
    }
    public void Show () {

    }
    public void Hide () {
        gameObject.SetActive (false);
    }
}
public interface IGameViewController {
    IGameViewControllerDatasource Datasource { get; set; }
    IGameViewControllerDelegate Delegate { get; set; }
}
public interface IGameViewControllerDelegate {
    void OnChangeItem (string item);
}
public interface IGameViewControllerDatasource {

}