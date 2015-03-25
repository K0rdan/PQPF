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

	/*
	void OnGUI() {
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
	public int NbTurns;
	public bool NewTurn;

	public List<GamePlayer> Players;
	public int CurrentPlayerIndex;
	public GameManager GM;

	public TurnManager (GameManager gm) : base("name")
	{
		NbTurns = 0;
		NewTurn = false;
		//Players = new List<Player> ();
		CurrentPlayerIndex = 0;
		GM = gm;

		//Players.Add (new Player ());
		Debug.Log ("Nb players = " + GamePlayer.Players.Count.ToString ());
		Players = GamePlayer.Players;

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

	public void update ()
	{
		Do ();
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

    private Defilement defilement;

	public NextTurnState(TurnManager tm) : base("NextTurn")
	{
		TM = tm;
		Done = false;

        defilement = GameObject.Find("Defilement_Text").AddComponent<Defilement>();
	}

	public override void DoBeforeEntering ()
	{
		Debug.Log ("Enter NT");
		Done = false;
	}
	
	public override void DoBeforeLeaving ()
	{
		Debug.Log ("Leave NT");	
        GameObject.Destroy(defilement);
	}
	
	public override void Do ()
	{
		++TM.NbTurns;
		// TODO Add stuff here
		Debug.Log("NextTurn");
		//Debug.Log(TM.NbTurns.ToString());
		//Done = true;
		//
	}
	
	public override bool IsDone ()
	{
		//return Done;
        return defilement.IsDone();
	}
}

public class PlayerBeginTurnState : FSMState
{
	public bool Done;
	public TurnManager TM;

	public PlayerBeginTurnState(TurnManager tm) : base("PlayerBeingTurn")
	{
		TM = tm;
	}

	public override void Do()
	{
		Debug.Log ("PlayerBeginTurn");


		Done = true;
	}

	public override void DoBeforeEntering ()
	{
		//base.DoBeforeLeaving ();
		//.AddComponent<Text>();
		//GM.DM.DisplayCanvas.AddComponent<Text> ();
		//GameObject o = GameObject.Instantiate(GameObject.Find("UI Talking Guy")) as GameObject;
		//o.SetActive (true);
		//TM.GM.DM.DisplayCanvas.AddComponent<Text>();
		//Text
		Debug.Log ("Enter PBT");
	}

	public override void DoBeforeLeaving ()
	{
		//base.DoBeforeLeaving ();
		Done = false;
		Debug.Log ("Leave PBT");
	}

	public override bool IsDone ()
	{
		return Done;
	}
}

public class PlayerTurnState : FSMState
{
	public bool Done;
	public TurnManager TM;

	public PlayerTurnState(TurnManager tm) : base("PlayerTurn")
	{
		TM = tm; // TODO ?
	}

	public override void Do()
	{
		Debug.Log ("PlayerTurn");

		Done = true; // TODO
	}
	public override void DoBeforeLeaving ()
	{
		//base.DoBeforeLeaving ();
		Done = false;
	}

	public override bool IsDone ()
	{
		return Done;
	}
}

public class PlayerEndTurnState : FSMState
{
	public bool Done;
	public bool PlayerSelected;
	public TurnManager TM;

	public PlayerEndTurnState(TurnManager tm) : base("PlayerEndTurn")
	{
		TM = tm;
	}

	public override void Do()
	{
		Debug.Log ("PlayerEndTurn");
		//Select next player
		if(!PlayerSelected)
		{
			/*
			Debug.Log(TM.ToString());
			Debug.Log(TM.CurrentPlayerIndex.ToString());
			Debug.Log(TM.Players.ToString());
			Debug.Log(TM.Players.Count.ToString());
			*/
			if (!(++TM.CurrentPlayerIndex < TM.Players.Count))
			{
				TM.CurrentPlayerIndex = 0;
				TM.NewTurn = true;
			}
			PlayerSelected = true;
		}

		Done = true;
	}


	public override void DoBeforeLeaving()//Entering()
	{
		TM.NewTurn = false;
		PlayerSelected = false;
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


