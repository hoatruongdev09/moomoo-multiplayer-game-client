using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SocketController : MonoBehaviour {
    public ISocketControllerDelegate Delegate {
        get { return controllerDelegate; }
        set { controllerDelegate = value; }
    }
    public string socketUrl;
    private Socket socket;
    public SocketCode socketCode;
    public GameCode gameCode;
    private ISocketControllerDelegate controllerDelegate;

    private void Start () {
        gameCode = new GameCode ();
        socketCode = new SocketCode ();
        InitSocket ();
    }
    public void SocketEmit (string eventName, object[] param) {
        socket.Emit (eventName, param);
    }
    public void SocketEmit (string eventName, JSONObject jsonObj) {
        socket.Emit (socket, eventName, jsonObj);
    }
    public void SocketEmit (string eventName) {
        socket.Emit (eventName);
    }
    private void InitSocket () {
        TimeSpan miliSecForReconnect = TimeSpan.FromMilliseconds (1000);

        var options = new SocketOptions ();
        options.ReconnectionAttempts = 3;
        options.AutoConnect = true;
        options.ReconnectionDelay = miliSecForReconnect;
        //use this option with WebGL build
        // options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;

        //Server URI
        var socketManagerRef = new SocketManager (new Uri ("http://" + socketUrl + "/socket.io/"), options);
        socket = socketManagerRef.Socket;
        ListenerRegister ();
    }
    private void ListenerRegister () {
        socket.On (socketCode.OnConnect, OnConnect);
        socket.On (socketCode.OnFailedToConnect, OnFailedConnect);

        socket.On (gameCode.gameData, OnReceiveData);
        socket.On (gameCode.spawnPlayer, OnSpawnPlayer);

        socket.On (gameCode.playerQuit, OnPlayerQuit);

        socket.On (gameCode.syncTransform, OnSyncTransform);

        socket.On (gameCode.triggerAttack, OnTriggerAttack);
        socket.On (gameCode.playerHit, OnPlayerHit);
        socket.On (gameCode.playerDie, OnPlayerDie);

        socket.On (gameCode.playerStatus, OnPlayerStatus);
        socket.On (gameCode.switchItem, OnSwitchItem);

        socket.On (gameCode.spawnnStructures, OnCreateStructure);
        socket.On (gameCode.removeStructures, OnDestroyStructure);

        socket.On (gameCode.upgradeItem, OnUpgradeItems);
        socket.On (gameCode.syncItem, OnSyncItems);

        socket.On (gameCode.createProjectile, OnCreateProjectile);
        socket.On (gameCode.removeProjectile, OnRemoveProjectTile);
        socket.On (gameCode.syncPositionProjectile, OnSyncPositionProjectile);
        socket.On (gameCode.playerChat, OnPlayerChat);

        socket.On (gameCode.syncNpcTransform, OnSyncNpcTransform);
        socket.On (gameCode.syncNpcDie, OnNpcDie);
        socket.On (gameCode.syncNpcHP, OnSyncNpcHp);
        socket.On (gameCode.spawnNpc, OnSpawnNpc);
        socket.On (gameCode.scoreBoard, OnReceiveScoreBoard);
    }

    private void OnReceiveScoreBoard (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnReceiveScoreBoard (packet.ToString ());
    }

    private void OnSpawnNpc (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSpawnNpc (packet.ToString ());
    }

    private void OnSyncNpcHp (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSyncNpcHp (packet.ToString ());
    }

    private void OnNpcDie (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnNpcDie (packet.ToString ());
    }

    private void OnSyncNpcTransform (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSyncNpcTransform (packet.ToString ());
    }

    private void OnPlayerChat (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnPlayerChat (packet.ToString ());
    }

    private void OnSyncPositionProjectile (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSyncPositionProjectile (packet.ToString ());
    }

    private void OnRemoveProjectTile (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnRemoveProjectTile (packet.ToString ());
    }

    private void OnCreateProjectile (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnCreateProjectile (packet.ToString ());
    }

    private void OnSyncItems (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSyncItem (packet.ToString ());
    }

    private void OnUpgradeItems (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnUpgradeItems (packet.ToString ());
    }

    private void OnDestroyStructure (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnDestroyStructure (packet.ToString ());
    }

    private void OnCreateStructure (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnCreateStructure (packet.ToString ());
    }

    private void OnSwitchItem (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSwitchItem (packet.ToString ());
    }

    private void OnPlayerHit (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnPlayerHit (packet.ToString ());
    }

    private void OnPlayerDie (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnPlayerDie (packet.ToString ());
    }

    private void OnPlayerStatus (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnPlayerStatus (packet.ToString ());
    }

    private void OnTriggerAttack (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnTriggerAttack (packet.ToString ());
    }

    private void OnSyncTransform (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSyncTransform (packet.ToString ());
    }

    private void OnPlayerQuit (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnPlayerQuit (packet.ToString ());
    }

    private void OnSpawnPlayer (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnSpawnPlayer (packet.ToString ());
    }

    private void OnReceiveData (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnReceiveData (packet.ToString ());
    }

    private void OnFailedConnect (Socket socket, Packet packet, object[] args) {
        Debug.Log ($"On Failed to Connect, {packet.ToString()}");
    }

    private void OnConnect (Socket socket, Packet packet, object[] args) {
        controllerDelegate.OnConnect (packet.ToString ());
    }

}

public interface ISocketControllerDelegate {
    void OnConnect (string data);
    void OnCreateProjectile (string v);
    void OnCreateStructure (string data);
    void OnDestroyStructure (string data);
    void OnNpcDie (string v);
    void OnPlayerChat (string v);
    void OnPlayerDie (string data);
    void OnPlayerHit (string data);
    void OnPlayerQuit (string data);
    void OnPlayerStatus (string data);
    void OnReceiveData (string data);
    void OnReceiveScoreBoard (string v);
    void OnRemoveProjectTile (string v);
    void OnSpawnNpc (string v);
    void OnSpawnPlayer (string data);
    void OnSwitchItem (string data);
    void OnSyncItem (string data);
    void OnSyncNpcHp (string v);
    void OnSyncNpcTransform (string v);
    void OnSyncPositionProjectile (string v);
    void OnSyncTransform (string data);
    void OnTriggerAttack (string data);
    void OnUpgradeItems (string data);
}