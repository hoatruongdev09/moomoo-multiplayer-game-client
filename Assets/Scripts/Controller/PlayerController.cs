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
                MeleeAttackEffect ();
                break;
            case WeaponType.Range:

                break;
        }
    }
    public int GetLastHealthPoint () {
        return Mathf.FloorToInt (character.healthBar.transform.localScale.x * 100);
    }
    public void SyncHealthPoint (int hp) {
        character.SyncHealthPoint (hp);
    }
    public void SwapItem (GameObject item) {
        character.ChangeItem (item);
    }
    public void ShowChat (string text) {
        character.Chat (text);
    }
    private void MeleeAttackEffect () {
        Collider2D[] colls = Physics2D.OverlapBoxAll (transform.position - transform.right, new Vector2 (1, 3), transform.rotation.eulerAngles.z);
        foreach (Collider2D col in colls) {
            if (col.GetComponent<Structure> ()) {
                col.GetComponent<Structure> ().Shake ();
            } else if (col.GetComponent<ResourceObject> ()) {
                col.GetComponent<ResourceObject> ().Shake ();
            }
        }
    }
    private void OnDrawGizmos () {
        Gizmos.DrawRay (transform.position, -transform.right);
        Gizmos.DrawWireCube (transform.position - transform.right, new Vector2 (1, 3));
    }

}

public enum WeaponType {
    Melee = 0,
    Range = 1
}