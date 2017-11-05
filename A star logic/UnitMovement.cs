using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMovement : MonoBehaviour {

    public List<Vector2> mapNodes;
	public bool canMove;
	public int counter;
	public int max;
	public float mySpeed = 50f;
	
	public void StartMoving()
	{
		canMove = true;
        if (mapNodes.Count > 0)
			counter = 1;
		else
			counter = 0;
        max = mapNodes.Count;

        mapNodes = Astar.instance.path;
    }
	
	void Update()
	{
        //if (!TimeManager.gameIsPaused)
        {
            if (canMove)
            {
                if (counter < mapNodes.Count)
                {
                    if (Mathf.Abs(transform.position.x - mapNodes[counter].x) > 0.01f ||
                       Mathf.Abs(transform.position.y - mapNodes[counter].y) > 0.01f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(mapNodes[counter].x, mapNodes[counter].y, transform.position.z), Time.deltaTime * mySpeed);
                    }
                    else
                    {
                        counter++;
                    }
                }
                else
                {
                    counter = 0;
                    canMove = false;
                    Destroy(this.gameObject);
                }
            }
        }
	}
}
