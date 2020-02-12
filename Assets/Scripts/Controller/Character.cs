using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour {
    public Transform healthBarHolder;
    public SpriteRenderer healthBar;
    public TextMeshPro textName;
    public SpriteRenderer[] bodyparts;
    public Transform hands;
    public Transform itemHolder;
    public GameObject currentItem;
    public Transform hatHolder;
    public GameObject currentHat;
    public Transform accessoryHolder;
    public GameObject currentAccessory;
    public Color color;
    private string charName;
    private string clanName = "";
    private Coroutine delayShowName;
    private bool isDelayShowName;
    private void Update () {
        healthBarHolder.position = transform.position + new Vector3 (0, -2);
        textName.transform.position = transform.position + new Vector3 (0, 3);

        healthBarHolder.rotation = Quaternion.identity;
        textName.transform.rotation = Quaternion.identity;
    }
    public void AnimateMeleeAttack () {
        if (LeanTween.isTweening (hands.gameObject)) {
            LeanTween.cancel (hands.gameObject);
        }
        LeanTween.value (hands.gameObject, 90, 180, .1f).setOnUpdate ((float value) => {
            hands.localRotation = Quaternion.Euler (0, 0, value);
        }).setEaseInSine ().setLoopPingPong (1);
    }
    public void ChangeColor (Color color) {
        this.color = color;
        foreach (SpriteRenderer sp in bodyparts) {
            sp.color = color;
        }
    }
    public void SyncHealthPoint (int hp) {
        this.healthBar.transform.LeanScaleX ((float) hp / 100f, .15f);
    }
    public void SetHealbarColor (Color color) {
        healthBar.color = color;
    }
    public void SetLocalHealthBarColor () {
        Color color;
        ColorUtility.TryParseHtmlString ("#18dcff", out color);
        SetHealbarColor (color);
    }
    public void SetRemoteHealthBarColor () {
        //EA8685
        Color color;
        ColorUtility.TryParseHtmlString ("#EA8685", out color);
        SetHealbarColor (color);
    }
    public void SetName (string name) {
        this.charName = name;
        gameObject.name = $"{name}";
        if (string.IsNullOrEmpty (clanName)) {
            textName.text = $"{name}";
        } else {
            textName.text = $"[{clanName}] {name}";
        }
    }
    public void SetClanName (string clanName) {
        this.clanName = clanName;
        SetName (charName);
    }
    public void Chat (string text) {
        LeanTween.alphaText (textName.rectTransform, 0, .3f).setOnComplete (() => {
            textName.text = text;
            LeanTween.alphaText (textName.rectTransform, 1, .3f).setDelay (.1f);
        });
        if (isDelayShowName) {
            if (delayShowName != null) {
                StopCoroutine (delayShowName);
            }
            delayShowName = StartCoroutine (DelayToShowName ());
        } else {
            delayShowName = StartCoroutine (DelayToShowName ());
        }
    }
    private IEnumerator DelayToShowName () {
        isDelayShowName = true;
        yield return new WaitForSeconds (3);
        LeanTween.alphaText (textName.rectTransform, 0, .3f).setOnComplete (() => {
            textName.text = gameObject.name;
            LeanTween.alphaText (textName.rectTransform, 1, .3f).setDelay (.1f);
            isDelayShowName = false;
        });

    }
    public void ChangeItem (GameObject item) {
        Destroy (currentItem);
        currentItem = Instantiate (item, itemHolder);
    }
    public void ChangeHat (GameObject item) {
        Destroy (currentHat);
        currentHat = Instantiate (item, hatHolder);
        currentHat.transform.localPosition = Vector3.zero;
        currentHat.transform.localRotation = Quaternion.Euler (0, 0, 0);
    }
    public void ChangeAccessory (GameObject item) {
        Destroy (currentAccessory);
        currentAccessory = Instantiate (item, accessoryHolder);
        currentAccessory.transform.localPosition = Vector3.zero;
        currentAccessory.transform.localRotation = Quaternion.Euler (0, 0, 0);
    }
}