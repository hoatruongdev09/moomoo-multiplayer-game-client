using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ClanManager : MonoBehaviour {
    public PlayerManager playerManager;
    public GameController gameController;
    public List<ClanInfoModel> listClan;
    public List<ClanMemberModel> listMember;
    public List<int> requestJoinQueue;
    public int localClanId = -1;
    public void Start () {
        requestJoinQueue = new List<int> ();
    }
    public void SetListClan (ClanInfoModel[] list) {
        listClan = list.ToList ();
    }
    public void SetListMember (ClanMemberModel[] list) {
        listMember = list.ToList ();
        foreach (ClanMemberModel model in listMember) {
            if (playerManager.GetPlayer (model.id) != null) {
                playerManager.GetPlayer (model.id).ShowClanName (GetClanById (model.idClan).name);
            }
        }
    }
    public VisualClanMemberModel[] GetListMemberLocalClan () {
        ClanMemberModel[] temp = listMember.Where ((member) => member.idClan == localClanId).ToArray ();
        VisualClanMemberModel[] result = new VisualClanMemberModel[temp.Length];
        for (int i = 0; i < result.Length; i++) {
            result[i] = new VisualClanMemberModel ();
            result[i].id = temp[i].id;
            result[i].idClan = temp[i].idClan;
            result[i].name = playerManager.GetPlayer (temp[i].id).character.name;
            result[i].role = temp[i].role;
        }
        return result;
    }
    public bool IsMasterLocalClan () {
        ClanMemberModel[] temp = listMember.Where ((member) => member.idClan == localClanId).ToArray ();
        foreach (ClanMemberModel member in temp) {
            if (member.id == gameController.GetLocalConnectId () && member.role == 1) {
                return true;
            }
        }
        return false;
    }
    public ClanInfoModel GetLocalClan () {
        if (localClanId == -1) {
            return null;
        }
        foreach (ClanInfoModel clan in listClan) {
            if (clan.id == localClanId) {
                return clan;
            }
        }
        return null;
    }
    public void AddClan (ClanInfoModel clan) {
        listClan.Add (clan);
    }
    public void AddRequestJoinClan (int id) {
        foreach (int i in requestJoinQueue) {
            if (i == id) {
                return;
            }
        }
        requestJoinQueue.Add (id);
    }
    public int PopARequest () {
        if (requestJoinQueue.Count == 0) {
            return -1;
        }
        int id = requestJoinQueue[0];
        requestJoinQueue.RemoveAt (0);
        return id;
    }
    public int GetCurrentJoinRequest () {
        if (requestJoinQueue.Count == 0) {
            return -1;
        }
        int id = requestJoinQueue[0];
        return id;
    }
    public void JoinClan (ClanMemberModel model) {
        for (int i = 0; i < listMember.Count; i++) {
            if (listMember[i].id == model.id) {
                listMember[i] = model;
                return;
            }
        }
        listMember.Add (model);
        if (playerManager.GetPlayer (model.id)) {
            playerManager.GetPlayer (model.id).ShowClanName (GetClanById (model.idClan).name);
        }
    }
    public void GroupLocalClanMember () {
        playerManager.GroupAllMemberClan (GetListMemberLocalClan ().Select ((member) => {
            return member.id;
        }).ToArray ());
    }

    public void KickMember (int id) {
        listMember.RemoveAll ((member) => member.id == id);
        playerManager.players[id].ShowClanName ("");
    }
    public void KickMembers (int[] ids) {
        for (int i = 0; i < listMember.Count; i++) {
            if (ids.Contains (listMember[i].id)) {
                if (playerManager.GetPlayer (listMember[i].id)) {
                    playerManager.RemovePlayerFromClan (listMember[i].id);
                    // playerManager.GetPlayer (listMember[i].id).ShowClanName ("");
                    // playerManager.GetPlayer (listMember[i].id).character.SetRemoteHealthBarColor ();
                }
                listMember.RemoveAt (i);
                i--;
            }
        }
    }
    public void KickMembersInClan (int idClan) {
        for (int i = 0; i < listMember.Count; i++) {
            if (listMember[i].idClan == idClan) {
                if (playerManager.GetPlayer (listMember[i].id)) {
                    playerManager.RemovePlayerFromClan (listMember[i].id);
                    // playerManager.GetPlayer (listMember[i].id).ShowClanName ("");
                    // playerManager.GetPlayer (listMember[i].id).character.SetRemoteHealthBarColor ();
                }
                listMember.RemoveAt (i);
                i--;
            }
        }
    }
    public void RemoveClan (int id) {
        listClan.RemoveAll ((clan) => clan.id == id);
        KickMembersInClan (id);
    }
    public ClanMemberModel[] GetMemberOfClan (int id) {
        return listMember.Where ((member) => member.id == id).ToArray ();
    }
    public ClanInfoModel GetClanById (int id) {
        return listClan.Single ((clan) => clan.id == id);
    }
}