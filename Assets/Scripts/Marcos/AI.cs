using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour {

	private int[] _distancePatrol = {1, 2, 3, 4, 5, 6, 7, 8, 9};

    public abstract void Patrol ();

    public virtual int GetNewDistancePatrol () {
    	return _distancePatrol[Random.Range(0, _distancePatrol.Length)];
    }


}
