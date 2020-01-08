using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ItemTray : MonoBehaviour {
    public HorizontalLayoutGroup layoutGroup;

    public ButtonSwapItem[] mainWeaponTray;
    public ButtonSwapItem[] subWeaponTray;
    public ButtonSwapItem[] foodTray;
    public ButtonSwapItem[] wallTray;
    public ButtonSwapItem[] pikeTray;
    public ButtonSwapItem[] windmillTray;

    public ButtonSwapItem[] spawnResourceTray;
    public ButtonSwapItem[] miscTray;

    public void RegisterDelegate (ButtonSwapItem.OnChooseButton clickAction) {
        List<ButtonSwapItem> temp = new List<ButtonSwapItem> ();
        temp.AddRange (mainWeaponTray);
        temp.AddRange (subWeaponTray);
        temp.AddRange (foodTray);
        temp.AddRange (wallTray);
        temp.AddRange (pikeTray);
        temp.AddRange (windmillTray);
        temp.AddRange (spawnResourceTray);
        temp.AddRange (miscTray);

        foreach (ButtonSwapItem item in temp) {
            item.click = clickAction;
        }
    }
    public List<ButtonSwapItem> AllItems () {
        List<ButtonSwapItem> temp = new List<ButtonSwapItem> ();
        temp.AddRange (mainWeaponTray);
        temp.AddRange (subWeaponTray);
        temp.AddRange (foodTray);
        temp.AddRange (wallTray);
        temp.AddRange (pikeTray);
        temp.AddRange (windmillTray);
        temp.AddRange (spawnResourceTray);
        temp.AddRange (miscTray);
        return temp;
    }
}