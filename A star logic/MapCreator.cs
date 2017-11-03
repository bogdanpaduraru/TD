using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {

    public static MapCreator instance;

	public enum MapSize
	{
		//1x1
		verBig,
		//2x2
		big,
		//3x3
		medium,
		//4x4
		small,
		//5x5
		verySmall
	}

	public MapSize mapSize;

	public GameObject square;

    public GameObject obstacle;

	public GameObject gridLayoutContainer;
    public GameObject obstaclesContainer;

	public GameObject start;
	public GameObject finish;

    public ImageTargetBehaviour planeTarget;
    private List<Animator> availablePaths;

    private float increaseRatio; 

    public float startingPointX; 
    public float finalPointX; 

    public float startingPointY;
    public float finalPointY;

    private bool canPlace;

    private bool mapFound;

    public GameObject selectedSpotForTower;

    public GameObject towerObject;

	void Awake()
	{
        GetCoordinatesFromImage();
	}

    void Start()
    {
        Astar.instance.CreateMatrix(finalPointX, startingPointX, startingPointY, finalPointY, (int)increaseRatio);    
    }

    private void GetCoordinatesFromImage()
    {
        instance = this;

        availablePaths = new List<Animator>();

        increaseRatio = (int)(mapSize + 1) * 3;

        //startingPointX = (planeTarget[1].gameObject.transform.position.x - planeTarget[1].GetSize().x / 2) + increaseRatio / 2;
        //finalPointX = (planeTarget[1].gameObject.transform.position.x + planeTarget[1].GetSize().x / 2) - increaseRatio / 2;

        //startingPointY = (planeTarget[1].gameObject.transform.position.y + planeTarget[1].GetSize().y / 2) - increaseRatio / 2;
        //finalPointY = (planeTarget[1].gameObject.transform.position.y - planeTarget[1].GetSize().y / 2) + increaseRatio / 2;

        startingPointX = (planeTarget.gameObject.transform.position.x - planeTarget.GetSize().x / 2) + increaseRatio / 2;
        finalPointX = (planeTarget.gameObject.transform.position.x + planeTarget.GetSize().x / 2) - increaseRatio / 2;

        startingPointY = (planeTarget.gameObject.transform.position.y + planeTarget.GetSize().y / 2) - increaseRatio / 2;
        finalPointY = (planeTarget.gameObject.transform.position.y - planeTarget.GetSize().y / 2) + increaseRatio / 2;

        square.transform.localScale = new Vector3(increaseRatio, increaseRatio, increaseRatio);
        start.transform.localScale = finish.transform.localScale = square.transform.localScale;

        Debug.Log("increaseRatio = " + increaseRatio);

        Debug.Log("startingPointX = " + startingPointX);
        Debug.Log("finalPointX = " + finalPointX);

        Debug.Log("startingPointY = " + startingPointY);
        Debug.Log("finalPointY = " + finalPointY);

        int numberOfColumns = (int)(Mathf.Abs(startingPointX) + Mathf.Abs(finalPointX)) / 2;
        int numberOfLines = (int)(Mathf.Abs(startingPointY) + Mathf.Abs(finalPointY)) / 2;

        int index = 0;

        for (float i = startingPointX; i <= finalPointX; i += increaseRatio)
        {
            for (float j = startingPointY; j >= finalPointY; j -= increaseRatio)
            {
                GameObject c = Instantiate(square, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                c.name = index.ToString();
                c.transform.parent = gridLayoutContainer.transform;
                availablePaths.Add(c.GetComponent<Animator>());
                index++;
            }
        }
        start.transform.localScale = finish.transform.localScale = square.transform.localScale;

        start.transform.localPosition = new Vector3(startingPointX, startingPointY, 0);
        finish.transform.localPosition = new Vector3(finalPointX, finalPointY, 0);

        availablePaths.RemoveAt(0);
        availablePaths.RemoveAt(availablePaths.Count - 1);

        FixObjectPosition(start);
        FixObjectPosition(finish);
    }

    private void FixObjectPosition(GameObject objectToBeFixed)
    {
        float distance = Mathf.Abs(objectToBeFixed.transform.localPosition.x - startingPointX);
        float multiplier = distance / increaseRatio;
        int multipliedFloored = Mathf.FloorToInt(multiplier);
        float newXPosition = startingPointX + multipliedFloored * increaseRatio;

        float distanceY = Mathf.Abs(objectToBeFixed.transform.localPosition.y - startingPointY);
        float multiplierY = distanceY / increaseRatio;
        int multipliedFlooredY = Mathf.FloorToInt(multiplierY);
        float newXPositionY = startingPointY - multipliedFlooredY * increaseRatio;

        objectToBeFixed.transform.localPosition = new Vector3(newXPosition, newXPositionY, 0);
    }

    public float GetEnemySize()
    {
        return increaseRatio;
    }

    public List<Vector2> GetAvailablePaths()
    {
        List<Vector2> result = new List<Vector2>();
        availablePaths.ForEach((Animator a) => result.Add(a.transform.position));
        return result;
    }

    public Vector2 GetMatrixCoordFromWorldCoords(float xCoord, float yCoord)
    {
        Vector2 result = new Vector2();

        float distance = Mathf.Abs(xCoord - startingPointX);
        float multiplier = distance / increaseRatio;
        int multipliedFloored = Mathf.FloorToInt(multiplier);

        float distanceY = Mathf.Abs(yCoord - startingPointY);
        float multiplierY = distanceY / increaseRatio;
        int multipliedFlooredY = Mathf.FloorToInt(multiplierY);

        result.y = multipliedFloored;
        result.x = multipliedFlooredY;

        return result;
    }

    public Vector2 GetWoorldCoordsFromMatrixCoords(Vector2 matrixCoords)
    {
        return new Vector2(startingPointX + increaseRatio * matrixCoords.y, startingPointY - increaseRatio * matrixCoords.x);
    }

    public List<Vector2> GetWoorldCoordsFromMatrixCoords(List<AstarNode> nodes)
    {
        List<Vector2> result = new List<Vector2>();

        for (int i = 0; i < nodes.Count;i++)
        {
            result.Add(new Vector2(startingPointX + increaseRatio * nodes[i].y, startingPointY - increaseRatio * nodes[i].x));
        }

        return result;
    }

    public void MapFound(string mapName)
    {
        if (!mapFound)
        {
            mapFound = true;

            switch (mapName)
            {
                case "BigMap":
                    AddObstaclesForGivenLevel(0);
                    break;
                case "Patine":
                    AddObstaclesForGivenLevel(1);
                    break;
                case "hobbit":
                    AddObstaclesForGivenLevel(2);
                    break;
                case "book":
                    AddObstaclesForGivenLevel(3);
                    break;
            }

            Vector2 start = MapCreator.instance.GetMatrixCoordFromWorldCoords(startingPointX, startingPointY);
            Vector2 end = MapCreator.instance.GetMatrixCoordFromWorldCoords(finalPointX, finalPointY);

            Astar.instance.CreatePath((int)start.x, (int)start.y, (int)end.x, (int)end.y);
        }
    }

    public void RemoveRoadSpots(List<Vector2> path)
    {
        List<Vector2> placesToBeRemoved = new List<Vector2>();

        for(int i=0;i<path.Count;i++)
        {
            if(IsNodeRemovable(path[i]))
            {
                placesToBeRemoved.Add(path[i]);
            }
            //int index = availablePaths.FindIndex(x => Mathf.Approximately(x.transform.position.x, path[i].x) && Mathf.Approximately(x.transform.position.y, path[i].y));

            //if(index != -1)
            //{
            //    availablePaths.RemoveAt(index);
            //}
        }
        for (int i = 0; i < placesToBeRemoved.Count; i++)
        {
            int index = availablePaths.FindIndex(x => Mathf.Approximately(x.transform.position.x, placesToBeRemoved[i].x) && Mathf.Approximately(x.transform.position.y, placesToBeRemoved[i].y));

            if(index != -1)
            {
                availablePaths.RemoveAt(index);
            }
        }
    }

    private void AddObstaclesForGivenLevel(int givenLevel)
    {
        //Debug.Log("numberOfLines=" + Astar.instance.numberOfLines);
        //Debug.Log("numberOfColumns=" + Astar.instance.numberOfColumns);
        Debug.Log("givenLevel=" + givenLevel);

        for(int i=0;i<Astar.instance.numberOfLines;i++)
        {
            for(int j=0;j<Astar.instance.numberOfColumns;j++)
            {
                if(Levels.levels[givenLevel][i,j] == 1)
                {
                    Astar.instance.RegisterObstacle(i, j);

                    Vector2 pos = GetWoorldCoordsFromMatrixCoords(new Vector2(i,j));
                    GameObject c = Instantiate(obstacle, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;
                    c.transform.localScale = new Vector3(increaseRatio, increaseRatio, increaseRatio);
                    c.name = "Obstacle " + i + "," + j;
                    c.transform.parent = obstaclesContainer.transform;

                    availablePaths.RemoveAt(availablePaths.FindIndex(x=> Mathf.Approximately(x.transform.position.x, pos.x) && Mathf.Approximately(x.transform.position.y, pos.y)));                 
                }
            }
        }
    }

    private bool IsNodeRemovable(Vector2 pathNode)
    {
        int numberOfNeighbours = 0;

        //right neighbour check
        if(Astar.instance.path.Contains(new Vector2(pathNode.x + increaseRatio, pathNode.y)))
        {
            numberOfNeighbours++;
        }

        //left neighbour check
        if (Astar.instance.path.Contains(new Vector2(pathNode.x - increaseRatio, pathNode.y)))
        {
            numberOfNeighbours++;
        }

        //upper neighbour check
        if (Astar.instance.path.Contains(new Vector2(pathNode.x, pathNode.y + increaseRatio)))
        {
            numberOfNeighbours++;
        }

        //bottom neighbour check
        if (Astar.instance.path.Contains(new Vector2(pathNode.x, pathNode.y - increaseRatio)))
        {
            numberOfNeighbours++;
        }

        if (numberOfNeighbours > 1)
        {
            return true;
        }
        return false;
    }

    public void TowerImageDetected()
    {
        Debug.Log("tower image detected");
        selectedSpotForTower = towerObject;
    }

    public void TowerImageLost()
    {
        Debug.Log("tower image lost");
        selectedSpotForTower = null;
    }

    void OnGUI()
    {
        if (!canPlace)
        {
            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 100), "Pause & Place a tower"))
            {
                availablePaths.ForEach((Animator anim) => { anim.Play("available"); anim.collider2D.enabled = true; });
                canPlace = !canPlace;

                TimeManager.gameIsPaused = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 100), "Resume game"))
            {
                availablePaths.ForEach((Animator anim) => { anim.Play("idle"); anim.collider2D.enabled = false; });
                canPlace = !canPlace;

                TimeManager.gameIsPaused = false;
            }
        }

        if(canPlace && selectedSpotForTower != null)
        {
            if (GUI.Button(new Rect(Screen.width - 200, 120, 200, 100), "Place on selected spot"))
            {
                GameObject g = (GameObject)Instantiate(towerObject, selectedSpotForTower.transform.position, Quaternion.identity);
                g.GetComponent<TowerCreator>().CreateTower();

                GameObject pathSpot = availablePaths[availablePaths.FindIndex(x => x.transform.position == selectedSpotForTower.transform.position)].gameObject;

                availablePaths.RemoveAt(availablePaths.FindIndex(x => x.transform.position == selectedSpotForTower.transform.position));

                Destroy(pathSpot);
                selectedSpotForTower = null;
            }
        }
    }
}
