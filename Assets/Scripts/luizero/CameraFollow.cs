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
		if (player == null)
			player = FindObjectOfType<PlayerBehaviour>();
	}

    void LateUpdate()
    {
		bool inRange = player.transform.position.x > transform.position.x - sightBoxSize.x/2 && player.transform.position.x < transform.position.x + sightBoxSize.x / 2;

		if (!inRange)
			transform.position = Vector3.Lerp(transform.position, player.transform.position + playerOffset, speed * Time.deltaTime);
    }
}
