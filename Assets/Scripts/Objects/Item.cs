using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public string id;
    public Sprite[] upgrades;
    public SpriteRenderer spriteRenderer;

    public void UpgradeImage (int id) {
        if (id >= upgrades.Length) {
            return;
        }
        if (upgrades[id] == null) {
            return;
        }
        spriteRenderer.sprite = upgrades[id];
    }
}