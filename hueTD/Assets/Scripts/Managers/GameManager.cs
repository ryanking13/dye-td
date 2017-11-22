using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm = null;
    private SCENES scene;

    /*
     * Managers
     */
    private BoardManager boardManager;

    
    private int stageLevel;
    private int life = 100; // if life becomes 0, stage fails
    private int money = 0;
    private List<string> essences;
    private List<string> currentEssences;

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
         * Initialize Databases
         */
        EnemyDatabase.init();
        EnemyGenerationDatabase.init();

        /*
         * Initialize Objects;
         */
        // enemies = new List<GameObject>();
	}

    public void StartLevel() {
        
    }
	
	void Update () {
		
	}



#region Getter/Setter/Updater
    
    public BoardManager GetBoardManager() {
        return boardManager;
    }

    public List<GameObject> GetEnemies() {
        return enemies;
    }

    public void RemoveEnemy(GameObject e) {
        enemies.Remove(e);
    }

    public int GetMoney() {
        return money;
    }

    public void UpdateMoney(int d) {
        money += d;
    }

    public int GetLife() {
        return life;
    }

    public void UpdateLife(int d) {
        life += d;
    }

    #endregion
}

// types of scenes
public enum SCENES { STAGE };