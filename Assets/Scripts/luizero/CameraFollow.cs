using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public PlayerBehaviour player;

	[SerializeField] float speed;
	[SerializeField] Vector3 playerOffset = new Vector3(0,0,-10);
	[SerializeField] Vector2 sightBoxSize;

	void Awake()
    {
        player = FindObjectOfType<PlayerBehaviour>();
    }

    void LateUpdate()
    {
		//bool inRange = player.transform.position < transform.position + sightBoxSize.x/2;

		transform.position = Vector3.Lerp(transform.position, player.transform.position + playerOffset, speed * Time.deltaTime);
    }
}
