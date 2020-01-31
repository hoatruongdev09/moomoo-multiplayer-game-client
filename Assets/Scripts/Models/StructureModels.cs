[System.Serializable]
public class CreateStructureModel {
    public int id;
    public int fromId;
    public string itemId;
    public PositionModel pos;
    public float rot;
}

[System.Serializable]
public class RemoveStructureModel {
    public int[] id;
}