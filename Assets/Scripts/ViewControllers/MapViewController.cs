using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapViewController : MonoBehaviour {
    public MapView mapView;
    public RectTransform[] playerStatues;
    public Vector2 serverMapSize;
    public Vector2 uiMapSize;
    private Image lastDie;

    private void Start () {
        uiMapSize = (mapView.positionHolder.transform as RectTransform).rect.size;
    }
    public void AddPlayerStatue (int id) {
        if (playerStatues[id] == null) {
            playerStatues[id] = Instantiate (mapView.playerStatue, mapView.positionHolder.transform).rectTransform;
        } else {
            if (lastDie == null) {
                lastDie = Instantiate (mapView.lastDieStatue, mapView.positionHolder.transform);
            }
            lastDie.rectTransform.anchoredPosition = playerStatues[id].anchoredPosition;
        }
    }
    public void UpdatePlayerPosition (int id, Vector3 worldPosition) {
        if (playerStatues[id] != null)
            playerStatues[id].anchoredPosition = Vector2.Lerp (playerStatues[id].anchoredPosition, WorldPositionToUIMapPosition (worldPosition), .1f); // new Vector2 (worldPosition.x * uiMapSize.x / serverMapSize.x, worldPosition.y * uiMapSize.y / serverMapSize.y);
    }
    public void AddWoodResource (Vector3 position) {
        Image temp = Instantiate (mapView.woodStatue, mapView.positionHolder.transform);
        temp.rectTransform.anchoredPosition = WorldPositionToUIMapPosition (position);
    }
    public void AddFoodResource (Vector3 position) {
        Image temp = Instantiate (mapView.foodStatue, mapView.positionHolder.transform);
        temp.rectTransform.anchoredPosition = WorldPositionToUIMapPosition (position);
    }
    public void AddStoneResource (Vector3 position) {
        Image temp = Instantiate (mapView.stoneStatue, mapView.positionHolder.transform);
        temp.rectTransform.anchoredPosition = WorldPositionToUIMapPosition (position);
    }
    public void AddGoldResource (Vector3 position) {
        Image temp = Instantiate (mapView.goldStatue, mapView.positionHolder.transform);
        temp.rectTransform.anchoredPosition = WorldPositionToUIMapPosition (position);
    }
    public void InitPlayerCount (int count) {
        playerStatues = new RectTransform[count];
    }
    private Vector2 WorldPositionToUIMapPosition (Vector3 worldPosition) {
        return new Vector2 (worldPosition.x * uiMapSize.x / serverMapSize.x, worldPosition.y * uiMapSize.y / serverMapSize.y);
    }

}