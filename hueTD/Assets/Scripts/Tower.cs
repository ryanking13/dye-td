using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    private GameManager gm;

    public int towerLevel;
    public string towerName;
    public string essence;
    public Sprite icon;

    public int damage;
    public int speed;
    public int range;
    public int size;
    public int price;
    // private towerEffect effect;

    private float shotInterval;

	void Start () {
        gm = GameManager.gm;
        shotInterval = 1 / speed;
	}

    void Update () {
		
	}
}
