using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region "Narration"
public class GameScenario
{
	public static List<GameScenario> Scenarios = new List<GameScenario>();
	public static GameScenario GetScenarioById(int id)
	{
		for(int i = 0; i < Scenarios.Count; ++i){
			if(Scenarios[i].Id == id)
			{
				return Scenarios[i];
			}
		}
		return null;
	}
	public static GameScenario Init (DisplayManager dm){
		// Scenario container
		GameScenario scenario = new GameScenario (1, "La Grande Evasion");

		// Scenario players
		GamePlayer tirette = scenario.InstantiatePlayer ("Tirette");
		GamePlayer niles = scenario.InstantiatePlayer ("Niles");
		GamePlayer piquette = scenario.InstantiatePlayer ("Piquette");
		GamePlayer volotom = scenario.InstantiatePlayer ("Volotom");

		//// Act 1 container
		GameAct ga = scenario.CreateAct (1, "La Grande Evasion");

		////// Scene container
		GameScene intro = ga.CreateScene (0, "Introduction");

		//////// Events
		string[] o = {"Jazz", "Bienvenue dans le monde impitoyable..."};
		intro.CreateEvent (dm.Narration, o.Clone ());
		o [0] = "Jazz";
		o [1] = "Lalala";
		intro.CreateEvent (dm.Narration, o.Clone ());
		o [0] = "Fidel";
		o [1] = "Nyahahahahanyanyananananyanyan";
		intro.CreateEvent (dm.Narration, o.Clone ());

		GameScene gs1 = ga.CreateScene (1, "");

		////// Scene container
		GameScene gs2 = ga.CreateScene (2, "");
		o [0] = "Fidel";
		o [1] = "CHACHACHA";
		gs2.CreateEvent (dm.Narration, o.Clone ());

		////// Scene container
		GameScene gs3 = ga.CreateScene (3, "");
		o [0] = "Eddy";
		o [1] = "Bonjour";
		gs3.CreateEvent (dm.Narration, o.Clone ());
		o [0] = "Fidel";
		o [1] = "Nope";
		gs3.CreateEvent (dm.Narration, o.Clone ());
		o [0] = "Jazz";
		o [1] = "Jamming with ya, yeaaaaaaaahhhhhh!";
		gs3.CreateEvent (dm.Narration, o.Clone ());


		////// Scene container
		GameScene gs4 = ga.CreateScene (4, "");

		////// Scene container
		GameScene gs5 = ga.CreateScene (5, "");

		////// Scene container
		GameScene gs6 = ga.CreateScene (6, "");

		////// Scene container
		GameScene gs7 = ga.CreateScene (7, "");

		////// Scene container
		GameScene gs8 = ga.CreateScene (8, "");

		////// Scene container
		GameScene gs9 = ga.CreateScene (9, "");

		////// Scene container
		GameScene gs10 = ga.CreateScene (10, "");

		////// Scene container
		GameScene gs11 = ga.CreateScene (11, "GameOver");

		//// Act 2 container
		GameAct ga2 = scenario.CreateAct (2, "En cavale");
		////// Scene container
		GameScene gs10_2 = ga2.CreateScene (10, "");

		//// Act 3 container
		GameAct ga3 = scenario.CreateAct (3, "Ad vitam eternam");
		////// Scene container
		GameScene gs100_3 = ga3.CreateScene (100, "");


		return scenario;
	}


	public int Id;
	public string Name;

	public List<GamePlayer> Players = new List<GamePlayer>();
	public int CurrentPlayer = 0;
	public GamePlayer GetCurrentPlayer()
	{
		return Players[CurrentPlayer];
	}
	public void SetCurrentPlayer(int cp)
	{
		CurrentPlayer = cp;
	}

	public List<GameAct> Acts = new List<GameAct> ();
	public int CurrentAct;

	public GameScenario(int id, string name)
	{
		Id = id;
		Name = name;
		Acts = new List<GameAct> ();

		// Insertion sort
		if (Scenarios.Count == 0) {
			Scenarios.Add(this);
			CurrentAct = 0;
		} else {
			for(int i = 0; i < Scenarios.Count; ++i)
			{
				if (id < Scenarios[i].Id)
				{
					Scenarios.Add (this);
					return;
				}else if (id == Scenarios[i].Id)
				{
					Debug.LogError("Scenario #" + id.ToString() + " already exists");
					return;
				}
			}
			Scenarios.Add (this);
		}
	}

	public GamePlayer InstantiatePlayer (string name) {
		GameObject pts = GameObject.Find("PlayerTurnSpriteAnchor");
		GameObject playerGo = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Players/" + name)) as GameObject;

		Vector3 pos = playerGo.transform.localPosition;
		pos.x += 150 * Players.Count;
		playerGo.transform.localPosition = pos;
		playerGo.transform.SetParent(pts.transform, false);

		GamePlayer player = playerGo.GetComponent<GamePlayer> ();

		Players.Add (player);

		return player;
	}

	public GameAct CreateAct (int id, string name) {
		GameAct ga = new GameAct (id, name, this);
		Acts.Add (ga);
		
		return ga;
	}

	public GameAct GetCurrentAct()
	{
		return Acts[CurrentAct];
	}

	public GameScene GetCurrentScene()
	{
		GameAct a = GetCurrentAct(); // TODO Id
		return a.Scenes[a.CurrentScene];
	}

	public GameScene GetSceneById(int id)
	{
		GameAct a = GetCurrentAct(); // TODO Id
		for (int i = 0; i < a.Scenes.Count; ++i) {
			if (a.Scenes[i].Id == id){
				return a.Scenes[i];
			} else if(a.Scenes[i].Id > id)
			{
				return null;
			}
		}
		return null;
	}

	public List<GameEvent> FetchEvents()
	{
		GameScene s = GetSceneById (GetCurrentAct ().CurrentScene);
		if (s != null) {
			return s.Events;
		} else {
			return new List<GameEvent> ();
		}
	}
}

public class GameAct
{
	public int Id;
	public string Name;
	public GameScenario Parent;
	public List<GameScene> Scenes;
	public int CurrentScene;

	public GameAct(int id, string name, GameScenario parent)
	{
		Id = id;
		Name = name;
		Parent = parent;
		Scenes = new List<GameScene>();

		// Insertion sort
		if (Parent.Acts.Count == 0) {
			Parent.Acts.Add(this);
			Play(0);
		} else {
			for(int i = 0; i < Parent.Acts.Count; ++i)
			{
				if (Id < Parent.Acts[i].Id)
				{
					Parent.Acts.Add (this);
					return;
				}else if (Id == Parent.Acts[i].Id)
				{
					Debug.LogError("Act " + Id.ToString() + " already exists");
					return;
				}
			}
			Parent.Acts.Add (this);
		}
	}

	public void Play(int i)
	{
		Parent.CurrentAct = i;
	}

	public GameScene CreateScene (int id, string name) {
		GameScene gs = new GameScene (id, name, this);
		//Scenes.Add (gs);
		
		return gs;
	}
}

public class GameScene
{
	public static GameScene CurrentScene;

	public int Id;
	public string Name;
	public GameAct Parent;
	public List<GameEvent> Events; // TurnEvents
	//public int CurrentEvent = 0;

	public GameScene(int id, string name, GameAct parent)
	{
		Id = id;
		Name = name;
		Parent = parent;
		Events = new List<GameEvent>();

		// Insertion sort
		if (Parent.Scenes.Count == 0) {
			Parent.Scenes.Add(this);
			Play(0);
		} else {
			for(int i = 0; i < Parent.Scenes.Count; ++i)
			{
				if (Id < Parent.Scenes[i].Id)
				{
					Parent.Scenes.Add (this);
					return;
				}else if (Id == Parent.Scenes[i].Id)
				{
					Debug.LogError("Scene " + Id.ToString() + " already exists");
					return;
				}
			}
			Parent.Scenes.Add (this);
		}
	}

	public void Play(int i)
	{
		Parent.CurrentScene = i;
	}

	public GameEvent CreateEvent (GameAction action, object args) {
		GameEvent ge = new GameEvent (action, args);
		Events.Add (ge);
		
		return ge;
	}
}

public class GameEvent
{
	public GameAction EventAction;
	public object EventArgument;

	public GameEvent(GameAction a, object o)
	{
		EventAction = a;
		EventArgument = o;
	}

	public void ExecuteEvent(){
		EventAction (EventArgument);
	}
}
# endregion

#region "Board"
public class GameBoard
{
	public GameObject BoardObject; // Reference to Board GameObject

	public GameBoard()
	{

	}
}

public class GameBoardSquare
{
	public GameObject BoardSquareObject; // Reference to BoardSquare GameObject

	public GameBoardSquare()
	{

	}
}
#endregion