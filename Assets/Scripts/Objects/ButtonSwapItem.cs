using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSwapItem : MonoBehaviour {
    public Sprite[] upgrades;
    public string code;
    public Image images;
    public delegate void OnChooseButton (string code);
    public OnChooseButton click;

    private Button button;

    private void Start () {
        button = GetComponent<Button> ();
        button.onClick.AddListener (OnClick);
    }
    public void UpgradeImage (int id) {
        if (id >= upgrades.Length) {
            return;
        }
        if (upgrades[id] == null) {
            return;
        }
        images.sprite = upgrades[id];
    }
    private void OnClick () {
        if (click != null) {
            click (code);
        }
    }
}