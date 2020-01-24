using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour {

	public int damage = 10;
	public float speed = 5f;

	[HideInInspector] public bool inverse = false;

    private Vector2 moveDirection;

    private BulletTest () {}

    private void Start () {
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        Transform target = GameObject.FindWithTag("Player").transform;
        moveDirection = (target.position - transform.position).normalized * speed;
        rb.velocity = new Vector2 (moveDirection.x, moveDirection.y);
    }

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
        if(col.gameObject.tag == "Ground" || col.gameObject.name == "Ground"){
            DestroyObject(gameObject);
        }
    }

    private BulletTest GetBulletTest () {
        return this;
    }

}
