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
	Animator anim;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponentInChildren<Animator>();
	}

    void Start()
    {
		dead = false;
	}

	float inputX;
	bool jump;

	void Update()
	{
		inputX = Input.GetAxis("Horizontal");
		jump = Input.GetButtonDown("Jump");
	}

	void FixedUpdate()
	{
		if(!dead)
		{
			Move(inputX);
			Jump(jump);
		}
	}

	void Move(float x)
	{		
		rb.velocity = new Vector2(x * movementSpeed, rb.velocity.y);

		if (x > 0)
			transform.eulerAngles = new Vector3(0, 0, 0);
		if (x < 0)
			transform.eulerAngles = new Vector3(0, 180, 0);
		
		anim.SetBool("moving", x != 0);
	}

	bool grounded;

	void Jump(bool jumpButton)
	{
		grounded = Physics2D.OverlapBox(jumpBoxTransform.position, jumpBoxTransform.localScale, 0f, rayCastLayer);

		if (grounded && jumpButton)
			rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}

		anim.SetBool("grounded", grounded);
	}

	bool dead;

	public void Damage(int damageValue)
	{
		if (life <= 0)
			dead = true;

		life -= damageValue;
	}

}
