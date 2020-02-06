using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelClanMemberController : MonoBehaviour, IListViewDatasource, IListViewDelegate {

    public Text textClanName;
    public ListView listClanMemberView;
    public Button buttonClose;
    public Button buttonLeaveClan;

    public CanvasGroup canvasGroup;
    public ClanMemberItem clanMemberItemPrefab;
    public VisualClanMemberModel[] clanMemberInfo;
    public IPanelClanMemberDelegate Delegate { get; set; }
    private bool permision = false;

    private void OnEnable () {
        Show ();
    }
    private void Start () {
        listClanMemberView.Datasource = this;
        listClanMemberView.Delegate = this;
        buttonClose.onClick.AddListener (Hide);
        buttonLeaveClan.onClick.AddListener (LeaveClan);
    }
    public void SetClanMemberInfo (VisualClanMemberModel[] infos) {
        clanMemberInfo = infos;
    }
    public void SetClanName (string text) {
        textClanName.text = text;
    }
    public void SetPermission (bool permision) {
        this.permision = permision;
    }
    public void ResetList () {
        listClanMemberView.gameObject.SetActive (false);
        listClanMemberView.gameObject.SetActive (true);
    }
    public void Show () {
        canvasGroup.alpha = 0;
        canvasGroup.LeanAlpha (1, .4f).setOnComplete (() => {
            listClanMemberView.gameObject.SetActive (true);
        });
    }
    public void Hide () {
        canvasGroup.LeanAlpha (0, .4f).setOnComplete (() => {
            gameObject.SetActive (false);
            listClanMemberView.gameObject.SetActive (false);
        });
    }
    public void LeaveClan () {
        if (Delegate != null) {
            Delegate.OnLeaveClan ();
        }
    }
    public ListViewItem CellOfRow (int id) {
        ClanMemberItem temp = Instantiate (clanMemberItemPrefab, listClanMemberView.transform);
        temp.transform.localScale = Vector3.one;
        temp.SetMember (clanMemberInfo[id], permision);
        return temp;
    }

    public void OnSelectRow (int id) {
        if (Delegate != null) {
            Delegate.OnKickMember (clanMemberInfo[id].id);
        }
    }

    public int RowCountInList () {
        return clanMemberInfo.Length;
    }

    public float SizeOfRow (int row) {
        return 100;
    }
}
public interface IPanelClanMemberDelegate {
    void OnLeaveClan ();
    void OnKickMember (int id);
}