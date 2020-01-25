using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	public GameObject playerBulletPrefab;

	public float shootSpeed;
	public int shootDamage;
	public float shootBreak;

	public float bulletLifeTime;

	AudioSource audio;

    void Start()
    {
		//instantiate necessary bullets and disable

		audio = GetComponent<AudioSource>();
	}

	public bool canShoot = true;

    void Update()
    {
		if (Input.GetButtonDown("Fire1") && canShoot)
		{
			Shoot();
			StartCoroutine(ShootDelay(shootBreak));
		}
    }

	IEnumerator ShootDelay(float delay)
	{
		float normalizedime = 0f;

		while (normalizedime <= 1f)
		{
			normalizedime += Time.deltaTime / delay;

			if(normalizedime >= 1f) canShoot = true;

			yield return null;
		}				
	}

	void Shoot()
	{		
		InstantiateBullet();
		audio.PlayOneShot(audio.clip);
		canShoot = false;
	}

	void InstantiateBullet()
	{
		Debug.Log("shoot");
		GameObject go = Instantiate(playerBulletPrefab, transform.position, transform.rotation);

		PlayerBullet pb = go.GetComponent<PlayerBullet>();
		pb.speed = shootSpeed;
		pb.damage = shootDamage;

		Destroy(go, bulletLifeTime);

		//take from storage and enable
	}

	void DestroyBullet()
	{
		//take to storage and disable
	}
}
