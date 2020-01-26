using UnityEngine;

public class KillPlayer : MonoBehaviour {
    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerBehaviour>().Damage(30);
        }
         
    }
}