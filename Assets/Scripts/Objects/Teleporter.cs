using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public SpriteRenderer effect;
    private void OnTriggerStay2D (Collider2D other) {
        if (other.tag == "playerBody") {
            ShowEffect ();
            effect.transform.Rotate (0, 0, 1000 * Time.deltaTime);
        }
    }
    private void OnTriggerExit2D (Collider2D other) {
        StartCoroutine (DelayHideEffect (3));
    }
    private void ShowEffect () {
        if (effect.color.a == 1) {
            return;
        }
        if (LeanTween.isTweening (effect.gameObject)) {
            return;
        }
        LeanTween.value (effect.gameObject, 0, 1, .5f).setOnUpdate ((float value) => {
            effect.color = new Color (effect.color.r, effect.color.g, effect.color.b, value);
        });
    }
    private IEnumerator DelayHideEffect (float time) {
        yield return new WaitForSeconds (time);
        if (!LeanTween.isTweening (effect.gameObject)) {
            LeanTween.value (effect.gameObject, 1, 0, .5f).setOnUpdate ((float value) => {
                effect.color = new Color (effect.color.r, effect.color.g, effect.color.b, value);
            });
        }

    }
}