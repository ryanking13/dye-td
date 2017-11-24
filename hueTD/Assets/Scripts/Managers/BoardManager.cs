using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    private GameManager gm;

    public GameObject[] stages;

    private GameObject currentStage;
    private Waypoints waypoints;

	public void init() {
        gm = GameManager.gm;
        currentStage = null;
        waypoints = null;
	}

    public void BoardSetup(SCENES scene) {

        switch (scene) {
            case SCENES.STAGE:
                StageSetup();
                break;
            default:
                break;
        }
    }

    private void StageSetup() {

        if (currentStage != null) {
            Destroy(currentStage);
        }

        int stage = gm.GetStage();

        currentStage = Instantiate(stages[stage]) as GameObject;
        waypoints = currentStage.transform.Find("Waypoints").GetComponent<Waypoints>();
    }

    public Transform[] GetWaypoints() {
        return waypoints.GetWaypoints();
    }
}
