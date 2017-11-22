using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 클래스
/// </summary>
public class Enemy : MonoBehaviour {

    private GameManager gm;
    private BoardManager bm;

    public int originalHP;
    public int originalDefense;
    public float originalSpeed;
    public int money; // money that enemy drops
    public bool hittable; // if false, towers cannot attack this

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
            // doSomeArriveFunction()
            destroySelf();
            return;
        }

        nextPoint = waypoints[waypointIndex];
    }

    private void destroySelf() {
        gm.removeEnemy(gameObject);
        Destroy(gameObject);
    }

    public int getHP() {
        return currentHP;
    }

    public int getDefense() {
        return currentDefense;
    }

    public float getSpeed() {
        return currentSpeed;
    }

    public bool isHittable() {
        return hittable;
    }

    public void updateHP(int d) {
        currentHP += d;

        if(currentHP <= 0) {
            // doSomeKillFunction()
            destroySelf();
        }

        currentHP = Mathf.Min(originalHP, currentHP); // currentHP can't be higher than original HP
    }

    public void updateDefense(int d) {
        currentDefense += d;
        currentDefense = Mathf.Max(0, currentDefense); // defense can't be negative
    }

    public void updateSpeed(float d) {
        currentSpeed += d;
        currentSpeed = Mathf.Max(0, currentSpeed); // speed can't be negative
    }
}
