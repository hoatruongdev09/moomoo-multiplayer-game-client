using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class NpcManager : MonoBehaviour {
    public NpcController[] listNpc;

    public void InitListNpc (int npcCount) {
        listNpc = new NpcController[npcCount];
    }

    public void SyncTransform (SyncTransformNPCModels model) {
        foreach (NpcTransformModel info in model.pos) {
            if (listNpc[info.id] != null) {
                // Debug.Log ($"{info.id} position: {info.pos.ToVector3()}");
                listNpc[info.id].SyncPosition (info.pos.ToVector3 ());
                listNpc[info.id].SyncRotation (info.rot * Mathf.Rad2Deg);
            }
        }
    }
    public void AddNpc (int id, NpcController npc) {
        listNpc[id] = npc;
    }

    public void RemoveNpc (NpcDieModel model) {
        if (listNpc[model.id] != null) {
            Destroy (listNpc[model.id].gameObject);
        }
    }

    public void SyncHp (SyncNpcHpModel model) {
        foreach (NpcHpModel info in model.data) {
            if (listNpc[info.id]) {
                listNpc[info.id].SyncHealthPoint (info.hp);
            }
        }
    }
}