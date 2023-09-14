using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    private string _createdRoomName;
    private string _joinedRoomName;

    [SerializeField] private PanelGroup _displayPanel = null;
    [SerializeField] private int playWindowIndex = 0;
    [SerializeField] private int waitingWindowIndex = 0;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            ConnectToServer();
    }

    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void SetNewRoomName(string name)
    {
        _createdRoomName = name;
    }

    public void SetJoinedRoomName(string name)
    {
        _joinedRoomName = name;
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_createdRoomName)) return;

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2,
            IsOpen = true,
            IsVisible = true
        };

        PhotonNetwork.JoinOrCreateRoom(_createdRoomName, options, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_joinedRoomName)) return;

        PhotonNetwork.JoinRoom(_joinedRoomName);
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        _displayPanel.SetPageIndex(waitingWindowIndex);
    }

    public override void OnLeftRoom()
    {
        _displayPanel.SetPageIndex(playWindowIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
