using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelShopViewController : MonoBehaviour, IListViewDatasource, IListViewDelegate {
    public ListView listItemView;
    public Toggle toggleAccessory;
    public Button buttonQuit;
    public CanvasGroup canvasGroup;
    public ShopItem shopItemPrefab;
    private bool isShowAccessories = true;
    public VisualShopItemInfo[] listItem;
    public IShopPanelDatasource Datasource { get; set; }
    public IShopPanelDelegate Delegate { get; set; }
    private void OnEnable () {
        if (listItem.Length == 0) {
            if (isShowAccessories) {
                listItem = Datasource.GetAccessoryItem ();
            } else {
                listItem = Datasource.GetHatItem ();
            }
        }
        Show ();
    }
    private void Start () {
        listItemView.Delegate = this;
        listItemView.Datasource = this;

        buttonQuit.onClick.AddListener (Hide);
        toggleAccessory.onValueChanged.AddListener (OnChangeTab);
    }

    private void OnChangeTab (bool action) {
        isShowAccessories = action;
        if (isShowAccessories) {
            listItem = Datasource.GetAccessoryItem ();
        } else {
            listItem = Datasource.GetHatItem ();
        }
        RecreateList ();
    }
    public void RecreateList () {
        if (isShowAccessories) {
            listItem = Datasource.GetAccessoryItem ();
        } else {
            listItem = Datasource.GetHatItem ();
        }
        listItemView.gameObject.SetActive (false);
        listItemView.gameObject.SetActive (true);
    }
    public void Show () {
        canvasGroup.alpha = 0;
        canvasGroup.LeanAlpha (1, .4f).setOnComplete (() => {
            listItemView.gameObject.SetActive (true);
        });
    }
    public void Hide () {
        canvasGroup.LeanAlpha (0, .4f).setOnComplete (() => {
            gameObject.SetActive (false);
            listItemView.gameObject.SetActive (false);
        });
    }

    public ListViewItem CellOfRow (int id) {
        ShopItem temp = Instantiate (shopItemPrefab, listItemView.transform);
        temp.transform.localScale = Vector3.one;
        temp.SetItem (listItem[id]);
        return temp;
    }

    public void OnSelectRow (int id) {
        if (Delegate != null) {
            Delegate.OnSelectItem (listItem[id].id);
        }
    }

    public int RowCountInList () {
        return listItem.Length;
    }

    public float SizeOfRow (int row) {
        return 150;
    }
}
public interface IShopPanelDatasource {
    VisualShopItemInfo[] GetHatItem ();
    VisualShopItemInfo[] GetAccessoryItem ();
}
public interface IShopPanelDelegate {
    void OnSelectItem (string id);
}