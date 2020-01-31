using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField] private PlayerManager playerManager;
    private int fromId;
    private GameObject temp;
    private void Start () {
        playerManager = FindObjectOfType<PlayerManager> ();
        fromId = GetComponent<Structure> ().fromId;
    }

    private void Update () {
        temp = FindClosestTarget ();
        if (temp != null) {
            Vector3 direct = temp.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, 0, Mathf.Atan2 (direct.y, direct.x) * Mathf.Rad2Deg), 10 * Time.deltaTime);
        }
    }

    private GameObject FindClosestTarget () {
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;
        float distance;
        for (int i = 0; i < playerManager.players.Length; i++) {
            if (i != fromId && playerManager.players[i] != null) {
                distance = (playerManager.players[i].transform.position - transform.position).sqrMagnitude;
                if (distance < closestDistance && distance <= 30 * 30) {
                    closestDistance = distance;
                    closestTarget = playerManager.players[i].gameObject;
                }
            }
        }
        return closestTarget;
    }
}