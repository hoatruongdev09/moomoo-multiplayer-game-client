using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public abstract class ListViewItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public int ID { get; set; }
    public Action<int> OnSelected;
    public virtual void OnPointerDown (PointerEventData eventData) {

    }

    public virtual void OnPointerUp (PointerEventData eventData) {
        Debug.Log ("UP");
        OnSelected (ID);
    }
}