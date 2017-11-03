using UnityEngine;
using System.Collections;

public class ObstacleRegistration : MonoBehaviour {

	void Awake()
	{
		AlignObject ();
	}

	void AlignObject()
	{
		Vector3 newPos = transform.position;

		newPos.x = Mathf.RoundToInt (newPos.x);
		newPos.y = Mathf.RoundToInt (newPos.y);

		transform.position = newPos;
	}

	void Start()
	{
		Astar.instance.RegisterObstacle ((int)transform.position.x, (int)transform.position.y);
	}
}
