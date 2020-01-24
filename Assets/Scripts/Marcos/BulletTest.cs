using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour {

	public int damage = 10;
	public float speed = 5f;

	[HideInInspector] public bool inverse = false;

    private BulletTest () {}

    private void Update() {
    	if(!inverse)
        	transform.Translate(Vector3.right * speed * Time.deltaTime);
        else
        	transform.Translate(-Vector3.right * speed * Time.deltaTime);	
    }

    void OnTriggerEnter2D (Collider2D col) {
    	if(col.gameObject.tag == "Player"){
    		if(col.gameObject.GetComponent<PlayerBehaviour>()){
    			col.gameObject.GetComponent<PlayerBehaviour>().Damage(damage);
    		}
    		DestroyObject(gameObject);
    	}
    }

    private BulletTest GetBulletTest () {
        return this;
    }

}
