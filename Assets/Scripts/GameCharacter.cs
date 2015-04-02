using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public class GameCharacter : MonoBehaviour
{	
	public static List<GameCharacter> Characters = new List<GameCharacter>();
	//public static Dictionary<string, List<GameCharacter>> Tags = new Dictionary<string, List<GameCharacter>>();
	public static GameCharacter GetCharacter(string name)
	{
		return Characters.Find(gc => gc.Name == name);
	}
	public static GameObject GetCharacterObject(string name)
	{
		return GetCharacter (name).gameObject;
	}

	public GameObject GameBoard;
	public int CurrentBoardSquare;
	public GameObject GetCurrentBoardSquare()
	{
		Board b = GameBoard.GetComponent<Board> ();
		GameObject go = null;

		switch(CurrentBoardSquare)
		{
		case -1:
			// Player Layout

			break;
		case 0:
			// Speaker place

			break;

		default:
			go = b.squares [CurrentBoardSquare].gameObject;
			break;
		}

		return go;
	}
	public void SetOnBoardSquare(int i)
	{
		CurrentBoardSquare = i;
		GameObject go = GetCurrentBoardSquare ();



	}

	public string Name;
	public string Tag;


	/*public GameCharacter(string displayedName, string tag)
	{
		DisplayedName = displayedName;
		Tag = tag;
		Characters.Add (this);
	}*/
	
	public virtual void GetActions(out Dictionary<string, GameAction> al)
	{
		al = new Dictionary<string, GameAction> ();
		
	}
	
	public void Say(string speech)
	{
		//TODO
		//transform.parent.
		Debug.Log (Name + " says : " + speech);
	}
}