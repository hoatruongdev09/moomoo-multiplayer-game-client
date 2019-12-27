using System;
[Serializable]
public class SyncTransformModel {
    public SyncPostionModel[] pos;
    public SyncRotationModel[] rot;
}

[Serializable]
public class SyncPostionModel {
    public int id;
    public PositionModel pos;
}

[Serializable]
public class SyncRotationModel {
    public int id;
    public float angle;
}