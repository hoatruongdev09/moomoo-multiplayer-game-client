using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public SpawnController spawnController;
    public PlayerController[] players;

    public PlayerController localPlayer;

    // public Stack inactivedPlayers;
    public UIController uIController;

    // private void Start () {
    //     inactivedPlayers = new Stack ();
    // }
    public void InitPlayers (int playerCount) {
        players = new PlayerController[playerCount];
    }

    public void SetPlayers (PlayerController[] players) {
        this.players = players;
    }
    public void SetPlayer (PlayerController player, int id) {
        players[id] = player;
    }
    public PlayerController GetPlayer (int id) {
        return players[id];
    }

    public void RemovePlayer (int id) {
        GameObject go = players[id].gameObject;
        players[id] = null;
        Destroy (go);
        // go.gameObject.SetActive (false);
        // inactivedPlayers.Push (go);
    }
    public void GroupAllMemberClan (int[] ids) {
        foreach (int id in ids) {
            if (players[id] != null) {
                players[id].character.SetLocalHealthBarColor ();
                uIController.AddPlayerToMap (id);
            }
        }
    }
    public void RemovePlayerFromClan (int id) {
        if (players[id] != null) {
            players[id].character.SetRemoteHealthBarColor ();
            uIController.RemovePlayerFromMap (id);
        }
    }
    public void SyncTransform (SyncTransformModel model) {
        foreach (SyncPostionModel posMod in model.pos) {
            if (players[posMod.id]) {
                players[posMod.id].SyncPosition (posMod.pos.ToVector3 ());
                uIController.UpdatePositionPlayer (posMod.id, posMod.pos.ToVector3 ());
            }
        }
        foreach (SyncRotationModel rotMod in model.rot) {
            if (players[rotMod.id]) {
                players[rotMod.id].SyncRotation (rotMod.angle);
            }
        }
        HideUnSeePlayers (model.rot);
    }
    private void HideUnSeePlayers (SyncRotationModel[] viewAblePlayers) {
        List<int> viewAbleIDs = viewAblePlayers.Select (p => { return p.id; }).ToList ();
        for (int i = 0; i < players.Length; i++) {
            if (players[i] != null) {
                if (viewAbleIDs.Contains (i)) {
                    players[i].gameObject.SetActive (true);
                } else {
                    players[i].gameObject.SetActive (false);
                }
            }
        }

    }
    public void TriggerPlayerAttack (TriggerAttackModel model) {
        Debug.Log ("Trigger attack");
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
    public void PlayerSyncItemShop (SyncEquipItemShop model) {
        if (players[model.id] != null) {
            if (string.IsNullOrEmpty (model.hat)) {
                players[model.id].SyncEquipedHat (null);
            } else {
                Debug.Log ("equip hat");
                players[model.id].SyncEquipedHat (spawnController.CreateHat (model.hat));
            }
            if (string.IsNullOrEmpty (model.acc)) {
                players[model.id].SyncEquipedAccessory (null);
            } else {
                Debug.Log ("equip accessory");
                players[model.id].SyncEquipedAccessory (spawnController.CreateAccessory (model.acc));
            }
        }
    }

    public void ShowPlayerChat (ChatModel model) {
        if (players[model.id] != null) {
            players[model.id].ShowChat (model.text);
        }
    }
}