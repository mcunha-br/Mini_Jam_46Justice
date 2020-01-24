using UnityEngine;

public class KillPlayer : MonoBehaviour {
    

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
            other.gameObject.SetActive(false);
    }
}