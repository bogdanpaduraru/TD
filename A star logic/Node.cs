using UnityEngine;
using System.Collections;

public class Node {

	public float value 
	{
		get;
		private set;
	}

	public Node left 
	{
		get;
		set;
	}


	public Node right 
	{
				get;
				set;
	}

	public Node(float _value)
	{
		value = _value;
		right = null;
		left = null;
	}
}
