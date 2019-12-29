using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewController : MonoBehaviour {

    private void OnEnable () {
        Show ();
    }

    public void Show () {

    }
    public void Hide () {
        gameObject.SetActive (false);
    }
}