using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour {
    public float speed = 180;
    private void Update () {
        transform.Rotate (0, 0, speed * Time.deltaTime);
    }
}