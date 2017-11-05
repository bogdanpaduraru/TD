using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Astar : MonoBehaviour {

	public static Astar instance;

	public List<AstarNode> Open;
	public List<AstarNode> Closed;

	public AVLNode open2;
	public AVLNode closed2;

	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	private int[,] matrix;

	//private int squareSize;

    public int numberOfColumns;
    public int numberOfLines;

    public List<Vector2> path;

	void Awake()
	{
		instance = this;

		GameObject plane = GameObject.FindGameObjectWithTag ("Plane");

		minX = plane.transform.position.x - plane.transform.localScale.x/2;
		minY = plane.transform.position.y - plane.transform.localScale.y/2;

		maxX = plane.transform.position.x + plane.transform.localScale.x/2;
		maxY = plane.transform.position.y + plane.transform.localScale.y/2;

		int x = (int)(Mathf.Abs (minX) + Mathf.Abs (maxX));
		int y = (int)(Mathf.Abs (minY) + Mathf.Abs (maxY));

		matrix = new int[x,y];

        Open = new List<AstarNode>();
        Closed = new List<AstarNode>();
	}

	public void CreateMatrix(float _maxX, float _minX, float _maxY, float _minY, int _squareSize)
	{
		maxX = _maxX;
		minX = _minX;

		maxY = _maxY;
		minY = _minY;

        numberOfColumns = ((int)(Mathf.Abs(_minX) + Mathf.Abs(_maxX)) / _squareSize) + 1;
        numberOfLines = ((int)(Mathf.Abs(_minY) + Mathf.Abs(_maxY)) / _squareSize) + 1;

		matrix = new int[numberOfLines, numberOfColumns];
		//squareSize = _squareSize;
	}

	public void RegisterObstacle(int lineIndex, int columnIndex)
	{
        matrix[lineIndex, columnIndex] = 1;
	}

    public void ShowMap()
    {
        for(int i=0;i<numberOfLines;i++)
        {
            Debug.Log("Line:" + (i + 1));
            for(int j=0;j<numberOfColumns;j++)
            {
                Debug.Log(matrix[i, j]);
            }
        }
    }

	private bool BestNodeIsNotFinishNode(AstarNode end)
	{
		double cost = 99999;
		AstarNode tempNode = null;
		//AstarNode tempNode2 = null;

		for (int i=0; i<Open.Count; i++) 
		{
			if(Open[i].rank <= cost && !ClosedContainsNode(Open[i]))
			{
				tempNode = Open[i];
				cost = Open[i].rank;
			}
		}

		//tempNode2 = (AstarNode)open2.GetLowestValueNood (open2).abstractObject;
		/*
		if (tempNode2.x == end.x && tempNode2.y == end.y) 
		{
			return false;
		}
		return true;
		*/
		///*
		if (tempNode.x == end.x && tempNode.y == end.y) 
		{
			return false;
		}
		return true;
		//*/
	}

	private AstarNode GetBestNode()
	{
		double cost = 99999;
		AstarNode tempNode = null;
		//AstarNode tempNode2 = null;

		for (int i=0; i<Open.Count; i++) 
		{
			if(Open[i].rank <= cost && !ClosedContainsNode(Open[i]))
			{
				tempNode = Open[i];
				cost = Open[i].rank;
			}
		}
		Open.Remove (tempNode);

		//tempNode2 = (AstarNode)open2.GetLowestValueNood (open2).abstractObject;
		
		//open2 = open2.Delete (open2, (int)tempNode2.rank);
		
		//return tempNode2;
		//Debug.Log ("Best node returned:" + tempNode.x + ":" + tempNode.y);
		return tempNode;
	}

	private bool ClosedContainsNode(AstarNode node)
	{
		for (int i=0; i<Closed.Count; i++) 
		{
			if(Closed[i].x == node.x && Closed[i].y == node.y)
				return true;
		}
		return false;
	}

	private double H(AstarNode start, AstarNode end)
	{
        double D = 1;//, D2 = Mathf.Sqrt (2);
		int dx, dy;

		dx = Mathf.Abs (start.x - end.x);
		dy = Mathf.Abs (start.y - end.y);
		//return System.Math.Round((D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy)),2);
		return D * (dx + dy);
	}

	private double MovementCost(AstarNode start, AstarNode end)
	{
        double D = 1;//, D2 = Mathf.Sqrt (2);
		int dx, dy;
		
		dx = Mathf.Abs (start.x - end.x);
		dy = Mathf.Abs (start.y - end.y);
		//return ((dx > 0) && (dy > 0)) ? System.Math.Round(D2,2) : D;
		return D * (dx + dy);
	}

	private List<AstarNode> GetNeighbours(AstarNode node)
	{
		List<AstarNode> result = new List<AstarNode> ();

        //Debug.Log("node coords:" + node.x + "," +node.y);

		//Debug.Log ("Node coord in matrix:" + (int)(maxY - node.y) + ":" + (int)(maxX + node.x));

        //Vector2 matrixCoords = MapCreator.instance.GetMatrixCoordFromWorldCoords(node.x, node.y);
        //node.x = (int)matrixCoords.x;
        //node.y = (int)matrixCoords.y;

		//int nodX, nodY;

		//left
        if (node.y - 1 >= 0 && matrix[node.x,node.y - 1] == 0)
        {
            result.Add(new AstarNode(node.x, node.y - 1));
        }
        //nodX = node.x - 1;
        //nodY = node.y;
        //if (nodX >= (int)minX)
        //    if(matrix[(int)(maxY - nodY), (int)(maxX + nodX)] == 0)
        //        result.Add(new AstarNode (nodX, nodY));

		//right
        if (node.y + 1 < numberOfColumns && matrix[node.x, node.y + 1] == 0)
        {
            result.Add(new AstarNode(node.x, node.y + 1));
        }
        //nodX = node.x + 1;
        //nodY = node.y;
        //if (nodX <= (int)maxX)
        //    if(matrix[(int)(maxY - nodY), (int)(maxX + nodX)] == 0)
        //        result.Add(new AstarNode (nodX, nodY));

		//down
        if(node.x + 1 < numberOfLines && matrix[node.x + 1,node.y ] == 0)
        {
            result.Add(new AstarNode(node.x + 1, node.y));
        }
        //nodX = node.x;
        //nodY = node.y - 1;
        //if (nodY >= (int)minY)
        //    if(matrix[(int)(maxY - nodY), (int)(maxX + nodX)] == 0)
        //        result.Add(new AstarNode (nodX, nodY));

		//up
        if (node.x - 1 >= 0 && matrix[node.x - 1,node.y] == 0)
        {
            result.Add(new AstarNode(node.x - 1, node.y));
        }
        //nodX = node.x;
        //nodY = node.y + 1;
        //if (nodY <= (int)maxY)
        //    if(matrix[(int)(maxY - nodY), (int)(maxX + nodX)] == 0)
        //        result.Add(new AstarNode (nodX, nodY));
		/*
		//left + up
		nodX = node.x - 1;
		nodY = node.y + 1;
		if (nodX >= (int)minX && nodY <= (int)maxY)
			if(matrix [(int)(maxY - nodY), (int)(maxX + nodX)] == 0) 
				result.Add (new AstarNode (nodX, nodY));

		//left + down
		nodX = node.x - 1;
		nodY = node.y - 1;
		if (nodX >= (int)minX && nodY >= (int)minY)
			if(matrix[(int)(maxY - nodY), (int)(maxX + nodX)] == 0)
				result.Add(new AstarNode (nodX, nodY));

		//right + up
		nodX = node.x + 1;
		nodY = node.y + 1;
		if (nodX <= (int)maxX && nodY <= (int)maxY)
			if(matrix[(int)(maxY - nodY), (int)(maxX + nodX)] == 0)
				result.Add(new AstarNode (nodX, nodY));

		//right + down
		nodX = node.x + 1;
		nodY = node.y - 1;
		if (nodX <= (int)maxX && nodY >= (int)minY)
		{
			if(matrix [(int)(maxY - nodY), (int)(maxX + nodX)] == 0) 
		
				result.Add (new AstarNode (nodX, nodY));
		}
		*/

		return result;
	}

	private void RemoveNodeFromOpen(AstarNode node)
	{
		for (int i=0; i<Open.Count; i++) 
		{
			if(Open[i].x == node.x && Open[i].y == node.y)
			{	
				Open.RemoveAt(i);
				return;
			}
		}
	}

	private void RemoveNodeFromClosed(AstarNode node)
	{
		for (int i=0; i<Closed.Count; i++) 
		{
			if(Closed[i].x == node.x && Closed[i].y == node.y)
			{	
				Closed.RemoveAt(i);
				return;
			}
		}
	}

	private bool ListContains(List<AstarNode> list, AstarNode node)
	{
		for(int i=0;i<list.Count;i++)
		{
			if(list[i].x == node.x && list[i].y == node.y)
				return true;
		}
		return false;
	}

    public void CreatePath(int startX, int startY, int endX, int endY)
    {
        Debug.Log("Path requested from (" + startX + "," + startY + ") to (" + endX + "," + endY + ")");
        double cost;

        AstarNode current;
        AstarNode start = new AstarNode(startX, startY);
        AstarNode end = new AstarNode(endX, endY);

        List<AstarNode> neighbours;

        Open.Clear();
        Closed.Clear();

        start.g = 0;
        Open.Add(start);

        while (BestNodeIsNotFinishNode(end))
        {
            current = GetBestNode();

            Closed.Add(current);

            neighbours = GetNeighbours(current);

            foreach (AstarNode neighbour in neighbours)
            {
                cost = current.g + MovementCost(current, neighbour);

                if (ListContains(Open, neighbour) && cost < neighbour.g)
                {
                    RemoveNodeFromOpen(neighbour);
                }

                if (ListContains(Closed, neighbour) && cost < neighbour.g)
                {
                    RemoveNodeFromClosed(neighbour);
                }

                if (!ListContains(Open, neighbour) && !ListContains(Closed, neighbour))
                {
                    neighbour.g = cost;
                    neighbour.parrent = current;
                    neighbour.rank = neighbour.g + H(neighbour, end);
                    Open.Add(neighbour);
                }
            }
        }

        List<AstarNode> finalResult = new List<AstarNode>();
        AstarNode a = Closed[Closed.Count - 1];

        finalResult.Add(end);
        while (a != null)
        {
            finalResult.Add(a);
            a = a.parrent;
        }

        finalResult.Reverse();

        //path = MapCreator.instance.GetWoorldCoordsFromMatrixCoords(finalResult);
        //MapCreator.instance.RemoveRoadSpots(path);
    }
}
