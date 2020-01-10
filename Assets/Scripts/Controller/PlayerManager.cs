using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public SpawnController spawnController;
    public PlayerController[] players;

    public PlayerController localPlayer;

    public Stack inactivedPlayers;
    public UIController uIController;

    private void Start () {
        inactivedPlayers = new Stack ();
    }
    public void InitPlayers (int playerCount) {
        players = new PlayerController[playerCount];
    }

    public void SetPlayers (PlayerController[] players) {
        this.players = players;
    }
    public void SetPlayer (PlayerController player, int id) {
        players[id] = player;
    }

    public void RemovePlayer (int id) {
        GameObject go = players[id].gameObject;
        players[id] = null;
        go.gameObject.SetActive (false);
        inactivedPlayers.Push (go);
    }

    public void SyncTransform (SyncTransformModel model) {
        foreach (SyncPostionModel posMod in model.pos) {
            players[posMod.id].SyncPosition (posMod.pos.ToVector3 ());
            uIController.UpdatePositionPlayer (posMod.id, posMod.pos.ToVector3 ());
        }
        foreach (SyncRotationModel rotMod in model.rot) {
            players[rotMod.id].SyncRotation (rotMod.angle);
        }
    }

    public void TriggerPlayerAttack (TriggerAttackModel model) {
        if (players[model.idGame] != null) {
            players[model.idGame].TriggerAttack ((WeaponType) model.type);
        }
    }

    public void PlayerDie (PlayerDieModel model) {
        if (players[model.id] != null) {
            RemovePlayer (model.id);
        }
    }

    public void PlayerHit (PlayerHitModel model) {
        foreach (HitInfoModel info in model.data) {
            if (players[info.id] != null) {
                spawnController.SpawnDamagePopUp (info.hp - players[info.id].GetLastHealthPoint (), players[info.id].transform.position);
                players[info.id].SyncHealthPoint (info.hp);
            }
        }

    }
    public void PlayerSwitchItem (SwitchItemModel model) {
        if (players[model.id] != null) {
            GameObject temp = spawnController.CreateItem (model.item);
            if (temp != null) {
                players[model.id].SwapItem (spawnController.CreateItem (model.item));
            } else {
                Debug.Log ("Switch item failed");
            }
        }
    }
}