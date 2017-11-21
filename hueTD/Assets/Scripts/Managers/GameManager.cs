using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm = null;

    private int stageLevel;
    private SCENES scene;

    private int money;
    private string[] essences;
    private string[] currentEssences;


	void Awake () {

        /*
         * GameManager initialize
         */

        // GameManager must be singleton object
        if (gm == null)
            gm = this;
        else
            Destroy(this.gameObject);
	}
	
	void Update () {
		
	}
}

// types of scenes
public enum SCENES { STAGE };