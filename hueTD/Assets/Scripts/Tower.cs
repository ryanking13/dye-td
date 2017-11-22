using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적을 공격하는 타워 클래스
/// </summary>
public class Tower : MonoBehaviour {

    private GameManager gm;

    public int towerLevel;
    public string towerName;
    public string essence;      // color
    public GameObject missile;

    public int size;        // size of the tower (1 = 1x1, 2 = 2x2, ... )
    public int price;       // price of the tower ( when buying )

    public int originalDamage;
    public int originalRange;
    public float originalShotSpeed;

    private int currentDamage;
    private int currentRange;
    private float currentShotSpeed;
    private float shotInterval;

    // private towerEffect effect;

    private List<GameObject> enemies;

    private float currentTime;

    void Start () {
        gm = GameManager.gm;
        initialize();
	}

    public void initialize() {
        currentDamage = originalDamage;
        currentRange = originalRange;
        currentShotSpeed = originalShotSpeed;

        shotInterval = 1 / currentShotSpeed;
        currentTime = Time.time;
        updateEnemies();
    }

    public void updateDamage(int d) {
        currentDamage += d;
        currentDamage = Mathf.Max(1, currentDamage); // damage must be positive
    }

    public void updateRange(int d) {
        currentRange += d;
        currentRange = Mathf.Max(1, currentRange); // range must be positive
    }

    public void updateShotSpeed(float d) {
        currentShotSpeed += d;
        currentShotSpeed = Mathf.Max((float)0.1, currentShotSpeed); // shot speed must be positive

        shotInterval = 1 / currentShotSpeed;
    }

    public void updateEnemies() {
        enemies = gm.GetEnemies();
    }

    void Update () {

        updateEnemies();

        // not ready to shot
        if (Time.time - currentTime < shotInterval) {
            return;
        }

        float closestDistance = 9999999;
        GameObject closestEnemy = null;
        foreach(GameObject e in enemies) {

            if (!e.GetComponent<Enemy>().isHittable()) // if enemy is not hittable state
                continue;

            float d = Vector3.Distance(transform.position, e.transform.position);
            Debug.Log(d);
            if (d <= currentRange && d < closestDistance) {
                closestDistance = d;
                closestEnemy = e;
            }
        }

        // no enemy is in range
        if(closestEnemy == null)
            return;

        // if target enemy is specified, generate missile, set entities
        GameObject m = Instantiate(missile, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        m.transform.SetParent(transform, false);
        m.GetComponent<Missile>().SetEntity(this, closestEnemy);
        currentTime = Time.time;
	}
}
