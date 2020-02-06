using System;

[Serializable]
public class ClanInfoModel {
    public int id;
    public string name;
}

[Serializable]
public class OpenClanModel {
    public ClanInfoModel[] allClan;
}

[Serializable]
public class OpenMemberModel {
    public ClanMemberModel[] allMember;
}

[Serializable]
public class ClanMemberModel {
    public int id;
    public int idClan;
    public int role;
}

[Serializable]
public class VisualClanMemberModel : ClanMemberModel {
    public string name;
}

[Serializable]
public class ClanKickMemberModel {
    public int[] id;
}

[Serializable]
public class RemoveClanModel {
    public int id;
}

[Serializable]
public class RequestJoinClanModel {
    public int id;
}