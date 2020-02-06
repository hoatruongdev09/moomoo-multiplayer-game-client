using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClanItem : ListViewItem {
    public override int ID {
        get { return id; }
        set { id = value; }
    }
    public override Action<int> OnSelected {
        get { return onSelected; }
        set { onSelected = value; }
    }

    public Text textNameClan;
    public Text textMemberCount;
    public Button btnJoin;
    private int id;
    private Action<int> onSelected;

    public void SetClan (ClanInfoModel info) {
        textNameClan.text = info.name;
        // textMemberCount.text = $"Members: {info.members}";
        btnJoin.onClick.AddListener (delegate { onSelected (ID); });
    }
}