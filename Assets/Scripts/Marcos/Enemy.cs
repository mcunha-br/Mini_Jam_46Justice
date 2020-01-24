using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AI, IEnemy {

	[Header("Stats")]
	public bool isDead = false;
	public EnemyStats stats;

	[Header("Axis State Patrol")]
	public DirectionState directionState;

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
					Patrol(Vector3.right);
					directionState = DirectionState.RIGHT;
				}else{
					Patrol(Vector3.left);
					directionState = DirectionState.LEFT;
				}
			}
			if(playerDetected && runAttack){
				StartCoroutine(AtackPlayer());
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
		}
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
	    	transform.Translate((dir * GetSpeed()) * Time.deltaTime);
	    	countdown++;
	    	if(countdown >= turnSecondsDir * 100){
	    		countdown = 0f;
	    		inPatrolling = false;
	    		StartCoroutine(WaitPatrolling());
	    	}
    	}else{
    		if(Vector2.Distance(transform.position, target.position) > attackDistance){
    			transform.position = Vector3.Lerp(transform.position, target.position, GetSpeed() * Time.deltaTime);
    			runAttack = false;
    		}else{
    			runAttack = true;
    		}
    	}
    }

    private IEnumerator WaitPatrolling () {
    	yield return new WaitForSeconds(2f);
    	SetupPatrol();
    	StopAllCoroutines();
    }

    float atackRate = 0.5f;
    private IEnumerator AtackPlayer () {
    	if(target){
    		yield return new WaitForSeconds(atackRate);
    		//print("Player atacado!");
    		if(directionState == DirectionState.RIGHT){
    			GameObject b = Instantiate(bulletObject, transform.position, transform.rotation);
    			b.gameObject.GetComponent<BulletTest>().inverse = false;
    		}else if (directionState == DirectionState.LEFT){
    			GameObject b = Instantiate(bulletObject, transform.position, transform.rotation);
    			b.gameObject.GetComponent<BulletTest>().inverse = true;
    		}
 			//Code atack player;
    	}
    	StopAllCoroutines();
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
    		StopAllCoroutines();
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