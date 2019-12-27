using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Character Character { get { return character; } }

    public Character character;
    private Vector3 lastPosition;
    private float lastRotattion;

    private void Update () {
        transform.position = Vector3.Lerp (transform.position, lastPosition, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, 0, lastRotattion), 20 * Time.deltaTime);
    }

    public void SyncPosition (Vector3 position) {
        lastPosition = position;
    }
    public void SyncRotation (float angle) {
        lastRotattion = angle * Mathf.Rad2Deg;
    }
    public void TriggerAttack (WeaponType wpType) {
        switch (wpType) {
            case WeaponType.Melee:
                character.AnimateMeleeAttack ();
                break;
            case WeaponType.Range:

                break;
        }
    }
    public void SwapItem (GameObject item) {
        character.ChangeItem (item);
    }
}

public enum WeaponType {
    Melee = 0,
    Range = 1
}