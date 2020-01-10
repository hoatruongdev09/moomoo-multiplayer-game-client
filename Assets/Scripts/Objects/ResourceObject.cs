using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour {
    private Vector3 initPosition;
    private void Start () {
        initPosition = transform.position;
    }
    public void Shake () {
        if (LeanTween.isTweening (gameObject)) {
            LeanTween.cancel (gameObject);
        }
        LeanTween.value (gameObject, 0, 1, .2f).setOnUpdate ((float value) => {
            transform.position = initPosition + new Vector3 (Random.Range (0, 0.5f) * value, Random.Range (0, 0.5f) * value);
        }).setOnComplete (() => {
            transform.position = initPosition;
        });
    }
}