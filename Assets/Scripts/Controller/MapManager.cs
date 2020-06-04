using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public SpriteRenderer mapBackGround;
    public SpriteRenderer snowBackground;
    public SpriteRenderer riverBackground;
    public SpriteRenderer gridBackground;

    private Vector2 mapSize;
    public void SetMapSize (Vector2 size) {
        gridBackground.transform.position = mapBackGround.transform.position = size / 2;
        mapBackGround.size = size;
        gridBackground.size = size;
        mapSize = size;
    }
    public void SetSnowSize (float size) {
        snowBackground.size = new Vector2 (mapSize.x, mapSize.y * size);
        snowBackground.transform.position = new Vector3 (mapSize.x / 2, mapSize.y - snowBackground.size.y / 2);
    }
    public void SetRiverSize (float size) {
        riverBackground.size = new Vector2 (mapSize.x, mapSize.y * size);
        riverBackground.transform.position = new Vector3 (mapSize.x / 2, mapSize.y / 2);
    }
}