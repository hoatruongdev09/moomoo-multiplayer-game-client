using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemTrayController : MonoBehaviour {
    public ItemTray itemTray;
    public Transform trayHolder;
    private void Start () {
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