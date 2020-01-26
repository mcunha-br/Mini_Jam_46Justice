using UnityEngine;

public class KillPlayer : MonoBehaviour {

    public int damage = 30;
    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerBehaviour>().Damage(damage);
        }
         
    }
}