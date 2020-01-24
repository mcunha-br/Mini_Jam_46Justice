using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public abstract class AI : MonoBehaviour {

	[HideInInspector] public bool inPatrolling;

	private byte[] _distancePatrol = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
	private float[] _secondsPatrolDir = {5, 6, 7, 8, 9};
	private float speed = 1.2f;

    public abstract void Patrol (Vector2 dir);

    public virtual byte GetNewDirectionPatrol () {
    	return _distancePatrol[Random.Range(0, _distancePatrol.Length)];
    }

    public virtual float GetNewSecondPatrolDir () {
    	return _secondsPatrolDir[Random.Range(0, _secondsPatrolDir.Length)];
    }

    public float GetSpeed () {return speed;}


}

public enum DirectionState {
	LEFT, RIGHT
}
