using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement instance;
	private List<AstarNode> myNodes;
	public bool canMove;
	public int counter;
	public int max;
	public float mySpeed = 3;

	void Awake()
	{
		instance = this;
	}

	public void StartMoving(List<AstarNode> nodes)
	{
		myNodes = nodes;
		canMove = true;
		if (nodes.Count > 0)
			counter = 1;
		else
			counter = 0;
		max = nodes.Count;
	}

	void Update()
	{
		if (canMove)
		{
			if(counter < myNodes.Count)
			{
				if(Mathf.Abs(transform.position.x - myNodes[counter].x) > 0.01f ||
				   Mathf.Abs(transform.position.y - myNodes[counter].y) > 0.01f)
				{
					transform.position = Vector3.MoveTowards(transform.position, 
					                                         new Vector3(myNodes[counter].x, 
					            myNodes[counter].y,
					            transform.position.z),
					                                         Time.deltaTime * mySpeed);
				}
				else	
				{
					counter ++;
				}
			}
			else
			{
				counter = 0;
				canMove = false;
			}
		}
	}
}
