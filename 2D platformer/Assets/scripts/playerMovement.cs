using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
	public float speed= 5f;
	public float jumpVelocity= 7f;
	public float fallMultiplier= 5f;
	public float riseMultiplier= 1.1f;
	public Transform wallCheck;
	public Transform groundCheck;
	public float platformCheckRadius= 0.2f;
	public LayerMask platform;
	public float wallSlideSpeed= 0.5f;

	private Vector2 direction= new Vector2(0, 0);
	private Rigidbody2D rb;
	private bool isGrounded= false;
	private bool isOnWall= false;

	void Start()
	{
		rb= GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		float x= Input.GetAxis("Horizontal");
		float y= Input.GetAxis("Vertical");
		direction= new Vector2(x, y);

		horizontalMovement();
		verticalMovement();
		wallSlide();
	}

	void FixedUpdate()
	{
		checkSurroundings();
	}

	private void checkSurroundings()
	{
		isGrounded= Physics2D.OverlapCircle(groundCheck.position, platformCheckRadius, platform);
		isOnWall= Physics2D.OverlapCircle(wallCheck.position, platformCheckRadius, platform);
	}

	private void wallSlide()
	{
		if(!isGrounded && isOnWall && !Input.GetButton("Jump"))
		{
			if(Mathf.Sign(direction.x) == transform.localScale.x)
			{
				rb.velocity= new Vector2(rb.velocity.x, -wallSlideSpeed);
			}
			else
			{

			}
		}
	}

	private void horizontalMovement()
	{
		rb.velocity= new Vector2(direction.x * speed, rb.velocity.y);
		if(direction.x > 0)
			transform.localScale= new Vector2(1, 1);
		else if(direction.x < 0)
			transform.localScale= new Vector2(-1, 1);
	}

	private void verticalMovement()
	{
		if(Input.GetButtonDown("Jump") && (isGrounded || isOnWall))
		{
			rb.gravityScale= riseMultiplier;
			rb.velocity= new Vector2(rb.velocity.x, (Vector2.up.y * jumpVelocity));
		}

		if(rb.velocity.y < 0)
		{
			rb.gravityScale= fallMultiplier;
		}
		else if(rb.velocity.y > 0 && Input.GetButtonUp("Jump"))
		{
			rb.velocity= new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color= isGrounded ? Color.red : Color.white;
		Gizmos.DrawWireSphere(groundCheck.position, platformCheckRadius);
		Gizmos.color= isOnWall ? Color.red : Color.white;
		Gizmos.DrawWireSphere(wallCheck.position, platformCheckRadius);
	}
}


