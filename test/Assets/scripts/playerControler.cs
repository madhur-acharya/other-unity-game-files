using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerControler : MonoBehaviour
{
	private Rigidbody2D player;
	private Animator animator;
	private Collider2D colider;
	private AudioSource footsteps;

	private enum PlayerStates {idle, running, jumping, falling, hurt}
	private PlayerStates currentState = PlayerStates.idle;

	[SerializeField] private float speed= 5f;
	[SerializeField] private float jumpForce= 7f;
	[SerializeField] private TMPro.TextMeshProUGUI cherryCounter;
	[SerializeField] private LayerMask standableLayer;
	[SerializeField] private GameObject mousePointer;
	[SerializeField] private GameObject orbPivot;
	private float direction= 0;
	private bool jumpButton= false;
	private bool onGround= false;
	private int collectableCount= 0;

	// Start is called before the first frame update
	void Start()
	{
		player= GetComponent<Rigidbody2D>();
		animator= GetComponent<Animator>();
		colider=  GetComponent<Collider2D>();
		footsteps= GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{

		Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z= 0;
		mousePointer.transform.position= mousePos;

		Vector3 diff= (mousePos - orbPivot.transform.position);
		float angle= Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		orbPivot.transform.rotation= Quaternion.Euler(new Vector3(0, 0, angle));

		direction= Input.GetAxisRaw("Horizontal");
		jumpButton= Input.GetButtonDown("Jump");
		onGround= colider.IsTouchingLayers(standableLayer);

		if(currentState != PlayerStates.hurt)
		{
			if(direction > 0)
			{
				player.velocity= new Vector2(speed, player.velocity.y);
				player.transform.localScale= new Vector2(1, player.transform.localScale.y);
			}
			else if(direction < 0)
			{
				player.velocity= new Vector2(-speed, player.velocity.y);
				player.transform.localScale= new Vector2(-1, player.transform.localScale.y);
			}
			else
			{
				if(onGround)
				{
					player.velocity= new Vector2(player.velocity.x * 0.95f, player.velocity.y);
				}
			}

			if(jumpButton && onGround)
			{
				player.velocity= new Vector2(player.velocity.x, jumpForce);
			}
		}
		
		handleAnimations();
	}

	void handleAnimations()
	{	

		if(currentState == PlayerStates.hurt)
		{

		}
		else
		{
			if(onGround)
			{
				if(jumpButton)
				{
					currentState= PlayerStates.jumping;
				}
				else if(Mathf.Abs(direction) > 0)
				{
					currentState= PlayerStates.running;
				}
				else
				{
					currentState= PlayerStates.idle;
				}
			}
			else
			{
				if(player.velocity.y < 0.1f)
				{
					currentState= PlayerStates.falling;
				}
			}
		}
			
		animator.SetInteger("playerState", (int)currentState);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "collectable")
		{
			Destroy(other.gameObject);
			collectableCount+= 1;
			cherryCounter.text= "Cherries: " + collectableCount.ToString();
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "enemy")
		{
			if(currentState == PlayerStates.falling)
			{
				EnimyControler enemy= other.gameObject.GetComponent<EnimyControler>();
				enemy.StartDeathAnimation();
				currentState= PlayerStates.jumping;
				player.velocity= new Vector2(player.velocity.x, 7);
			}
			else
			{
				currentState= PlayerStates.hurt;
				if(other.gameObject.transform.position.x > transform.position.x)
				{
					player.velocity= new Vector2(-5, 5);
				}
				else
				{
					player.velocity= new Vector2(5, 5);
				}
				StartCoroutine(hurtAnimationRoutine());
			}
			
		}
	}

	IEnumerator hurtAnimationRoutine()
	{
		yield return new WaitForSeconds(1f);
		currentState= PlayerStates.idle;
	}

	public void playFootSteps()
	{
		footsteps.Play();
	}

}

