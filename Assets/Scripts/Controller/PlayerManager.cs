using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public SpawnController spawnController;
    public PlayerController[] players;

    public PlayerController localPlayer;

    public Stack inactivedPlayers;

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
        if (players[model.id] != null) {
            spawnController.SpawnDamagePopUp (model.hp - players[model.id].GetLastHealthPoint (), players[model.id].transform.position);
            players[model.id].SyncHealthPoint (model.hp);
        }
    }
}