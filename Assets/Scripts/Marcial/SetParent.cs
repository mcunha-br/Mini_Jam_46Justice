using UnityEngine;

public class SetParent : MonoBehaviour {
    
    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("Player")) {
            other.transform.SetParent(this.transform);
        }
    }


    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")) {
            other.transform.SetParent(null);
        }
    }
}