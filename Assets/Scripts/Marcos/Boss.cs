using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : AI , IEnemy {

	public bool isDead;

	public BossStats stats;

	private Transform target;
	private bool playerDetected;

	public DirectionState directionState;

	public GameObject bulletObject;

	private byte direction;
	private float turnSecondsDir;

    private Boss () {}

    private void Start()  {
        inPatrolling = true;
    	GetComponent<CircleCollider2D>().radius = 25;
    	GetComponent<CircleCollider2D>().isTrigger = true;
    	direction = GetNewDirectionPatrol();
    	turnSecondsDir = GetNewSecondPatrolDir();
    }

    private void Update() {

    	if(!isDead){
	    	if(inPatrolling){
		    	if(direction %2 == 0){
		    		Patrol(Vector3.right);
					directionState = DirectionState.RIGHT;
		    	}else{
		    		Patrol(Vector3.left);
					directionState = DirectionState.LEFT;
		    	}
		        if(target && playerDetected){
		        	 InvokeRepeating("Shot", .4f, 0.3f);
		        }
	    	}
	    	if(target){
				if (target.position.x < transform.position.x){
				   directionState = DirectionState.LEFT;
				}else{
				   directionState = DirectionState.RIGHT;
				}
			}
    	}

    }

    private Boss GetBoss () {
        return this;
    }

    public void Demage (int damage) {
    	if(stats.GetHealth() <= 0){
    		Die();
    	}
    	stats.Damage(damage);
    }

    public void Die () {
    	isDead = true;
    	stats.SetHealth(0);

		SceneManager.LoadScene("Congratulations");
    }

    public override void Patrol (Vector2 dir) {

    }

    public void Shot () {
    	if(target){
    		if(directionState == DirectionState.RIGHT){
    			GameObject b = Instantiate(bulletObject, transform.position, transform.rotation);
    			b.gameObject.GetComponent<BulletTest>().inverse = false;
    			CancelInvoke();
    		}else if (directionState == DirectionState.LEFT){
    			GameObject b = Instantiate(bulletObject, transform.position, Quaternion.identity);
    			b.gameObject.GetComponent<BulletTest>().inverse = true;
    			CancelInvoke();
    		}
    	}
    }

    void OnTriggerEnter2D (Collider2D col) {
    	if(col.gameObject.tag == "Player"){
    		target = col.transform;
    		playerDetected = true;
    	}
    }

    void OnTriggerExit2D (Collider2D col) {
    	if(col.gameObject.tag == "Player"){
    		target = null;
    		playerDetected = false;
    	}
    }
}

[System.Serializable]
public class BossStats {
	
	public const int DEFAULT_HEALTH = 100;
	public int health = DEFAULT_HEALTH;

	public void Setup () {
		SetHealth(DEFAULT_HEALTH);
	}

	public void SetHealth (int health) {this.health = health;}
	public int GetHealth () {return health;}
	public void Damage (int damage) {health -= damage;}

}

public interface IBoss {
	void Damage(int damage);
	void Die();
	void Shot();
}

public enum BossDifficulty {
	EASY, MEDIUM, HARD
}
