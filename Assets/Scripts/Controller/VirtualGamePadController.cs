using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualGamePadController : Controller, IJoyStickControllerDatasource, IJoyStickControllerDelegate {
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

    public JoystickController leftJoy;
    public JoystickController rightJoy;
    public Button buttonUseItem;

    private IEnumerator coroutineCheckAttack;
    [SerializeField] private bool checkAttackIsRunning;
    [SerializeField] private bool holdingAttack = false;
    public void Start () {
        leftJoy.Delegate = this;
        rightJoy.Delegate = this;

        leftJoy.Datasource = this;
        rightJoy.Datasource = this;

        leftJoy.JoystickId = 0;
        rightJoy.JoystickId = 1;

        coroutineCheckAttack = WaitForNextAttack (.3f);
        buttonUseItem.onClick.AddListener (() => { controllerDelegate.OnTriggerAttack (true); });
    }

    public void JoystickDrag (int id, Vector2 inputVT) {
        if (id == 0) {
            if (inputVT.sqrMagnitude != 0) {
                controllerDelegate.OnChangeMovement (inputVT);
            }
        }
        if (id == 1) {
            float angle = Mathf.Atan2 (inputVT.y, inputVT.x) + Mathf.PI;
            if (Mathf.Abs (angle * Mathf.Rad2Deg - controllerDatasource.GetLocalPlayerRotattion ()) >= .5f) {
                controllerDelegate.OnChangeLookDirect (angle);
            }
        }
    }

    public void JoystickEndDrag (int id, PointerEventData eventData) {
        if (id == 0) {
            controllerDelegate.OnChangeMovement (Vector2.zero);
        }
        if (id == 1) {
            controllerDelegate.OnChangeLookDirect (Mathf.NegativeInfinity);
        }
    }

    public void JoystickBeginDrag (int id, PointerEventData eventData) {

    }

    public void JoystickOverTheshold (int id, bool action) {
        if (id == 1 && action) {
            if (!checkAttackIsRunning && !holdingAttack) {
                controllerDelegate.OnTriggerAttack (false);
                holdingAttack = true;
                StartCoroutine (WaitForNextAttack (.3f));
            }
        }
        if (id == 1 && !action) {
            checkAttackIsRunning = false;
            if (holdingAttack) {
                holdingAttack = false;
                controllerDelegate.OnTriggerAutoAttack (false);
            }
        }
    }

    private IEnumerator WaitForNextAttack (float time) {
        Debug.Log ("Start Wait for autoAttack");
        checkAttackIsRunning = true;
        yield return new WaitForSeconds (time);
        if (holdingAttack) {
            controllerDelegate.OnTriggerAutoAttack (true);
        }
        checkAttackIsRunning = false;
    }
}