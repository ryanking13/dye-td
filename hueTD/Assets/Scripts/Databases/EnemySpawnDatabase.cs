using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemySpawnDatabase {

    private static XmlDocument doc;
    private static string DB = "Data/EnemySpawn";

    public static void init() {
        TextAsset xml = (TextAsset)Resources.Load(DB);
        doc = new XmlDocument();
        doc.LoadXml(xml.text);
    }

    public static List<EnemySpawn> GetEnemySpawnList(int map, int wave) {
        List<EnemySpawn> gen = new List<EnemySpawn>();

        XmlNodeList enemySets = doc.GetElementsByTagName("enemyset");
        XmlNode enemySet = null;

        for(int i = 0; i < enemySets.Count; i++) {
            XmlNode e = enemySets[i];
            if(int.Parse(e.SelectSingleNode("map").InnerText) == map &&
                int.Parse(e.SelectSingleNode("wave").InnerText) == wave) {
                enemySet = e;
                break;
            }
        }

        if(enemySet == null) {
            Debug.Log("ERROR: wrong map/wave");
            return gen;
        }

        foreach(XmlNode e in enemySet.SelectNodes("enemy")) {
            gen.Add(new EnemySpawn(
                int.Parse(e.SelectSingleNode("id").InnerText),
                int.Parse(e.SelectSingleNode("lane").InnerText),
                e.SelectSingleNode("type").InnerText,
                int.Parse(e.SelectSingleNode("number").InnerText),
                e.SelectSingleNode("time").InnerText
                ));
        }

        return gen;
    }
}

public class EnemySpawn {
    public int id;
    public int number;
    public int lane;
    public int[] times;

    public EnemySpawn(int id, int lane, string type, int number, string times) {
        this.id = id;
        this.number = number;
        this.lane = lane;

        string[] t = times.Split(',');
        this.times = new int[number];
        switch (type) {
            case "each":
                for (int i = 0; i < number; i++) {
                    this.times[i] = int.Parse(t[i]);
                }
                break;

            case "interval":
                int st = int.Parse(t[0]);
                int ed = int.Parse(t[1]);
                int interval = (ed - st) / this.number;
                
                for(int i = 0; i < number; i++) {
                    this.times[i] = st + interval * i;
                }

                break;

            default:
                Debug.Log("ERROR: EnemySpawnDatabase - wrong type");
                break;    
        }
    }
}
