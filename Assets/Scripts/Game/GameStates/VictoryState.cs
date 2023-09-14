using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryState : States<GameStates>
{
    public override void Awake()
    {
        //Show Win screen
        PhotonNetwork.AutomaticallySyncScene = false;
        var win = Resources.Load(Path.Combine("GhostHunt", "UI", "WinScreen")) as GameObject;
        GameObject.Instantiate(win, Vector3.zero, Quaternion.identity);
    }

    public override void Execute()
    {
        // Transition to MainMenu
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance.State = GameStates.MainMenu;
    }

    public override void Sleep()
    {
        //Hide WinScreen
        //Show load screen. Wait until next scene is loaded. Hide loadscreen when load is Done
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync(0);
    }
}
