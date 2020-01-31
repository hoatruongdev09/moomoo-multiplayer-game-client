using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : Controller {
    public override IControllerDatasource Datasource {
        get { return controllerDatasource; }
        set { controllerDatasource = value; }
    }
    public override IControllerDelegate Delegate {
        get { return controllerDelegate; }
        set { controllerDelegate = value; }
    }

    private IControllerDatasource controllerDatasource;
    private IControllerDelegate controllerDelegate;

    private Vector2 moveDirect = Vector2.zero;
    private Vector3 lastMousePosition;
    [SerializeField] private Vector2 lookDirect = Vector2.zero;
    private bool sendNullPosition = false;
    private bool sendNullRotation = false;
    private Camera mainCam;
    private void OnEnable () {
        moveDirect = Vector2.zero;
    }
    private void Start () {
        mainCam = Camera.main;
    }
    private void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            controllerDelegate.OnTriggerAttack (true);
        }
        if (Input.GetMouseButton (0)) {
            controllerDelegate.OnTriggerAttack (true);
        }
        if (Input.GetMouseButtonUp (0)) {
            controllerDelegate.OnTriggerAttack (false);
        }
        // if (Input.GetMouseButtonDown (0)) {
        //     controllerDelegate.OnTriggerAttack (true);
        // }
        Movement ();
        Rotation ();
    }
    private void Rotation () {
        if (lastMousePosition != Input.mousePosition) {
            lookDirect = controllerDatasource.GetLocalPlayerPosition () - mainCam.ScreenToWorldPoint (Input.mousePosition);
            lastMousePosition = Input.mousePosition;

            SyncLookDirect ();
        } else {
            if (sendNullRotation) {
                return;
            }
            sendNullRotation = true;
            controllerDelegate.OnChangeLookDirect (Mathf.NegativeInfinity);

        }
    }
    private void Movement () {
        moveDirect.x = Input.GetAxis ("Horizontal");
        moveDirect.y = Input.GetAxis ("Vertical");

        if (moveDirect.sqrMagnitude == 0) {
            if (sendNullPosition) {
                return;
            }
            SyncMovement ();
            sendNullPosition = true;
        } else {
            sendNullPosition = false;
            SyncMovement ();
        }
    }
    private void SyncMovement () {
        if (controllerDelegate != null)
            controllerDelegate.OnChangeMovement (moveDirect);
    }
    private void SyncLookDirect () {
        float angle = Mathf.Atan2 (lookDirect.y, lookDirect.x);
        if (Mathf.Abs (angle * Mathf.Rad2Deg - controllerDatasource.GetLocalPlayerRotattion ()) >= .5f) {
            if (controllerDelegate != null)
                controllerDelegate.OnChangeLookDirect (angle);
            sendNullRotation = false;
        } else {
            if (sendNullRotation) {
                return;
            }
            sendNullRotation = true;
            controllerDelegate.OnChangeLookDirect (Mathf.NegativeInfinity);
        }
    }
}