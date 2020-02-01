using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class GameController : MonoBehaviour, ISocketControllerDelegate, IControllerDelegate, IControllerDatasource {
    [Header ("CONTROLLER")]
    public SocketController socketController;
    public UIController uIController;
    public SpawnController spawnController;
    public PlayerManager playerManager;
    public StructuresController structuresController;
    public ProjectileController projectileController;
    public CameraController cameraController;
    public MapManager mapManager;
    public NpcManager npcManager;
    [Header ("INPUT CONTROLLER")]
    public KeyboardController keyboardController;
    public VirtualGamePadController virtualGamePadController;
    private GameObject[] listResources;
    [SerializeField] private ConnectServerModel connectInfo;
    [SerializeField] private Controller[] inputController;
    public GameDataModel gameInfo;
    // public UpdatePositionProjectileModel projectPositionInfo;

    private void Start () {
        socketController.Delegate = this;
        inputController = new Controller[] { keyboardController, virtualGamePadController };
        foreach (Controller ctrler in inputController) {
            ctrler.Datasource = this;
            ctrler.Delegate = this;
        }
#if UNITY_ANDROID || UNITY_IOS
        Debug.Log ("Unity android");
        keyboardController.gameObject.SetActive (false);
        virtualGamePadController.gameObject.SetActive (true);
#endif
#if UNITY_EDITOR
        Debug.Log ("Unity editor");
        keyboardController.gameObject.SetActive (true);
        virtualGamePadController.gameObject.SetActive (false);
#endif
    }

    #region SOCKET COMMAND
    public ConnectServerModel RequestConnectInfo () {
        return connectInfo;
    }
    public void RequestJoinGame (string name, int gameId, int skinId) {
        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);
        data.AddField ("name", name);
        data.AddField ("gameId", gameId);
        data.AddField ("skinId", skinId);
        socketController.SocketEmit (socketController.socketCode.OnRequestJoin, data);
    }
    public void RequestSwitchItem (string code) {
        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);
        data.AddField ("code", code);
        socketController.SocketEmit (socketController.gameCode.switchItem, data);
    }
    public void RequestUpgradeItem (string code) {
        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);
        data.AddField ("code", code);
        socketController.SocketEmit (socketController.gameCode.upgradeItem, data);
    }
    public void SendChat (string text) {
        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);
        data.AddField ("id", connectInfo.id);
        data.AddField ("text", text);
        socketController.SocketEmit (socketController.gameCode.playerChat, data);
    }
    public void RequestScore () {
        socketController.SocketEmit (socketController.gameCode.scoreBoard);
    }
    #endregion
    #region SOCKET LISTENER 
    public void OnConnect (string data) {
        var temp = JSON.Parse (data);
        // Debug.Log ($"{temp[1]}");
        ConnectServerModel model = JsonUtility.FromJson<ConnectServerModel> (temp[1].ToString ());
        connectInfo = model;
        uIController.OnConnect ();

    }

    public void OnReceiveData (string data) {
        // Debug.Log ($"INTRO DATA:\n {data}");
        var temp = JSON.Parse (data);
        GameDataModel model = JsonUtility.FromJson<GameDataModel> (temp[1].ToString ());
        gameInfo = model;
        listResources = spawnController.SpawnResources (model.resource);
        uIController.gameViewController.InitPlayerMapCount (model.maxPlayer);
        uIController.SetServerMapSize (model.mapSize.ToVector2 ());
        playerManager.InitPlayers (model.maxPlayer);
        PlayerController[] players = spawnController.SpawnPlayers (model.players, playerManager.players);
        playerManager.SetPlayers (players);
        spawnController.SpawnStructures (model.structures, structuresController);
        mapManager.SetMapSize (model.mapSize.ToVector2 ());
        mapManager.SetSnowSize (model.snowSize);
        mapManager.SetRiverSize (model.riverSize);
        npcManager.InitListNpc (model.maxNpcCount);
        spawnController.SpawnNpc (model.npc, npcManager);
        socketController.SocketEmit (socketController.gameCode.receivedData, new JSONObject (true));
    }

    public void OnSpawnPlayer (string data) {
        var temp = JSON.Parse (data);
        // Debug.Log ($"on Spawn player {data}");
        PlayerJoinGameModel model = JsonUtility.FromJson<PlayerJoinGameModel> (temp[1].ToString ());
        PlayerController pc; // = spawnController.SpawnPlayer (model.id, model.name, model.skinId, model.pos.ToVector3 ());
        if (model.clientId == connectInfo.id) {
            pc = spawnController.SpawnLocalPlayer (model.id, model.name, model.skinId, model.pos.ToVector3 ());
            cameraController.SetForcus (pc.transform);
            playerManager.localPlayer = pc;
            uIController.AddPlayerToMap (model.id);
        } else {
            pc = spawnController.SpawnPlayer (model.id, model.name, model.skinId, model.pos.ToVector3 ());
        }

        playerManager.SetPlayer (pc, model.id);

        uIController.OnJoinGame ();
    }

    public void OnPlayerQuit (string data) {
        // Debug.Log ($"On Player quit: {data}");
        var temp = JSON.Parse (data);
        PlayerQuitModel model = JsonUtility.FromJson<PlayerQuitModel> (temp[1].ToString ());
        playerManager.RemovePlayer (model.id);
    }
    public void OnSyncTransform (string data) {
        var temp = JSON.Parse (data);
        SyncTransformModel model = JsonUtility.FromJson<SyncTransformModel> (temp[1].ToString ());
        playerManager.SyncTransform (model);
    }
    public void OnTriggerAttack (string data) {
        var temp = JSON.Parse (data);
        TriggerAttackModel model = JsonUtility.FromJson<TriggerAttackModel> (temp[1].ToString ());
        playerManager.TriggerPlayerAttack (model);
    }
    public void OnPlayerDie (string data) {
        var temp = JSON.Parse (data);
        PlayerDieModel model = JsonUtility.FromJson<PlayerDieModel> (temp[1].ToString ());
        playerManager.PlayerDie (model);
        // Debug.Log ($"player die id: { model.id}");
        if (model.id == connectInfo.id) {
            uIController.OnGameOver ();
        }
    }

    public void OnPlayerHit (string data) {
        var temp = JSON.Parse (data);
        PlayerHitModel model = JsonUtility.FromJson<PlayerHitModel> (temp[1].ToString ());
        playerManager.PlayerHit (model);
    }

    public void OnPlayerStatus (string data) {
        var temp = JSON.Parse (data);
        PlayerStatusModel model = JsonUtility.FromJson<PlayerStatusModel> (temp[1].ToString ());
        // SYNC
        uIController.UpdatePlayerInfo (model);
    }

    public void OnSwitchItem (string data) {
        var temp = JSON.Parse (data);
        SwitchItemModel model = JsonUtility.FromJson<SwitchItemModel> (temp[1].ToString ());
        playerManager.PlayerSwitchItem (model);
    }
    public void OnCreateStructure (string data) {
        Debug.Log ($"spawn structure: {data}");
        var temp = JSON.Parse (data);
        CreateStructureModel model = JsonUtility.FromJson<CreateStructureModel> (temp[1].ToString ());
        structuresController.AddStructure (spawnController.SpawnStructure (model));
    }
    public void OnDestroyStructure (string data) {
        Debug.Log ($"{data}");
        var temp = JSON.Parse (data);
        RemoveStructureModel model = JsonUtility.FromJson<RemoveStructureModel> (temp[1].ToString ());
        structuresController.RemoveStructures (model.id);
    }
    public void OnUpgradeItems (string data) {
        var temp = JSON.Parse (data);
        UpgradeItemModel model = JsonUtility.FromJson<UpgradeItemModel> (temp[1].ToString ());
        uIController.ShowUpgradeItem (model);
    }
    public void OnSyncItem (string data) {
        // Debug.Log ($"items sync: {data}");
        var temp = JSON.Parse (data);
        SyncItemModel model = JsonUtility.FromJson<SyncItemModel> (temp[1].ToString ());
        uIController.SyncItemTray (model);
    }
    public void OnCreateProjectile (string data) {
        Debug.Log ($"{data}");
        var temp = JSON.Parse (data);
        CreateProjectileModel model = JsonUtility.FromJson<CreateProjectileModel> (temp[1].ToString ());
        projectileController.AddProjectile (spawnController.SpawnProjectile (model));
    }

    public void OnRemoveProjectTile (string data) {
        // Debug.Log ($"{data}");
        var temp = JSON.Parse (data);
        RemoveProjectileModel model = JsonUtility.FromJson<RemoveProjectileModel> (temp[1].ToString ());
        projectileController.RemoveProjectiles (model.id);
    }

    public void OnSyncPositionProjectile (string data) {
        // Debug.Log ($"{data}");
        var temp = JSON.Parse (data);
        UpdatePositionProjectileModel model = JsonUtility.FromJson<UpdatePositionProjectileModel> (temp[1].ToString ());
        projectileController.SyncProjectilePosition (model);
    }
    public void OnPlayerChat (string data) {
        var temp = JSON.Parse (data);
        ChatModel model = JsonUtility.FromJson<ChatModel> (temp[1].ToString ());
        playerManager.ShowPlayerChat (model);
    }
    public void OnSyncNpcTransform (string data) {
        // Debug.Log ($"npc transform: {data}");
        var temp = JSON.Parse (data);
        SyncTransformNPCModels model = JsonUtility.FromJson<SyncTransformNPCModels> (temp[1].ToString ());
        npcManager.SyncTransform (model);
    }
    public void OnNpcDie (string data) {
        Debug.Log ($"{data}");
        var temp = JSON.Parse (data);
        NpcDieModel model = JsonUtility.FromJson<NpcDieModel> (temp[1].ToString ());
        npcManager.RemoveNpc (model);
    }

    public void OnSyncNpcHp (string data) {
        // Debug.Log ($"{data}");
        var temp = JSON.Parse (data);
        SyncNpcHpModel model = JsonUtility.FromJson<SyncNpcHpModel> (temp[1].ToString ());
        npcManager.SyncHp (model);
    }
    public void OnSpawnNpc (string data) {
        var temp = JSON.Parse (data);
        SpawnNpcModel model = JsonUtility.FromJson<SpawnNpcModel> (temp[1].ToString ());
        spawnController.SpawnNpc (new SpawnNpcModel[] { model }, npcManager);
    }
    public void OnReceiveScoreBoard (string data) {
        var temp = JSON.Parse (data);
        PlayerScoreModel model = JsonUtility.FromJson<PlayerScoreModel> (temp[1].ToString ());
        uIController.ReceiveScoreInfo (model.data);
    }
    public void OnReceivePing (float ping) {
        uIController.gameViewController.UpdatePing (ping);
    }

    #endregion

    #region CONTROLLER
    public Vector3 GetLocalPlayerPosition () {
        return playerManager.localPlayer? playerManager.localPlayer.transform.position : Vector3.negativeInfinity;
    }

    public float GetLocalPlayerRotattion () {
        return playerManager.localPlayer? playerManager.localPlayer.transform.rotation.eulerAngles.z : float.NegativeInfinity;
    }

    public void OnChangeMovement (Vector2 direct) {
        if (!playerManager.localPlayer) {
            return;
        }
        if (direct.sqrMagnitude == 0) {
            socketController.SocketEmit (socketController.gameCode.syncMoveDirect, new JSONObject ("null"));
        } else {
            float angle = Mathf.Atan2 (direct.y, direct.x);
            socketController.SocketEmit (socketController.gameCode.syncMoveDirect, new JSONObject (angle));
        }
    }

    public void OnChangeLookDirect (float angle) {
        if (!playerManager.localPlayer) {
            return;
        }
        if (angle == Mathf.NegativeInfinity) {
            socketController.SocketEmit (socketController.gameCode.syncLookDirect, new JSONObject ("null"));
        } else {
            socketController.SocketEmit (socketController.gameCode.syncLookDirect, new JSONObject (angle));
        }
    }

    public void OnTriggerAttack (bool byButton) {
        if (!playerManager.localPlayer) {
            return;
        }
        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);
        data.AddField ("isbtn", byButton);
        socketController.SocketEmit (socketController.gameCode.triggerAttack, data);
    }

    public void OnTriggerAutoAttack (bool action) {
        if (!playerManager.localPlayer) {
            return;
        }
        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);
        data.AddField ("action", action);
        socketController.SocketEmit (socketController.gameCode.triggerAutoAttack, data);
    }

    #endregion
}