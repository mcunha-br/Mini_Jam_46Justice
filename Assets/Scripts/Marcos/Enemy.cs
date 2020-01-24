using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AI, IEnemy {

	[Header("Stats")]
	public bool isDead = false;
	public EnemyStats stats;

	[Header("Axis State Patrol")]
	public DirectionState directionState;

	private const string playerTag = "Player";
	private byte direction;
	private float countdown = 0f;
	private float turnSecondsDir;

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
		}
    }

    private void SetupPatrol () {
    	inPatrolling = true;
    	direction = GetNewDirectionPatrol();
    	turnSecondsDir = GetNewSecondPatrolDir();
    }

    private void SetupStats () {
    	isDead = false;
    	stats = new EnemyStats("Player", 100);
    }

    public override void Patrol (Vector2 dir) {
    	transform.Translate((dir * GetSpeed()) * Time.deltaTime);
    	countdown++;
    	if(countdown >= turnSecondsDir * 100){
    		countdown = 0f;
    		inPatrolling = false;
    		StartCoroutine(WaitPatrolling());
    	}
    }

    private IEnumerator WaitPatrolling () {
    	yield return new WaitForSeconds(2f);
    	SetupPatrol();
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