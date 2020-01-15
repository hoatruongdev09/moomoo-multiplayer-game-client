using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour {
    public SpriteRenderer graphic;
    public SpriteRenderer healthBar;
    public Transform healthBarHolder;
    private float lastRotation;
    private Vector3 lastPosition;

    private void Update () {
        transform.position = Vector3.Lerp (transform.position, lastPosition, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, 0, lastRotation), 10 * Time.deltaTime);

        healthBarHolder.position = transform.position + new Vector3 (0, -2);
        healthBarHolder.rotation = Quaternion.identity;
    }
    public void SyncHealthPoint (int hp) {
        this.healthBar.transform.LeanScaleX ((float) hp / 100f, .15f);
    }
    public void SyncRotation (float angle) {
        this.lastRotation = angle;
    }
    public void SyncPosition (Vector3 position) {
        this.lastPosition = position;
    }
}