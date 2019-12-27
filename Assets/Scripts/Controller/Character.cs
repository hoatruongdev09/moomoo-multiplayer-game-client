using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour {
    public SpriteRenderer healthBar;
    public TextMeshPro textName;
    public SpriteRenderer[] bodyparts;
    public Transform hands;
    public Transform itemHolder;
    public GameObject currentItem;
    public Color color;

    private void Update () {
        healthBar.transform.position = transform.position + new Vector3 (0, -2);
        textName.transform.position = transform.position + new Vector3 (0, 3);

        healthBar.transform.rotation = Quaternion.identity;
        textName.transform.rotation = Quaternion.identity;
    }
    public void AnimateMeleeAttack () {
        if (LeanTween.isTweening (hands.gameObject)) {
            LeanTween.cancel (hands.gameObject);
        }
        LeanTween.value (hands.gameObject, 90, 170, .2f).setOnUpdate ((float value) => {
            hands.localRotation = Quaternion.Euler (0, 0, value);
        }).setEaseInBack ().setLoopPingPong (1);
    }
    public void ChangeColor (Color color) {
        this.color = color;
        foreach (SpriteRenderer sp in bodyparts) {
            sp.color = color;
        }
    }
    public void SetName (string name) {
        gameObject.name = $"Player: {name}";
        textName.text = name;
    }
    public void ChangeItem (GameObject item) {
        Destroy (currentItem);
        currentItem = Instantiate (item, itemHolder);
    }
}