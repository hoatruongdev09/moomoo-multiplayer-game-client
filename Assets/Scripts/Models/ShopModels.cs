using System;

[Serializable]
public class AllShopItemModel {
    public ShopItemInfo[] hats;
    public ShopItemInfo[] accessories;
}

[Serializable]
public class ShopItemInfo {
    public string id;
    public string name;
    public string description;
    public int price;

    public ShopItemInfo (string id, string name, string description, int price) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
    }
}

[Serializable]
public class VisualShopItemInfo : ShopItemInfo {
    public int status;

    public VisualShopItemInfo (int status, string id, string name, string description, int price) : base (id, name, description, price) {
        this.status = status;
    }
}

[Serializable]
public class SyncItempShopModel {
    public string[] owned;
    public string equipedHat;
    public string equipedAccessory;
}