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
    private EffectManager effectManager;

    /*
     * Stage related variables
     */
    public GameObject[] maps;
    private int currentMap;
    private int wave;
    private float waveTime;

    /*
     * Gameplay related variables
     */
    private int life = 100; // if life becomes 0, stage fails
    private int money = 10000;
    private List<string> essences;
    private List<string> currentEssences;

    /*
     * Enemy related variables
     */
    private List<Pair<EnemyInfo, int>>[] enemyGenerationList;

    private int enemyGenerationListIndex; // index pointer for enemyGenerationList ( indecating lastly generated enemy )
    private List<GameObject> enemies;
    private int totalEnemyNumber;

    public GameObject enemyPrefab;

    /*
     * Tower ralated variables
     */

    private bool onGeneration = false;
    public GameObject towerPrefab;
    public TowerInfo towerInfo;

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
        boardManager = new BoardManager();
        boardManager.init(maps);
        effectManager = new EffectManager();
        effectManager.init();

        /*
         * Initialize Databases
         */
        EnemyDatabase.init();
        EnemySpawnDatabase.init();
        TowerDatabase.init();

        /*
         * Initialize Objects;
         */

        enemyGenerationList = new List<Pair<EnemyInfo, int>>[CONFIG.MAX_WAVE_SECOND * CONFIG.ENEMY_GENERATION_TIMESCALE];
        for(int i = 0; i < CONFIG.MAX_WAVE_SECOND * CONFIG.ENEMY_GENERATION_TIMESCALE; i++)
            enemyGenerationList[i] = new List<Pair<EnemyInfo, int>>();

        enemies = new List<GameObject>();

        scene = SCENES.MAP;
        state = GAMESTATE.PAUSED;

        SetMap(0);
        SetCurrentWave(1);
        boardManager.BoardSetup(scene);
        StartNewWave();
	}

    // start new wave of the map
    public void StartNewWave() {

        /*
         * set enemy list of this wave
         */

        // get enemy list of this map/wave
        List<EnemySpawn> l = EnemySpawnDatabase.GetEnemySpawnList(currentMap, wave);

        // clear previous list
        for(int i = 0; i < enemyGenerationList.Length; i++)
            enemyGenerationList[i].Clear();

        // update enemy list
        totalEnemyNumber = 0;
        foreach (EnemySpawn eg in l) {
            EnemyInfo e = EnemyDatabase.GetEnemyById(eg.id);
            totalEnemyNumber += eg.number;

            foreach(int t in eg.times) {
                enemyGenerationList[t].Add(new Pair<EnemyInfo, int>(e, eg.lane));
            }
        }

        // initialize index, enemies, time
        enemyGenerationListIndex = 0;
        enemies.Clear();
        waveTime = 0.0f;

        state = GAMESTATE.ONWAVE;
    }
	
	void Update () {
        switch (state) {
            case GAMESTATE.PAUSED:
                // game paused
                break;
            case GAMESTATE.ONWAVE:
                RunWave();
                break;
            case GAMESTATE.INTERWAVE:
                // TODO: implement
                break;
            default:
                Debug.Log("ERROR : GameManager.state is in wrong value - " + state.ToString());
                break;
        }
	}

    // when game is running, and is on wave
    private void RunWave() {

        if(totalEnemyNumber == 0) {
            // end wave
            Debug.Log("wave clear");
        }

        waveTime += Time.deltaTime;

        int levelTimeInt = (int)(waveTime * CONFIG.ENEMY_GENERATION_TIMESCALE);

        while(enemyGenerationListIndex < levelTimeInt) {
            foreach(Pair<EnemyInfo, int> info in enemyGenerationList[enemyGenerationListIndex]) {
                GenerateEnemy(info.First, info.Second);
            }
            enemyGenerationListIndex++;
        }

        effectManager.RunContinuousEffects();
    }

    // generate new enemy, add it to the enemies list
    private void GenerateEnemy(EnemyInfo info, int lane) {
        GameObject enemy = Instantiate(enemyPrefab, boardManager.GetWaypoints(lane)[0].transform.position, Quaternion.identity) as GameObject;
        enemy.GetComponent<Enemy>().init(info, lane);
        enemies.Add(enemy);
    }

    public void HoldTower(int id) {
        towerInfo = TowerDatabase.GetTowerById(id);

        if(towerInfo.price > GetMoney()) {
            // TODO : can't buy
            return;
        }

        towerPrefab.GetComponent<Tower>().init(towerInfo);
        onGeneration = true;
    }

    public void FinishGeneration() {
        UpdateMoney(GetTowerPrefab().GetComponent<Tower>().GetPrice());
        onGeneration = false;
    }

    public void ProcessEffect<T>(Effect effect, T target) {
        effectManager.process(effect, target);
    }

    #region Getter/Setter/Updater

    public BoardManager GetBoardManager() {
        return boardManager;
    }

    public int GetMap() {
        return currentMap;
    }

    public void SetMap(int mapNumber) {
        currentMap = mapNumber;
    }

    public int GetStageLevel() {
        return wave;
    }

    public void SetCurrentWave(int newWave) {
        wave = newWave;
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

    public bool IsGeneratingTower() {
        return onGeneration;
    }

    public GameObject GetTowerPrefab() {
        return towerPrefab;
    }

    public TowerInfo GetTowerInfo() {
        return towerInfo;
    }

    #endregion
}

// some config ( const ) values that are game related 
public static class CONFIG {
    public static int MAX_WAVE_SECOND = 120; // max second for one level
    public static int ENEMY_GENERATION_TIMESCALE = 10;
}

// types of scenes
public enum SCENES { MAP };

public enum GAMESTATE { PAUSED, ONWAVE, INTERWAVE };