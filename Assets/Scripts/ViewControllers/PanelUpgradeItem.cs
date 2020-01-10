using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelUpgradeItem : MonoBehaviour, IListViewDatasource, IListViewDelegate {
    public ListView listItemView;
    public string[] upgradeCodes;
    public ListUpgradeItem[] reuseItems;
    public CanvasGroup canvasGroup;

    public IPanelUpgradeItemDelegate Delegate { get; set; }
    private void Start () {
        listItemView.Datasource = this;
        listItemView.Delegate = this;
    }
    public void Show () {
        if (LeanTween.isTweening (gameObject)) {
            return;
        }
        transform.localScale = Vector3.one * 1.3f;
        canvasGroup.alpha = 0;
        transform.LeanScale (Vector3.one, .5f).setEaseInQuint ();
        canvasGroup.LeanAlpha (1, .5f).setEaseInQuint ().setOnComplete (() => {
            listItemView.gameObject.SetActive (true);
            canvasGroup.blocksRaycasts = true;
        });
    }
    public void Hide () {
        if (LeanTween.isTweening (gameObject)) {
            return;
        }
        transform.LeanScale (Vector3.one * 1.3f, .5f).setEaseOutQuint ();
        canvasGroup.LeanAlpha (0, .5f).setEaseOutQuint ().setOnComplete (() => {
            listItemView.gameObject.SetActive (false);
            canvasGroup.blocksRaycasts = false;
        });
    }

    #region LIST VIEW

    public ListViewItem CellOfRow (int id) {
        ListUpgradeItem item = FindItemByCode (upgradeCodes[id]);
        if (item == null) {
            return null;
        }
        return Instantiate (item, Vector3.zero, Quaternion.identity);
    }
    private ListUpgradeItem FindItemByCode (string code) {
        foreach (ListUpgradeItem item in reuseItems) {
            if (item.serverCode == code) {
                return item;
            }
        }
        return null;
    }
    public void OnSelectRow (int id) {
        Delegate.OnChooseCode (upgradeCodes[id]);
        Hide ();
    }

    public int RowCountInList () {
        return upgradeCodes.Length;
    }

    public float SizeOfRow (int row) {
        return 100;
    }
    #endregion
}

public interface IPanelUpgradeItemDelegate {
    void OnChooseCode (string code);
}