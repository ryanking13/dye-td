using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타워에서 발사되는 오브젝트 클래스
/// </summary>
public class Missile : MonoBehaviour {

    private GameManager gm;

    private float speed = 10;
    private Tower tower;
    private GameObject target;

    void Start () {
        gm = GameManager.gm;    	
	}
	
	void Update() {

        if (target == null || tower == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // if arrived the point
        if (Vector3.Distance(target.transform.position, transform.position) < 0.2f) {
            target.GetComponent<Enemy>().hit(tower.GetDamage());
            Destroy(gameObject);
        }
    }
    
    // set tower and target
    public void SetEntity(Tower t, GameObject g) {
        tower = t;
        target = g;
    }
}
