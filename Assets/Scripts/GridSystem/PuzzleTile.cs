using UnityEngine;
using UnityEngine.UI;

public class PuzzleTile : MonoBehaviour
{
    public Image image;
    public int x, y; // grid position
    public int colorIndex;

    private PuzzleManager manager;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClicked);
    }

    public void Init(PuzzleManager m, int x, int y, Color color, int index)
    {
        manager = m;
        this.x = x;
        this.y = y;
        this.colorIndex = index;
        image.color = color;
    }

    void OnTileClicked()
    {
        manager.OnTileClicked(this);
    }

    public void Highlight(bool active)
    {
        image.color = active ? Color.white : manager.GetColor(colorIndex);
    }

    public void Clear()
    {
        image.color = Color.clear;
        colorIndex = -1;
    }
}

