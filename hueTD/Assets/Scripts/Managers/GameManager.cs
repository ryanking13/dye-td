using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm = null;

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
	
	// Update is called once per frame
	void Update () {
		
	}
}
