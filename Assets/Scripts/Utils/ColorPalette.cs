using UnityEngine;

public class ColorPalette : ScriptableObject
{
    [SerializeField] private Color[] colors;

    public int GetColorCount()
    {
        return colors.Length;
    }

    public Color GetColor(int index)
    {
        return colors[index % colors.Length];
    }
}
