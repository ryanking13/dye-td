using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 클래스
/// </summary>
public class Enemy : MonoBehaviour {

    private GameManager gm;

    public int originalHP;
    public int originalDefense;
    public float originalSpeed;

    private int currentHP;
    private int currentDefense;
    private float currentSpeed;

	void Start () {
        gm = GameManager.gm;
	}
	
	void Update () {
		
	}
}
