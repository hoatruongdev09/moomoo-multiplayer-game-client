using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructuresController : MonoBehaviour {
    public List<Structure> listStruture;

    private void Start () {
        listStruture = new List<Structure> ();
    }
    public void AddStructure (Structure structure) {
        listStruture.Add (structure);
    }
    public Structure FindStructureById (int id) {
        return listStruture.SingleOrDefault ((s) => s.id == id);
    }
    public void RemoveStructure (int id) {
        for (int i = 0; i < listStruture.Count; i++) {
            if (listStruture[i].id == id) {
                GameObject go = listStruture[i].gameObject;
                listStruture.RemoveAt (i);
                Destroy (go);
                return;
            }
        }
    }
    public void RemoveStructures (int[] id) {
        for (int i = 0; i < listStruture.Count; i++) {
            foreach (int j in id) {
                if (listStruture[i].id == j) {
                    GameObject go = listStruture[i].gameObject;
                    listStruture.RemoveAt (i);
                    i--;
                    Destroy (go);
                    break;
                }
            }
        }
    }
}