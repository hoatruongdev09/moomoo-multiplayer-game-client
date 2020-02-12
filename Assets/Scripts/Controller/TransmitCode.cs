public class SocketCode {
    public string OnConnect = "s0";
    public string OnRequestJoin = "s1";
    public string OnFailedToConnect = "s2";
    public string OnPing = "s3";
    public string Error = "s4";
    public string ClientStatus = "s5";
}
public class GameCode {
    public string gameData = "g0";
    public string receivedData = "g1";
    public string spawnPlayer = "g2";

    public string playerQuit = "g3";

    public string syncLookDirect = "g4";
    public string syncMoveDirect = "g5";
    public string syncTransform = "g6";
    public string triggerAttack = "g7";
    public string triggerAutoAttack = "g8";
    public string playerHit = "g9";
    public string playerDie = "g10";
    public string playerStatus = "g11";
    public string switchItem = "g12";
    public string spawnnStructures = "g13";
    public string removeStructures = "g14";
    public string upgradeItem = "g15";

    public string syncItem = "g16";

    public string createProjectile = "g17";
    public string removeProjectile = "g18";
    public string syncPositionProjectile = "g19";
    public string playerChat = "g20";
    public string syncNpcTransform = "g21";
    public string syncNpcHP = "g22";
    public string syncNpcDie = "g23";
    public string spawnNpc = "g24";
    public string scoreBoard = "g25";
    public string syncShop = "g26";
    public string shopSelectItem = "g27";
    public string syncEquipItem = "g28";
}
public class ClanCode {
    public string createClan = "c0";
    public string removeClan = "c1";
    public string requestJoin = "c2";
    public string joinClan = "c3";
    public string kickMember = "c4";
    public string member = "c5";
    public string listClan = "c6";
}