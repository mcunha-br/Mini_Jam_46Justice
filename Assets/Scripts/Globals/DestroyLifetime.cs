using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLifetime : MonoBehaviour {

    private DestroyLifetime () {}

    public float lifetime = 5f;
     
    private void  Awake (){
        Destroy(this, lifetime);
    }

    private void Start()  {
        
    }

    private void Update() {
        
    }

    private DestroyLifetime GetDestroyLifetime () {
        return this;
    }

}
