using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IJoyStickController, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public IJoyStickControllerDatasource Datasource {
        get { return controllerDatasource; }
        set { controllerDatasource = value; }
    }
    public IJoyStickControllerDelegate Delegate {
        get { return controllerDelegate; }
        set { controllerDelegate = value; }
    }

    public int JoystickId {
        get { return joystickId; }
        set { joystickId = value; }
    }

    protected IJoyStickControllerDatasource controllerDatasource;
    protected IJoyStickControllerDelegate controllerDelegate;

    public Image imgBackGround;
    public Image imgJoystick;

    protected Vector2 pos;
    protected Vector2 temp;
    protected int joystickId;
    [SerializeField] protected Vector2 inputVT;
    protected virtual void Start () {
        inputVT = Vector2.zero;
    }
    public virtual void OnDrag (PointerEventData eventData) {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle (imgBackGround.rectTransform, eventData.position, eventData.pressEventCamera, out pos)) {
            temp = MousePosOnRect (pos);
            pos.x = (pos.x / imgBackGround.rectTransform.sizeDelta.x);
            pos.y = (pos.y / imgBackGround.rectTransform.sizeDelta.y);
            temp = new Vector3 (pos.x * 2 - 1, pos.y * 2 - 1);
            temp = (temp.magnitude > 1.01) ? temp.normalized : temp;
            imgJoystick.rectTransform.anchoredPosition = new Vector2 (temp.x * (imgBackGround.rectTransform.sizeDelta.x / 3), temp.y * (imgBackGround.rectTransform.sizeDelta.y / 3));
            inputVT = temp.sqrMagnitude > 0.1f * 0.1f?temp : inputVT;
            if (controllerDelegate != null) {
                controllerDelegate.JoystickDrag (joystickId, inputVT);
            }
        }
    }
    protected virtual Vector2 MousePosOnRect (Vector2 pos) {
        Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle (imgBackGround.rectTransform, pos, GetComponentInParent<Canvas> ().worldCamera, out localpoint);
        return localpoint;
    }
    public virtual void OnBeginDrag (PointerEventData eventData) {
        if (LeanTween.isTweening (imgJoystick.gameObject)) {
            LeanTween.cancel (imgJoystick.gameObject);
        }
        imgJoystick.rectTransform.anchoredPosition = MousePosOnRect (eventData.position);
        if (controllerDelegate != null) {
            controllerDelegate.JoystickBeginDrag (joystickId, eventData);
        }
    }
    public virtual void ResetController () {
        inputVT = temp = pos = Vector2.zero;
        imgJoystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public virtual void OnEndDrag (PointerEventData eventData) {
        Vector2 crnAnchored = imgJoystick.rectTransform.anchoredPosition;
        LeanTween.value (imgJoystick.gameObject, crnAnchored, Vector2.zero, .25f).setOnUpdate ((Vector2 value) => {
            imgJoystick.rectTransform.anchoredPosition = value;
        }).setEaseInOutBack ();

        if (controllerDelegate != null) {
            controllerDelegate.JoystickEndDrag (joystickId, eventData);
        }
    }
}
public interface IJoyStickController {
    IJoyStickControllerDatasource Datasource { get; set; }
    IJoyStickControllerDelegate Delegate { get; set; }
    int JoystickId { get; set; }
}
public interface IJoyStickControllerDatasource {

}
public interface IJoyStickControllerDelegate {
    void JoystickDrag (int id, Vector2 inputVT);
    void JoystickEndDrag (int id, PointerEventData eventData);
    void JoystickBeginDrag (int id, PointerEventData eventData);
}