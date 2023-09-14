using Photon.Pun;

public class MenuState : States<GameStates>
{
    public override void Awake()
    {
        //Show splash screen
    }

    public override void Execute()
    {
        // Create Room
        // Search Rooms
        // Check for players
        // Transition to playmode when ready *Maybe not responsible for this*
    }

    public override void Sleep()
    {
        // Show load screen. Wait until next scene is loaded. Hide loadscreen when load is Done

        // El await deberia estar en Transition
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }
}
