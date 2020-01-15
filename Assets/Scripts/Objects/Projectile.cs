using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public int id;
    public SpriteRenderer graphic;
    private Vector3 lastPosition;
    private bool isDisable = false;
    private void Update () {
        if (!isDisable)
            transform.position = Vector3.Lerp (transform.position, lastPosition, 10 * Time.deltaTime);
    }
    public void SyncPosition (Vector3 position) {
        lastPosition = position;
    }
    public void Destroy () {
        StartCoroutine (DelayToDestroy (3));
        isDisable = true;
    }
    private void OnTriggerStay2D (Collider2D other) {
        Debug.Log ($"stay: {other.tag}");
        if (other.tag == "playerBody" && isDisable) {
            GetComponent<Rigidbody2D> ().isKinematic = true;
            transform.SetParent (other.transform);
        }
    }

    private IEnumerator DelayToDestroy (float time) {
        yield return new WaitForSeconds (time);
        LeanTween.value (gameObject, 1, 0, .5f).setOnUpdate ((float value) => {
            graphic.color = new Color (graphic.color.r, graphic.color.g, graphic.color.b, value);
        }).setOnComplete (() => {
            Destroy (gameObject);
        });
    }
}