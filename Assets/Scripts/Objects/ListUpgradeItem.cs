using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListUpgradeItem : ListViewItem {
    public string serverCode;
    public Image icon;
    public Text textTitle;
    public Text textDescription;
    public Button button;

    public override int ID { get { return id; } set { id = value; } }

    public override Action<int> OnSelected { get { return onSelect; } set { onSelect = value; } }
    private int id;
    private Action<int> onSelect;

    private void Start () {
        button = GetComponent<Button> ();
        button.onClick.AddListener (delegate { onSelect (ID); });
    }
}