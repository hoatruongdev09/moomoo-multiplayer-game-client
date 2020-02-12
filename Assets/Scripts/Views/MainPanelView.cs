using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelView : MonoBehaviour {
    public JoinPanelViewController joinPanelViewController;
    public MainErrorPanelViewController errorPanelViewController;
    public ConnectingIndicateController connectingIndicateController;
    public Image imgLogo;
    public CanvasGroup canvasGroup;
    public Button buttonQuit;
}