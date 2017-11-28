using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 적을 공격하는 타워 클래스
/// </summary>
public class Tower : MonoBehaviour {

    private GameManager gm;

    private int towerLevel;
    private string towerName;
    // private string essence;      // color

    public GameObject missile;

    private int size;        // size of the tower (1 = 1x1, 2 = 2x2, ... )
    private int price;       // price of the tower ( when buying )

    private int originalDamage;
    private int originalRange;
    private float originalShotSpeed;

    private int currentDamage;
    private int currentRange;
    private float currentShotSpeed;
    private float shotInterval;

    private bool onActive; // is false, do not shot

    private Effect effect;

    private List<GameObject> enemies;

    private float currentTime;

    void Start () {
        gm = GameManager.gm;
	}

    public void init(TowerInfo info) {

        gm = GameManager.gm;

        towerLevel = info.level;
        towerName = info.name;

        originalDamage = info.damage;
        originalRange = info.range;
        originalShotSpeed = info.shotSpeed;
        size = info.size;
        price = info.price;

        effect = new Effect(info.effect, info.effectParams);

        GetComponent<Image>().sprite = info.towerSprite;
        missile.GetComponent<Image>().sprite = info.missileSprite;

        currentDamage = originalDamage;
        currentRange = originalRange;
        currentShotSpeed = originalShotSpeed;

        shotInterval = 1 / currentShotSpeed;
        currentTime = Time.time;
        UpdateEnemies();
        onActive = true;

    }

    # region Getter/Setter

    public int GetDamage() {
        return currentDamage;
    }

    public void UpdateDamage(int d) {
        currentDamage += d;
        currentDamage = Mathf.Max(1, currentDamage); // damage must be positive
    }

    public int GetRange() {
        return currentRange;
    }

    public void UpdateRange(int d) {
        currentRange += d;
        currentRange = Mathf.Max(1, currentRange); // range must be positive
    }

    public float GetShotSpeed() {
        return currentShotSpeed;
    }

    public void UpdateShotSpeed(float d) {
        currentShotSpeed += d;
        currentShotSpeed = Mathf.Max((float)0.1, currentShotSpeed); // shot speed must be positive

        shotInterval = 1 / currentShotSpeed;
    }

    public void SetActivity(bool b) {
        onActive = b;
    }

    public int GetPrice() {
        return price;
    }

    public Effect GetEffect() {
        return effect;
    }

    # endregion

    public void UpdateEnemies() {
        enemies = gm.GetEnemies();
    }

    void Update () {

        UpdateEnemies();

        if (!onActive) return;

        // not ready to shot
        if (Time.time - currentTime < shotInterval) {
            return;
        }

        float closestDistance = 9999999;
        GameObject closestEnemy = null;
        foreach(GameObject e in enemies) {

            if (!e.GetComponent<Enemy>().IsHittable()) // if enemy is not hittable state
                continue;

            float d = Vector3.Distance(transform.position, e.transform.position);
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

/*
public class TowerEffect {

    public string name;
    public List<float> parameters;

    public TowerEffect(string name, List<float> parameters) {
        this.name = name;
        this.parameters = parameters;
    }
}
*/