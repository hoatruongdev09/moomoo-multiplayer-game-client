using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class GameController : MonoBehaviour, ISocketControllerDelegate, IControllerDelegate, IControllerDatasource {
    public SocketController socketController;
    public UIController uIController;
    public SpawnController spawnController;
    public PlayerManager playerManager;
    public StructuresController structuresController;
    public CameraController cameraController;
    [Header ("CONTROLLER")]
    public KeyboardController keyboardController;
    public VirtualGamePadController virtualGamePadController;
    private GameObject[] listResources;
    private ConnectServerModel connectInfo;
    [SerializeField] private Controller[] inputController;
    public GameDataModel gameInfo;

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
        var temp = JSON.Parse (data);
        // Debug.Log ($"{temp[1].ToString()}");
        GameDataModel model = JsonUtility.FromJson<GameDataModel> (temp[1].ToString ());
        gameInfo = model;
        listResources = spawnController.SpawnResources (model.resource);
        playerManager.InitPlayers (model.maxPlayer);
        PlayerController[] players = spawnController.SpawnPlayers (model.players, playerManager.players);
        playerManager.SetPlayers (players);
        spawnController.SpawnStructures (model.structures, structuresController);
        socketController.SocketEmit (socketController.gameCode.receivedData, new JSONObject (true));
    }

    public void OnSpawnPlayer (string data) {
        var temp = JSON.Parse (data);
        // Debug.Log ($"spawn plaer {temp[1].ToString()}");
        PlayerJoinGameModel model = JsonUtility.FromJson<PlayerJoinGameModel> (temp[1].ToString ());

        PlayerController pc = spawnController.SpawnPlayer (model.id, model.name, model.skinId, model.pos.ToVector3 ());
        playerManager.SetPlayer (pc, model.id);

        if (model.clientId == connectInfo.id) {
            cameraController.SetForcus (pc.transform);
            playerManager.localPlayer = pc;
        }
        uIController.OnJoinGame ();
    }

    public void OnPlayerQuit (string data) {
        Debug.Log ($"On Player quit: {data}");
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
        if (model.id == connectInfo.id) {
            uIController.OnGameOver ();
        }
    }

    public void OnPlayerHit (string data) {
        Debug.Log ($"Player hit: {data}");
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
        var temp = JSON.Parse (data);
        CreateStructureModel model = JsonUtility.FromJson<CreateStructureModel> (temp[1].ToString ());
        structuresController.AddStructure (spawnController.SpawnStructure (model));
    }
    public void OnDestroyStructure (string data) {
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
        Debug.Log ($"items sync: {data}");
        var temp = JSON.Parse (data);
        SyncItemModel model = JsonUtility.FromJson<SyncItemModel> (temp[1].ToString ());
        uIController.SyncItemTray (model);
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