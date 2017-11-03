using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private static MapCreator m_Instance;

    //ELENA IS THE BEST



    // I LOVE HER SO MUCH 





    //editable variables from editor
    public int m_NumberOfLines;
    public int m_NumberOfColumns;

    public int m_NumberOfColectibles;

    public GameObject m_MapContainer;
    public GameObject m_ColectiblesContainer;

    //prefabs
    public GameObject m_MapTile;
    public GameObject m_Colectible;

    private int[,] m_Map;

    void Awake()
    {
        m_Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        m_Map = new int[m_NumberOfLines, m_NumberOfColumns];
        GenerateTiles();
        GenerateColectibles();
    }

    public MapCreator GetInstance()
    {
        return m_Instance;
    }

    private void GenerateTiles()
    {
        float fStartingLinePosition = m_NumberOfLines % 2 == 0 ? m_NumberOfLines / 2 + 0.5f : m_NumberOfLines / 2;
        float fStartingColumnPosition = m_NumberOfColumns % 2 == 0 ? -m_NumberOfColumns / 2 + 0.5f : -m_NumberOfColumns / 2;

        for (int i = 0; i < m_NumberOfLines; ++i)
        {
            for (int j = 0; j < m_NumberOfColumns; ++j)
            {
                GameObject go = Instantiate(m_MapTile, new Vector3(fStartingColumnPosition + j, 0, fStartingLinePosition - i), Quaternion.identity) as GameObject;
                go.name = i + ":" + j;

                go.transform.parent = m_MapContainer.transform;
            }
        }
    }

    private void GenerateColectibles()
    {
        int generatedColectibles = 0;
        while (generatedColectibles < m_NumberOfColectibles)
        {
            int randomLineIndex = Random.Range(1, m_NumberOfLines);
            int randomColumnIndex = Random.Range(1, m_NumberOfColumns);
            if (m_Map[randomLineIndex, randomColumnIndex] == 0)
            {
                GameObject go = Instantiate(m_Colectible, GetPositionAtIndexes(randomLineIndex, randomColumnIndex), Quaternion.identity) as GameObject;
                go.name = "Colectible:[" + randomLineIndex + ":" + randomColumnIndex + "]";

                go.transform.parent = m_ColectiblesContainer.transform;

                m_Map[randomLineIndex, randomColumnIndex] = 1;
                ++generatedColectibles;
            }
        }
    }

    public Vector3 GetPositionAtIndexes(int _lineIndex, int _columnIndex)
    {
        if (_lineIndex > m_NumberOfLines || _columnIndex > m_NumberOfColumns)
        {
            return Vector3.negativeInfinity;
        }

        float fStartingLinePosition = m_NumberOfLines % 2 == 0 ? m_NumberOfLines / 2 + 0.5f : m_NumberOfLines / 2;
        float fStartingColumnPosition = m_NumberOfColumns % 2 == 0 ? -m_NumberOfColumns / 2 + 0.5f : -m_NumberOfColumns / 2;

        return new Vector3(fStartingColumnPosition + _columnIndex, 0, fStartingLinePosition - _lineIndex);
    }
}
