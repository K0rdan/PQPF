using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayer : MonoBehaviour
{
	private GameManager GM;

	public string Name;
	public int Craftiness;
	public int MaxLiveliness;

	private int Liveliness;
	private int Damage = 0;
	private int HealRest = 0;

	private int Range = 0;
	private int DigUpCount = 0;

	public object[] Skills;
	
	public object[] Inventory;
	
	/*public GamePlayer (string name, string displayedName, string tag) : base(displayedName, tag)
	{
		Name = name;
		
		Players.Add (this);
	}*/
	public void Awake()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager> ();
	}
	
	public void Refresh()
	{
		Damage -= HealRest;
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
			//if(this square is threaten
			// al.Add ("Fuite", null);
			//  if Liveliness > 1
			
			//else
			ls.Add ("Déplacement");
		}

		if (Liveliness > 2) {
			//if(this square is threaten
			// al.Add ("Fuite", null);
			//  if Liveliness > 1
			
			//else
			ls.Add ("Combat");
		}

		if (true) {
			ls.Add ("Capacité");
		}


		ls.Add ("Inventaire");
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
		Liveliness -= DigUpCost();
		// TODO

		++DigUpCount;
	}

	public void Move(){
		// UI
		
		DigUpCount = 0; // Only if new square  is different
		Liveliness--;
	}

	public bool CanFight(){
		return true;
	}
	public void Fight(){
		Liveliness -= 2;
	}

	public void Hurt(){
		++Damage;
	}

	public bool HasActivableSkill(){
		return true;
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