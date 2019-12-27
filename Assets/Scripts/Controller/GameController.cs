using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class GameController : MonoBehaviour, ISocketControllerDelegate, IControllerDelegate, IControllerDatasource {
    public SocketController socketController;
    public UIController uIController;
    public SpawnController spawnController;
    public PlayerManager playerManager;
    public CameraController cameraController;
    public Controller[] inputController;
    public GameObject[] listResources;

    [SerializeField] private ConnectServerModel connectInfo;
    public GameDataModel gameInfo;

    private void Start () {
        socketController.Delegate = this;
        foreach (Controller ctrler in inputController) {
            ctrler.Datasource = this;
            ctrler.Delegate = this;
        }
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

    public void OnTriggerAttack () {
        if (!playerManager.localPlayer) {
            return;
        }
        socketController.SocketEmit (socketController.gameCode.triggerAttack);
    }

    #endregion
}