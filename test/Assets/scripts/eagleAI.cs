using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eagleAI : EnimyControler
{
	private Rigidbody2D rb;
	private Collider2D colider;

	[SerializeField] private GameObject limitTop;
	[SerializeField] private GameObject limitBottom;
	[SerializeField] private float filghtSpeed= 2;
	[SerializeField] private int direction= 1;
	private bool safeSwitch= true;

	protected override void Start()
	{
		base.Start();
		rb= gameObject.GetComponent<Rigidbody2D>();
		colider= gameObject.GetComponent<Collider2D>();
	}

	void Update()
	{
		if(transform.position.y > limitTop.transform.position.y || transform.position.y < limitBottom.transform.position.y)
		{
			if(safeSwitch)
			{
				direction= direction < 0 ? 1 : -1;
				safeSwitch= false;
			}
		}
		else
		{
			safeSwitch= true;
		}
		rb.velocity = new Vector2(0, filghtSpeed * direction);
	}
}
