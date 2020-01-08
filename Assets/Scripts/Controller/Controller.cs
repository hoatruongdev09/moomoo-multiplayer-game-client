using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public abstract IControllerDatasource Datasource { get; set; }
    public abstract IControllerDelegate Delegate { get; set; }
}
public interface IControllerDatasource {
    Vector3 GetLocalPlayerPosition ();
    float GetLocalPlayerRotattion ();
}
public interface IControllerDelegate {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle">in radian</param>
    void OnChangeMovement (Vector2 direct);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ange">in radian</param>
    void OnChangeLookDirect (float ange);

    void OnTriggerAttack (bool byButton);
    void OnTriggerAutoAttack (bool action);
}