using System;
[Serializable]
public class PlayerScoreModel {
    public ScoreInfo[] data;
}

[Serializable]
public class ScoreInfo {
    public int id;
    public string name;
    public int skinId;
    public int score;
}