using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ShopManager : MonoBehaviour, IShopPanelDatasource, IShopPanelDelegate {
    public GameController gameController;
    public ShopItemInfo[] hats;
    public ShopItemInfo[] accessories;
    public string[] ownedItem;
    public string equipedHat;
    public string equipedAccesory;
    public PanelShopViewController shopViewController;

    private void Start () {
        shopViewController.Datasource = this;
        shopViewController.Delegate = this;
    }

    public VisualShopItemInfo[] GetAccessoryItem () {
        return accessories.Select ((item) => {
            int status = 0;
            if (ownedItem.Contains (item.id) || item.price == 0) {
                status = 1;
            }
            if (equipedAccesory == item.id) {
                status = 2;
            }
            return new VisualShopItemInfo (status, item.id, item.name, item.description, item.price);
        }).ToArray ();
    }

    public VisualShopItemInfo[] GetHatItem () {
        return hats.Select ((item) => {
            int status = 0;
            if (ownedItem.Contains (item.id) || item.price == 0) {
                status = 1;
            }
            if (equipedHat == item.id) {
                status = 2;
            }
            return new VisualShopItemInfo (status, item.id, item.name, item.description, item.price);
        }).ToArray ();
    }
    public void SyncItem (SyncItemShopModel model) {
        ownedItem = model.owned;
        equipedHat = model.equipedHat;
        equipedAccesory = model.equipedAccessory;
        if (shopViewController.gameObject.activeSelf) {
            shopViewController.RecreateList ();
        }
    }

    public void OnSelectItem (string id) {
        gameController.RequestSelectItem (id);
    }
}