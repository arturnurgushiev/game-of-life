using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button pauseButton;
    public Button randomButton;
    public Button clearButton;
    public Slider speedSlider;

    void Start()
    {
        startButton.onClick.AddListener(StartSimulation);
        pauseButton.onClick.AddListener(StopSimulation);
        randomButton.onClick.AddListener(RandomizeGrid);
        clearButton.onClick.AddListener(ClearGrid);
        speedSlider.onValueChanged.AddListener(SetSpeed);
        FindAnyObjectByType<GridManager>().OnSimulationStateChanged += OnSimulationStateChanged;
        UpdateButtons(false);
    }

    void OnSimulationStateChanged(bool isSimulating)
    {
        UpdateButtons(isSimulating);
    }

    void UpdateButtons(bool isSimulating)
    {
        startButton.interactable = !isSimulating;
        pauseButton.interactable = isSimulating;
    }

    void StartSimulation()
    {
        FindAnyObjectByType<GridManager>().StartSimulation();
    }

    void StopSimulation()
    {
        FindAnyObjectByType<GridManager>().StopSimulation();
    }

    void RandomizeGrid()
    {
        FindAnyObjectByType<GridManager>().RandomizeGrid();
    }

    void ClearGrid()
    {
        FindAnyObjectByType<GridManager>().ClearGrid();
    }

    void SetSpeed(float speed)
    {
        FindAnyObjectByType<GridManager>().SetSimulationSpeed(speed);
    }
}
