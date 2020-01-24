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
		Vector3 playerPos = player.transform.position;
		Vector3 pos = transform.position;

		bool inRangeX = playerPos.x > pos.x - sightBoxSize.x / 2 && playerPos.x < pos.x + sightBoxSize.x / 2;
		bool inRangeY = playerPos.y > pos.y - sightBoxSize.y / 2 && playerPos.y < pos.y + sightBoxSize.y / 2;

		Vector3 newPosition = transform.position;

		if (!inRangeX)
			newPosition.x = playerPos.x;
		if (!inRangeY)
			newPosition.y = playerPos.y;
		newPosition.z = 0;


		transform.position = Vector3.Lerp(pos, newPosition + playerOffset, speed * Time.deltaTime);
    }
}
