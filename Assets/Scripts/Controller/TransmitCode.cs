public class SocketCode {
    public string OnConnect = "oncon";
    public string OnRequestJoin = "onJoin";
    public string OnFailedToConnect = "onConFailed";
    public string OnPing = "ping";
    public string Error = "err";
}
public class GameCode {
    public string gameData = "gameData";
    public string receivedData = "rvGameData";
    public string spawnPlayer = "spwnPlayer";

    public string playerQuit = "plQuit";

    public string syncLookDirect = "syncLook";
    public string syncMoveDirect = "syncMove";
    public string syncTransform = "syncTrsform";
    public string triggerAttack = "attk";
    public string triggerAutoAttack = "autoAttk";
    public string playerHit = "phit";
    public string playerDie = "pdie";
    public string playerStatus = "pstt";
    public string switchItem = "swtItm";
    public string spawnnStructures = "spwnStrc";
    public string removeStructures = "rmvStrc";
    public string upgradeItem = "ugdItem";

    public string syncItem = "syncItem";

    public string createProjectile = "prjti0";
    public string removeProjectile = "prjti1";
    public string syncPositionProjectile = "prjtiUp";
    public string playerChat = "chat";
    public string syncNpcTransform = "npcTrans";
    public string syncNpcHP = "npcHP";
    public string syncNpcDie = "npcDie";
    public string spawnNpc = "spwnNpc";
    public string scoreBoard = "scrBoard";
    public string syncShop = "shopSync";
    public string shopSelectItem = "shopSltItem";
}
public class ClanCode {
    public string createClan = "clCreate";
    public string removeClan = "clRemove";
    public string requestJoin = "clRequestJoin";
    public string joinClan = "clJoin";
    public string kickMember = "clKick";
    public string member = "clMember";
    public string listClan = "clList";
}