using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement2 : MonoBehaviour
{
	public float speed= 5f;
	public float jumpVelocity= 7f;
	public float fallMultiplier= 5f;
	public float riseMultiplier= 1.1f;
	public Transform wallCheck;
	public Transform groundCheck;
	public Vector2 groundCheckDimentions= new Vector2(0.5f, 0.1f);
	public Vector2 wallCheckDimentions= new Vector2(0.1f, 0.5f);
	public LayerMask platform;
	public float wallSlideSpeed= 0.5f;
	public Vector2 wallJumpNudge= new Vector2(5, 5);
	public Vector2 wallJumpOff= new Vector2(17, 5);

	private float horizontalMovement= 0f;
	private int direction= 1;
	private Rigidbody2D rb;
	private bool isGrounded= false;
	private bool isOnWall= false;
	private bool isInAir= false;
	private bool canJump= true;
	private bool wallGripped= false;
	private bool wallJumped= false;

	void Start()
	{
		rb= GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		horizontalMovement= Input.GetAxis("Horizontal");

		isGrounded= Physics2D.OverlapBox(groundCheck.position, groundCheckDimentions, 0, platform);
		isOnWall= Physics2D.OverlapBox(wallCheck.position, groundCheckDimentions, 0, platform);
		isInAir= !isGrounded && !isOnWall;
		canJump= isGrounded;

		handleMovement();
	}

	void FixedUpdate()
	{
		
	}

	private void handleMovement()
	{
		bool isJumping= Input.GetButton("Jump");

		if(horizontalMovement > 0)
		{
			transform.localScale= new Vector2(1, 1);
			direction= 1;
		}
		else if(horizontalMovement < 0)
		{
			transform.localScale= new Vector2(-1, 1);
			direction= -1;
		}

		if(Input.GetButtonDown("Jump"))
		{
			if(isGrounded)
			{
				rb.gravityScale= riseMultiplier;
				rb.velocity= new Vector2(rb.velocity.x, rb.velocity.y + jumpVelocity);
			}
			else if(isOnWall)
			{
				if(direction == Mathf.Sign(horizontalMovement))
				{
					if(rb.velocity.y < 0)
					{
						rb.velocity= new Vector2(wallJumpOff.x * -direction, wallJumpOff.y);
					}
					else
					{
						rb.velocity= new Vector2(wallJumpOff.x * -direction, rb.velocity.y + wallJumpOff.y);
					}
					wallJumped= true;
					StopCoroutine(disableWallJumped(0));
					StartCoroutine(disableWallJumped(0.4f));
				}
				else
				{
					if(rb.velocity.y < 0)
					{
						rb.velocity= new Vector2(wallJumpNudge.x * -direction, wallJumpNudge.y);
					}
					else
					{
						rb.velocity= new Vector2(wallJumpNudge.x * -direction, rb.velocity.y + wallJumpNudge.y);
					}
					wallJumped= true;
					StopCoroutine(disableWallJumped(0));
					StartCoroutine(disableWallJumped(0.2f));
				}
			}
		}
		else if(Input.GetButtonUp("Jump"))
		{
			if(isInAir)
			{
				if(rb.velocity.y > 0)
				{
					rb.velocity= new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
				}
			}
		}
		else
		{
			if(isInAir)
			{
				if(rb.velocity.y < 0)
				{
					rb.gravityScale= fallMultiplier;
				}
				if(!wallJumped)
				{
					rb.velocity= new Vector2(horizontalMovement * speed, rb.velocity.y);
				}
			}
			else if(isGrounded)
			{
				rb.velocity= new Vector2(horizontalMovement * speed, rb.velocity.y);
			}
		}
	}

	private IEnumerator disableWallJumped(float delay= 0.35f)
	{
		yield return new WaitForSeconds(delay);
		wallJumped= false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color= isGrounded ? Color.red : Color.white;
		Gizmos.DrawCube(groundCheck.position, groundCheckDimentions);

		Gizmos.color= isOnWall ? Color.red : Color.white;
		Gizmos.DrawCube(wallCheck.position, wallCheckDimentions);
	}
}
