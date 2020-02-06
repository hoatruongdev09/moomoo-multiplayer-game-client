using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanMemberItem : ListViewItem {
    public override int ID {
        get { return id; }
        set { id = value; }
    }
    public override Action<int> OnSelected {
        get { return onSelected; }
        set { onSelected = value; }
    }

    public Text textMember;
    public Button btnKick;
    private int id;
    private Action<int> onSelected;
    public void SetMember (VisualClanMemberModel info, bool canKick) {
        textMember.text = $"{(info.role == 1 ?"*":"")} {info.name}";
        if (canKick && info.role == 0) {
            btnKick.onClick.AddListener (delegate { onSelected (id); });
            btnKick.gameObject.SetActive (true);
        } else {
            btnKick.gameObject.SetActive (false);
        }
    }
}