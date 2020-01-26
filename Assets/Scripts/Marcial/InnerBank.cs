using UnityEngine;

public class InnerBank : MonoBehaviour {

    public GameObject virtualCam;
    public Transform startPosition;
    public Animator anim;

    private Transform player;

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            anim.SetTrigger ("fade");
            player = other.transform;
            Invoke ("FADE", 0.3f);
        }
    }

    private void FADE () {
        player.position = startPosition.position;
        virtualCam.SetActive (true);
        player.GetComponent<PlayerBehaviour> ().startPos = startPosition.position;
    }
}