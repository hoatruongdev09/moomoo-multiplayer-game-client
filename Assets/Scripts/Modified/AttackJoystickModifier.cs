using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackJoystickModifier : JoystickController {
    public Image imgOutThresholdFX;
    private Vector2 tempPosition;
    public override void OnDrag (PointerEventData eventData) {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle (imgBackGround.rectTransform, eventData.position, eventData.pressEventCamera, out pos)) {
            temp = MousePosOnRect (pos);
            pos.x = (pos.x / imgBackGround.rectTransform.sizeDelta.x);
            pos.y = (pos.y / imgBackGround.rectTransform.sizeDelta.y);
            // temp.Set (pos.x * 2 - 1, pos.y * 2 - 1);
            tempPosition = temp = new Vector3 (pos.x * 2 - 1, pos.y * 2 - 1);

            if (tempPosition.sqrMagnitude > 1.5 * 1.5) {
                Debug.Log ("wtf0");
                if (controllerDelegate != null) {
                    Debug.Log ("wtf");
                    controllerDelegate.JoystickOverTheshold (joystickId, true);
                    LeanAlphaFX (1);
                }
                tempPosition = (tempPosition.sqrMagnitude >= 1.3 * 1.3) ? tempPosition.normalized * 2 : tempPosition;
                // imgJoystick.rectTransform.anchoredPosition = new Vector2 (tempPosition.x * (imgBackGround.rectTransform.sizeDelta.x / 2.5f),
                //     tempPosition.y * (imgBackGround.rectTransform.sizeDelta.y / 2.5f));
            } else {
                if (controllerDelegate != null) {
                    controllerDelegate.JoystickOverTheshold (joystickId, false);
                    LeanAlphaFX (0.4f);
                }
                // tempPosition = (tempPosition.sqrMagnitude >= 1.01 * 1.01) ? tempPosition.normalized : tempPosition;

            }
            imgJoystick.rectTransform.anchoredPosition = new Vector2 (tempPosition.x * (imgBackGround.rectTransform.sizeDelta.x / 3),
                tempPosition.y * (imgBackGround.rectTransform.sizeDelta.y / 3));
            temp = (temp.sqrMagnitude > 1.01 * 1.01) ? temp.normalized : temp;
            // imgJoystick.rectTransform.anchoredPosition = new Vector2 (temp.x * (imgBackGround.rectTransform.sizeDelta.x / 3), temp.y * (imgBackGround.rectTransform.sizeDelta.y / 3));
            inputVT = temp.sqrMagnitude > 0.1f * 0.1f?temp : inputVT;
            if (controllerDelegate != null) {
                controllerDelegate.JoystickDrag (joystickId, inputVT);
            }
        }
    }

    public override void OnEndDrag (PointerEventData eventData) {
        base.OnEndDrag (eventData);
        if (controllerDelegate != null) {
            controllerDelegate.JoystickOverTheshold (joystickId, false);
            LeanAlphaFX (0);
        }
    }
    private void LeanAlphaFX (float to) {
        if (LeanTween.isTweening (imgOutThresholdFX.gameObject)) {
            return;
        }
        float current = imgOutThresholdFX.color.a;
        LeanTween.value (imgBackGround.gameObject, current, to, .5f).setOnUpdate ((float value) => {
            imgOutThresholdFX.color = new Color (imgOutThresholdFX.color.r, imgOutThresholdFX.color.g, imgOutThresholdFX.color.b, value);
        });
    }
}