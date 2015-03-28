using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using NSBoard;
using NSBoardGameItem;
using NSBoardSquare;
using NSFSM;
using NSGameNarrator;

using System.IO.Ports;


public class GameManager : MonoBehaviour
{
	public Board gameBoard;

	public GameNarrator Narrator;
	public TurnManager TM;

	public EventManager EM;
	public DisplayManager DM;

	
	//private SerialPort SP;
	private string s="";

	void Start ()
	{
		TM = new TurnManager (this);
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


	/*void OnGUI() {
		//GUILayout.Label("Press Enter To Start Game");
		if (Event.current.Equals (Event.KeyboardEvent ("KeyPad1")))
			s += "1";
		else if (Event.current.Equals (Event.KeyboardEvent ("Alpha1")))
			s += "1";
		else if (Event.current.Equals (Event.KeyboardEvent ("KeyPad0")))
			s += "0";
		else if (Event.current.Equals (Event.KeyboardEvent ("Alpha0")))
			s += "0";
		else if (Event.current.Equals (Event.KeyboardEvent ("return"))) {
			Debug.Log (s);
			Text t = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<Text>();
			t.text = s;
			// TODO is s a caseCode?

			s = "";

		}
		Debug.Log("Detected key code: " + Event.current.keyCode);
		
	}*/
}

public class GameEvent
{
	
}

public class TimeEvent : GameEvent
{
	
}

public class PlayerEvent : GameEvent
{
	
}

public class EventManager
{
	public List<GameEvent> events;

	public EventManager ()
	{
		
	}
	
	public void update ()
	{
		
	}
}

[System.Serializable]
public class DisplayManager
{
	public Canvas DisplayCanvas;
    public Canvas CanvasUI;

	/*DisplayManager ()
	{
	
	}*/

	void update ()
	{
		
	}
}

//[System.Serializable]
public class TurnManager : FSM
{
	public bool Started = false;

	public int NbTurns;
	public bool NewTurn;

	public List<GamePlayer> Players;
	//public int CurrentPlayerIndex;
	public GameManager GM;

	public TurnManager (GameManager gm) : base("TurnManager")
	{
		NbTurns = 0;
		NewTurn = false;
		//Players = new List<Player> ();
		//CurrentPlayerIndex = 0;
		GM = gm;

		//Players.Add (new Player ());
		Debug.Log ("Nb players = " + GamePlayer.Players.Count.ToString ());
		Players = GamePlayer.Players; // static list from class GamePlayer

		for (int i = 1; i < GamePlayer.Players.Count; ++i) {
			//Texture2D t = Resources.Load("Eddy") as Texture2D;
			//t.LoadImage(System.Convert.FromBase64String("Eddy.png"));
			GameObject pts = GameObject.Find("PlayerTurnSprite");
			GameObject s = GameObject.Instantiate (pts, new Vector3(-335 + 50 * i, 180, 0), Quaternion.Euler(0, 0, 0)) as GameObject;// as Sprite;
			s.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Eddy");

			s.transform.SetParent(pts.transform.parent.transform, false);
			//Sprite.Create(t, new Rect(-285 - 50 * i, 130, 100,100), new Vector2(-335 - 50 * i, 180), 100);
		}


		//Create all states and sub-FSM
		NextTurnState nts = new NextTurnState (this);
		PlayerTurnFSM playerturn = new PlayerTurnFSM (this);
		// TODO eventhandlerstate

		// Add states and sub-FSM
		AddState (nts);
		AddState (playerturn);

		// Create all transitions
		IsDoneTransition idt2playerturn = new IsDoneTransition (playerturn);
		NewTurnTransition idt2nts = new NewTurnTransition (this, nts, 1);
		IsDoneTransition idt2playerturnself = new IsDoneTransition (playerturn);

		// Add transitions to states
		nts.AddTransition(idt2playerturn);
		playerturn.AddTransition(idt2nts);
		playerturn.AddTransition (idt2playerturnself); 
	}

	public void Start(){
		Started = true;
		DoBeforeEntering ();
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
	public TurnManager TM;

	public PlayerTurnFSM(TurnManager tm) : base("Turn")
	{
		TM = tm;

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

	//private Defilement defilement;
	private Animator TurnTextAnimation;

	public NextTurnState(TurnManager tm) : base("NextTurn")
	{
		TM = tm;
		Done = false;

		TurnTextAnimation = GameObject.Find("TurnText").GetComponent<Animator>();
		//TurnTextAnimation.Play (0);
		//defilement.animation.Play ();
	}

	public override void DoBeforeEntering ()
	{
		Debug.Log ("Enter NT");
		TurnTextAnimation.SetTrigger ("NextTurn");

		//defilement.animation.Rewind ();
		++TM.NbTurns;
		TM.NewTurn = false;

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
	public TurnManager TM;

	public GameObject PlayerCursorAnchor;
	private Animator PlayerCursorSpriteAnimation;
	private int ToRightCount = 0;
	private float DeltaPosition;
	private Vector3 CursorPosition;

	public PlayerBeginTurnState(TurnManager tm) : base("PlayerBeingTurn")
	{
		TM = tm;

		PlayerCursorAnchor = GameObject.Find ("PlayerCursorAnchor");
		PlayerCursorSpriteAnimation = PlayerCursorAnchor.GetComponent<Animator> ();

		DeltaPosition = 50f;
	}

	public override void DoBeforeEntering ()
	{
		Debug.Log ("Enter PBT");

		GamePlayer.CurrentPlayer = GamePlayer.Players [GamePlayer.CurrentPlayerIndex];
		Debug.Log (GamePlayer.CurrentPlayer.GNO.DisplayedName);// = GamePlayer.Players [GamePlayer.CurrentPlayerIndex];

		/*
		if (GamePlayer.CurrentPlayerIndex > ToRightCount) {
			PlayerCursorSpriteAnimation.SetTrigger ("ToRight");
			ToRightCount++;
		} else {
			if(ToRightCount > GamePlayer.CurrentPlayerIndex){
				PlayerCursorSpriteAnimation.SetTrigger ("ToLeft");
				ToRightCount--;
			}
		}*/

		Done = false;
	}

	public override void Do()
	{
		Debug.Log ("PlayerBeginTurn");
		
		Debug.Log ("Player #" + GamePlayer.CurrentPlayerIndex);//.Current.ToString ());
		
		if (PlayerCursorSpriteAnimation.GetCurrentAnimatorStateInfo (0).IsName ("CursorSpriteIdle")){
			if (GamePlayer.CurrentPlayerIndex == ToRightCount) {
				SolidifyPosition();
				Done = true;
				return;
			}else if(ToRightCount < GamePlayer.CurrentPlayerIndex){
				ToRightCount++;

				PlayerCursorSpriteAnimation.SetTrigger ("ToRight");
			}else if(ToRightCount > GamePlayer.CurrentPlayerIndex){
				ToRightCount--;

				PlayerCursorSpriteAnimation.SetTrigger ("ToLeft");
			}
			SolidifyPosition();
		}
	}

	public void SolidifyPosition()
	{
		CursorPosition = PlayerCursorAnchor.transform.position;
		CursorPosition.x = ToRightCount * DeltaPosition;// * 0.02197802f;
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

	private List<string> ActionList;

	public PlayerTurnState(TurnManager tm) : base("PlayerTurn")
	{
		TM = tm; // TODO ?
		PlayerCursorSpriteAnimation = GameObject.Find ("PlayerCursorAnchor").GetComponent<Animator> ();
	}

	public override void DoBeforeEntering()	
	{
		PlayerCursorSpriteAnimation.SetBool("PlayerTurn", true);

		//
		ActionList = new List<string> ();
		ActionList.Add ("EndTurn");


		UIGenerator.UpdateActionButtonsFromList(GameObject.Find ("ActionPrompter"), ActionList);
	}

	public override void Do()
	{
		Debug.Log ("PlayerTurn");


		Done = true; // TODO
	}
	public override void DoBeforeLeaving ()
	{
		PlayerCursorSpriteAnimation.SetBool("PlayerTurn", false);
		Done = false;
	}

	public override bool IsDone ()
	{
		return ActionList.Count == 0;//Done;
	}
}

public class PlayerEndTurnState : FSMState
{
	public bool Done;
	public TurnManager TM;

	public PlayerEndTurnState(TurnManager tm) : base("PlayerEndTurn")
	{
		TM = tm;
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

		if (!(++GamePlayer.CurrentPlayerIndex < TM.Players.Count))
		{
			GamePlayer.CurrentPlayerIndex = 0;
			TM.NewTurn = true;
		}
	}

	public override void DoBeforeLeaving()//Entering()
	{
		Debug.Log("Leaving PET");

		Done = false;
	}

	public override bool IsDone ()
	{
		return Done;
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


