using System;
[Serializable]
public class SyncTransformNPCModels {
    public NpcTransformModel[] pos;
}

[Serializable]
public class NpcTransformModel {
    public int id;
    public PositionModel pos;
    public float rot;
}

[Serializable]
public class SpawnNpcModel {
    public int id;
    public int skinId;
    public PositionModel pos;
    public float rot;
}

[Serializable]
public class NpcDieModel {
    public int id;
}

[Serializable]
public class SyncNpcHpModel {
    public NpcHpModel[] data;
}

[Serializable]
public class NpcHpModel {
    public int id;
    public int hp;
}