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
    void OnPlayerDie (string v);
    void OnPlayerHit (string v);
    void OnPlayerQuit (string data);
    void OnPlayerStatus (string v);
    void OnReceiveData (string data);
    void OnSpawnPlayer (string data);
    void OnSyncTransform (string data);
    void OnTriggerAttack (string data);
}

public class SocketCode {
    public string OnConnect = "oncon";
    public string OnRequestJoin = "onJoin";
    public string OnFailedToConnect = "onConFailed";
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

    public string playerHit = "phit";
    public string playerDie = "pdie";
    public string playerStatus = "pstt";
}