using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AstarNode
{
	public int x; 
	public int y; 
	public AstarNode parrent;

	public double g;
	public double rank;
	
	public AstarNode(int xx, int yy)
	{
		x = xx;
		y = yy;
		rank = 0;
		g = 0;
	}
}