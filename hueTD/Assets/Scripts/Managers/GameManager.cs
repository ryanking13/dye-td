using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm = null;
    private SCENES scene;
    private GAMESTATE state; // game is running, paused, else;

    /*
     * Managers
     */
    private BoardManager boardManager;

    /*
     * Stage related variables
     */
    private int stage;
    private int stageLevel;
    private float levelTime;

    /*
     * Gameplay related variables
     */
    private int life = 100; // if life becomes 0, stage fails
    private int money = 0;
    private List<string> essences;
    private List<string> currentEssences;

    /*
     * Enemy related variables
     */
    private List<EnemyInfo>[] enemyGenerationList;
    private int enemyGenerationListIndex; // index pointer for enemyGenerationList ( indecating lastly generated enemy )
    private List<GameObject> enemies;
    private int totalEnemyNumber;

    public GameObject enemyPrefab;

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
        boardManager.init();

        /*
         * Initialize Databases
         */
        EnemyDatabase.init();
        EnemyGenerationDatabase.init();

        /*
         * Initialize Objects;
         */

        enemyGenerationList = new List<EnemyInfo>[CONFIG.MAX_LEVEL_SECOND * CONFIG.ENEMY_GENERATION_TIMESCALE];
        for(int i = 0; i < CONFIG.MAX_LEVEL_SECOND * CONFIG.ENEMY_GENERATION_TIMESCALE; i++)
            enemyGenerationList[i] = new List<EnemyInfo>();

        enemies = new List<GameObject>();

        scene = SCENES.STAGE;
        state = GAMESTATE.PAUSED;

        SetStage(0);
        SetStageLevel(1);
        boardManager.BoardSetup(scene);
        StartNewLevel();
	}

    // start new level of the stage
    public void StartNewLevel() {

        /*
         * set enemy list of this level
         */

        // get enemy list of this stage/level
        List<EnemyGeneration> l = EnemyGenerationDatabase.GetEnemyGenerationList(stage, stageLevel);

        // clear previous list
        for(int i = 0; i < enemyGenerationList.Length; i++)
            enemyGenerationList[i].Clear();

        // update enemy list
        totalEnemyNumber = 0;
        foreach (EnemyGeneration eg in l) {
            EnemyInfo e = EnemyDatabase.GetEnemyById(eg.id);
            totalEnemyNumber += eg.number;

            foreach(int t in eg.times) {
                enemyGenerationList[t].Add(e);
            }
        }

        // initialize index, enemies, time
        enemyGenerationListIndex = 0;
        enemies.Clear();
        levelTime = 0.0f;

        state = GAMESTATE.ONLEVEL;
    }
	
	void Update () {
        switch (state) {
            case GAMESTATE.PAUSED:
                // game paused
                break;
            case GAMESTATE.ONLEVEL:
                RunLevel();
                break;
            case GAMESTATE.INTERLEVEL:
                // TODO: implement
                break;
            default:
                Debug.Log("ERROR : GameManager.state is in wrong value - " + state.ToString());
                break;
        }
	}

    // when game is running, and is on level
    private void RunLevel() {

        if(totalEnemyNumber == 0) {
            // end level
            Debug.Log("level clear");
        }

        levelTime += Time.deltaTime;

        int levelTimeInt = (int)(levelTime * CONFIG.ENEMY_GENERATION_TIMESCALE);

        while(enemyGenerationListIndex < levelTimeInt) {
            foreach(EnemyInfo info in enemyGenerationList[enemyGenerationListIndex]) {
                GenerateEnemy(info);
            }
            enemyGenerationListIndex++;
        }
    }

    // generate new enemy, add it to the enemies list
    private void GenerateEnemy(EnemyInfo info) {
        GameObject enemy = Instantiate(enemyPrefab, boardManager.GetWaypoints()[0].transform.position, Quaternion.identity) as GameObject;
        enemy.GetComponent<Enemy>().init(info);
        enemies.Add(enemy);
    }


#region Getter/Setter/Updater
    
    public BoardManager GetBoardManager() {
        return boardManager;
    }

    public int GetStage() {
        return stage;
    }

    public void SetStage(int stageNumber) {
        stage = stageNumber;
    }

    public int GetStageLevel() {
        return stageLevel;
    }

    public void SetStageLevel(int level) {
        stageLevel = level;
    }

    public List<GameObject> GetEnemies() {
        return enemies;
    }

    public void RemoveEnemy(GameObject e) {
        enemies.Remove(e);
        totalEnemyNumber--;
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

// some config ( const ) values that are game related 
public static class CONFIG {
    public static int MAX_LEVEL_SECOND = 120; // max second for one level
    public static int ENEMY_GENERATION_TIMESCALE = 10;
}

// types of scenes
public enum SCENES { STAGE };

public enum GAMESTATE { PAUSED, ONLEVEL, INTERLEVEL };