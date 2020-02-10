using System;
using UnityEngine;

[Serializable]
public class GameDataModel {
    public int maxPlayer;
    public int maxNpcCount;
    public PositionModel mapSize;
    public ResourceInfoModel[] resource;
    public float snowSize;
    public float riverSize;
    public InitPlayerModel[] players;
    public CreateStructureModel[] structures;
    public SpawnNpcModel[] npc;
    public ClanInfoModel[] clans;
    public ClanMemberModel[] clansMember;
    public AllShopItemModel shop;
}

[Serializable]
public class ResourceInfoModel {
    public int id;
    public int type;
    public PositionModel pos;
}

[Serializable]
public class InitPlayerModel {
    public int id;
    public string name;
    public int skinId;
    public string itemId;
    public int hp;
    public PositionModel pos;
}

[Serializable]
public class PositionModel {
    public float x;
    public float y;

    public Vector2 ToVector2 () {
        return new Vector2 (x, y);
    }
    public Vector3 ToVector3 () {
        return new Vector3 (x, y);
    }
}