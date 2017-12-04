using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지속 효과 관련 클래스들
/// </summary>

// continous effects parent class
public abstract class ContinuousEffect {

    public float period;
    public float interval;
    public float startTime;
    public float formerTime;

    public abstract bool run(); // if return false, this effect is no more in period
}

public class TowerPoisonEffect: ContinuousEffect {

    private GameManager gm;

    public Enemy target;
    public int damage;

    public TowerPoisonEffect(Enemy target, int damage, float period, float interval, float startTime) {

        gm = GameManager.gm;

        this.target = target;
        this.damage = damage;
        this.period = period;
        this.interval = interval;

        this.startTime = startTime;
        this.formerTime = startTime;

        target.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 200, 0); // for test, please remove if this is remained
    }

    public override bool run() {

        if (target == null)
            return false;

        float currentTime = gm.GetTime();

        if (currentTime - startTime >= period) {
            target.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255); // for test, please remove if this is remained
            return false;
        }


        if (currentTime - formerTime >= interval) {
            target.UpdateHP(-damage);
        }

        return true;
        
    }
}
