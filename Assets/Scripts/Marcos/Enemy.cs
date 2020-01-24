using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AI, IEnemy {

	[Header("Stats")]
	public bool isDead = false;
	public EnemyStats stats;

	[Header("Axis State Patrol")]
	public DirectionState directionState;
	public EnemyRayCollider[] rayColliders = new EnemyRayCollider[2];

	[Header("Bullet")]
	public GameObject bulletObject;

	private const string playerTag = "Player";
	private byte direction;
	private float countdown = 0f;
	private float turnSecondsDir;
	private float detectPlayerDistance = 3f;
	private float radiusDetectPlayerDistance = 6f;
	private bool playerDetected;
	private Transform target;
	private float attackDistance = 5f;
	private bool runAttack = false;
	private float jumpForce = 5f;

    protected Enemy () {}

    private void Start()  {
    	SetupStats();
    	SetupPatrol();
    }

    private void Update() {
    	PatrollingAI();
    }

    private void PatrollingAI () {
    	if(!isDead){
	    	if(inPatrolling){
		    	if(direction %2 == 0){
		    		//if(rayColliders[0].grounded && rayColliders[1].grounded){
						//Patrol(Vector3.left);
						//directionState = DirectionState.LEFT;
					//}else{
						Patrol(Vector3.right);
						directionState = DirectionState.RIGHT;
					//}
				}else{
					//if(rayColliders[0].grounded && rayColliders[1].grounded){
						//Patrol(Vector3.right);
						//directionState = DirectionState.RIGHT;
					//}else{
						Patrol(Vector3.left);
						directionState = DirectionState.LEFT;
					//}
				}
			}
			if(playerDetected && runAttack){
				 InvokeRepeating("AtackPlayer", 1.0f, 0.3f);
			}
			if(target){
				if (target.position.x < transform.position.x){
				   //print("Player left");
				   directionState = DirectionState.LEFT;
				}else{
				   //print("Player right");
				   directionState = DirectionState.RIGHT;
				}
			}
			if(!rayColliders[0].grounded && rayColliders[1].grounded){
				Jump();
			}
		}
    }

    private void Jump () {
    	if(directionState == DirectionState.RIGHT){
    		StartCoroutine(JumpStep(Vector3.right));
    	}else if (directionState == DirectionState.LEFT){
    		StartCoroutine(JumpStep(Vector3.left));
    	}
    }

    bool jupping = false;
    private IEnumerator JumpStep (Vector3 axis) {
    	jupping = true;
    	if(jupping){
	    	yield return new WaitForSeconds(0f);
	    	GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce);
	    	yield return new WaitForSeconds(1f);
	    	GetComponent<Rigidbody2D>().AddForce(Vector3.up + axis);
	    	jupping = false;
    	}
    	StopCoroutine("JumpStep");
    }

    private void SetupPatrol () {
    	inPatrolling = true;
    	GetComponent<CircleCollider2D>().radius = radiusDetectPlayerDistance;
    	GetComponent<CircleCollider2D>().isTrigger = true;
    	direction = GetNewDirectionPatrol();
    	turnSecondsDir = GetNewSecondPatrolDir();
    }

    private void SetupStats () {
    	isDead = false;
    	stats = new EnemyStats("Player", 100);
    }

    public override void Patrol (Vector2 dir) {
    	if(!playerDetected){
    		//if(!rayColliders[0].grounded && rayColliders[1].grounded){
	    		transform.Translate((dir * GetSpeed()) * Time.deltaTime);
	    	//}
	    	countdown++;
	    	if(countdown >= turnSecondsDir * 100){
	    		countdown = 0f;
	    		inPatrolling = false;
	    		StartCoroutine(WaitPatrolling());
	    	}
    	}else{
    		if(Vector2.Distance(transform.position, target.position) > attackDistance){
    			transform.position = Vector3.Lerp(transform.position, target.position, GetSpeed() * 0.2f * Time.deltaTime);
    			runAttack = false;
    		}else{
    			runAttack = true;
    		}
    	}
    }

    private IEnumerator WaitPatrolling () {
    	yield return new WaitForSeconds(2f);
    	SetupPatrol();
    	StopCoroutine("WaitPatrolling");
    }

    private void AtackPlayer () {
    	if(target){
    		if(directionState == DirectionState.RIGHT){
    			GameObject b = Instantiate(bulletObject, transform.position, transform.rotation);
    			b.gameObject.GetComponent<BulletTest>().inverse = false;
    			CancelInvoke();
    		}else if (directionState == DirectionState.LEFT){
    			GameObject b = Instantiate(bulletObject, transform.position, transform.rotation);
    			b.gameObject.GetComponent<BulletTest>().inverse = true;
    			CancelInvoke();
    		}
    	}
    }

    public void Demage (int dmg) {
    	if(stats.GetHealth() <= 0){
    		Die();
    	}
    	stats.Demage(dmg);
    }

    public void Die () {
    	isDead = true;
    	countdown = 0f;
    	stats.SetHealth(0);
    }

    void OnTriggerEnter2D (Collider2D col) {
    	if(col.gameObject.tag == playerTag){
    		playerDetected = true;
    		target = col.transform;
    		//print("Player detectado!");
    	}
    }

    void OnTriggerExit2D (Collider2D col) {
    	if(col.gameObject.tag == playerTag){
    		target = null;
    		countdown = 0f;
	    	inPatrolling = true;
    		playerDetected = false;
    	}
    }

    public Enemy GetEnemy () {return this;}
    public string GetTargetTag () {return playerTag;}

}

[System.Serializable]
public class EnemyStats {

	private const int DEFAULT_HEALTH = 100;

	public string name;
	public int health;

	public EnemyStats (string name = "", int health = DEFAULT_HEALTH) {
		this.health = health;
		this.name = name;
		if(name == ""){
			name = "Player";
		}
	}

	public void Setup () {
		SetHealth(DEFAULT_HEALTH);
	}

	public void Demage (int dmg) {health -= dmg;}

	public void SetHealth (int health) {this.health = health;}
	public int GetHealth () {return health;}

}

public interface IEnemy {
	void Demage(int dmg);
	void Die();
}