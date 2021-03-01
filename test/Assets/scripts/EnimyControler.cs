using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnimyControler : MonoBehaviour
{
	protected Animator animator;
	private Rigidbody2D enemy_rb;

	protected virtual void Start()
	{
		animator= GetComponent<Animator>();
		enemy_rb= gameObject.GetComponent<Rigidbody2D>();
	}

	public void StartDeathAnimation()
	{
		enemy_rb.velocity= new Vector2(0, 0);
		animator.SetTrigger("death");
		enemy_rb.bodyType= RigidbodyType2D.Static;
		GetComponent<Collider2D>().enabled= false;
	}

	private void destroyGameObject()
	{
		Destroy(this.gameObject);
	}
}
