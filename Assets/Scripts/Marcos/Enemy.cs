using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AI {

	private const string playerTag = "Player";

    private Enemy () {}

    private void Start()  {
        
    }

    private void Update() {
        
    }

    public override void Patrol () {
    	
    }

    private Enemy GetEnemy () {return this;}
    private string GetTargetTag () {return playerTag;}

}
