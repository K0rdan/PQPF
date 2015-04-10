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

		//TODO do this with events in intro
		// Initial squares
		tirette.MoveTo(dm.GM.GameBoard.squares[0]);
		niles.MoveTo(dm.GM.GameBoard.squares[1]);
		piquette.MoveTo(dm.GM.GameBoard.squares[1]);
		volotom.MoveTo(dm.GM.GameBoard.squares[2]);
		//

		//// Act 1 container
		GameAct ga = scenario.CreateAct (1, "La Grande Evasion");

		////// Scene container
		GameScene intro = ga.CreateScene (0, "Introduction");

		//////// Events
        #region Scenario
        string[] o = { "Jazz", "Bienvenue dans le monde impitoyable de Pour Quelques Poignées de Ferraille !\nAu cours de cette première aventure Vous incarnerez un habitant d’une décharge abandonnée par l’homme, dont les animaux ont repris le contrôle et où ils vivent sous le joug féroce des chats, et de leur terrifiant chef, Fidel Chastro !" };
        intro.CreateEvent(dm.Narration, o.Clone());
        o[0] = "Fidel";
        o[1] = "Terrifiant oui ! Ici c’est moi le chef, et vous avez pas interêt à me chercher des noises !\nComme vous le voyiez, cette carte sous vos yeux représente notre bonne vieille décharge : du Terrier au Cimetière Mécanique, en passant par L’Anneau ou le lac d’Etoffe, moi et mes chats on contrôle tout ! Alors tenez vous à carreau !";
        intro.CreateEvent(dm.Narration, o.Clone());
        o[0] = "Jazz";
        o[1] = "“Pas commode n’est-ce pas ? Heureusement les choses sont sur le point de changer, mais pour ça il va d’abord falloir vivre pas mal d’aventures avec nous !\nVous êtes prêts ? Alors allons-y !";
        intro.CreateEvent(dm.Narration, o.Clone());
        o[0] = "Narrator";
        o[1] = "Afin de démarrer cette première aventure, saisissez vous chacun d’une fiche personnage, et de sa figurine associée, puis placez la sur une des cases de la Grande Muraille (1, 2 ou 3).\nPrenez le temps de découvrir votre fiche : vous y trouverez vos Caractéristiques, vos Capacités Spéciales, et votre Equipement de départ.\nL’Astuce représente votre capacité à vous tirer des situations dangereuses, et la Vivacité votre état physique.\nVous êtes actuellement emprisonnés pour un crime que vous n’avez pas commis, heureusement, une figure amie apparaît pour vous sortir du pétrin";
        intro.CreateEvent(dm.Narration, o.Clone());
        o[0] = "Narrator";
        o[1] = "LA GRANDE EVASION : Un scénario d’initiation pour Quelques Poignées de Ferraille.\n\nAlors que vous participiez chacun de votre côté à la Grande Braderie organisée par les Corbeaux, un stand particulier attire votre attention. Une taupe du nom de Favio Estocar y fait la promotion de ses Fioles d’Ether : une lampée suffira à vous rendre aussi fort qu’un Boeuf, promet-il en s’epoumonant auprès des passants.\nMais soudain, c’est l’esclandre : un rat débarque à toute berzingue, renversant l’étal de marchandises sous les yeux desespérés de Favio, poursuivi par une troupe de chat bien décidé à attraper le responsable de tout ce vacarme.\nA bout de force, le rat s’effondre sous vos yeux et vous interpelle d’une voix affaiblie : “Les chats préparent un coup terrible. Prévenez Eddy, dites-lui que j’ai échoué…”\n\nLes chats se saisissent alors du fauteur de trouble, et, accusés de complicité, vous êtes expédiés manu militari en Prison, malgré toutes vos tentatives de justification…";
        intro.CreateEvent(dm.Narration, o.Clone());
        o[0] = "Eddy";
        o[1] = "Pssst…\nHey vous 4, approchez vous !\nJe suis Eddy La Pipe, pas de temps à perdre dans de longues discussions, dites-vous juste qu’ici je suis votre seul ami !\nJ’ai un double des clés des cellules, et je connais un chemin pour sortir d’ici discrètement, mais une fois dehors je pourrais plus rien pour vous…\nPas de panique, un autre de mes amis pourra vous reccueillir… A condition que vous arriviez avec de quoi payer votre séjour, si vous voyez ce que je veux dire !";
        intro.CreateEvent(dm.Narration, o.Clone());
        o[0] = "Narrator";
        o[1] = "Vous êtes désormais libre et avez un but à atteindre en coopérant. Récoltez ensemble 2 ressources de chaque type disponible : 2 Bouffes, 2 Ferrailles, 2 Plastiques et 2 Verres. Vous pourrez obtenir ces dernières en fouillant les cases du plateau de jeu. Puis une fois ces ressources en votre possession, foncez en case °7 : si vous y parvenez avant le tour 10, c’est la victoire !";
        intro.CreateEvent(dm.Narration, o.Clone());

        #endregion

		GameScene gs1 = ga.CreateScene (1, "");

		////// Scene container
		GameScene gs2 = ga.CreateScene (2, "");
		o [0] = "Fidel";
		o [1] = "CHACHACHA";
		gs2.CreateEvent (dm.Narration, o.Clone ());
		object[] oo = {"Chat", dm.GM.GameBoard.squares[4-1]};
		gs2.CreateEvent (dm.Spawn, oo.Clone ());
		oo[0] = "Chat";
		oo[1] = dm.GM.GameBoard.squares[27-1];
		gs2.CreateEvent (dm.Spawn, oo.Clone ());

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
					Scenarios.Insert(i, this);
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
		player.DefaultSpritePosition = pos;

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
					Parent.Acts.Insert(i, this);
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
					Parent.Scenes.Insert(i, this);
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