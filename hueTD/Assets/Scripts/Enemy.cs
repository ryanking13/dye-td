using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 클래스
/// </summary>
public class Enemy : MonoBehaviour {

    private GameManager gm;
    private BoardManager bm;

    public int originalHP = 10;
    public int originalDefense = 0;
    public float originalSpeed = 3;
    public int money = 0; // money that enemy drops
    public bool hittable = true; // if false, towers cannot attack this
    // public enemyEffect effect;

    private int currentHP;
    private int currentDefense;
    private float currentSpeed;

    private Transform[] waypoints;  // points that defines enemy move path
    private int waypointIndex;
    private Transform nextPoint;    // point that enemy will currently move to

	void Start () {
        gm = GameManager.gm;
        bm = gm.GetBoardManager();

        currentHP = originalHP;
        currentDefense = originalDefense;
        currentSpeed = originalSpeed;

        waypoints = bm.GetWaypoints();
        waypointIndex = 0;
        nextPoint = waypoints[waypointIndex];
	}
	
	void Update () {
        Vector3 direction = (nextPoint.position - transform.position).normalized;
        transform.Translate(direction * currentSpeed * Time.deltaTime);

        // if arrived the point
        if(Vector3.Distance(nextPoint.position, transform.position) < 0.2f) {
            UpdateWaypoint();
        }
	}

    // update the waypoint
    private void UpdateWaypoint() {
        waypointIndex++;

        // arrived the end
        if(waypointIndex >= waypoints.Length) {
            arrive();
            return;
        }

        nextPoint = waypoints[waypointIndex];
    }

    // enemy arrive the goal
    private void arrive() {
        gm.UpdateLife(-1); // update life count
        DestroySelf();
    }

    // enemy dead
    private void die() {
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
    public void hit(int dmg) {
        int realDmg = Mathf.Max(1, dmg - GetDefense());
        UpdateHP(-realDmg);
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
            die();
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
