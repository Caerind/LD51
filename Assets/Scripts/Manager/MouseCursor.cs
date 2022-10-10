using UnityEngine;

public class MouseCursor : Singleton<MouseCursor>
{
    [SerializeField] private Texture2D mouseCursorTexture;

    private bool showCursor = false; // Start as false because there is a check

    private void Start()
    {
        ShowCursor(true);
    }

    private void OnMouseEnter()
    {
        ShowCursor(true);
    }

    private void OnMouseExit()
    {
        ShowCursor(false);
    }

    public void ShowCursor(bool visible)
    {
        if (showCursor != visible)
        {
            if (visible)
            {
                Vector2 hotspot = new Vector2(64 * 0.5f, 64 * 0.5f); // TODO : Unhardcode the hotspot
                Cursor.SetCursor(mouseCursorTexture, hotspot, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            showCursor = visible;
        }
    }
}
