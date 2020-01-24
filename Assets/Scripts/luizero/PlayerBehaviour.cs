using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	[SerializeField] Transform jumpBoxTransform;
	[SerializeField] LayerMask rayCastLayer;
	[SerializeField] float groundCheckRange = .25f;
	[Space]
	[SerializeField] int life = 100;
	[SerializeField] float movementSpeed;
	[SerializeField] float jumpVelocity;
	[SerializeField] float fallMultiplier;

	Rigidbody2D rb;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

	void Update()
	{
		if(canMoveInAir)
		Move();
		Jump();

		//canMoveInAir = true;
		ground = Grounded();
	}

	public bool ground;

	void Move()
	{
		float x = Input.GetAxis("Horizontal");

		rb.velocity = new Vector2(x * movementSpeed, rb.velocity.y);
	}

	void Jump()
	{
		if (Grounded() && Input.GetButtonDown("Jump"))
			rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		
	}

	bool Grounded()
	{
		return Physics2D.OverlapBox(jumpBoxTransform.position, jumpBoxTransform.localScale, 0f, rayCastLayer);
		//return Physics2D.BoxCast(raycastTransform.position, groundCheckBoxSize, 0f, Vector2.down, groundCheckRange);
		//return Physics2D.Raycast(raycastTransform.position, Vector2.down * groundCheckSize);
	}

	public void Damage(int damageValue)
	{
		life -= damageValue;
	}

	public bool canMoveInAir;

	void OnCollisionStay2D(Collision2D other)
	{
		if (Grounded())
			canMoveInAir = true;
		else
		if (!Grounded())
		{
			canMoveInAir = false;
			Debug.Log("asd");
			return;
		}

	}

	void OnCollisionExit2D(Collision2D other)
	{		
			canMoveInAir = true;
		Debug.Log("tru");
	}
}
