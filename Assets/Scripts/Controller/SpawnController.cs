using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    [Header ("PLAYER")]
    public GameObject playerPrefab;
    public Color[] colorId;
    private GameObject playerHolder;
    [Header ("RESOURCES")]
    public GameObject rsWood;
    public GameObject rsFood;
    public GameObject rsRock;
    public GameObject rsGold;
    private GameObject rsHolder;
    [Header ("EFFECTS")]
    public TextPopup textPopup;
    private GameObject fxHolder;

    private void Start () {
        rsHolder = new GameObject ("RESOURCES HOLDER");
        playerHolder = new GameObject ("PLAYERS HOLDER");
        fxHolder = new GameObject ("EFFECTS HOLDER");
    }
    public PlayerController[] SpawnPlayers (InitPlayerModel[] playerModels, PlayerController[] listPlayer) {
        for (int i = 0; i < playerModels.Length; i++) {
            listPlayer[playerModels[i].id] = SpawnPlayer (playerModels[i].id, playerModels[i].name, playerModels[i].skinId, playerModels[i].pos.ToVector3 ());

        }
        return listPlayer;
    }
    public PlayerController SpawnPlayer (int id, string name, int skinId, Vector3 position) {
        GameObject playGO = Instantiate (playerPrefab, position, Quaternion.identity, playerHolder.transform);
        PlayerController pc = playGO.GetComponent<PlayerController> ();
        pc.Character.ChangeColor (colorId[skinId]);
        pc.Character.SetName ($"{name} ({id})");
        pc.SyncPosition (position);
        return pc;
    }
    public GameObject[] SpawnResources (ResourceInfoModel[] resourceModel) {
        GameObject[] resources = new GameObject[resourceModel.Length];
        for (int i = 0; i < resourceModel.Length; i++) {
            resources[resourceModel[i].id] = SpawnResource (resourceModel[i].id, (ResourceType) resourceModel[i].type, resourceModel[i].pos.ToVector3 ());
        }
        return resources;
    }
    public GameObject SpawnResource (int id, ResourceType type, Vector2 position) {
        GameObject prefab = GetResource (type);
        if (prefab == null) {
            return null;
        }
        GameObject rs = Instantiate (prefab, position, Quaternion.Euler (0, 0, Random.Range (0, 359)), rsHolder.transform);
        rs.name = id.ToString ();
        return rs;
    }
    public GameObject GetResource (ResourceType type) {
        switch (type) {
            case ResourceType.Food:
                return rsFood;
            case ResourceType.Wood:
                return rsWood;
            case ResourceType.Gold:
                return rsGold;
            case ResourceType.Rock:
                return rsRock;
            default:
                return null;

        }
    }

    public void SpawnDamagePopUp (int damage, Vector3 position) {
        Color color;
        ColorUtility.TryParseHtmlString (damage < 0 ? "#ff5252" : "#33d9b2", out color);
        TextPopup temp = Instantiate (textPopup, position, Quaternion.identity, fxHolder.transform);
        temp.SetText (damage < 0 ? $"{damage}" : $"+{damage}", position, color);
    }
}
public enum ResourceType {
    Wood = 0,
    Rock = 1,
    Gold = 2,
    Food = 3
}