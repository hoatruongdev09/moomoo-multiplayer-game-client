using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ItemTrayController : MonoBehaviour {
    public ItemTray itemTray;
    public Transform trayHolder;
    private void Start () {
        UpdateSize ();
    }
    public void SyncItemTray (string[] codes) {
        List<ButtonSwapItem> allItems = itemTray.AllItems ();
        allItems.ForEach ((item) => {
            if (codes.Contains (item.code)) {
                item.gameObject.SetActive (true);
            } else {
                item.gameObject.SetActive (false);
            }
        });
        UpdateSize ();
    }
    private void UpdateSize () {
        int activeItemInTray = 0;
        foreach (Transform t in trayHolder) {
            if (t.gameObject.activeSelf) {
                activeItemInTray++;
            }
        }
        (transform as RectTransform).LeanSize (new Vector2 (activeItemInTray * (100 + 10) + 20, 130), .1f);
    }
    public void RegisterDelegateForButton (System.Action<string> action) {
        itemTray.RegisterDelegate (new ButtonSwapItem.OnChooseButton (action));
    }
}