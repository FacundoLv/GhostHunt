using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Canvas _canvas = null;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 pos);
        transform.position = _canvas.transform.TransformPoint(pos);
    }
}
