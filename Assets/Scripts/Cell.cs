using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsAlive = false;
    public int X, Y;

    private SpriteRenderer spriteRenderer;
    public static bool SimulationRunning = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (GetComponent<Collider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();

        UpdateVisual();
    }

    public void SetAlive(bool alive)
    {
        IsAlive = alive;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = IsAlive ? Color.white : Color.black;
    }

    void OnMouseDown()
    {
        if (!SimulationRunning)
            SetAlive(!IsAlive);
    }
}
