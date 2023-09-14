using Photon.Pun;
using UnityEngine;

public class PunCrosshair : MonoBehaviourPun
{
    [SerializeField] private Canvas _canvas = null;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 pos);
        photonView.RPC(nameof(PointTo), RpcTarget.All, new object[] { photonView.ViewID, pos });
    }

    [PunRPC]
    private void PointTo(int id, Vector2 pos)
    {
        if (id != photonView.ViewID) return;
        transform.position = _canvas.transform.TransformPoint(pos);
    }
}
