using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GridScriptableDatas gridScriptableDatas;

    private Dictionary<Coordinates, GameObject> allGrids = new Dictionary<Coordinates, GameObject>();
    private Dictionary<Coordinates, GameObject> grassGrids = new Dictionary<Coordinates, GameObject>();
    private Dictionary<Coordinates, GameObject> pipeGrids = new Dictionary<Coordinates, GameObject>();
    private Stack<Coordinates> prevCoordinates = new Stack<Coordinates>();
    private GameObject GridParent;
    private Coordinates currentCoordinates;
    private int YMove;

    private const int maxBgSize = 3;
    private const int gridMinSize = 3;
    private const int gridMaxSize = 9;

    public static int gridSize;

    private void Awake()
    {
        gridSize = gridScriptableDatas.gridSize;
        GridParent = Instantiate(gridScriptableDatas.gridParent);
    }
    private void Start()
    {
        SetGridSize();
        GenerateGrid();
        GeneratePipes();
    }

    #region GENERATE GRIDS
    private void GenerateGrid()
    {
        Vector3 startPos = Vector3.left * (float)gridSize / 2 + Vector3.down * (float)gridSize / 2;
        startPos.x += .5f;
        startPos.y += .5f;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 spawnPosition = startPos + Vector3.right * i + Vector3.up * j;
                GameObject _gridCell = Instantiate(gridScriptableDatas.gridCell, spawnPosition, Quaternion.identity, GridParent.transform);
                GameObject _grid = Instantiate(gridScriptableDatas.grid, _gridCell.transform);
                Coordinates _coordinates = new Coordinates(i, j);
                allGrids[_coordinates] = _gridCell;
                grassGrids[_coordinates] = _gridCell;
            }
        }
        GridParent.transform.SetParent(transform);
        float size = 1 / ((float)gridSize / (float)maxBgSize);
        GridParent.transform.localScale = GridParent.transform.localScale * size;
    }
    #endregion

    #region SET GRID SIZE
    private void SetGridSize()
    {
        transform.localScale = transform.localScale * maxBgSize;

        if (gridSize < 5)
        {
            Debug.LogWarning("Min grid size is 5");
            gridSize = 5;
        }
        else if (gridSize > 9)
        {
            Debug.LogWarning("Max grid size is 9");
            gridSize = 9;
        }
    }
    #endregion

    #region GENERATE PIPES
    private void GeneratePipes()
    {
        StartPipe();
        End();
        OtherPipes();
        RandomizePipes();
    }

    #region START PIPE
    private void StartPipe()
    {
        // To make the game simple i instantiate the start pipe only in x coordinates 0)

        int randomY = Random.Range(0, gridSize);
        Coordinates _coordinates = new Coordinates(0, randomY);
        GameManager.startPipeCordinates = _coordinates;
        GameObject child = allGrids[_coordinates].transform.GetChild(0).gameObject;
        Destroy(child);
        Instantiate(gridScriptableDatas.startPipe, allGrids[_coordinates].transform);
        grassGrids.Remove(_coordinates);
    }
    #endregion

    #region END PIPE
    private void End()
    {
        // To make the game simple i instantiate the end item(Bomb) only in x coordinates of the last coloumns)

        int randomY = Random.Range(0, gridSize);
        Coordinates _coordinates = new Coordinates(gridSize - 1, randomY);
        GameManager.endCordinates = _coordinates;
        GameObject child = allGrids[_coordinates].transform.GetChild(0).gameObject;
        Destroy(child);
        Instantiate(gridScriptableDatas.endItem, allGrids[_coordinates].transform);
        grassGrids.Remove(_coordinates);
    }
    #endregion

    #region OTHER PIPES
    private void OtherPipes()
    {
        currentCoordinates = GameManager.startPipeCordinates;
        bool MoveToY = false;
        YMove = 0;
        int i = 0; // To avoid bug, infinite loop

        while (i != 15)
        {
            i++;         

            if (MoveToY)
            {
                if (currentCoordinates.Y != GameManager.endCordinates.Y || YMove < 1) MoveYCoordinates();
                else if (currentCoordinates.X != GameManager.endCordinates.X) MoveXCoordinates();
                else i = 15;
            }
            else
            {
                if (currentCoordinates.X != GameManager.endCordinates.X) MoveXCoordinates();
                else if (currentCoordinates.Y != GameManager.endCordinates.Y) MoveYCoordinates();
                else i = 15;
            }

            if (i == 15) break;

            CreatePipe(false);

            MoveToY = MoveToY == true ? false : true;
        }
        prevCoordinates.Push(GameManager.endCordinates);
        CreatePipe(true);
    }

    /// <summary>
    /// Move to Y Coordinates
    /// </summary>
    private void MoveYCoordinates()
    {
        WhereIsYTarget whereIsYTarget;     
        
        if (YMove < 1)
        {
            whereIsYTarget = currentCoordinates.Y == gridSize - 1 ? WhereIsYTarget.Down :
                currentCoordinates.Y == 0 ? WhereIsYTarget.Up : Random.Range(0, 2) == 0 ? WhereIsYTarget.Down : WhereIsYTarget.Up;
        }
        else
        {
            whereIsYTarget = GameManager.endCordinates.Y > currentCoordinates.Y ? WhereIsYTarget.Up : WhereIsYTarget.Down;
        }

        if (prevCoordinates.Peek().X == currentCoordinates.X && prevCoordinates.Peek().Y == currentCoordinates.Y) Debug.Log("Same Value");
        else AddToPreviousCoordinates();

        if (whereIsYTarget == WhereIsYTarget.Up) currentCoordinates.Y += 1;
        else if (whereIsYTarget == WhereIsYTarget.Down) currentCoordinates.Y -= 1;

        YMove++;
    }

    /// <summary>
    /// Move to X Coordinates
    /// </summary>
    private void MoveXCoordinates()
    {
        if (prevCoordinates.Count < 1) AddToPreviousCoordinates(); // first prevCoordinates 
        else if (prevCoordinates.Peek().X == currentCoordinates.X && prevCoordinates.Peek().Y == currentCoordinates.Y) Debug.Log("Same Value");
        else AddToPreviousCoordinates();
        currentCoordinates.X++;
    }

    /// <summary>
    /// Saves Moved Coordinates
    /// </summary>
    private void AddToPreviousCoordinates()
    {
        Coordinates _coordinates = new Coordinates(currentCoordinates.X, currentCoordinates.Y);
        prevCoordinates.Push(_coordinates);
    }

    /// <summary>
    /// Create Pipes
    /// </summary>
    private void CreatePipe(bool lastPipe)
    {
        //Debug.Log(prevCoordinates.Count);

        if (prevCoordinates.Count > 2)
        {
            int i = 0;
            Coordinates[] last3Coordinates = new Coordinates[3];
            foreach (Coordinates _coordinates in prevCoordinates)
            {
                if (i < 3)
                {
                    last3Coordinates[i] = _coordinates;
                }
                i++;
            }
            GameObject pipe = GetPipe(last3Coordinates);

            if(pipe != null)
            {
                Coordinates coordinates = new Coordinates(last3Coordinates[1].X, last3Coordinates[1].Y);
                GameObject child = allGrids[coordinates].transform.GetChild(0).gameObject;
                Destroy(child);
                GameObject _pipe = Instantiate(pipe, allGrids[coordinates].transform);
                if (lastPipe)
                {
                    GameManager.endPipeCordinates = coordinates;
                    _pipe.GetComponent<Collider2D>().enabled = false;
                }
                else pipeGrids[coordinates] = allGrids[coordinates];
                grassGrids.Remove(coordinates);
            }

        }
    }

    /// <summary>
    /// Creating appropriate pipes
    /// </summary>
    /// <param name="last3Coordinates"></param>
    /// <returns></returns>
    private GameObject GetPipe(Coordinates[] last3Coordinates)
    {
        GameObject pipe;

        if (last3Coordinates[0].X == last3Coordinates[2].X) pipe = gridScriptableDatas.vertical;
        else if (last3Coordinates[0].Y == last3Coordinates[2].Y) pipe = gridScriptableDatas.horizontal;
        else if (XIncreased(last3Coordinates[0].X, last3Coordinates[2].X) && YIncreased(last3Coordinates[0].Y, last3Coordinates[2].Y) && XIncreased(last3Coordinates[1].X, last3Coordinates[2].X)) pipe = gridScriptableDatas.leftUp;
        else if (XIncreased(last3Coordinates[0].X, last3Coordinates[2].X) && !YIncreased(last3Coordinates[0].Y, last3Coordinates[2].Y) && XIncreased(last3Coordinates[1].X, last3Coordinates[2].X)) pipe = gridScriptableDatas.leftDown;
        else if (XIncreased(last3Coordinates[0].X, last3Coordinates[2].X) && YIncreased(last3Coordinates[0].Y, last3Coordinates[2].Y) && !XIncreased(last3Coordinates[1].X, last3Coordinates[2].X)) pipe = gridScriptableDatas.rightDown;
        else if (XIncreased(last3Coordinates[0].X, last3Coordinates[2].X) && !YIncreased(last3Coordinates[0].Y, last3Coordinates[2].Y) && !XIncreased(last3Coordinates[1].X, last3Coordinates[2].X)) pipe = gridScriptableDatas.rightUp;
        else pipe = null;

        return pipe;
    }


    private bool XIncreased(int first, int second) => first > second;
    private bool YIncreased(int first, int second) => first > second;

    #endregion

    #region RANDOMIZE PIPES
    private void RandomizePipes()
    {
        for (int i = 0; i < 25; i++)
        {
            Coordinates grassCoordinates = new Coordinates(GetRandomCoordinate(), GetRandomCoordinate());
            Coordinates pipeCoordinates = new Coordinates(20,20); 
            if (grassGrids.ContainsKey(grassCoordinates) && pipeGrids.Count > 0)
            {
                int n = 0;
                foreach (KeyValuePair<Coordinates,GameObject> _coordinates in pipeGrids)
                {
                    if (n == 0)
                    {
                        pipeCoordinates = _coordinates.Key;

                        Vector3 temp = allGrids[pipeCoordinates].transform.position;
                        allGrids[pipeCoordinates].transform.position = allGrids[grassCoordinates].transform.position;
                        allGrids[grassCoordinates].transform.position = temp;
                    }
                }
            }

            if(pipeGrids.ContainsKey(pipeCoordinates)) pipeGrids.Remove(pipeCoordinates);
        }
    }

    private int GetRandomCoordinate() => Random.Range(0, gridSize);
    #endregion

    #endregion
}
