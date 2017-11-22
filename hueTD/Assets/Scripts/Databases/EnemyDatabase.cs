using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyDatabase {

    private static List<EnemyInfo> enemyList;
    private static string DB = "Data/Enemies";

    public static void init() {

        enemyList = new List<EnemyInfo>();

        TextAsset xml = (TextAsset)Resources.Load(DB);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml.text);

        XmlNodeList enemies = doc.GetElementsByTagName("enemy");

        foreach(XmlNode e in enemies) {
            enemyList.Add(new EnemyInfo(
                    int.Parse(e.SelectSingleNode("id").InnerText),
                    e.SelectSingleNode("name").InnerText,
                    int.Parse(e.SelectSingleNode("HP").InnerText),
                    int.Parse(e.SelectSingleNode("defense").InnerText),
                    float.Parse(e.SelectSingleNode("speed").InnerText),
                    int.Parse(e.SelectSingleNode("money").InnerText),
                    e.SelectSingleNode("effect").InnerText
                ));
        }
    }

    public EnemyInfo GetEnemyById(int id) {
        // current lenear search
        // if database grows, please change this to binary search
        for(int i = 0; i < enemyList.Count; i++) {
            if (enemyList[i].id == id)
                return enemyList[i];
        }

        Debug.Log("ERROR: GetEnemyById - Wrong id");
        return null;
    }
}

public class EnemyInfo {

    public int id;
    public string name;
    public int HP;
    public int defense;
    public float speed;
    public int money;
    public string effect;

    public EnemyInfo(int id, string name, int HP, int defense, float speed, int money, string effect) {
        this.id = id;
        this.name = name;
        this.HP = HP;
        this.defense = defense;
        this.speed = speed;
        this.money = money;
        this.effect = effect;
    }
}
