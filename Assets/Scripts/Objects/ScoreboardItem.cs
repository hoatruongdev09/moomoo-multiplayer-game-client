using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreboardItem : ListViewItem {

    public override int ID {
        get { return id; }
        set { id = value; SetIndex (id); }
    }
    public override Action<int> OnSelected {
        get { return onSelected; }
        set { onSelected = value; }
    }
    public Text textIndex;
    public Text textName;
    public Text textScore;
    public Image background;
    private Action<int> onSelected;
    private int id;

    public void SetScore (ScoreInfo info) {
        textName.text = info.name;
        textScore.text = $"{info.score}";
    }
    public void SetScolor (Color color) {
        // textName.color = textIndex.color = textScore.color = color;
        background.color = color;
    }
    public void SetIndex (int index) {
        textIndex.text = $"{index}.";
    }
}