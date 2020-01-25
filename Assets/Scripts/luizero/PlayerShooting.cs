using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	public float shootBreak;
	public int shootTimes;

    void Start()
    {
		//instantiate necessary bullets

		coroutine = asd(shootBreak);
		StartCoroutine(coroutine);
	}

	bool shooting = true;

    void Update()
    {
		//shooting = Input.GetKey(KeyCode.J);

		//if (!shooting && coroutine != null)
		//	StopCoroutine(coroutine);

		if (Input.GetKeyDown(KeyCode.J))
		{
			//if (coroutine != null)
			//	StopCoroutine(coroutine);

			//coroutine = asd(shootBreak);
			//StartCoroutine(coroutine);
		}
    }
	IEnumerator coroutine;
	IEnumerator asd(float shootBreak)
	{
		while (shooting)
		{
			shootTimes++;
			yield return new WaitForSeconds(shootBreak);
		}
	}

	void Shoot()
	{
		shootTimes++;
	}

	void InstantiateBullet()
	{
		//take from storage and enable
	}

	void DestroyBullet()
	{
		//take to storage and disable
	}
}
