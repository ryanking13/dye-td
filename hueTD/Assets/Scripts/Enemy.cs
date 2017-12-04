using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 클래스
/// </summary>
public class Enemy : MonoBehaviour {

    private GameManager gm;
    private BoardManager bm;

    private int originalHP = 10;
    private int originalDefense = 0;
    private float originalSpeed = 3;
    private int money = 0; // money that enemy drops
    private bool hittable = true; // if false, towers cannot attack this
    // public enemyEffect effect;

    private int currentHP;
    private int currentDefense;
    private float currentSpeed;

    private int lane;
    private Transform[] waypoints;  // points that defines enemy move path
    private int waypointIndex;
    private Transform nextPoint;    // point that enemy will currently move to

	void Start () {

	}
	
	void Update () {

        // if point is not set
        if (nextPoint == null)
            return;

        Vector3 direction = (nextPoint.position - transform.position).normalized;
        transform.Translate(direction * currentSpeed * Time.deltaTime);

        // if arrived the point
        if(Vector3.Distance(nextPoint.position, transform.position) < 0.2f) {
            UpdateWaypoint();
        }
	}

    public void init(EnemyInfo info, int lane) {

        gm = GameManager.gm;
        bm = gm.GetBoardManager();

        originalHP = info.HP;
        originalDefense = info.defense;
        originalSpeed = info.speed;
        money = info.money;
        this.lane = lane;
        // TODO: effect = iinfo.effect

        currentHP = originalHP;
        currentDefense = originalDefense;
        currentSpeed = originalSpeed;
        
        waypoints = bm.GetWaypoints(this.lane);
        waypointIndex = 0;
        nextPoint = waypoints[waypointIndex];
    }

    // update the waypoint
    private void UpdateWaypoint() {
        waypointIndex++;

        // arrived the end
        if(waypointIndex >= waypoints.Length) {
            Arrive();
            return;
        }

        nextPoint = waypoints[waypointIndex];
    }

    // enemy arrive the goal
    private void Arrive() {
        gm.UpdateLife(-1); // update life count
        DestroySelf();
    }

    // enemy dead
    private void Die() {
        gm.UpdateMoney(money);  // earn money
        DestroySelf();          // destroy gameObject
    }

    // destroy gameObject
    private void DestroySelf() {
        gm.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    // is enemy in hittable state
    public bool IsHittable() {
        return hittable;
    }
    
    // enemy is hit
    public void hit(Tower t) {
        int realDmg = Mathf.Max(1, t.GetDamage() - GetDefense());
        UpdateHP(-realDmg);

        if(t.GetEffect().type == "TH") // if effect activates on hit
            gm.ProcessEffect(t.GetEffect(), this);
    }

    #region Getter/Setter

    public int GetHP() {
        return currentHP;
    }

    public int GetDefense() {
        return currentDefense;
    }

    public float GetSpeed() {
        return currentSpeed;
    }


    public void UpdateHP(int d) {
        currentHP += d;

        // if dead
        if(currentHP <= 0) {
            Die();
        }

        currentHP = Mathf.Min(originalHP, currentHP); // currentHP can't be higher than original HP
    }

    public void UpdateDefense(int d) {
        currentDefense += d;
        currentDefense = Mathf.Max(0, currentDefense); // defense can't be negative
    }

    public void UpdateSpeed(float d) {
        currentSpeed += d;
        currentSpeed = Mathf.Max(0, currentSpeed); // speed can't be negative
    }

    #endregion
}
