using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public static class TowerDatabase {

    private static List<TowerInfo> towerList;
    private static string DB = "Data/Towers";

    public static void init() {

        towerList = new List<TowerInfo>();

        TextAsset xml = (TextAsset)Resources.Load(DB);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml.text);

        XmlNodeList towers = doc.GetElementsByTagName("tower");

        foreach (XmlNode e in towers) {
            towerList.Add(new TowerInfo(
                    int.Parse(e.SelectSingleNode("id").InnerText),
                    e.SelectSingleNode("name").InnerText,
                    int.Parse(e.SelectSingleNode("size").InnerText),
                    int.Parse(e.SelectSingleNode("price").InnerText),
                    int.Parse(e.SelectSingleNode("damage").InnerText),
                    int.Parse(e.SelectSingleNode("range").InnerText),
                    float.Parse(e.SelectSingleNode("shotspeed").InnerText),
                    e.SelectSingleNode("effect").InnerText,
                    e.SelectSingleNode("effectparam").InnerText,
                    e.SelectSingleNode("towersprite").InnerText,
                    e.SelectSingleNode("missilesprite").InnerText
                ));
        }
    }

    public static TowerInfo GetTowerById(int id) {
        // current lenear search
        // if database grows, please change this to binary search
        for (int i = 0; i < towerList.Count; i++) {
            if (towerList[i].id == id)
                return towerList[i];
        }

        Debug.Log("ERROR: GetTowerById - Wrong id");
        return null;
    }

    public static TowerInfo GetTowerByName(string name) {
        // current lenear search
        // if database grows, please change this to binary search
        for (int i = 0; i < towerList.Count; i++) {
            if (towerList[i].name == name)
                return towerList[i];
        }

        Debug.Log("ERROR: GetTowerByName - Wrong name");
        return null;
    }
}

public class TowerInfo {

    public int id;
    public string name;
    public int level;
    public int size;
    public int price;
    public int damage;
    public int range;
    public float shotSpeed;
    public string effect;
    public List<float> effectParams;

    public Sprite towerSprite;
    public Sprite missileSprite;

    public TowerInfo(int id, string name, int size, int price, int damage,
                    int range, float shotSpeed, string effect, string effectParams,
                    string towerSprite, string missileSprite) {

        this.id = id;
        this.name = name;
        this.size = size;
        this.price = price;
        this.damage = damage;
        this.range = range;
        this.shotSpeed = shotSpeed;
        this.effect = effect;

        string[] p = effectParams.Split(',');
        this.effectParams = new List<float>();

        for(int i = 0; i < p.Length; i++) {
            this.effectParams.Add(float.Parse(p[i]));
        }

        // set sprite
        this.towerSprite = Resources.Load<Sprite>("Image/Tower/" + towerSprite);
        this.missileSprite = Resources.Load<Sprite>("Image/Missile/" + missileSprite);
    }
}
