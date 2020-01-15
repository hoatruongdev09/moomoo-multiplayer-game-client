using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinPanelViewController : MonoBehaviour, IJoinPanelViewController {

    public IJoinPanelViewControllerDatasource Datasource {
        get { return controllerDatasource; }
        set { controllerDatasource = value; }
    }
    public IJoinPanelViewControllerDelegate Delegate {
        get { return controllerDelegate; }
        set { controllerDelegate = value; }
    }
    private JoinPanelView view;
    private IJoinPanelViewControllerDatasource controllerDatasource;
    private IJoinPanelViewControllerDelegate controllerDelegate;

    [SerializeField] private int skinSelected = 0;
    [SerializeField] private int gameSelected = 0;
    [SerializeField] private string playerName = "";
    [SerializeField] private GameStatusModel[] listGame;

    public Vector2 originalPosition;
    private void OnEnable () {
        originalPosition = (transform as RectTransform).anchoredPosition;
        InitializeView ();
        Show ();
    }
    public void Show () {
        view.canvasGroup.LeanAlpha (1, .4f);
        LeanTween.value (gameObject, originalPosition - new Vector2 (0, 50), originalPosition, .5f).setOnUpdate ((Vector2 value) => {
            (transform as RectTransform).anchoredPosition = value;
        }).setEaseOutBack ();
    }
    public void Hide () {
        view.canvasGroup.LeanAlpha (0, .4f);
        LeanTween.value (gameObject, originalPosition, originalPosition - new Vector2 (0, 50), .5f).setOnUpdate ((Vector2 value) => {
            (transform as RectTransform).anchoredPosition = value;
        }).setEaseOutBack ().setOnComplete (() => {
            gameObject.SetActive (false);
        });

    }
    private void Start () {
        view.btnJoin.onClick.AddListener (ButtonJoin);
        view.tgSkinSelect.OnChange += SkinChange;
        view.dSelectGame.onValueChanged.AddListener (GameChanged);
    }

    private void InitializeView () {
        view = GetComponent<JoinPanelView> ();
        listGame = controllerDatasource.ListGame ();
        List<Dropdown.OptionData> listOption = new List<Dropdown.OptionData> ();
        foreach (GameStatusModel ss in listGame) {
            listOption.Add (new Dropdown.OptionData ($"{ss.name}  {(ss.full?"FULL":"")}"));
        }
        view.dSelectGame.ClearOptions ();
        view.dSelectGame.options = listOption;
        if (!string.IsNullOrEmpty (playerName)) {
            view.ipName.text = playerName;
        }
    }

    private void ButtonJoin () {
        string name = view.ipName.text;
        if (!string.IsNullOrEmpty (name) || !string.IsNullOrWhiteSpace (name)) {
            playerName = name;
            controllerDelegate.OnJoinClick (name, gameSelected, skinSelected);
        } else {
            Debug.Log ("Name error");
        }
    }
    private void SkinChange (Toggle newActive) {
        skinSelected = int.Parse (newActive.name);
    }
    private void GameChanged (int gameid) {
        gameSelected = listGame[gameid].id;
    }

}
public interface IJoinPanelViewController {
    IJoinPanelViewControllerDatasource Datasource { get; set; }
    IJoinPanelViewControllerDelegate Delegate { get; set; }
}
public interface IJoinPanelViewControllerDelegate {
    void OnJoinClick (string name, int gameId, int skinId);
}
public interface IJoinPanelViewControllerDatasource {
    GameStatusModel[] ListGame ();
}