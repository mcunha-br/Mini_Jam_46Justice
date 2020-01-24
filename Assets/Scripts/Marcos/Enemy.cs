using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AI {

	private const string playerTag = "Player";

    protected Enemy () {}

    private void Start()  {
        
    }

    private void Update() {
        
    }

    public override void Patrol () {

    }

    public Enemy GetEnemy () {return this;}
    public string GetTargetTag () {return playerTag;}

}
