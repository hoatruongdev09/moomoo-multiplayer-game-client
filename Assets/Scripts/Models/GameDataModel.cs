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
    public string hat;
    public string acc;
    public string itemId;
    public int hp;
    public PositionModel pos;
}

[Serializable]
public class PositionModel {
    public float x;
    public float y;
    public PositionModel (float x, float y) {
        this.x = x;
        this.y = y;
    }
    public PositionModel () {
        x = y = 0;
    }
    public Vector2 ToVector2 () {
        return new Vector2 (x, y);
    }
    public Vector3 ToVector3 () {
        return new Vector3 (x, y);
    }
    public static PositionModel FromVector3 (Vector3 vector) {
        return new PositionModel (vector.x, vector.y);
    }
    public static PositionModel FromVector2 (Vector2 vector) {
        return new PositionModel (vector.x, vector.y);
    }
}