using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AI, IEnemy {

	[Header("Stats")]
	public bool isDead = false;
	public EnemyStats stats;

    [Header("Level Difficulty")]
    public EnemyDifficulty difficulty;

	[Header("Axis State Patrol")]
	public DirectionState directionState;
	public EnemyRayCollider[] rayColliders = new EnemyRayCollider[2];
    public EnemyRayCollider head;

	[Header("Bullet")]
    public Transform[] gunPos = new Transform[2];
	public GameObject bulletObject;
    public GameObject bulletSpawn;
    public GameObject gun;
    public GameObject gunGraph;
    public GameObject spawnSound;

    [Header("Others")]
    public GameObject eyes;
    public Transform[]eyePos = new Transform[2];

	private const string playerTag = "Player";
	private byte direction;
	private float countdown = 0f;
	private float turnSecondsDir;
	private float radiusDetectPlayerDistance = 15f;
	private bool playerDetected;
	private Transform target;
	private float attackDistance = 10f;
	private bool runAttack = false;
	private float jumpForce = 86.5f;

    private EnemyAnimator animator;

    protected Enemy () {}

    private void Awake () {
        animator = GetComponent<EnemyAnimator>();
    }

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
                if(difficulty == EnemyDifficulty.EASY){
				    InvokeRepeating("AtackPlayer", 1.3f, 0.3f);
                }else if(difficulty == EnemyDifficulty.MEDIUM){
                    InvokeRepeating("AtackPlayer", .8f, 0.3f);
                }else if(difficulty == EnemyDifficulty.HARD){
                    InvokeRepeating("AtackPlayer", .4f, 0.3f);
                }
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
            if(directionState == DirectionState.RIGHT){
                gun.transform.position = gunPos[0].position;
                gun.transform.rotation = new Quaternion(0, 0, 0, 0);
                eyes.transform.position = Vector3.Lerp(eyes.transform.position, eyePos[0].transform.position, 10f * Time.deltaTime);
            }else if(directionState == DirectionState.LEFT){
                gun.transform.position = gunPos[1].position;
                gun.transform.rotation = new Quaternion(0, 180, 0, 0);
                eyes.transform.position = Vector3.Lerp(eyes.transform.position, eyePos[1].transform.position, 10f * Time.deltaTime);
            }
            float force = 87.5f;
            if(head.grounded){
                if(head.target){
                    if(head.target.gameObject.tag == "Player"){
                        if(directionState == DirectionState.RIGHT){
                            head.target.gameObject.GetComponent<Rigidbody2D>().AddForce((Vector3.up + Vector3.right) * force, ForceMode2D.Force);
                        }else if(directionState == DirectionState.LEFT){
                            head.target.gameObject.GetComponent<Rigidbody2D>().AddForce((Vector3.up + Vector3.left) * force, ForceMode2D.Force);
                        }
                    }
                }
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
    	stats = new EnemyStats("Player", stats.health);
    }

    public override void Patrol (Vector2 dir) {
    	if(!playerDetected){
    		//if(!rayColliders[0].grounded && rayColliders[1].grounded){
	    		transform.Translate((dir * GetSpeed()) * Time.deltaTime);
                RunWalk();
                animator.Play("idle", false);
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
                RunWalk();
    			animator.Play("idle", false);
                runAttack = false;
    		}else{
                StopWalk();
                animator.Play("idle", true);
    			runAttack = true;
    		}
    	}
    }

    private void RunWalk () {
        if(directionState == DirectionState.RIGHT){
            animator.Play("walk", true);
            animator.Play("walkbackword", false);
        }else if (directionState == DirectionState.LEFT){
            animator.Play("walk", false);
            animator.Play("walkbackword", true);
        }
    }

    private void StopWalk () {
        animator.Play("walk", false);
        animator.Play("walkbackword", false);
    }

    private IEnumerator WaitPatrolling () {
    	yield return new WaitForSeconds(2f);
    	SetupPatrol();
    	StopCoroutine("WaitPatrolling");
    }

    private void AtackPlayer () {
    	if(target){
    		if(directionState == DirectionState.RIGHT){
    			GameObject b = Instantiate(bulletObject, bulletSpawn.transform.position, Quaternion.identity);
    			GameObject bs = Instantiate(spawnSound, transform.position, Quaternion.identity);
                animator.Play("shot");
                b.gameObject.GetComponent<BulletTest>().inverse = false;
    			CancelInvoke();
    		}else if (directionState == DirectionState.LEFT){
    			GameObject b = Instantiate(bulletObject, bulletSpawn.transform.position, Quaternion.identity);
    			GameObject bs = Instantiate(spawnSound, transform.position, Quaternion.identity);
                animator.Play("shot");
                b.gameObject.GetComponent<BulletTest>().inverse = true;
    			CancelInvoke();
    		}
    	}
    }

    public void Demage (int dmg) {

		stats.Demage(dmg);

		if (stats.GetHealth() <= 0){
    		Die();
    	}
    }

    public void Die () {
    	isDead = true;
    	countdown = 0f;
    	stats.SetHealth(0);

		Destroy(gameObject);
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

public enum EnemyDifficulty {
    EASY, MEDIUM, HARD
}