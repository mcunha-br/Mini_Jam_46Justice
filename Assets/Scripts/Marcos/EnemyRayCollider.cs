using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayCollider : MonoBehaviour {

	public RayType rayType;

	public bool grounded;
    public GameObject target;

    private EnemyRayCollider () {}

    private EnemyRayCollider GetEnemyRayCollider () {
        return this;
    }

   	void OnTriggerEnter2D (Collider2D col) {
    	if(col.gameObject.tag == "Ground"){
    		grounded = true;
            target = col.gameObject;
    	}
        if(col.gameObject.tag == "Player"){
            target = col.gameObject;
            grounded = true;
        }
    }

    void OnTriggerExit2D (Collider2D col) {
    	if(col.gameObject.tag == "Ground"){
            target = null;
    		grounded = false;
    	}
        if(col.gameObject.tag == "Player"){
            target = null;
            grounded = false;
        }
    }

}

public enum RayType {
	TOP, BOTTOM, HEAD
}