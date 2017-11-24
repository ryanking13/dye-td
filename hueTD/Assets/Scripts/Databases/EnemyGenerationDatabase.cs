using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyGenerationDatabase {

    private static XmlDocument doc;
    private static string DB = "Data/EnemyGeneration";

    public static void init() {
        TextAsset xml = (TextAsset)Resources.Load(DB);
        doc = new XmlDocument();
        doc.LoadXml(xml.text);
    }

    public static List<EnemyGeneration> GetEnemyGenerationList(int stage, int level) {
        List<EnemyGeneration> gen = new List<EnemyGeneration>();

        XmlNodeList enemySets = doc.GetElementsByTagName("enemyset");
        XmlNode enemySet = null;

        for(int i = 0; i < enemySets.Count; i++) {
            XmlNode e = enemySets[i];
            if(int.Parse(e.SelectSingleNode("stage").InnerText) == stage &&
                int.Parse(e.SelectSingleNode("level").InnerText) == level) {
                enemySet = e;
                break;
            }
        }

        if(enemySet == null) {
            Debug.Log("ERROR: wrong stage/level");
            return gen;
        }

        foreach(XmlNode e in enemySet.SelectNodes("enemy")) {
            gen.Add(new EnemyGeneration(
                int.Parse(e.SelectSingleNode("id").InnerText),
                int.Parse(e.SelectSingleNode("number").InnerText),
                e.SelectSingleNode("time").InnerText
                ));
        }

        return gen;
    }
}

public class EnemyGeneration {
    public int id;
    public int number;
    public int[] times;

    public EnemyGeneration(int id, int number, string times) {
        this.id = id;
        this.number = number;

        this.times = new int[number];
        string[] t = times.Split(',');
        for(int i = 0; i < number; i++) {
            this.times[i] = int.Parse(t[i]);
        }
    }
}
