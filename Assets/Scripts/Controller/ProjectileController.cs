using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    public List<Projectile> listProjectile;

    private void Start () {
        listProjectile = new List<Projectile> ();
    }

    public void AddProjectile (Projectile projectile) {
        listProjectile.Add (projectile);
    }
    public void SyncProjectilePosition (UpdatePositionProjectileModel model) {
        Projectile temp = null;
        foreach (ProjectilePosInfoModel info in model.pos) {
            temp = FindProjectileById (info.id);
            if (temp) {
                temp.SyncPosition (info.pos.ToVector3 ());
            }
        }
    }

    public Projectile FindProjectileById (int id) {
        return listProjectile.SingleOrDefault (p => p.id == id);
    }
    public void RemoveProjectile (int id) {
        for (int i = 0; i < listProjectile.Count; i++) {
            if (listProjectile[i].id == id) {
                Destroy (listProjectile[i].gameObject);
                listProjectile.RemoveAt (i);
                return;
            }
        }
    }
    public void RemoveProjectiles (int[] id) {
        for (int i = 0; i < listProjectile.Count; i++) {
            if (id.Contains (listProjectile[i].id)) {
                listProjectile[i].Destroy ();
                listProjectile.RemoveAt (i);
                i--;
            }
        }
    }
}