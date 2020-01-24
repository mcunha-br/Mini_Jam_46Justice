using UnityEngine;

public class SpikeController : MonoBehaviour {
    
    public float speed;
    public float countdown = 3;
    public Transform spike;
    public Transform A, B;

    private AudioSource source;
    private Vector3 target;
    private float timer;

    private void Start() {
        source = GetComponent<AudioSource>();
        target = A.position;
    }


    private void Update() {
        timer += Time.deltaTime;

        spike.position = Vector2.MoveTowards(spike.position, target, speed * Time.deltaTime);

        if(timer >= countdown && spike.position == target) {
            timer = 0;
            target = (target == B.position) ? A.position : B.position;
            source.Play();
        }
    }


}