using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using System.IO.Ports;


public class GameManager : MonoBehaviour
{
	//public bool NextClicked = false;
	public bool Next = false;


	public GameObject MapDisplay;
	public GameObject PlayerDisplay;
	public GameObject NarrationDisplay;
	public GameObject TurnDisplay;
	public GameObject RandomSliderDisplay;


	public EventManager EM;
	public TurnManager TM;
	public DisplayManager DM;
	
	public void DoNext()
	{
		Next = true;
	}
	
	//private SerialPort SP;
	//private string s="";

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start ()
	{
		//List<GamePlayer> Players = GamePlayer.Players;

		DM = new DisplayManager (this);
		EM = new EventManager (GameScenario.Init (DM));
		TM = new TurnManager (this);

		Debug.Log ("NB tours: " + EM.Scenario.Acts [0].Scenes.Count);

		TM.Start ();
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
	
	void Update ()
	{
		TM.update ();

		if(Next){
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


	void OnGUI() {
		//GUILayout.Label("Press Enter To Start Game");
		if (Event.current.Equals (Event.KeyboardEvent ("return"))) {
			/*Debug.Log (s);
			Text t = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<Text>();
			t.text = s;*/
			Application.CaptureScreenshot("screenshot.png");
			// TODO is s a caseCode?
		}
	}

	#region "ActionPrompter methods"
	public void DigUp()
	{
		EM.Scenario.GetCurrentPlayer ().DigUp (null); // TODO add Niles modifier
		DM.ReloadActionPrompter();
		DM.SetMapActive(true);
		DM.SetPlayerActive(false);
	}
	public void Move()
	{
		EM.Scenario.GetCurrentPlayer ().Move ();
		DM.ReloadActionPrompter();
		DM.SetMapActive(true);
		DM.SetPlayerActive(false);
	}
	public void Fight()
	{
		EM.Scenario.GetCurrentPlayer ().Fight ();
		DM.ReloadActionPrompter();
	}
	public void Skill() // TODO Skills?
	{
		EM.Scenario.GetCurrentPlayer ().Skill ();
		DM.ReloadActionPrompter();
	}
	public void Inventory()
	{
		EM.Scenario.GetCurrentPlayer ().LookInventory ();
		DM.ReloadActionPrompter();
	}
	public void Rest()
	{
		EM.Scenario.GetCurrentPlayer ().Rest ();
		DM.ReloadActionPrompter();

		DoNext ();
	}
	#endregion
}

public class EventManager
{
	public GameScenario Scenario;

	public EventManager (GameScenario scenario)
	{
		Scenario = scenario;
	}


}

[System.Serializable]
public class DisplayManager
{
	public GameObject GameCanvas;

	private GameManager GM;
	//private TurnManager TM;
	public DisplayManager (GameManager gm)//TurnManager tm)
	{
		//tm = tm;
		GM = gm;//TM.GM;
	}

	void update ()
	{
		
	}

	public void ReloadActionPrompter()
	{
		List<string> actionList;// = new Dictionary<string, GameAction> ();
		GM.EM.Scenario.GetCurrentPlayer().GetActions (out actionList);

		//DisplayManager.ActionLayout(GameObject.Find ("ActionPrompter"), actionList);

		GameObject container = GameObject.Find ("ActionPrompter");
		List<GameObject> lgo = new List<GameObject>();
		
		for (int i = 0; i < container.transform.childCount; ++i) {
			GameObject go = container.transform.GetChild(i).gameObject;
			Vector3 pos = go.transform.localPosition;

			if(actionList.Contains(go.name))
			{
				pos.x = 0;
			}else{
				pos.x = 1000;
			}
			go.transform.localPosition = pos;

		}

			/*Button b = go.GetComponent<Button>();
			b.interactable = true;
			
			string s = entry.Key; // Closure issue : store external value (entry.Key) in internal variable (s)
			b.onClick.AddListener(() => {ActionManager.DoAction(s);});
			 */
		
		Debug.Log ("Prompter reloaded");
	}

	public void SetMapActive(bool b)
	{
		GM.MapDisplay.SetActive (b);
	}

	public void SetPlayerActive(bool b)
	{
		GM.PlayerDisplay.SetActive (b);
	}

	public void SetNarrationActive(bool b)
	{
		GM.NarrationDisplay.SetActive (b);
	}

	public void SetTurnActive(bool b)
	{
		GM.TurnDisplay.SetActive (b);
	}
	public void SetRandomSliderDisplayActive(bool b)
	{
		GM.RandomSliderDisplay.SetActive (b);
	}


	public void Narration(object o){
		string[] s = o as string[];
		string speaker = s [0];
		string speech = s [1];

		Debug.Log ("speaker is : " + speaker);

		GameObject go = GameObject.Instantiate(Resources.Load<GameObject> ("Prefabs/Characters/" + speaker)) as GameObject;
		if (go != null) {

			// Display Character
			Transform t = GM.NarrationDisplay.transform.FindChild ("CharactersSpriteAnchor");
			List<GameObject> lgo = new List<GameObject> ();
			for (int i=0; i< t.childCount; ++i) {
				lgo.Add (t.GetChild (i).gameObject);
			}

			for (int i=0; i < lgo.Count; ++i) {
				GameObject.Destroy (lgo [i]);
			}

			go.transform.SetParent (t);

			// Change Text Content
			Transform tt = GM.NarrationDisplay.transform.FindChild ("CharactersTextAnchor").Find ("CharactersBubble").Find ("Text");
			Debug.Log(tt);
			Text txt = tt.gameObject.GetComponent<Text>();
			Debug.Log (txt);
			//Text txt = GM.NarrationDisplay.transform.FindChild ("CharactersTextAnchor/Text").gameObject.GetComponent<Text>();
			txt.text = speech;
		}


	}
}

public class TurnManager : FSM
{
	public bool Started = false;

	public int NbTurns;
	public bool NewTurn;

	public GameManager GM;

	public TurnManager (GameManager gm) : base("TurnManager")
	{
		NbTurns = 0;
		NewTurn = false;

		GM = gm;
		
		//Create all states and sub-FSM
		TurnEventState intro = new TurnEventState (GM, "_intro");
		NextTurnState nts = new NextTurnState (this);
		TurnEventState tes = new TurnEventState (GM);
		EnemyTurnState enemyturn = new EnemyTurnState (GM);
		PlayerTurnFSM playerturn = new PlayerTurnFSM (this);

		// Add states and sub-FSM
		AddState (intro);
		AddState (nts);
		AddState (tes);
		AddState (enemyturn);
		AddState (playerturn);

		// Create all transitions
		IsDoneTransition idt2nts = new IsDoneTransition (nts);
		IsDoneTransition idt2tes = new IsDoneTransition (tes);
		IsDoneTransition idt2enemyturn = new IsDoneTransition (enemyturn);
		IsDoneTransition idt2playerturn = new IsDoneTransition (playerturn);
		NewTurnTransition t2nts = new NewTurnTransition (this, nts, 1);
		IsDoneTransition idt2playerturnself = new IsDoneTransition (playerturn);

		// Add transitions to states
		intro.AddTransition(idt2nts);
		nts.AddTransition(idt2tes);
		tes.AddTransition(idt2enemyturn);
		enemyturn.AddTransition(idt2playerturn);
		playerturn.AddTransition(t2nts); // priority over playerturnself
		playerturn.AddTransition (idt2playerturnself); 
	}

	public void Start(){
		Started = true;
		DoBeforeEntering ();

		// Intro
		GM.DM.SetMapActive (false);
		GM.DM.SetNarrationActive (true);
		GM.DM.SetTurnActive (false);
		GM.DM.SetPlayerActive (false);
		GM.DM.SetRandomSliderDisplayActive (false);
	}

	public void update ()
	{
		if (Started) {
			Do ();
		}
	}
}

public class PlayerTurnFSM : FSM
{
	public GameManager GM;
	public TurnManager TM;

	public PlayerTurnFSM(TurnManager tm) : base("Turn")
	{
		TM = tm;
		GM = TM.GM;

		PlayerBeginTurnState pbt = new PlayerBeginTurnState (TM);
		PlayerTurnState pt = new PlayerTurnState (TM);
		PlayerEndTurnState pet = new PlayerEndTurnState (TM);
		AddState (pbt);
		AddState (pt);
		AddState (pet);

		IsDoneTransition idt2pt = new IsDoneTransition (pt);
		IsDoneTransition idt2pet = new IsDoneTransition (pet);
		pbt.AddTransition (idt2pt);
		pt.AddTransition (idt2pet);
	}
}

public class NextTurnState : FSMState
{
	public TurnManager TM;
	public bool Done;

	private Animator TurnTextAnimation;

	public NextTurnState(TurnManager tm) : base("NextTurn")
	{
		TM = tm;
		Done = false;

		TurnTextAnimation = GameObject.Find("TurnAnchor").GetComponent<Animator>();
	}

	public override void DoBeforeEntering ()
	{
		Debug.Log ("Enter NT");
		TurnTextAnimation.SetTrigger ("NextTurn");

		//defilement.animation.Rewind ();
		++TM.NbTurns;
		++(TM.GM.EM.Scenario.GetCurrentAct ().CurrentScene);
		TM.NewTurn = false;

		List<GamePlayer> players = TM.GM.EM.Scenario.Players;
		for (int i = 0; i < players.Count; ++i) {
			players[i].Refresh ();
		}

		Debug.Log ("Turn " + TM.NbTurns.ToString());

		//defilement.Play (0);
		//TurnTextAnimation.
		Done = false;
	}
	
	public override void Do ()
	{
		// TODO Add stuff here
		Debug.Log("NextTurn");

		TurnTextAnimation.Update (0.001f);

		//Debug.Log(TM.NbTurns.ToString());

		//if (Input.GetMouseButtonDown (0) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended)) {
		//	if(defilement.GetCurrentAnimatorStateInfo (0).IsName ("End"))

		//	TurnTextAnimation.SetTrigger("NextTurn");
		if (TurnTextAnimation.GetCurrentAnimatorStateInfo (0).IsName ("TurnTextUp"))
		{
			GameObject.Find("TurnText").GetComponentInChildren<Text>().text = "Tour " + TM.NbTurns.ToString ();
			TurnTextAnimation.SetTrigger ("NextTurn");
			Done = true;
		}
	}

	public override void DoBeforeLeaving ()
	{
		Debug.Log ("Leave NT");	
		
		//GameObject.Destroy(defilement);
		//defilement = GameObject.Find("Defilement_Text").AddComponent<Defilement>();
		
		Done = false;
	}

	public override bool IsDone ()
	{
		//return Done;
		return Done && TurnTextAnimation.GetCurrentAnimatorStateInfo (0).IsName ("TurnTextDown");// IsDone();
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

	public PlayerBeginTurnState(TurnManager tm) : base("PlayerBeingTurn")
	{
		TM = tm;
		GM = TM.GM;

		//PlayerCursorAnchor = cursorGameObject.Find ("PlayerCursorAnchor");
		//PlayerCursorSpriteAnimation = PlayerCursorAnchor.GetComponent<Animator> ();

		DeltaPosition = 150f;
	}

	public override void DoBeforeEntering ()
	{
		Debug.Log ("Enter PBT");
		if (PlayerCursorAnchor == null)
		{
			PlayerCursorAnchor = GameObject.Find ("PlayerCursorAnchor");
		}
		if (PlayerCursorSpriteAnimation == null)
		{
			PlayerCursorSpriteAnimation = PlayerCursorAnchor.GetComponent<Animator> ();
		}

		Debug.Log (GM.EM.Scenario.GetCurrentPlayer().Name);

		Done = false;
	}

	public override void Do()
	{
		Debug.Log ("PlayerBeginTurn");
		
		Debug.Log ("Player #" + GM.EM.Scenario.CurrentPlayer);
		
		if (PlayerCursorSpriteAnimation.GetCurrentAnimatorStateInfo (0).IsName ("CursorSpriteIdle")){
			if (GM.EM.Scenario.CurrentPlayer == ToRightCount) {
				SolidifyPosition();
				Done = true;
				return;
			}else if(ToRightCount < GM.EM.Scenario.CurrentPlayer){
				ToRightCount++;

				PlayerCursorSpriteAnimation.SetTrigger ("ToRight");
			}else if(ToRightCount > GM.EM.Scenario.CurrentPlayer){
				ToRightCount--;

				PlayerCursorSpriteAnimation.SetTrigger ("ToLeft");
			}
			SolidifyPosition();
		}
	}

	public void SolidifyPosition()
	{
		CursorPosition = PlayerCursorAnchor.transform.position;
		CursorPosition.x = ToRightCount * DeltaPosition;

		PlayerCursorAnchor.transform.position = PlayerCursorAnchor.transform.TransformVector (CursorPosition);
	}

	public override void DoBeforeLeaving ()
	{
		Done = false;
		Debug.Log ("Leave PBT");
	}

	public override bool IsDone ()
	{
		return Done && PlayerCursorSpriteAnimation.GetCurrentAnimatorStateInfo (0).IsName ("CursorSpriteIdle");
	}
}

public class PlayerTurnState : FSMState
{
	public bool Done;
	public TurnManager TM;
	private Animator PlayerCursorSpriteAnimation;

	private static Dictionary<string, GameAction> ActionList;

	public PlayerTurnState(TurnManager tm) : base("PlayerTurn")
	{
		TM = tm;
		PlayerCursorSpriteAnimation = GameObject.Find ("PlayerCursorAnchor").GetComponent<Animator> ();
	}

	public override void DoBeforeEntering()	
	{
		PlayerCursorSpriteAnimation.SetBool("PlayerTurn", true);


		TM.GM.DM.ReloadActionPrompter();
	}

	public override void Do()
	{
		/*if (!ActionManager.Busy) {
			Debug.Log ("PlayerTurn");

			Done = true; // TODO
		} else {
			if(ChooseExpression.Once > 0)
			{
				ActionManager.Busy = false;
				ChooseExpression.Test ();
				--ChooseExpression.Once;
			}
		}*/
	}
	public override void DoBeforeLeaving ()
	{
		PlayerCursorSpriteAnimation.SetBool("PlayerTurn", false);
		Done = false;

	}

	public override bool IsDone ()
	{
		//return ActionList.Count == 0;//Done;
		return TM.GM.Next;
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

	public PlayerEndTurnState(TurnManager tm) : base("PlayerEndTurn")
	{
		TM = tm;
		GM = TM.GM;
	}

	public override void Do()
	{
		Debug.Log ("PlayerEndTurn");
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

	public override bool IsDone ()
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

	public TurnEventState(GameManager gm, string s = "") : base("TurnEvent"+s)
	{
		GM = gm;
		TM = gm.TM;
		EM = gm.EM;
	}

	public override void DoBeforeEntering()
	{
		Debug.Log ("Enter TurnEventState");
		Events = EM.Scenario.FetchEvents ();
		CurrentEvent = 0;

		//TODO
		if (Events.Count > 0)
		{
			hasChanged = true;
			GM.DM.SetMapActive (false);
			GM.DM.SetPlayerActive (false);
			GM.DM.SetNarrationActive (true);
			GM.DM.SetTurnActive (true);
			GM.DM.SetRandomSliderDisplayActive (false);
		}
	}

	public override void Do()
	{
		Debug.Log ("TurnEventState");
		if (CurrentEvent < Events.Count) {
			Events [CurrentEvent].ExecuteEvent ();
		}
		if (GM.Next) {
			++CurrentEvent;
			Debug.Log ("ce" +CurrentEvent);
			Debug.Log ("ec " +Events.Count);
		}
	}

	public override void DoBeforeLeaving()
	{
		//TODO
		if (hasChanged)
		{
			hasChanged = false;
			GM.DM.SetMapActive (false);
			GM.DM.SetPlayerActive (true);
			GM.DM.SetNarrationActive (false);
			GM.DM.SetTurnActive (true);
			GM.DM.SetRandomSliderDisplayActive (false);
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
	public EnemyTurnState(GameManager gm) : base("EnemyTurn")
	{
		GM = gm;
		Enemies = new List<GameEnemy>();
	}

	public override void DoBeforeEntering()
	{
		Debug.Log ("Enter TurnEnemyState");
		if (Enemies.Count > 0) {
			GM.DM.SetMapActive (true);
			GM.DM.SetPlayerActive (true);
			GM.DM.SetNarrationActive (false);
			GM.DM.SetTurnActive (true);
			GM.DM.SetRandomSliderDisplayActive (false);
		}
	}
	
	public override void Do()
	{
		
	}

	public override void DoBeforeLeaving()
	{
	}
	
	public override bool IsDone()
	{
		return Enemies.Count == 0; // TODO
	}

}

public class IsDoneTransition : FSMTransition
{
	public IsDoneTransition (FSMState toState, int priority = 5) : base("IsDone", toState, priority)
	{
	}

	public override bool Check (){
		return FromState.IsDone ();
	}
}

public class NewTurnTransition : FSMTransition
{
	public TurnManager TM;
	public NewTurnTransition (TurnManager tm, FSMState toState, int priority = 5) : base("NewTurn", toState, priority)
	{
		TM = tm;
	}
	
	public override bool Check (){
		return TM.NewTurn;
	}
}


