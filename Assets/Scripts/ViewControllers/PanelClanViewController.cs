using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelClanViewController : MonoBehaviour, IListViewDatasource, IListViewDelegate {

    public ListView listClanView;
    public Button buttonClose;
    public Button buttonCreateClan;
    public InputField clanNameInputField;
    public CanvasGroup canvasGroup;
    public ClanItem clanItemPrefab;
    public ClanInfoModel[] clansInfo;
    public IPanelClanDelegate Delegate { get; set; }
    private void OnEnable () {
        Show ();
    }
    private void Start () {
        listClanView.Datasource = this;
        listClanView.Delegate = this;
        buttonClose.onClick.AddListener (Hide);
        buttonCreateClan.onClick.AddListener (CreateClan);
    }
    public void ResetList () {
        listClanView.gameObject.SetActive (false);
        listClanView.gameObject.SetActive (true);
    }
    public void SetClansInfo (ClanInfoModel[] infosModel) {
        this.clansInfo = infosModel;
    }
    public void Show () {
        canvasGroup.alpha = 0;
        canvasGroup.LeanAlpha (1, .4f).setOnComplete (() => {
            listClanView.gameObject.SetActive (true);
        });
    }
    public void Hide () {
        Debug.Log ("Hide caln view controller");
        canvasGroup.LeanAlpha (0, .4f).setOnComplete (() => {
            gameObject.SetActive (false);
            listClanView.gameObject.SetActive (false);
        });
    }
    public void CreateClan () {
        if (string.IsNullOrEmpty (clanNameInputField.text) || string.IsNullOrWhiteSpace (clanNameInputField.text)) {
            clanNameInputField.text = "";
        } else {
            if (Delegate != null) {
                Delegate.CreateClan (clanNameInputField.text);
                clanNameInputField.text = "";
            }
        }
    }
    public ListViewItem CellOfRow (int id) {
        ClanItem temp = Instantiate (clanItemPrefab, listClanView.transform);
        temp.transform.localScale = Vector3.one;
        temp.SetClan (clansInfo[id]);
        return temp;
    }

    public void OnSelectRow (int id) {
        Debug.Log ($"Select row: {id}");
        if (Delegate != null) {
            Delegate.RequestJoinClan (clansInfo[id].id);
        }
    }

    public int RowCountInList () {
        if (clansInfo == null) {
            return 0;
        }
        return clansInfo.Length;
    }

    public float SizeOfRow (int row) {
        return 100;
    }
}
public interface IPanelClanDelegate {
    void CreateClan (string text);
    void RequestJoinClan (int id);
}