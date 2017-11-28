using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 타워를 놓을 수 있는 슬롯에 해당하는 클래스
/// </summary>
public class TowerSlot : MonoBehaviour {

    private GameManager gm;

    private GameObject towerImage;
    private GameObject tower;

    private void Start() {
        gm = GameManager.gm;
        towerImage = null;
        tower = null;
    }

    private void OnMouseEnter() {
        // is on generating tower and no tower is on this slot
        if (gm.IsGeneratingTower() && tower == null) {
            towerImage = Instantiate(gm.GetTowerPrefab(), transform.position, Quaternion.identity) as GameObject;
            towerImage.transform.SetParent(transform);
            towerImage.GetComponent<Tower>().SetActivity(false);

            // show it half transparent
            Color originalColor = towerImage.GetComponent<Image>().color;
            towerImage.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
        }
    }

    private void OnMouseExit() {
        Destroy(towerImage);
    }

    public void OnClick() {
        // is on generating tower and no tower is on this slot
        if (gm.IsGeneratingTower() && tower == null) {
            Destroy(towerImage);
            tower = Instantiate(gm.GetTowerPrefab(), transform.position, Quaternion.identity) as GameObject;
            tower.transform.SetParent(transform);
            tower.GetComponent<Tower>().init(gm.GetTowerInfo());
            tower.GetComponent<Tower>().SetActivity(true);
            gm.FinishGeneration();
        }

    }
}
