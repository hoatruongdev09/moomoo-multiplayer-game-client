using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Camera Camera { get; private set; }
    public Vector2 CameraSize { get; private set; }
    public bool useSmooth = true;
    public float smoothSpeed = 5;
    private Transform focusedTarget;
    private Camera mainCam;
    private void Start () {
        mainCam = Camera.main;
        Camera = mainCam;

        float height = 2f * mainCam.orthographicSize;
        float width = height * mainCam.aspect;

        CameraSize = new Vector2 (width, height);
    }
    public void SetForcus (Transform target) {
        focusedTarget = target;
    }
    private void Update () {
        if (focusedTarget) {
            if (useSmooth) {
                transform.position = Vector3.Lerp (transform.position, focusedTarget.position, smoothSpeed * Time.deltaTime);
            } else {
                transform.position = focusedTarget.position;
            }
        }
    }
}