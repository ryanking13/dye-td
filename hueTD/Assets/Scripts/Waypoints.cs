using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

    private Transform[] waypoints;

	// Use this for initialization
	void Awake () {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++) {
            waypoints[i] = transform.GetChild(i);
        }
	}

    public void UpdateWaypoints() {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++) {
            waypoints[i] = transform.GetChild(i);
        }
    }

    public Transform[] GetWaypoints() {
        return waypoints;
    }
}
