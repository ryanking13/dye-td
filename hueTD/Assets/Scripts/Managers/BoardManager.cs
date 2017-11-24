using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    private GameManager gm;

    public GameObject[] maps;

    private GameObject currentMap;
    private List<Lane> lanes;

	public void init() {
        gm = GameManager.gm;
        currentMap = null;
        lanes = null;
	}

    public void BoardSetup(SCENES scene) {

        switch (scene) {
            case SCENES.MAP:
                MapSetup();
                break;
            default:
                break;
        }
    }

    private void MapSetup() {

        if (currentMap != null) {
            Destroy(currentMap);
        }

        int map = gm.GetMap();
        Debug.Log(map);
        currentMap = Instantiate(maps[map]) as GameObject;

        // add lanes
        int laneIdx = 0;
        lanes = new List<Lane>();
        while(true){
            Transform l = currentMap.transform.Find("Lane" + laneIdx.ToString());
            if (l == null)
                break;

            lanes.Add(l.GetComponent<Lane>());
            laneIdx++;
        }
    }

    public Transform[] GetWaypoints(int idx) {
        return lanes[idx].GetWaypoints();
    }
}
