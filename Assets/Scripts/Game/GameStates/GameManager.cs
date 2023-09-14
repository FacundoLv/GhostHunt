using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO: change this
    public GameStates State { set => _fsm.Transition(value); }

    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<GameManager>();

            if (_instance == null)
                _instance = new GameObject("GameManager").AddComponent<GameManager>();

            return _instance;
        }
    }

    private static GameManager _instance;
    private FSM<GameStates> _fsm;

    private void Awake()
    {
        if (_instance != null) Destroy(gameObject);
        _instance = this;

        DontDestroyOnLoad(gameObject);

        //Open - Closed principle violation
        /*
         * Se puede:
         * 1 - Reemplazar los llamados a GameManager.Instance.State por eventos y subscribir a esos eventos desde los estados para hacer transiciones.
         * Para esto deberia pasar una referencia de la fsm a cada estado. (No me gusta)
         * 2 - Agregar una clase EventManager (una clase estatica) para que contenga eventos que se puedan crear desde las distintas clases y a las que uno pueda subscribirse.
         * (Creo que da bastante lugar a error humano)
         */
        var menu = new MenuState();
        var victory = new VictoryState();
        var loose = new LooseState();
        var play = new PlayState();

        menu.AddTransition(GameStates.Play, play);

        play.AddTransition(GameStates.Victory, victory);
        play.AddTransition(GameStates.Loose, loose);

        victory.AddTransition(GameStates.MainMenu, menu);

        loose.AddTransition(GameStates.MainMenu, menu);

        _fsm = new FSM<GameStates>(menu);
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }
}

public enum GameStates
{
    MainMenu,
    Victory,
    Loose,
    Play,
}