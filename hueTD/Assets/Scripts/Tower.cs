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

    public int damage;
    public int range;
    public int size;
    public int price;
    public float shotSpeed;
    // private towerEffect effect;

    private float shotInterval;

	void Start () {
        gm = GameManager.gm;
        shotInterval = 1 / shotSpeed;
	}

    void Update () {
		
	}
}
