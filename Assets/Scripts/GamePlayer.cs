using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayer : MonoBehaviour
{
	private GameManager GM;

	public string Name;
	public int Craftiness;
	public int MaxLiveliness;

	public int Liveliness;
	public int Damage = 0;
	public int Range = 0;
	private int HealRest = 0;

	private int DigUpCount = 0;

	public List<object> Skills = new List<object> ();
	
	public List<object> Inventory = new List<object> ();

	public BoardSquare CurrentSquare;
	public List<BoardSquare> Reach;
	public List<BoardSquare> Path;

	public List<BoardSquare> FightReach;
	public List<BoardSquare> TargetableSquares = new List<BoardSquare>();
	public List<GameEnemy> TargetableEnemies = new List<GameEnemy>();
	public GameEnemy TargetEnemy;

	public void Awake()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager> ();
	}
	
	public void Refresh()
	{

		Damage -= HealRest;
		Damage = (Damage < 0) ? 0 : Damage;
		HealRest = 0;
		Liveliness = MaxLiveliness - Damage;
		DigUpCount = 0;

		if (!IsActive ()) {
			GM.DoNext();
		}
	}

	public bool IsActive(){
		return Liveliness > 0;
	}

	public int GetUsedLiveliness()
	{
		return MaxLiveliness - Damage - Liveliness;
	}

	public void GetActions(out List<string> ls) // List<GameCharacterAction>
	{
		ls = new List<string> ();

		if(!(Liveliness < DigUpCost()))
		{
			ls.Add("Fouille");
		}

		if (Liveliness > 0) {
			ls.Add ("Déplacement");
		}


		if (Liveliness > 2){
			FightReach = GM.GameBoard.Reach(CurrentSquare, Range);
			Debug.Log(CurrentSquare);
			Debug.Log(Range);
			Debug.Log(FightReach);
			Debug.Log(FightReach.Count);

			TargetableSquares.Clear();
			TargetableEnemies.Clear();
			for(int i = 0; i < FightReach.Count; ++i)
			{
				if(FightReach[i].IsThreatened(this)){
					TargetableSquares.Add (FightReach[i]);
					TargetableEnemies.AddRange(FightReach[i].ThreatList(this));
				}
			}
			if(TargetableSquares.Count > 0)
			{ 
				ls.Add ("Combat");
			}
		}

		// TODO, not ready yet
		if (HasActivableSkill()) {
			ls.Add ("Capacité");
		}

		// TODO, not ready yet
		ls.Add ("Inventaire");

		// Always available
		ls.Add ("Repos");

	}
	
	public void GetActiveSkills(){
		// search skill
	}
	
	public int DigUpCost()
	{
		return 1 + DigUpCount;
	}
	public void	DigUp(object modifier)
	{
		GamePlayer p = GM.EM.Scenario.GetCurrentPlayer ();
		Resource rsr = p.CurrentSquare.digUpResource ();
		Debug.Log ("Dug up : " + rsr.Name);
		// TODO launch animation


		p.Inventory.Add(rsr);

		Liveliness -= DigUpCost();
		++DigUpCount;
	}

	public void Move(){
		// UI

		// TODO Resolve Path
		Liveliness -= (Path.Count-1);
		Debug.Log(Liveliness);
		DigUpCount = 0; // Only if new square  is different
		MoveTo (GM.GameBoard.SelectedSquare);

	}
	public void MoveTo(BoardSquare s){
		if (s != null) {
			if(CurrentSquare != null){
				CurrentSquare.Players.Remove(this);
			}
			CurrentSquare = s;
			s.Players.Add (this);
		} else {
			
		}
	}
		
	public bool CanFight(){
		return true;
	}
	public void Fight(){
		Liveliness -= 2;
		//TODO
		// ...
		//
	}

	public void Hurt(){
		Debug.Log (Name + " is hurt");

		if (GetUsedLiveliness () == 0) {
			--Liveliness;
		}
		++Damage;
	}

	public void Die(){
		CurrentSquare.Players.Remove (this);
	}
	public void Heal(int life){
		//TODO Respawn on this square

	}


	//TODO
	public bool HasActivableSkill(){
		return false;
	}
	public void Skill() // TODO Skills?
	{
		Liveliness -= 2;
	}

	public void LookInventory()
	{

	}

	public void Rest()
	{
		HealRest = Liveliness / 2;
	}
	
	
	public static void Spawn(string name)
	{

	}
}