using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;

//using System.Xml; 
//using System.Xml.Serialization;
using System.IO;

using NSBoardGameItem;
using NSBoardSquare;

namespace NSBoard
{
//[XmlRoot("Board")]
	public class Board : MonoBehaviour, IEventSystemHandler, ILoad, ISave
	{
		public BoardGameInterpreter		interpreter;
		public string 					scenarioFileName;
		public Scenario					scenario;

		public Character[]				characters;
		public Card[] 					deck;
		public BoardSquare[] 			squares;

		protected delegate void TimeEvent();
		protected event TimeEvent timeEvent;



		// Use this for initialization
		void Start ()
		{
			scenario = new Scenario ();
			scenario.loadFromFile (scenarioFileName);
			scenario.start ();

			//loadFromFile ("testWriteSave.rbt");
			//save ("testWriteSave.rbt");

			/*_squares = new BoardSquare[36];
			for (int i = 0; i < 36; ++i) {
				_squares [i] = gameObject.AddComponent<BoardSquare> ();
				_squares [i].Start ();
				_squares [i].SetId(i + 1);
			}

			float s = 1.5f;

			//1
			_squares [0].setBorder (new Vector2[]{
				new Vector2 (52*s, 8*s),
				new Vector2 (64*s, 0*s),
				new Vector2 (68*s, 4*s),
				new Vector2 (60*s, 12*s),
				new Vector2 (52*s, 12*s)
			});
			//2
			_squares [1].setBorder (new Vector2[]{
				new Vector2 (60*s, 12*s),
				new Vector2 (72*s, 0*s),
				new Vector2 (76*s, 4*s),
				new Vector2 (68*s, 12*s)
			});
			//3
			_squares [2].setBorder (new Vector2[]{
				new Vector2 (68*s, 12*s),
				new Vector2 (72*s, 8*s),
				new Vector2 (76*s, 12*s),
				new Vector2 (76*s, 20*s)
			});
			//4
			_squares [3].setBorder(new Vector2[]{
				new Vector2(68*s,12*s),
				new Vector2(72*s,16*s),
				new Vector2(72*s,34*s),
				new Vector2(68*s,32*s)
			});
			//5
			_squares [4].setBorder(new Vector2[]{
				new Vector2(72*s,16*s),
				new Vector2(80*s,24*s),
				new Vector2(76*s,36*s),
				new Vector2(72*s,34*s)
			});
			//6
			_squares [5].setBorder(new Vector2[]{
				new Vector2(68*s,32*s),
				new Vector2(76*s,36*s),
				new Vector2(72*s,48*s),
				new Vector2(60*s,44*s),
				new Vector2(68*s,44*s)
			});
			//7
			_squares [6].setBorder(new Vector2[]{
				new Vector2(76*s,36*s),
				new Vector2(80*s,40*s),
				new Vector2(76*s,48*s),
				new Vector2(72*s,48*s)
			});
			//8
			_squares [7].setBorder(new Vector2[]{
				new Vector2(48*s,44*s),
				new Vector2(60*s,44*s),
				new Vector2(72*s,48*s),
				new Vector2(48*s,48*s)
			});
			//9
			_squares [8].setBorder(new Vector2[]{
				new Vector2(40*s,44*s),
				new Vector2(48*s,44*s),
				new Vector2(48*s,48*s),
				new Vector2(28*s,48*s)
			});
			//10
			_squares [9].setBorder(new Vector2[]{
				new Vector2(28*s,48*s),
				new Vector2(72*s,48*s),
				new Vector2(64*s,52*s),
				new Vector2(32*s,52*s)
			});
			//11
			_squares [10].setBorder(new Vector2[]{
				new Vector2(28*s,40*s),
				new Vector2(28*s,44*s),
				new Vector2(40*s,44*s),
				new Vector2(28*s,48*s),
				new Vector2(20*s,44*s)
			});
			//12
			_squares [11].setBorder(new Vector2[]{
				new Vector2(18.667f*s,36*s),
				new Vector2(28*s,36*s),
				new Vector2(28*s,40*s),
				new Vector2(20*s,44*s),
				new Vector2(20*s,40*s)
			});
			//13
			_squares [12].setBorder(new Vector2[]{
				new Vector2(16*s,28*s),
				new Vector2(28*s,28*s),
				new Vector2(28*s,36*s),
				new Vector2(18.667f*s,36*s)
			});
			//14
			_squares [13].setBorder(new Vector2[]{
				new Vector2(12*s,32*s),
				new Vector2(16*s,28*s),
				new Vector2(20*s,40*s),
				new Vector2(16*s,40*s)
			});
			//15
			_squares [14].setBorder(new Vector2[]{
				new Vector2(12*s,32*s),
				new Vector2(16*s,40*s),
				new Vector2(12*s,48*s)
			});
			//16
			_squares [15].setBorder(new Vector2[]{
				new Vector2(12*s,32*s),
				new Vector2(12*s,48*s),
				new Vector2(4*s,40*s)
			});
			//17
			_squares [16].setBorder(new Vector2[]{
				new Vector2(4*s,40*s),
				new Vector2(12*s,48*s),
				new Vector2(4*s,48*s),
				new Vector2(0*s,44*s)
			});
			//18
			_squares [17].setBorder(new Vector2[]{
				new Vector2(0*s,36*s),
				new Vector2(8*s,28*s),
				new Vector2(12*s,32*s),
				new Vector2(0*s,44*s)
			});
			//19
			_squares [18].setBorder(new Vector2[]{
				new Vector2(0*s,28*s),
				new Vector2(4*s,24*s),
				new Vector2(8*s,28*s),
				new Vector2(0*s,36*s)
			});
			//20
			_squares [19].setBorder(new Vector2[]{
				new Vector2(20*s,28*s),
				new Vector2(24*s,22.9845f*s),
				new Vector2(28*s,24*s),
				new Vector2(28*s,28*s)
			});
			//21
			_squares [20].setBorder(new Vector2[]{
				new Vector2(8*s,20*s),
				new Vector2(12*s,20*s),
				new Vector2(24*s,22.9845f*s),
				new Vector2(20*s,28*s),
				new Vector2(16*s,28*s),
				new Vector2(8*s,24*s)
			});
			//22
			_squares [21].setBorder(new Vector2[]{
				new Vector2(8*s,16*s),
				new Vector2(24*s,8*s),
				new Vector2(12*s,20*s),
				new Vector2(8*s,20*s)
			});
			//23
			_squares [22].setBorder(new Vector2[]{
				new Vector2(12*s,20*s),
				new Vector2(20*s,12*s),
				new Vector2(24*s,16*s),
				new Vector2(20*s,22*s)
			});
			//24
			_squares [23].setBorder(new Vector2[]{
				new Vector2(20*s,22*s),
				new Vector2(24*s,16*s),
				new Vector2(28*s,20*s),
				new Vector2(28*s,24*s)
			});
			//25
			_squares [24].setBorder(new Vector2[]{
				new Vector2(20*s,12*s),
				new Vector2(24*s,8*s),
				new Vector2(28*s,12*s),
				new Vector2(28*s,20*s)
			});
			//26
			_squares [25].setBorder(new Vector2[]{
				new Vector2(24*s,8*s),
				new Vector2(36*s,8*s),
				new Vector2(36*s,12*s),
				new Vector2(28*s,12*s)
			});
			//27
			_squares [26].setBorder(new Vector2[]{
				new Vector2(36*s,8*s),
				new Vector2(44*s,8*s),
				new Vector2(44*s,12*s),
				new Vector2(36*s,12*s)
			});
			//28
			_squares [27].setBorder(new Vector2[]{
				new Vector2(44*s,8*s),
				new Vector2(52*s,8*s),
				new Vector2(52*s,12*s),
				new Vector2(44*s,12*s)
			});
			//29
			_squares [28].setBorder(new Vector2[]{
				new Vector2(48*s,12*s),
				new Vector2(68*s,12*s),
				new Vector2(64*s,16*s),
				new Vector2(48*s,16*s)
			});
			//30
			_squares [29].setBorder(new Vector2[]{
				new Vector2(64*s,16*s),
				new Vector2(68*s,12*s),
				new Vector2(68*s,28*s),
				new Vector2(64*s,28*s)
			});
			//31
			_squares [30].setBorder(new Vector2[]{
				new Vector2(64*s,28*s),
				new Vector2(68*s,28*s),
				new Vector2(68*s,44*s),
				new Vector2(64*s,40*s)
			});
			//32
			_squares [31].setBorder(new Vector2[]{
				new Vector2(48*s,40*s),
				new Vector2(64*s,40*s),
				new Vector2(68*s,44*s),
				new Vector2(48*s,44*s)
			});
			//33
			_squares [32].setBorder(new Vector2[]{
				new Vector2(32*s,40*s),
				new Vector2(48*s,40*s),
				new Vector2(48*s,44*s),
				new Vector2(28*s,44*s)
			});
			//34
			_squares [33].setBorder(new Vector2[]{
				new Vector2(28*s,28*s),
				new Vector2(32*s,28*s),
				new Vector2(32*s,40*s),
				new Vector2(28*s,44*s)
			});
			//35
			_squares [34].setBorder(new Vector2[]{
				new Vector2(28*s,12*s),
				new Vector2(32*s,16*s),
				new Vector2(32*s,28*s),
				new Vector2(28*s,28*s)
			});
			//36
			_squares [35].setBorder(new Vector2[]{
				new Vector2(28*s,12*s),
				new Vector2(48*s,12*s),
				new Vector2(48*s,16*s),
				new Vector2(32*s,16*s)
			});
			*/

		}
	
		// Update is called once per frame
		void Update ()
		{
			scenario.update ();
					
			//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);

			//RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);

			//Vector3 worldTouch = Input.mousePosition;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//RaycastHit2D hit = Physics2D.Raycast (new Vector2(worldTouch.x,worldTouch.y),Vector2.zero, Mathf.Infinity);
			//Vector2 dos = new Vector2 (worldTouch.x, worldTouch.y);

			//RaycastHit hit;
			//RaycastHit2D hit2;
			//bool isHit;// = _squares[10].getBorder().collider2D.OverlapPoint(dos);//.collider.Raycast(ray, out hit, Mathf.Infinity);

			/*
			//for (int i = 0; i < 36; ++i) {
			//	if (_squares[i].getBorder().collider2D.OverlapPoint(dos)) {//(hit.collider != null) {
				if (hit.collider != null) {
					//Debug.Log (GameObject.Find (hit.collider.gameObject.name));
					Debug.Log (hit.collider.gameObject.name);
					//Debug.Log (hit.collider.GetComponent<BoardSquare>().Id);
					//Debug.Log ("touché #" + i.ToString());
					//Debug.DrawLine(ray.origin, ray.direction, Color.red);
					//return;
				}
			//}*/
			/*GraphicRaycaster grc = GetComponentInChildren<Canvas> ().GetComponent<GraphicRaycaster> ();
			PointerEventData eventData = new PointerEventData (GetComponentInChildren<Canvas> ().GetComponentInChildren<EventSystem> ());
			List<RaycastResult> hits = new List<RaycastResult> ();

			grc.Raycast(eventData, hits);
			*/
			/*if (hits.size() > 0) {
				Debug.Log ("touché!");
			}*/

		}

		public void interaction01 (string msg)
		{
			Debug.Log ("board message: " + msg);
			GetComponentInChildren<Canvas> ().enabled = !GetComponentInChildren<Canvas> ().enabled;
		}

		public bool						loadFromFile (string fileName)
		{
			// Fill this instance
			StreamReader r;
			FileInfo t = new FileInfo (Application.dataPath + "\\" + fileName);
			if (!t.Exists) { 
				Debug.Log ("File Does not exist."); 
				return false;
			} 

			string s;
			r = t.OpenText ();
			//r = t.OpenRead ();
			while (!r.EndOfStream) {
				s = r.ReadLine ();
				//Debug.Log (s);
			}

			r.Close ();

			return true;
		}

		public bool 					save (string fileName)
		{
			StreamWriter w;
			FileInfo t = new FileInfo (Application.dataPath + "\\" + fileName);
			if (!t.Exists) { 
				w = t.CreateText (); 
			} else { 
				t.Delete ();
				w = t.CreateText (); 
			} 
			w.Write ("hohooho_data\nwaza\nteset\n\tezrjkl\n\tjshdfkj\nend"); 
			w.Close (); 
			Debug.Log ("File written."); 

			return true;
		}
	}

	public class Character //: BoardGameItem
	{
		public string  					name;
		public Species 					species;

		public void Start ()
		{
			species = null;
		}
	
		public void Update ()
		{
		}

		/*public override bool			loadFromFile(string fileName)
	{
		// Fill this instance
		return true;
	}
	public override bool 			save(string fileName)
	{
		return true;
	}*/
	}

	public class Species
	{
		static string					name;
		static int						type;
	}

	public class Card //: BoardGameItem
	{
		string 							name;
		Property[] 						properties;
		/*public override bool 			loadFromFile(string fileName)
	{
		// Fill this instance
		return true;
	}
	public override bool 			save(string fileName)
	{
		return true;
	}*/
	}

	//[System.Serializable]
	public class Player : Character
	{
		BoardGameItem[] 				inventory;
		Card[]					 		hand;
		BoardSquare						square;

		/*public override bool			loadFromFile(string fileName)
	{
		// Fill this instance
		return true;
	}
	public override bool			save(string fileName)
	{
		return true;
	}*/
	}

	[System.Serializable]
	public class ResourceFindingRate
	{
		public Resource resource;
		public float rate;
	}

	[System.Serializable]
	public class Resource //: BoardGameItem
	{
		public string test;
		/*public override bool			loadFromFile(string fileName)
	{
		// Fill this instance
		return true;
	}
	public override bool 			save(string fileName)
	{
		return true;
	}*/
	}

	public class ScrapMetal : Resource // Ferraille
	{

	}

	public class Food : Resource // Bouffe
	{

	}
		
	public class Glass : Resource // Verre
	{

	}

	public class Plastic : Resource // Plastique
	{

	}

	public class Scenario : ILoad, ISave//BoardGameItem
	{
		public void start ()
		{
		
		}
	
		public void update ()
		{
		
		}

		public /*override*/ bool			loadFromFile (string fileName)
		{
			// Fill this instance
			return true;
		}

		public /*override*/ bool 			save (string fileName)
		{
			return true;
		}
	}

	public class BoardGameInterpreter
	{

	}

}