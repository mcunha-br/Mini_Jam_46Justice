using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNaoSubaNaMinhaCabeca : MonoBehaviour {

    private EnemyNaoSubaNaMinhaCabeca () {}

    public GameObject espinhos;
    public bool estaNoMeuCranio;

    GameObject target;

    float countdown = 0f;

    private void Start()  {
        countdown = 0f;
    }

    private void Update() {
     	if(estaNoMeuCranio){
     		countdown++;
	     	if(countdown >= 150){
	     		espinhos.SetActive(true);
	     	}   
	     	InvokeRepeating("TakeDamage", 1.3f, 0.3f);
    	}else{
    		espinhos.SetActive(false);
    		countdown = 0f;
    	}
    }

    void TakeDamage () {
    	if(target){
    		if(target.GetComponent<PlayerBehaviour>()){
    			target.GetComponent<PlayerBehaviour>().Damage(10);
    		}
    	}
    }

    void OnTriggerEnter2D (Collider2D col) {
    	if(col.gameObject.tag == "Player"){
    		estaNoMeuCranio = true;
    		target = col.gameObject;
    	}
    }

    void OnTriggerExit2D (Collider2D col) {
    	if(col.gameObject.tag == "Player"){
    		estaNoMeuCranio = false;
    		target = null;
    	}
    }


    private EnemyNaoSubaNaMinhaCabeca GetEnemyNaoSubaNaMinhaCabeca () {
        return this;
    }

}
