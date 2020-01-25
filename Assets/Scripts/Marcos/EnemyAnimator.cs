using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour {

	public Animator animator;

    private EnemyAnimator () {}

    private void Start()  {
    	this.Play("idle", true);
    }

    private EnemyAnimator GetEnemyAnimator () {
        return this;
    }

    public Animator GetAnimator () {return animator;}

    public void Play (string anim) {
    	animator.Play(anim);
    }

    public void Play (string anim, bool key) {
    	animator.SetBool(anim, key);
    }

}
