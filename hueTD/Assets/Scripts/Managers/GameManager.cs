using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm = null;

    /*
     * Managers
     */
    private BoardManager boardManager;

    private int stageLevel;
    private SCENES scene;

    private int money;
    private string[] essences;
    private string[] currentEssences;

    public List<GameObject> enemies;

	void Awake () {

        /*
         * GameManager initialize
         */

        // GameManager must be singleton object
        if (gm == null)
            gm = this;
        else
            Destroy(this.gameObject);

        /*
         * Initialize Managers
         */
        boardManager = GetComponent<BoardManager>();
        boardManager.BoardSetup(scene);


        /*
         * Initialize Objects;
         */
        // enemies = new List<GameObject>();
	}
	
	void Update () {
		
	}



#region Getter/Setter

    public BoardManager GetBoardManager() {
        return boardManager;
    }

    public List<GameObject> GetEnemies() {
        return enemies;
    }

    public void removeEnemy(GameObject e) {
        enemies.Remove(e);
    }

    #endregion
}

// types of scenes
public enum SCENES { STAGE };