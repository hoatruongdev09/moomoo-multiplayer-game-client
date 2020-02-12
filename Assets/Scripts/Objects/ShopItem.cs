using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : ListViewItem {
    public Text textItemName;
    public Text textItemPrice;
    public Text textItemStatus;
    public Text textDescription;
    public Button buttonPurchase;
    public Image imageItem;
    public override int ID {
        get { return id; }
        set { id = value; }
    }
    public override Action<int> OnSelected {
        get { return onSelected; }
        set { onSelected = value; }
    }

    private Action<int> onSelected;

    private int id;

    public void SetItem (VisualShopItemInfo info) {
        textItemName.text = info.name;
        if (info.status == 1) {
            textItemPrice.text = "";
            textItemStatus.text = "Equip";
        } else if (info.status == 2) {
            textItemPrice.text = "";
            textItemStatus.text = "Unequip";
        } else {
            textItemPrice.text = $"{info.price}";
            textItemStatus.text = "Purchase";
        }
        textDescription.text = info.description;
        imageItem.sprite = Resources.Load<Sprite> ($"ShopItemSprite/{info.id}");
        buttonPurchase.onClick.AddListener (delegate { onSelected (id); });
    }
}