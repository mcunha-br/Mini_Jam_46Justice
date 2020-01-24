using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformController : MonoBehaviour {

    public Transform plataform;
    public float speed;
    public Transform A, B;

    private Vector3 target;

    private void Start () {
        target = A.position;
    }

    private void Update () {

        plataform.position = Vector2.MoveTowards(plataform.position, target, speed * Time.deltaTime);

        if(plataform.position == target) {
            target = (target == B.position) ? A.position : B.position;
        }
    }
}