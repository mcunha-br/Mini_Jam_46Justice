using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
	[SerializeField] Slider lifebar;
	[SerializeField] Transform jumpBoxTransform;
	[SerializeField] LayerMask rayCastLayer;
	[Space]
	[SerializeField] int life = 100;
	[SerializeField] float movementSpeed = 8f;
	[SerializeField] float jumpVelocity = 10f;
	[SerializeField] float fallMultiplier = 2f;
	[SerializeField] float lowJumpMultiplier = 3f;
	
	Rigidbody2D rb;
	Animator anim;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponentInChildren<Animator>();

		lifebar.value = life;
	}

    void Start()
    {
		dead = false;			
	}

	float inputX;
	public bool jump;

	void Update()
	{
		inputX = Input.GetAxis("Horizontal");
		holdingJump = Input.GetButton("Jump");

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;

			if (jumpCoroutine != null)
				StopCoroutine(jumpCoroutine);

			jumpCoroutine = JumpDelay(0.25f);
			StartCoroutine(jumpCoroutine);
		}
	}

	IEnumerator jumpCoroutine;

	IEnumerator JumpDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		jump = false;
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

	public bool grounded;
	bool holdingJump;

	void Jump(bool jumpButton)
	{
		grounded = Physics2D.OverlapBox(jumpBoxTransform.position, jumpBoxTransform.localScale, 0f, rayCastLayer);

		if (grounded && jumpButton)
			rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else
		if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		anim.SetBool("grounded", grounded);
	}

	bool dead;

	public void Damage(int damageValue)
	{
		if (life <= 0)
			dead = true;

		life -= damageValue;
		lifebar.value = life;
	}

}
