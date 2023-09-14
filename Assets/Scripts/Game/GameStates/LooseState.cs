using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LooseState : States<GameStates>
{
    public override void Awake()
    {
        //Show Loose screen
        PhotonNetwork.AutomaticallySyncScene = false;
        var loose = Resources.Load(Path.Combine("GhostHunt", "UI", "LooseScreen")) as GameObject;
        GameObject.Instantiate(loose, Vector3.zero, Quaternion.identity);
    }

    public override void Execute()
    {
        // Transition to MainMenu
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance.State = GameStates.MainMenu;
    }

    public override void Sleep()
    {
        //Hide Loose screen
        //Show load screen. Wait until next scene is loaded. Hide loadscreen when load is Done
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync(0);
    }
}
