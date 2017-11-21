using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타워에서 발사되는 오브젝트 클래스
/// </summary>
public class Missile : MonoBehaviour {

    private GameManager gm;

    private float speed;
    private int damage;
    // private towerEffect effect;
    // private enemy target;

    void Start () {
        gm = GameManager.gm;    	
	}
	
	void Update () {
		
	}
}
