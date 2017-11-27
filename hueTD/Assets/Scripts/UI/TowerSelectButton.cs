using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectButton : MonoBehaviour {

    private GameManager gm;

    public int towerId;

	// Use this for initialization
	void Start () {
        gm = GameManager.gm;
	}
	
    public void onClick() {
        gm.HoldTower(towerId);
    }
}
