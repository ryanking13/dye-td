using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오라 효과 관련 클래스들
/// </summary>

// aura effects parent class
public abstract class AuraEffects {

    public GameObject self; // gameobject that spouts the aura
    public float range;

    public abstract void run(); // update effect
    public abstract bool clear(GameObject o);  // if clear condition, clear effect and return true, else return false
}

public class TowerAccelerateEffect : AuraEffects {

    private GameManager gm;
    private float accelAmount;

    private List<GameObject> targets;

    public TowerAccelerateEffect(GameObject self, float accelAmount, float range) {
        gm = GameManager.gm;

        this.self = self;
        this.accelAmount = accelAmount;
        this.range = range;

        targets = new List<GameObject>();
    }

    public override void run() {
        List<GameObject> towers = gm.GetTowers();

        for(int i = 0; i < towers.Count; i++) {
            GameObject t = towers[i];

            if (self.Equals(t)) continue; // no effect to itself

            if (targets.IndexOf(t) != -1) continue; // if already in target list
            
            if (Vector3.Distance(self.transform.position, t.transform.position) <= range) {
                targets.Add(t);
                t.GetComponent<Tower>().UpdateShotSpeed(accelAmount);
            }
        }
    }

    public override bool clear(GameObject o) {

        // if on clear condition, clear effects
        if(self.Equals(o)) {
            for (int i = 0; i < targets.Count; i++) {
                if (targets[i] != null) {
                    targets[i].GetComponent<Tower>().UpdateShotSpeed(-accelAmount);
                }
            }

            return true;
        }

        return false;
    }
}
