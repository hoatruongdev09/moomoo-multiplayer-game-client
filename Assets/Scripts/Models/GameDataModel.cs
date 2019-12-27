using System;
using UnityEngine;

[Serializable]
public class GameDataModel {
    public int maxPlayer;
    public ResourceInfoModel[] resource;
    public InitPlayerModel[] players;
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