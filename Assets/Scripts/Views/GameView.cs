using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameView : MonoBehaviour {
    public PlayerInfoText woodInfo;
    public PlayerInfoText foodInfo;
    public PlayerInfoText stoneInfo;
    public PlayerInfoText goldInfo;
    public PlayerInfoText killInfo;

    public Button buttonChat;
    public Button buttonShop;
    public Button buttonClan;
    public Button scoreBoard;

    public CanvasGroup canvasGroup;
    public GameObject virtualGamePad;
    public XpBar xpBar;

    public void UpdateInfo (PlayerStatusModel model) {
        woodInfo.SetText (model.wood.ToString ());
        foodInfo.SetText (model.food.ToString ());
        stoneInfo.SetText (model.stone.ToString ());
        goldInfo.SetText (model.gold.ToString ());
        killInfo.SetText (model.kills.ToString ());
        xpBar.SetXp (model.xp);
        xpBar.SetLevel (model.level);
    }
}