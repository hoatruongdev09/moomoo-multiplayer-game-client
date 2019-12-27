using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public PlayerController[] players;

    public PlayerController localPlayer;

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
        Destroy (go);
    }

    public void SyncTransform (SyncTransformModel model) {
        foreach (SyncPostionModel posMod in model.pos) {
            players[posMod.id].SyncPosition (posMod.pos.ToVector3 ());
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
}