using System;

[Serializable]
public class ConnectServerModel {
    public int id;
    public GameStatusModel[] listGame;
}

[Serializable]
public class GameStatusModel {
    public int id;
    public string name;
    public bool full;
}

[Serializable]
public class FailedToConnectModel {
    public string reason;
}