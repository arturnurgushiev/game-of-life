using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int gridWidth = 30;
    public int gridHeight = 20;
    public float cellSize = 1f;

    private Cell[,] grid;
    private bool isSimulating = false;
    private float simulationSpeed = 0.5f;

    public System.Action<bool> OnSimulationStateChanged;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Cell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(
                    (x - gridWidth/2) * cellSize,
                    (y - gridHeight/2) * cellSize,
                    0
                );

                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cellObj.transform.localScale = Vector3.one * cellSize;

                Cell cell = cellObj.GetComponent<Cell>();
                if (cell != null)
                {
                    cell.X = x;
                    cell.Y = y;
                    cell.SetAlive(false);
                    grid[x, y] = cell;
                }
            }
        }
    }

    public void StartSimulation()
    {
        if (!isSimulating)
        {
            isSimulating = true;
            Cell.SimulationRunning = true;
            OnSimulationStateChanged?.Invoke(true);
            StartCoroutine(Simulate());
        }
    }

    public void StopSimulation()
    {
        isSimulating = false;
        Cell.SimulationRunning = false;
        OnSimulationStateChanged?.Invoke(false);
        StopAllCoroutines();
    }

    IEnumerator Simulate()
    {
        while (isSimulating)
        {
            CalculateNextGeneration();
            yield return new WaitForSeconds(simulationSpeed);
        }
    }

    void CalculateNextGeneration()
    {
        bool[,] nextState = new bool[gridWidth, gridHeight];
        bool hasChanges = false;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int aliveNeighbors = CountAliveNeighbors(x, y);
                bool currentlyAlive = grid[x, y].IsAlive;
                bool nextAlive = currentlyAlive;

                if (currentlyAlive && (aliveNeighbors == 2 || aliveNeighbors == 3))
                    nextAlive = true;
                else if (!currentlyAlive && aliveNeighbors == 3)
                    nextAlive = true;
                else
                    nextAlive = false;

                nextState[x, y] = nextAlive;
                if (currentlyAlive != nextAlive) hasChanges = true;
            }
        }

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                grid[x, y].SetAlive(nextState[x, y]);

        int aliveCells = CountAliveCells();
        if (aliveCells == 0 || !hasChanges)
        {
            StopSimulation();
        }
    }

    int CountAliveNeighbors(int x, int y)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int neighborX = x + i;
                int neighborY = y + j;

                if (neighborX >= 0 && neighborX < gridWidth &&
                    neighborY >= 0 && neighborY < gridHeight)
                {
                    if (grid[neighborX, neighborY].IsAlive)
                        count++;
                }
            }
        }

        return count;
    }

    int CountAliveCells()
    {
        int aliveCount = 0;
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                if (grid[x, y].IsAlive)
                    aliveCount++;
        return aliveCount;
    }

    public void RandomizeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                grid[x, y].SetAlive(Random.value > 0.7f);
    }

    public void SetSimulationSpeed(float speed)
    {
        simulationSpeed = 1f - speed;
    }

    public void ClearGrid()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                grid[x, y].SetAlive(false);
    }
}
