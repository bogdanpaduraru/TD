using UnityEngine;
using System.Collections;

public class AVLTest : MonoBehaviour {

	public int valueToInsert;

	[SerializeField]
	public AVLNode rootNode;

	void Awake()
	{
		rootNode = new AVLNode ();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.S)) 
		{
			rootNode.PrintTree(rootNode);
		}

		if (Input.GetKeyDown (KeyCode.D)) 
		{
			Debug.Log("root = " + rootNode.key);
			rootNode.SRD(rootNode);
		}

		if (Input.GetKeyDown (KeyCode.I)) 
		{
			rootNode = rootNode.Insert(rootNode, valueToInsert, null);
		}
	}
}
