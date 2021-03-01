using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogAI : EnimyControler
{
	[SerializeField] protected float jumpHeight= 2f;
	[SerializeField] protected float jumplength= 2f;
	[SerializeField] private GameObject limitLeft;
	[SerializeField] private GameObject limitRight;
	[SerializeField] private LayerMask standableLayer;

	private Rigidbody2D frog;
	private Collider2D colider;

	private enum direction {left= -1, right= 1}
	private enum states {idle, jumping, falling}
	private direction currentDirection;
	private states currentState;

	private bool onGround= false;
	private bool routineReady= true;

    // Start is called before the first frame update
    protected override void Start()
    {
    	base.Start();

        currentDirection= direction.left;
        currentState= states.idle;

        colider=  GetComponent<Collider2D>();
        frog= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		onGround= colider.IsTouchingLayers(standableLayer);

		if(onGround)
		{
			if(currentState != states.jumping)
			{
				currentState= states.idle;
				if(routineReady == true)
				{
					StartCoroutine(waitForJumpRoutine());
				}
			}
		}
		else
		{
			if(frog.velocity.y < 0.1f)
			{
				currentState= states.falling;
			}
		}

    	animator.SetInteger("frogState", (int)currentState);
    }

	IEnumerator waitForJumpRoutine()
	{
		routineReady= false;
		yield return new WaitForSeconds(2f);
		
		System.Array arr = System.Enum.GetValues(typeof(direction));

		if(currentDirection == direction.left)
		{
			if(limitLeft.transform.position.x < (transform.position.x - jumplength))
			{
				currentDirection= direction.right;
				transform.localScale= new Vector2(1, 1);
				frog.velocity = new Vector2(-jumplength, jumpHeight);
			}
			else
			{
				currentDirection = (direction)arr.GetValue(UnityEngine.Random.Range(0,arr.Length));
				transform.localScale= new Vector2((int)currentDirection, 1);
				frog.velocity = new Vector2(-Mathf.Abs(transform.position.x - limitLeft.transform.position.x) * (int)currentDirection, jumpHeight);
			}
		}
		else
		{
			if(limitRight.transform.position.x > (transform.position.x + jumplength))
			{
				currentDirection= direction.left;
				transform.localScale= new Vector2(-1, 1);
				frog.velocity = new Vector2(jumplength, jumpHeight);
			}
			else
			{
				currentDirection = (direction)arr.GetValue(UnityEngine.Random.Range(0,arr.Length));
				transform.localScale= new Vector2((int)currentDirection, 1);
				frog.velocity = new Vector2(-Mathf.Abs(transform.position.x - limitRight.transform.position.x) * (int)currentDirection, jumpHeight);
			}
		}

		currentState= states.jumping;

		routineReady= true;
	}

	/*public void StartDeathAnimation()
	{
		jumpHeight= 0f;
		jumplength= 0f;
		animator.SetTrigger("death");
	}

	public void destroyGameObject()
	{
		Destroy(this.gameObject);
	}*/

}




