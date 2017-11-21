using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    private GameManager gm;

	void Start () {
        gm = GameManager.gm;
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
        
    }
}
