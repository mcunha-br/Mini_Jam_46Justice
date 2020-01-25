using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	public float speed;
	public int damage;

	public float direction;

	void Start()
	{
		if (transform.eulerAngles.z == 0)
			direction = 1f;
		if (transform.eulerAngles.z == 180 || transform.eulerAngles.z == -180)
			direction = -1f;
	}

    void Update()
    {
		transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag != "Player" && other.transform.tag != "PlayerBullet" && !other.isTrigger)
		{
			if (other.GetComponent<IEnemy>() != null)
				other.GetComponent<IEnemy>().Demage(damage);

			Destroy(gameObject);
		}
	}
}
