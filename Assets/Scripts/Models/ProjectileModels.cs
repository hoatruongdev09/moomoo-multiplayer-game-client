using System;

[Serializable]
public class CreateProjectileModel {
    public int id;
    public PositionModel pos;
    public float angle;
}

[Serializable]
public class RemoveProjectileModel {
    public int[] id;
}

[Serializable]
public class UpdatePositionProjectileModel {
    public ProjectilePosInfoModel[] pos;
}

[Serializable]
public class ProjectilePosInfoModel {
    public int id;
    public PositionModel pos;
}