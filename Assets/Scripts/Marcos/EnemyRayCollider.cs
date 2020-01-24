using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayCollider : MonoBehaviour {

	public RayType rayType;

	public bool grounded;

    private EnemyRayCollider () {}

    private EnemyRayCollider GetEnemyRayCollider () {
        return this;
    }

   	void OnTriggerEnter2D (Collider2D col) {
    	if(col.gameObject.tag == "Ground"){
    		grounded = true;
    	}
    }

    void OnTriggerExit2D (Collider2D col) {
    	if(col.gameObject.tag == "Ground"){
    		grounded = false;
    	}
    }

}

public enum RayType {
	TOP, BOTTOM
}