using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;


public class GameManager : MonoBehaviour
{
    public bool Next = false;
    public bool NextPlayer = false;

    public GameObject MapScreen;
    public GameObject PlayerProfileScreen;
    public GameObject NarrationScreen;
    public GameObject FightScreen;

    public GameObject TurnDisplay;
    public GameObject PlayersSpriteDisplay;
    public GameObject ThreatSpawnDisplay;
    public GameObject PlayerProfileDisplay;
    public GameObject ActionPrompterDisplay;

    public GameObject DigUpResult;
    public GameObject ObjectiveDisplayManager;

    public MoveButtonHandler mvt_btn;

    IntPtr physicaloid;

    public EventManager EM;
    public TurnManager TM;
    public DisplayManager DM;

    public Board GameBoard;

    public void DoNext()
    {
        JzzSoundManager sm = JzzSoundManager.instance;
        if (sm != null)
        {
            sm.StopChannel(JzzSoundManager.bgChannel);
            sm.StopChannel(JzzSoundManager.narrationChannel);
            sm.StopChannel(JzzSoundManager.screenSoundChannel);
            sm.StopChannel(JzzSoundManager.fxChannel);
        }

        Next = true;
    }

    //private SerialPort SP;
    //private string s="";

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        DM = new DisplayManager(this);
        EM = new EventManager(GameScenario.Init(DM));
        TM = new TurnManager(this);

        //Debug.Log ("NB tours: " + EM.Scenario.Acts [0].Scenes.Count);


        //AndroidJNI.NewByteArray (6);


        TM.Start();
        //gameBoard.registerEventHandlers (TurnManager);

        //Text t = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<Text>();

        /*Debug.Log ("Ports:");
        string[] ports = SerialPort.GetPortNames ();
        string s = "";
        for (int i = ports.Length - 1; i > -1; --i) {
            s += (i+1).ToString() + ports[i].ToString() + "\n";
        }
        if (ports.Length > 0) {
            t.text = s;

            SP = new SerialPort (ports [0].ToString ());
            SP.BaudRate = 9600;

            SP.ReadTimeout = 100;
            SP.Open ();
        } else {
            t.text = "NO COM PORT DETECTED...";
        }*/


    }

    void Update()
    {
        TM.update();

        if (Next)
        {
            Next = false;
        }

        //displayManager.update();
        /*if (SP != null) {
            try 
            { 
                if (SP.IsOpen == false) 
                { 
                    SP.Open(); 
                } 
            }
            catch 
            { 
                Debug.Log ("Serial Error: Is your serial plugged in?"); 
            } 

            //SP.WriteLine("GET");
            SP.Write("g");
            //Debug.Log (SP.ReadLine());
            //Text t = GetComponentsInParent<Text>()[0];
            Text t = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<Text>();
			
            t.text = SP.ReadLine();


        }*/
    }

    #region "ActionPrompter methods"
    public void DigUp()
    {
        EM.Scenario.GetCurrentPlayer().DigUp(null); // TODO add Niles modifier
        DM.ReloadActionPrompter();
        //DM.SetMapActive(true);
        //DM.SetPlayerActive(false);
    }
    public void Move()
    {
        GamePlayer p = EM.Scenario.GetCurrentPlayer();
        switch (GameBoard.Phase)
        {
            case Board.BoardPhase.PlayerHasSelectedSquare:
                p.Move();
                GameBoard.Phase = Board.BoardPhase.Unactive;
                break;
            default:
                if (!p.CurrentSquare.IsThreatened(p) || p.Fleeing)
                {
                    GameBoard.Phase = Board.BoardPhase.PlayerMoving;
                    //DM.ReloadActionPrompter();
                    DM.SetMapScreenActive(true);
                    DM.SetPlayerProfileScreenActive(false);
                }
                else
                {
                    // TODO Remove and use Flee() instead
                    GameBoard.Phase = Board.BoardPhase.PlayerFleeing;
                }

                break;
        }
    }
    public void Flee()
    {
        DM.GM.GameBoard.Phase = Board.BoardPhase.PlayerFleeing;
        EM.Scenario.GetCurrentPlayer().FleeIterator = 0;
    }
    public void Fight()
    {
        EM.Scenario.GetCurrentPlayer().GetFightTarget();
        //DM.ReloadActionPrompter();
    }
    public void Skill() // TODO Skills?
    {
        EM.Scenario.GetCurrentPlayer().Skill();
        DM.ReloadActionPrompter();
    }
    public void Inventory()
    {
        EM.Scenario.GetCurrentPlayer().LookInventory();
        DM.ReloadActionPrompter();
    }
    public void Rest()
    {
        EM.Scenario.GetCurrentPlayer().Rest();
        DM.ReloadActionPrompter();

        NextPlayer = true;

        DoNext();
    }
    #endregion
}

public class EventManager
{
    public GameScenario Scenario;

    public EventManager(GameScenario scenario)
    {
        Scenario = scenario;
    }


}

[System.Serializable]
public class DisplayManager
{
    public GameObject GameCanvas;

    public GameManager GM;

    public bool eventDoOnce = false;

    public GameObject ActionPrompter;
    //public GameObject PlayerProfile;

    //private TurnManager TM;
    public DisplayManager(GameManager gm)//TurnManager tm)
    {
        GM = gm;//TM.GM;
    }

    void update()
    {

    }

    public void ReloadActionPrompter()
    {
        List<string> actionList;
        GM.EM.Scenario.GetCurrentPlayer().GetActions(out actionList);

        GameObject container = GM.ActionPrompterDisplay;

        for (int i = 0; i < container.transform.childCount; ++i)
        {
            GameObject buttonObject = container.transform.GetChild(i).gameObject;
            if (actionList.Contains(buttonObject.name))
            {
                buttonObject.GetComponent<Image>().material = Resources.Load<Material>("Materials/Opaque");
                buttonObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                buttonObject.GetComponent<Image>().material = Resources.Load<Material>("Materials/Transparent");
                buttonObject.GetComponent<Button>().interactable = false;
            }
        }

        GM.mvt_btn.UpdateState();

        GameObject go = GM.PlayerProfileDisplay;
        go.GetComponent<ActivePlayerStatsManager>().UpdateState();

        Debug.Log("Prompter reloaded");
    }

    //TODO change loop content
    public void HideActionPrompter()
    {
        GameObject container = GM.ActionPrompterDisplay;

        for (int i = 0; i < container.transform.childCount; ++i)
        {
            GameObject buttonObject = container.transform.GetChild(i).gameObject;
            buttonObject.GetComponent<Image>().material = Resources.Load<Material>("Materials/Transparent");
            buttonObject.GetComponent<Button>().interactable = false;
        }

        GameObject go = GM.PlayerProfileDisplay;
        go.GetComponent<ActivePlayerStatsManager>().UpdateState();
    }

    public void ClearEnemyList()
    {
        Transform enemyListTransform = GM.PlayerProfileScreen.transform.Find("EnemyList");
        List<GameObject> lgo = new List<GameObject>();

        for (int i = 0; i < enemyListTransform.childCount; ++i)
        {
            lgo.Add(enemyListTransform.GetChild(i).gameObject);
        }

        for (int i = 0; i < lgo.Count; ++i)
        {
            GameObject.Destroy(lgo[i]);
        }

    }

    public void SetMapScreenActive(bool b)
    {
        Debug.Log("Set M : " + b);
        GM.MapScreen.SetActive(b);
        // Undisplay all other if activated
        if (b)
        {
            SetPlayerProfileScreenActive(false);
            SetNarrationScreenActive(false);
            SetFightScreenActive(false);

            SetTurnActive(true);
            SetPlayersSpriteActive(true);
            SetRandomSliderActive(false);
        }
        else
        {
            GamePlayer p = GM.EM.Scenario.GetCurrentPlayer();
            p.gameObject.transform.localPosition = p.DefaultSpritePosition;
        }
    }

    public void SetPlayerProfileScreenActive(bool b)
    {
        Debug.Log("Set P : " + b);
        GM.PlayerProfileScreen.SetActive(b);
        if (b)
        {
            GM.DM.ReloadActionPrompter();
            SetMapScreenActive(false);
            SetNarrationScreenActive(false);
            SetFightScreenActive(false);

            SetTurnActive(true);
            SetPlayersSpriteActive(true);
            SetRandomSliderActive(false);
        }
    }

    public void SetNarrationScreenActive(bool b)
    {
        Debug.Log("Set N : " + b);
        GM.NarrationScreen.SetActive(b);
        if (b)
        {
            SetMapScreenActive(false);
            SetPlayerProfileScreenActive(false);
            SetFightScreenActive(false);

            SetTurnActive(true);
            SetPlayersSpriteActive(false);
            SetRandomSliderActive(false);
        }
    }

    public void SetFightScreenActive(bool b)
    {
        GM.FightScreen.SetActive(b);
        if (b)
        {
            SetMapScreenActive(false);
            SetNarrationScreenActive(false);

            SetTurnActive(false);
            SetPlayersSpriteActive(false);
            SetRandomSliderActive(true);

        }
    }



    public void SetTurnActive(bool b)
    {
        GM.TurnDisplay.SetActive(b);
    }

    public void SetPlayersSpriteActive(bool b)
    {
        GM.PlayersSpriteDisplay.SetActive(b);
    }

    public void SetRandomSliderActive(bool b)
    {
        GM.GameBoard.RandomSlider.SetActive(b);
        GenerateNumbers gn = GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>();
        gn.isAnimationStarted = false;
        gn.isAnimationEnded = false;
    }


    public void Narration(object o)
    {
        if (eventDoOnce)
        {
            if (GM.Next)
            {
                eventDoOnce = false;
            }
            return;
        }
        eventDoOnce = true;

        string[] s = o as string[];
        string speaker = s[0];
        string speech = s[1];
        string sound = s[2];

        // 
        Transform t = GM.NarrationScreen.transform.FindChild("CharactersSpriteAnchor");
        for (int i = 0; i < t.childCount; ++i)
        {
            GameObject.Destroy(t.GetChild(i).gameObject);
        }

        // Change text
        if (speaker == "Narrator")
        {
            GM.NarrationScreen.transform.FindChild("CharactersTextAnchor").Find("CharactersBubble").gameObject.GetComponent<BGOfTextScenario>().SetBgForScenario("Narration");
            GM.NarrationScreen.transform.FindChild("CharactersTextAnchor").Find("CharactersBubble").Find("Text").GetComponent<Text>().text = "";
            GM.NarrationScreen.transform.FindChild("NarratorTextAnchor").Find("NarratorText").GetComponent<Text>().text = speech;
        }
        else
        {
            GM.NarrationScreen.transform.FindChild("NarratorTextAnchor").Find("NarratorText").GetComponent<Text>().text = "";
            GM.NarrationScreen.transform.FindChild("CharactersTextAnchor").Find("CharactersBubble").gameObject.GetComponent<BGOfTextScenario>().SetBgForScenario("Character");
            GM.NarrationScreen.transform.FindChild("CharactersTextAnchor").Find("CharactersBubble").Find("Text").GetComponent<Text>().text = speech;

            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Characters/" + speaker)) as GameObject;
            go.transform.SetParent(t);
        }

        // Display Narration after changing text
        GM.DM.SetNarrationScreenActive(true);

        // Sound
        if (sound != "")
        {
            int soundId = int.Parse(sound);
            if (soundId >= 0)
            {
                //idOfVoiceChannel = 
                JzzSoundManager.instance.PlayClip(soundId, JzzSoundManager.narrationChannel, false);
            }
        }
        else
        {
            JzzSoundManager.instance.StopChannel(JzzSoundManager.narrationChannel);
        }

    }

    public void Spawn(object o)
    {
        object[] oo = o as object[];
        string threat = oo[0] as string;
        BoardSquare square = oo[1] as BoardSquare;

        // TODO, change directory to Characters/Enemies
        GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Pieces/" + threat)) as GameObject;


        go.transform.SetParent(GM.ThreatSpawnDisplay.transform, false); // TODO add ref to ThreaSpawn
        GameEnemy enemy = go.GetComponent<GameEnemy>();
        GM.GameBoard.Enemies.Add(go.GetComponent<GameEnemy>());

        enemy.MoveTo(square);
        /*RectTransform rect = go.GetComponent<RectTransform>();
        rect.position = new Vector3(square.Center.x, square.Center.y, 0);
        */


        GM.DoNext();
    }
}

public class TurnManager : FSM
{
    public bool Started = false;

    public int NbTurns;
    public bool NewTurn;

    public GameManager GM;

    public TurnManager(GameManager gm)
        : base("TurnManager")
    {
        NbTurns = 0;
        NewTurn = false;

        GM = gm;

        //Create all states and sub-FSM
        TurnEventState intro = new TurnEventState(GM, "_intro");
        NextTurnState nts = new NextTurnState(this);
        TurnEventState tes = new TurnEventState(GM);
        EnemyTurnState enemyturn = new EnemyTurnState(GM);
        PlayerTurnFSM playerturn = new PlayerTurnFSM(this);

        // Add states and sub-FSM
        AddState(intro);
        AddState(nts);
        AddState(tes);
        AddState(enemyturn);
        AddState(playerturn);

        // Create all transitions
        IsDoneTransition idt2nts = new IsDoneTransition(nts);
        IsDoneTransition idt2tes = new IsDoneTransition(tes);
        IsDoneTransition idt2enemyturn = new IsDoneTransition(enemyturn);
        IsDoneTransition idt2playerturn = new IsDoneTransition(playerturn);
        NewTurnTransition t2nts = new NewTurnTransition(this, nts, 1);
        IsDoneTransition idt2playerturnself = new IsDoneTransition(playerturn);

        // Add transitions to states
        intro.AddTransition(idt2nts);
        nts.AddTransition(idt2tes);
        tes.AddTransition(idt2enemyturn);
        enemyturn.AddTransition(idt2playerturn);
        playerturn.AddTransition(t2nts); // priority over playerturnself
        playerturn.AddTransition(idt2playerturnself);
    }

    public void Start()
    {
        Started = true;
        DoBeforeEntering();

        // Intro
        GM.DM.SetNarrationScreenActive(true);
        GM.DM.SetTurnActive(false); // Deactivated for the intro only
    }

    public void update()
    {
        if (Started)
        {
            Do();
        }
    }
}

public class PlayerTurnFSM : FSM
{
    public GameManager GM;
    public TurnManager TM;

    public PlayerTurnFSM(TurnManager tm)
        : base("Turn")
    {
        TM = tm;
        GM = TM.GM;

        PlayerBeginTurnState pbt = new PlayerBeginTurnState(TM);
        PlayerTurnState pt = new PlayerTurnState(TM);
        PlayerEndTurnState pet = new PlayerEndTurnState(TM);
        AddState(pbt);
        AddState(pt);
        AddState(pet);

        IsDoneTransition idt2pt = new IsDoneTransition(pt);
        IsDoneTransition idt2pet = new IsDoneTransition(pet);
        pbt.AddTransition(idt2pt);
        pt.AddTransition(idt2pet);
    }
}

public class NextTurnState : FSMState
{
    public TurnManager TM;
    public bool Done;

    private Animator TurnTextAnimation;

    public NextTurnState(TurnManager tm)
        : base("NextTurn")
    {
        TM = tm;
        Done = false;

        TurnTextAnimation = GameObject.Find("TurnAnchor").GetComponent<Animator>();
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("Enter NT");
        TurnTextAnimation.SetTrigger("NextTurn");

        //defilement.animation.Rewind ();
        ++TM.NbTurns;
        ++(TM.GM.EM.Scenario.GetCurrentAct().CurrentScene);
        TM.NewTurn = false;

        List<GamePlayer> players = TM.GM.EM.Scenario.Players;
        for (int i = 0; i < players.Count; ++i)
        {
            players[i].Refresh();
            Debug.Log(players[i].Name + ".MaxLiveliness : " + players[i].MaxLiveliness);
            Debug.Log(players[i].Name + ".Liveliness : " + players[i].Liveliness);
            Debug.Log(players[i].Name + ".UsedLiveliness : " + players[i].GetUsedLiveliness());
            Debug.Log(players[i].Name + ".Damage : " + players[i].Damage);
            Debug.Log(players[i].Name + ".Craftiness : " + players[i].Craftiness);
        }

        Debug.Log("Turn " + TM.NbTurns.ToString());

        //defilement.Play (0);
        //TurnTextAnimation.
        Done = false;
    }

    public override void Do()
    {
        // TODO Add stuff here
        Debug.Log("NextTurn");

        TurnTextAnimation.Update(0.001f);

        //Debug.Log(TM.NbTurns.ToString());

        //if (Input.GetMouseButtonDown (0) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended)) {
        //	if(defilement.GetCurrentAnimatorStateInfo (0).IsName ("End"))

        //	TurnTextAnimation.SetTrigger("NextTurn");
        if (TurnTextAnimation.GetCurrentAnimatorStateInfo(0).IsName("TurnTextUp"))
        {
            GameObject.Find("Turn_Indicator").SendMessage("SetTurnValue", TM.NbTurns);
            TurnTextAnimation.SetTrigger("NextTurn");
            Done = true;
        }
    }

    public override void DoBeforeLeaving()
    {
        Debug.Log("Leave NT");

        //GameObject.Destroy(defilement);
        //defilement = GameObject.Find("Defilement_Text").AddComponent<Defilement>();

        Done = false;
    }

    public override bool IsDone()
    {
        //return Done;
        return Done && TurnTextAnimation.GetCurrentAnimatorStateInfo(0).IsName("TurnTextDown");// IsDone();
    }
}

public class PlayerBeginTurnState : FSMState
{
    public bool Done;
    public GameManager GM;
    public TurnManager TM;

    public GameObject PlayerCursorAnchor = null;
    private Animator PlayerCursorSpriteAnimation = null;
    private int ToRightCount = 0;
    private float DeltaPosition;
    private Vector3 CursorPosition;

    public PlayerBeginTurnState(TurnManager tm)
        : base("PlayerBeingTurn")
    {
        TM = tm;
        GM = TM.GM;

        //PlayerCursorAnchor = cursorGameObject.Find ("PlayerCursorAnchor");
        //PlayerCursorSpriteAnimation = PlayerCursorAnchor.GetComponent<Animator> ();

        DeltaPosition = 150f;
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("Enter PBT");
        Debug.Log(GM.EM.Scenario.GetCurrentPlayer().Name);


        GM.NextPlayer = false;

        //TODO Layout
        GM.GameBoard.MoveButton.SetActive(false);
        GM.GameBoard.CancelButton.SetActive(false);

        GM.DM.SetPlayerProfileScreenActive(true);
        GM.DM.SetTurnActive(true);

        GM.DM.ReloadActionPrompter();


        if (PlayerCursorAnchor == null)
        {
            PlayerCursorAnchor = GameObject.Find("PlayerCursorAnchor");
        }
        if (PlayerCursorSpriteAnimation == null)
        {
            PlayerCursorSpriteAnimation = PlayerCursorAnchor.GetComponent<Animator>();
        }

        Done = false;
    }

    public override void Do()
    {
        Debug.Log("PlayerBeginTurn");

        Debug.Log("Player #" + GM.EM.Scenario.CurrentPlayer);

        if (PlayerCursorSpriteAnimation.GetCurrentAnimatorStateInfo(0).IsName("CursorSpriteIdle"))
        {
            if (GM.EM.Scenario.CurrentPlayer == ToRightCount)
            {
                SolidifyPosition();
                Done = true;
                return;
            }
            else if (ToRightCount < GM.EM.Scenario.CurrentPlayer)
            {
                ToRightCount++;

                PlayerCursorSpriteAnimation.SetTrigger("ToRight");
            }
            else if (ToRightCount > GM.EM.Scenario.CurrentPlayer)
            {
                ToRightCount--;

                PlayerCursorSpriteAnimation.SetTrigger("ToLeft");
            }
            SolidifyPosition();
        }
    }

    public void SolidifyPosition()
    {
        CursorPosition = PlayerCursorAnchor.transform.position;
        CursorPosition.x = ToRightCount * DeltaPosition;

        PlayerCursorAnchor.transform.position = PlayerCursorAnchor.transform.TransformVector(CursorPosition);
    }

    public override void DoBeforeLeaving()
    {
        Done = false;
        Debug.Log("Leave PBT");
    }

    public override bool IsDone()
    {
        return Done && PlayerCursorSpriteAnimation.GetCurrentAnimatorStateInfo(0).IsName("CursorSpriteIdle");
    }
}

public class PlayerTurnState : FSMState
{
    public bool Done;
    private TurnManager TM;
    private GameManager GM;

    private Animator PlayerCursorSpriteAnimation;

    private static Dictionary<string, GameAction> ActionList;

    public PlayerTurnState(TurnManager tm)
        : base("PlayerTurn")
    {
        TM = tm;
        GM = TM.GM;
        PlayerCursorSpriteAnimation = GameObject.Find("PlayerCursorAnchor").GetComponent<Animator>();
    }

    public override void DoBeforeEntering()
    {
        TM.GM.DM.ReloadActionPrompter();

        PlayerCursorSpriteAnimation.SetBool("PlayerTurn", true);
    }

    public override void Do()
    {
        if (GM.GameBoard.PhaseHasChanged || GM.Next || GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>().isAnimationEnded)
        {
            GM.GameBoard.PhaseHasChanged = false;
            GamePlayer p;

            switch (GM.GameBoard.Phase)
            {
                case Board.BoardPhase.WaitNext:

                    GM.DM.SetRandomSliderActive(false);
                    GM.GameBoard.NextButton.SetActive(true); // TODO

                    Debug.Log("Wait NExt");

                    if (GM.Next)
                    {
                        Debug.Log("GM NEXT");
                        GM.GameBoard.Phase = GM.GameBoard.NextPhase;
                    }

                    break;

                case Board.BoardPhase.Unactive:

                    Debug.Log("Player unactive");
                    GM.DM.ReloadActionPrompter();
                    // TODO Setup Player screen
                    //...
                    ///
                    GM.DM.SetPlayerProfileScreenActive(true);

                    GM.GameBoard.MoveButton.SetActive(false);
                    GM.GameBoard.CancelButton.SetActive(false);
                    GM.DM.SetTurnActive(true);

                    break;

                case Board.BoardPhase.PlayerMoving:
                    // Uncolorize board
                    Color col = new Color(1F, 1F, 1F, 0.0f);
                    for (int i = 0; i < GM.GameBoard.squares.Length; ++i)
                    {
                        GM.GameBoard.squares[i].SetMaterial(GM.GameBoard.squares[i].matExit, col);
                    }

                    // Colorize Reach in green
                    p = GM.EM.Scenario.GetCurrentPlayer();
                    if (p.Reach != null)
                    {
                        for (int i = 0; i < p.Reach.Count; i++)
                        {
                            p.Reach[i].SetMaterial(p.Reach[i].matExit, new Color(0, 0, 0, 0));
                        }
                    }
                    Color green = new Color(0.19f, 0.78f, 0.45f);
                    p.Reach = GM.GameBoard.Reach(p.CurrentSquare, p.Liveliness);

                    if (p.Reach != null)
                    {
                        for (int i = 0; i < p.Reach.Count; i++)
                        {
                            p.Reach[i].SetMaterial(p.Reach[i].matEnter, green);
                        }
                    }

                    GM.DM.SetMapScreenActive(true);
                    GM.GameBoard.CancelButton.SetActive(false);
                    GM.GameBoard.NextButton.SetActive(false);


                    // Put the player's pawn on the square
                    Vector3 tempPos = p.gameObject.transform.localPosition;
                    tempPos.x = p.CurrentSquare.Center.x;
                    tempPos.y = p.CurrentSquare.Center.y + 40; //offset
                    p.gameObject.transform.localPosition = tempPos;

                    break;
                case Board.BoardPhase.PlayerHasSelectedSquare:

                    GM.GameBoard.MoveButton.SetActive(true);
                    GM.GameBoard.CancelButton.SetActive(true);
                    GM.GameBoard.NextButton.SetActive(false);

                    break;

                case Board.BoardPhase.PlayerFleeing:

                    GameEnemy enemy;
                    p = GM.EM.Scenario.GetCurrentPlayer();

                    if (GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>().isAnimationEnded)
                    {
                        int dice = GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>().Value();
                        if (p.Craftiness + dice < p.CurrentSquare.Enemies[p.FleeIterator].Threat)
                        {
                            // Player is caught by the Enemy threat
                            p.HasFailedToFlee = true;
                            p.Fleeing = false;
                            bool isDead = p.Hurt();

                            enemy = p.CurrentSquare.Enemies[p.FleeIterator];
                            GM.FightScreen.GetComponent<FightDisplayMan>().GenerateCombatText(enemy.Threat, p.Craftiness, dice, 1, "FLEE", false, "The Cat", p.Name, isDead);
                            GM.FightScreen.GetComponent<FightDisplayMan>().UpdateStatus(enemy.Threat, enemy.Life,
                                p.Craftiness, p.Liveliness, "cat", p.Name, false);

                            // Pause
                            GM.GameBoard.Phase = Board.BoardPhase.WaitNext;
                            GM.GameBoard.NextPhase = Board.BoardPhase.Unactive;

                            Debug.Log("Player hurt");

                            return;
                        }

                        ++(p.FleeIterator);
                        if (p.FleeIterator < p.CurrentSquare.Enemies.Count)
                        {
                            Debug.Log("Fleeing again from another threat");

                            enemy = p.CurrentSquare.Enemies[p.FleeIterator];

                            GM.FightScreen.GetComponent<FightDisplayMan>().GenerateCombatText(enemy.Threat, p.Craftiness, dice, 1, "FLEE", true, "The Cat", p.Name);
                            GM.FightScreen.GetComponent<FightDisplayMan>().UpdateStatus(enemy.Threat, enemy.Life,
                                p.Craftiness, p.Liveliness, "cat", p.Name, true);

                            GM.GameBoard.Phase = Board.BoardPhase.WaitNext;
                            GM.GameBoard.NextPhase = Board.BoardPhase.PlayerFleeing;

                            Debug.Log("Player Fleeing");
                        }
                        else
                        {
                            p.Fleeing = true;
                            enemy = p.CurrentSquare.Enemies[p.CurrentSquare.Enemies.Count - 1];

                            GM.FightScreen.GetComponent<FightDisplayMan>().GenerateCombatText(enemy.Threat, p.Craftiness, dice, 1, "FLEE", true, "The Cat", p.Name);
                            GM.FightScreen.GetComponent<FightDisplayMan>().UpdateStatus(enemy.Threat, enemy.Life,
                                p.Craftiness, p.Liveliness, "cat", p.Name, true);

                            GM.GameBoard.Phase = Board.BoardPhase.WaitNext;
                            GM.GameBoard.NextPhase = Board.BoardPhase.PlayerMoving;

                            Debug.Log("Player Moving");
                        }

                        return;
                    }

                    Debug.Log("Y");

                    GM.DM.SetFightScreenActive(true);
                    enemy = p.CurrentSquare.Enemies[p.FleeIterator];
                    GM.FightScreen.GetComponent<FightDisplayMan>().UpdateStatus(enemy.Threat, enemy.Life,
                        p.Craftiness, p.Liveliness, "cat", p.Name, false);

                    break;

                case Board.BoardPhase.PlayerAttacking:
                    Debug.Log("The player is attacking");

                    p = GM.EM.Scenario.GetCurrentPlayer();

                    if (GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>().isAnimationEnded)
                    {
                        Debug.Log("The animation has ended");
                        p.Fight();

                        GM.DM.SetRandomSliderActive(false);

                        GM.GameBoard.Phase = Board.BoardPhase.WaitNext;
                        GM.GameBoard.NextPhase = Board.BoardPhase.Unactive;
                        return;
                    }

                    GM.DM.SetFightScreenActive(true);
                    enemy = p.TargetEnemy;
                    GM.FightScreen.GetComponent<FightDisplayMan>().UpdateStatus(p.Craftiness, p.Liveliness, enemy.Threat, enemy.Life, p.Name, "cat", false);
                    //GM.DM.SetRandomSliderActive(true);

                    break;
            }
        }
    }

    public override void DoBeforeLeaving()
    {
        PlayerCursorSpriteAnimation.SetBool("PlayerTurn", false);
        Done = false;
    }

    public override bool IsDone()
    {
        return TM.GM.NextPlayer;// TM.GM.Next;
    }

    /*
    public static void EndPlayerTurn()
    {
        ActionList.Clear ();
        return;
    }*/
}

public class PlayerEndTurnState : FSMState
{
    public bool Done;
    public GameManager GM;
    public TurnManager TM;

    public PlayerEndTurnState(TurnManager tm)
        : base("PlayerEndTurn")
    {
        TM = tm;
        GM = TM.GM;
    }

    public override void Do()
    {
        Debug.Log("PlayerEndTurn");
        Done = true;
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("Entering PET");
        Done = false;

        if (!(++(GM.EM.Scenario.CurrentPlayer) < GM.EM.Scenario.Players.Count))
        {
            GM.EM.Scenario.CurrentPlayer = 0;
            TM.NewTurn = true;
        }
    }

    public override void DoBeforeLeaving()
    {
        Debug.Log("Leaving PET");

        Done = false;
    }

    public override bool IsDone()
    {
        return Done;
    }
}

public class TurnEventState : FSMState
{
    public GameManager GM;
    public TurnManager TM;
    public EventManager EM;
    public List<GameEvent> Events;
    public int CurrentEvent;

    private bool hasChanged = false;

    public TurnEventState(GameManager gm, string s = "")
        : base("TurnEvent" + s)
    {
        GM = gm;
        TM = gm.TM;
        EM = gm.EM;
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("Enter TurnEventState");
        Events = EM.Scenario.FetchEvents();
        CurrentEvent = 0;

        //TODO
        if (Events.Count > 0)
        {
            hasChanged = true;
            //GM.DM.SetNarrationActive(true);
            GM.DM.SetTurnActive(true);
        }
    }

    public override void Do()
    {
        Debug.Log("TurnEventState");
        if (CurrentEvent < Events.Count)
        {
            Events[CurrentEvent].ExecuteEvent();
        }
        if (GM.Next)
        {
            ++CurrentEvent;
            Debug.Log("ce" + CurrentEvent);
            Debug.Log("ec " + Events.Count);
        }
    }

    public override void DoBeforeLeaving()
    {
        //TODO
        if (hasChanged)
        {
            hasChanged = false;
            //GM.DM.SetPlayerActive(true);
            GM.DM.SetTurnActive(true);
            //GM.DM.SetRandomSliderDisplayActive (false);
        }
    }

    public override bool IsDone()
    {
        return !(CurrentEvent < Events.Count);
    }
}

public class EnemyTurnState : FSMState
{
    private GameManager GM;
    public List<GameEnemy> Enemies;

    private int enemyIterator = 0;
    private bool Done = false;
    private bool willAttack = false;
    private bool waitingForSlider = false;

    public EnemyTurnState(GameManager gm)
        : base("EnemyTurn")
    {
        GM = gm;
        Enemies = GM.GameBoard.Enemies;//new List<GameEnemy>();
    }

    public override void DoBeforeEntering()
    {
        Debug.Log("Enter TurnEnemyState");

        enemyIterator = 0;
        willAttack = false;
        Done = false;

        if (Enemies.Count > 0)
        {
            //GM.DM.SetMapActive(true);
            //GM.DM.SetPlayerActive(true);
            GM.DM.SetTurnActive(true);

            GM.GameBoard.MoveButton.SetActive(false);
            GM.GameBoard.CancelButton.SetActive(false);
            GM.GameBoard.NextButton.SetActive(true);

            GM.GameBoard.Phase = Board.BoardPhase.EnemyMoving;
        }
        else
        {
            //++enemyIterator;
            Done = true;
        }
    }

    public override void Do()
    {
        if (Enemies.Count == 0)
        {
            Done = true;
            return;
        }

        if (GM.GameBoard.PhaseHasChanged || GM.Next || GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>().isAnimationEnded)
        {
            GM.GameBoard.PhaseHasChanged = false;


            switch (GM.GameBoard.Phase)
            {
                case Board.BoardPhase.EnemyMoving:

                    // TODO Pause before phase change?
                    if (GM.Next)
                    {
                        GM.GameBoard.Phase = willAttack ?
                            Board.BoardPhase.EnemyAttacking :
                                Board.BoardPhase.Unactive;
                        return;
                    }

                    Debug.Log("Enemies Moving");
                    // Cat turn - moving
                    GM.DM.SetMapScreenActive(true);
                    GM.DM.SetTurnActive(true);
                    //
                    GM.GameBoard.CancelButton.SetActive(false);
                    GM.GameBoard.MoveButton.SetActive(false);
                    GM.GameBoard.NextButton.SetActive(true);

                    // Uncolorize board
                    Color col = new Color(1F, 1F, 1F);
                    for (int i = 0; i < GM.GameBoard.squares.Length; ++i)
                    {
                        GM.GameBoard.squares[i].SetMaterial(GM.GameBoard.squares[i].matExit, col);
                    }

                    // Move each enemy
                    for (int i = 0; i < Enemies.Count; ++i)
                    {
                        Enemies[i].Move();
                        if (Enemies[i].CurrentSquare.IsThreatened(Enemies[i]))
                        {
                            willAttack = true;
                        }
                    }

                    break;
                case Board.BoardPhase.EnemyAttacking:
                    Debug.Log("Enemies Attacking - Iterator : #" + enemyIterator);
                    Debug.Log("Enemies Attacking - Waitingforslider : #" + waitingForSlider);
                    Debug.Log("Enemies Attacking - Slider animation : #" + GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>().isAnimationEnded);

                    GameEnemy enemy = Enemies[enemyIterator];

                    if (waitingForSlider)
                    {
                        // Block if waiting for slider and the slider has not been slided
                        if (!GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers>().isAnimationEnded)
                        {
                            return;
                        }

                        /*GameEnemy enemy = Enemies[enemyIterator];
                        // Find the first enemy attacking
                        while (enemyIterator < GM.GameBoard.Enemies.Count
                               && !Enemies[enemyIterator].CurrentSquare.IsThreatened(Enemies[enemyIterator])
                               || !Enemies[enemyIterator].CanFight())
                        {
                            // Next enemy
                            ++enemyIterator;
                            if (enemyIterator >= GM.GameBoard.Enemies.Count)
                            {
                                waitingForSlider = false;

                                //TODO pause with waitnext phase
                                GM.GameBoard.NextButton.SetActive(true);
                                GM.GameBoard.Phase = Board.BoardPhase.Unactive;

                                return;
                            }

                            enemy = Enemies[enemyIterator];

                           
                            
                        }*/

                        Debug.Log("Enemy #" + enemyIterator + " on square #" + (enemy.CurrentSquare.Id + 1));
                        Debug.Log("Fight!");

                        enemy.Fight();
                        //GM.FightScreen.GetComponent<FightDisplayMan>().UpdateStatus(enemy.Threat, enemy.Life,
                        //enemy.TargetPlayer.Craftiness, enemy.TargetPlayer.Liveliness, "cat", enemy.TargetPlayer.Name);
                        // Next enemy
                        ++enemyIterator;
                        if (enemyIterator >= GM.GameBoard.Enemies.Count)
                        {
                            waitingForSlider = false;

                            //TODO pause with waitnext phase

                            GM.GameBoard.Phase = Board.BoardPhase.WaitNext;
                            GM.GameBoard.NextPhase = Board.BoardPhase.Unactive;
                        }
                        else
                        {
                            // wait for next Fight
                            // Reset random slider
                            //GM.DM.SetRandomSliderActive(true);
                            GM.GameBoard.Phase = Board.BoardPhase.WaitNext;
                            GM.GameBoard.NextPhase = Board.BoardPhase.EnemyAttacking;
                            waitingForSlider = false;
                        }

                        return;
                    }

                    // Display fight screen
                    GM.DM.SetFightScreenActive(true);

                    // Find the first enemy attacking
                    while (enemyIterator < GM.GameBoard.Enemies.Count
                           && !Enemies[enemyIterator].CurrentSquare.IsThreatened(Enemies[enemyIterator])
                           || !Enemies[enemyIterator].CanFight())
                    {
                        // Next enemy
                        ++enemyIterator;
                        if (enemyIterator >= GM.GameBoard.Enemies.Count)
                        {
                            waitingForSlider = false;

                            //TODO pause with waitnext phase

                            GM.GameBoard.Phase = Board.BoardPhase.WaitNext;
                            GM.GameBoard.NextPhase = Board.BoardPhase.Unactive;

                            return;
                        }

                        enemy = Enemies[enemyIterator];
                    }

                    enemy.GetFightTarget();
                    GM.FightScreen.GetComponent<FightDisplayMan>().UpdateStatus(enemy.Threat, enemy.Life,
                        enemy.TargetPlayer.Craftiness, enemy.TargetPlayer.Liveliness, "cat", enemy.TargetPlayer.Name, false);

                    waitingForSlider = true;

                    break;
                case Board.BoardPhase.Unactive:

                    Debug.Log("Board set to Unactive");

                    //GM.DM.SetPlayerProfileScreenActive(true);

                    //if (GM.Next)
                    //{
                        Done = true;
                    //}
                    /*GM.DM.SetRandomSliderActive(false);
                    enemyIterator = Enemies.Count; // go to Next State immediately

                    if (GM.Next)
                    {
                    Done = true;
                    }
                    */
                    break;

                case Board.BoardPhase.WaitNext:

                    GM.DM.SetRandomSliderActive(false);
                    GM.GameBoard.NextButton.SetActive(true); // TODO

                    if (GM.Next)
                    {
                        GM.GameBoard.Phase = GM.GameBoard.NextPhase;
                    }

                    break;
            }

            //++enemyIterator;
        }
    }

    public override void DoBeforeLeaving()
    {
    }

    public override bool IsDone()
    {
        return Done;// GM.GameBoard.Phase == Board.BoardPhase.Unactive;
    }

}

public class IsDoneTransition : FSMTransition
{
    public IsDoneTransition(FSMState toState, int priority = 5)
        : base("IsDone", toState, priority)
    {
    }

    public override bool Check()
    {
        return FromState.IsDone();
    }
}

public class NewTurnTransition : FSMTransition
{
    public TurnManager TM;
    public NewTurnTransition(TurnManager tm, FSMState toState, int priority = 5)
        : base("NewTurn", toState, priority)
    {
        TM = tm;
    }

    public override bool Check()
    {
        return TM.NewTurn;
    }
}


