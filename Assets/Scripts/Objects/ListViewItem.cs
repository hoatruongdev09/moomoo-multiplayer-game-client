using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public abstract class ListViewItem : MonoBehaviour {
    public abstract int ID { get; set; }
    public abstract Action<int> OnSelected { get; set; }

}