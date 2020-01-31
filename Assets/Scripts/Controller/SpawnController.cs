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
    [Header ("WEAPONS")]
    public Item[] itemPrefabs;
    [Header ("STRUCTURES")]
    public Structure[] structurePrefabs;
    private GameObject structureHolder;
    [Header ("PROJECTILE")]
    public Projectile[] projectilePrefab;
    private GameObject projectileHolder;
    [Header ("NPC")]
    public NpcController[] skinNpc;
    private GameObject npcHolder;
    [Header ("Accessory")]
    public Accessory[] accessories;

    private void Start () {
        rsHolder = new GameObject ("RESOURCES HOLDER");
        playerHolder = new GameObject ("PLAYERS HOLDER");
        fxHolder = new GameObject ("EFFECTS HOLDER");
        structureHolder = new GameObject ("STRUCTURE HOLDER");
        projectileHolder = new GameObject ("PROJECTILE HOLDER");
        npcHolder = new GameObject ("NPC HOLDER");
    }
    public PlayerController[] SpawnPlayers (InitPlayerModel[] playerModels, PlayerController[] listPlayer) {
        for (int i = 0; i < playerModels.Length; i++) {
            // listPlayer[playerModels[i].id] = SpawnPlayer (playerModels[i].id, playerModels[i].name, playerModels[i].skinId, playerModels[i].pos.ToVector3 ());
            listPlayer[playerModels[i].id] = SpawnPlayer (playerModels[i]);
        }
        return listPlayer;
    }
    public PlayerController SpawnPlayer (InitPlayerModel model) {
        GameObject playGO = Instantiate (playerPrefab, model.pos.ToVector3 (), Quaternion.identity, playerHolder.transform);
        PlayerController pc = playGO.GetComponent<PlayerController> ();
        pc.character.ChangeColor (colorId[model.skinId]);
        pc.character.SetName ($"{model.name}");
        pc.SyncPosition (model.pos.ToVector3 ());
        pc.SyncHealthPoint (model.hp);
        pc.SwapItem (CreateItem (model.itemId));
        return pc;
    }
    public PlayerController SpawnPlayer (int id, string name, int skinId, Vector3 position) {
        GameObject playGO = Instantiate (playerPrefab, position, Quaternion.identity, playerHolder.transform);
        PlayerController pc = playGO.GetComponent<PlayerController> ();
        pc.Character.ChangeColor (colorId[skinId]);
        pc.Character.SetName ($"{name}");
        pc.SyncPosition (position);
        return pc;
    }
    public PlayerController SpawnLocalPlayer (int id, string name, int skinId, Vector3 position) {
        GameObject playGO = Instantiate (playerPrefab, position, Quaternion.identity, playerHolder.transform);
        PlayerController pc = playGO.GetComponent<PlayerController> ();
        Color color;
        ColorUtility.TryParseHtmlString ("#18dcff", out color);
        pc.character.SetHealbarColor (color);
        pc.Character.ChangeColor (colorId[skinId]);
        pc.Character.SetName ($"{name}");
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
            case ResourceType.Stone:
                return rsRock;
            default:
                return null;

        }
    }

    public void SpawnDamagePopUp (int damage, Vector3 position) {
        Color color;
        Debug.Log ($"damage: {damage}");
        ColorUtility.TryParseHtmlString (damage < 0 ? "#ff5252" : "#33d9b2", out color);
        TextPopup temp = Instantiate (textPopup, position, Quaternion.identity, fxHolder.transform);
        temp.SetText (damage < 0 ? $"{damage}" : $"+{damage}", position, color);
    }
    public GameObject CreateItem (string id) {
        Item prefab = null;
        foreach (Item wp in itemPrefabs) {
            if (wp.id == id) {
                prefab = wp;
                break;
            }
        }
        return prefab.gameObject;
    }
    public GameObject CreateAccessory (string id) {
        Accessory prefab = null;
        foreach (Accessory acc in accessories) {
            if (acc.id == id) {
                prefab = acc;
                break;
            }
        }
        return prefab.gameObject;
    }
    public Structure SpawnStructure (CreateStructureModel model) {
        Structure prefab = null;
        foreach (Structure str in structurePrefabs) {
            if (str.itemId == model.itemId) {
                prefab = str;
                break;
            }
        }
        if (prefab == null) {
            Debug.Log ("SPAWN NULL");
            return null;
        }
        Structure temp = Instantiate (prefab, model.pos.ToVector3 (), Quaternion.Euler (0, 0, model.rot * Mathf.Rad2Deg - 90), structureHolder.transform);
        temp.id = model.id;
        temp.fromId = model.fromId;
        return temp;
    }
    public void SpawnStructures (CreateStructureModel[] models, StructuresController controller) {
        Structure temp;
        foreach (CreateStructureModel model in models) {
            temp = SpawnStructure (model);
            if (controller) {
                controller.AddStructure (temp);
            }
        }
    }
    public Projectile SpawnProjectile (CreateProjectileModel model) {
        Projectile temp = Instantiate (projectilePrefab[model.skinId], model.pos.ToVector3 (), Quaternion.Euler (0, 0, model.angle * Mathf.Rad2Deg), projectileHolder.transform);
        temp.transform.position = model.pos.ToVector3 ();
        temp.SyncPosition (model.pos.ToVector3 ());
        temp.id = model.id;
        return temp;
    }

    public void SpawnNpc (SpawnNpcModel[] models, NpcManager manager) {
        NpcController temp;
        foreach (SpawnNpcModel model in models) {
            temp = Instantiate (skinNpc[model.skinId], model.pos.ToVector3 (), Quaternion.Euler (0, 0, model.rot * Mathf.Rad2Deg), npcHolder.transform);
            temp.SyncPosition (model.pos.ToVector3 ());
            temp.SyncHealthPoint (model.hp);
            manager.AddNpc (model.id, temp);
        }
    }
}
public enum ResourceType {
    Wood = 0,
    Stone = 1,
    Gold = 2,
    Food = 3
}