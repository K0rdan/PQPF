using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System;
using System.Collections;
using System.Collections.Generic;

//using System.Xml; 
//using System.Xml.Serialization;
using System.IO;


//[XmlRoot("Board")]
public class Board : MonoBehaviour, IEventSystemHandler
{
	public GameManager GM;
	public BoardSquare[] squares;
	public enum BoardPhase {
		PlayerMoving, 				// OK
		PlayerHasSelectedSquare, 	// OK
		PlayerSelectTarget,
		PlayerAttacking,
		EnemyMoving,
		EnemyAttacking,
		Unactive					// ?OK?
	};
	private BoardPhase phase = BoardPhase.Unactive;
	public bool PhaseHasChanged = false;
	public BoardPhase Phase {
		get{ return phase;}
		set{ PhaseHasChanged = true; phase = value;}
	}
	public BoardSquare SelectedSquare;
	public GameObject MoveButton;
	public GameObject CancelButton;
	public GameObject NextButton;
	public GameObject RandomSlider;

	public List<GameEnemy> Enemies = new List<GameEnemy>();

	// Use this for initialization
	void Start ()
	{

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

		// Link all squares to the Game Manager and this
		for (int i = 0; i < squares.Length; ++i) {
			squares[i].GM = GM;
			squares[i].GameBoard = this;
			squares[i].Id = i;

			squares[i].SetMaterial(squares[i].matExit, new Color(1,1,1,.1f));
		}

		//Make all connections
		// sqr1
		squares [0].Neighbours.Add (squares [1]);
		squares [1].Neighbours.Add (squares [0]);

		squares [0].Neighbours.Add (squares [26]);
		squares [26].Neighbours.Add (squares [0]);

		squares [0].Neighbours.Add (squares [27]);
		squares [27].Neighbours.Add (squares [0]);

		//sqr2
		squares [1].Neighbours.Add (squares [27]);
		squares [27].Neighbours.Add (squares [1]);

		squares [1].Neighbours.Add (squares [2]);
		squares [2].Neighbours.Add (squares [1]);

		//sqr3
		squares [2].Neighbours.Add (squares [3]);
		squares [3].Neighbours.Add (squares [2]);

		squares [2].Neighbours.Add (squares [4]);
		squares [4].Neighbours.Add (squares [2]);

		//sqr4
		squares [3].Neighbours.Add (squares [4]);
		squares [4].Neighbours.Add (squares [3]);

		squares [3].Neighbours.Add (squares [5]);
		squares [5].Neighbours.Add (squares [3]);

		squares [3].Neighbours.Add (squares [28]);
		squares [28].Neighbours.Add (squares [3]);

		squares [3].Neighbours.Add (squares [29]);
		squares [29].Neighbours.Add (squares [3]);

		//sqr5
		squares [4].Neighbours.Add (squares [5]);
		squares [5].Neighbours.Add (squares [4]);

		//sqr6
		squares [5].Neighbours.Add (squares [6]);
		squares [6].Neighbours.Add (squares [5]);

		squares [5].Neighbours.Add (squares [7]);
		squares [7].Neighbours.Add (squares [5]);

		squares [5].Neighbours.Add (squares [29]);
		squares [29].Neighbours.Add (squares [5]);

		squares [5].Neighbours.Add (squares [30]);
		squares [30].Neighbours.Add (squares [5]);

		//sqr7

		//sqr8
		squares [7].Neighbours.Add (squares [8]);
		squares [8].Neighbours.Add (squares [7]);

		squares [7].Neighbours.Add (squares [9]);
		squares [9].Neighbours.Add (squares [7]);

		squares [7].Neighbours.Add (squares [30]);
		squares [30].Neighbours.Add (squares [7]);

		//sqr9
		squares [8].Neighbours.Add (squares [9]);
		squares [9].Neighbours.Add (squares [8]);

		squares [8].Neighbours.Add (squares [10]);
		squares [10].Neighbours.Add (squares [8]);

		squares [8].Neighbours.Add (squares [31]);
		squares [31].Neighbours.Add (squares [8]);

		//sqr10

		//sqr11
		squares [10].Neighbours.Add (squares [11]);
		squares [11].Neighbours.Add (squares [10]);

		squares [10].Neighbours.Add (squares [31]);
		squares [31].Neighbours.Add (squares [10]);

		squares [10].Neighbours.Add (squares [32]);
		squares [32].Neighbours.Add (squares [10]);

		//sqr12
		squares [11].Neighbours.Add (squares [12]);
		squares [12].Neighbours.Add (squares [11]);

		squares [11].Neighbours.Add (squares [17]);
		squares [17].Neighbours.Add (squares [11]);

		squares [11].Neighbours.Add (squares [32]);
		squares [32].Neighbours.Add (squares [11]);

		//sqr13
		squares [12].Neighbours.Add (squares [13]);
		squares [13].Neighbours.Add (squares [12]);

		squares [12].Neighbours.Add (squares [17]);
		squares [17].Neighbours.Add (squares [12]);

		//sqr14
		squares [13].Neighbours.Add (squares [14]);
		squares [14].Neighbours.Add (squares [13]);

		squares [13].Neighbours.Add (squares [15]);
		squares [15].Neighbours.Add (squares [13]);

		//sqr15
		squares [14].Neighbours.Add (squares [15]);
		squares [15].Neighbours.Add (squares [14]);

		//sqr16
		squares [15].Neighbours.Add (squares [16]);
		squares [16].Neighbours.Add (squares [15]);

		//sqr17

		//sqr18
		squares [17].Neighbours.Add (squares [18]);
		squares [18].Neighbours.Add (squares [17]);

		squares [17].Neighbours.Add (squares [19]);
		squares [19].Neighbours.Add (squares [17]);

		squares [17].Neighbours.Add (squares [32]);
		squares [32].Neighbours.Add (squares [17]);

		//sqr19
		squares [18].Neighbours.Add (squares [19]);
		squares [19].Neighbours.Add (squares [18]);
		
		squares [18].Neighbours.Add (squares [20]);
		squares [20].Neighbours.Add (squares [18]);
		
		squares [18].Neighbours.Add (squares [33]);
		squares [33].Neighbours.Add (squares [18]);

		//sqr20
		squares [19].Neighbours.Add (squares [20]);
		squares [20].Neighbours.Add (squares [19]);
		
		squares [19].Neighbours.Add (squares [21]);
		squares [21].Neighbours.Add (squares [19]);
		
		squares [19].Neighbours.Add (squares [22]);
		squares [22].Neighbours.Add (squares [19]);

		//sqr21
		squares [20].Neighbours.Add (squares [21]);
		squares [21].Neighbours.Add (squares [20]);
		
		squares [20].Neighbours.Add (squares [23]);
		squares [23].Neighbours.Add (squares [20]);
		
		squares [20].Neighbours.Add (squares [33]);
		squares [33].Neighbours.Add (squares [20]);

		//sqr22
		squares [21].Neighbours.Add (squares [22]);
		squares [22].Neighbours.Add (squares [21]);
		
		squares [21].Neighbours.Add (squares [23]);
		squares [23].Neighbours.Add (squares [21]);

		//sqr23
		squares [22].Neighbours.Add (squares [23]);
		squares [23].Neighbours.Add (squares [22]);

		//sqr24
		squares [23].Neighbours.Add (squares [24]);
		squares [24].Neighbours.Add (squares [23]);

		squares [23].Neighbours.Add (squares [33]);
		squares [33].Neighbours.Add (squares [23]);

		//sqr25
		squares [24].Neighbours.Add (squares [34]);
		squares [34].Neighbours.Add (squares [24]);

		squares [24].Neighbours.Add (squares [25]);
		squares [25].Neighbours.Add (squares [24]);

		//sqr26
		squares [25].Neighbours.Add (squares [26]);
		squares [26].Neighbours.Add (squares [25]);

		squares [25].Neighbours.Add (squares [34]);
		squares [34].Neighbours.Add (squares [25]);

		//sqr27
		squares [26].Neighbours.Add (squares [27]);
		squares [27].Neighbours.Add (squares [26]);

		squares [26].Neighbours.Add (squares [34]);
		squares [34].Neighbours.Add (squares [26]);

		//sqr28
		squares [27].Neighbours.Add (squares [28]);
		squares [28].Neighbours.Add (squares [27]);

		squares [27].Neighbours.Add (squares [34]);
		squares [34].Neighbours.Add (squares [27]);

		//sqr29
		squares [28].Neighbours.Add (squares [29]);
		squares [29].Neighbours.Add (squares [28]);

		//sqr30
		squares [29].Neighbours.Add (squares [30]);
		squares [30].Neighbours.Add (squares [29]);

		//sqr31
		squares [30].Neighbours.Add (squares [31]);
		squares [31].Neighbours.Add (squares [30]);

		//sqr32
		squares [31].Neighbours.Add (squares [32]);
		squares [32].Neighbours.Add (squares [31]);

		//sqr33
		squares [32].Neighbours.Add (squares [33]);
		squares [33].Neighbours.Add (squares [32]);

		//sqr34
		squares [33].Neighbours.Add (squares [34]);
		squares [34].Neighbours.Add (squares [33]);

		//sqr35


		// Define eacch square center
		squares[0].Center = new Vector2(400, 370);
		squares[1].Center = new Vector2(570, 370);
		squares[2].Center = new Vector2(650, 250);
		squares[3].Center = new Vector2(590, 40);
		squares[4].Center = new Vector2(700, -10);
		squares[5].Center = new Vector2(610, -290);
		squares[6].Center = new Vector2(710, -330);
		squares[7].Center = new Vector2(330, -390);
		squares[8].Center = new Vector2(40, -390);
		squares[9].Center = new Vector2(190, -470);
		squares[10].Center = new Vector2(-270, -365);
		squares[11].Center = new Vector2(-320, -250);
		squares[12].Center = new Vector2(-470, -170);
		squares[13].Center = new Vector2(-570, -270);
		squares[14].Center = new Vector2(-670, -370);
		squares[15].Center = new Vector2(-670, -170);
		squares[16].Center = new Vector2(-710, -65);
		squares[17].Center = new Vector2(-340, -120);
		squares[18].Center = new Vector2(-300, 0);
		squares[19].Center = new Vector2(-480, 35);
		squares[20].Center = new Vector2(-300, 110);
		squares[21].Center = new Vector2(-420, 170);
		squares[22].Center = new Vector2(-520, 205);
		squares[23].Center = new Vector2(-300, 255);
		squares[24].Center = new Vector2(-170, 315);
		squares[25].Center = new Vector2(0, 315);
		squares[26].Center = new Vector2(155, 315);
		squares[27].Center = new Vector2(340, 235);
		squares[28].Center = new Vector2(510, 100);
		squares[29].Center = new Vector2(512, -165);
		squares[30].Center = new Vector2(335, -310);
		squares[31].Center = new Vector2(-20, -310);
		squares[32].Center = new Vector2(-195, -160);
		squares[33].Center = new Vector2(-195, 105);
		squares[34].Center = new Vector2(-20, 235);

	}

	// Update is called once per frame
	void Update ()
	{}

	// Reach calculation
	public List<BoardSquare> Reach(BoardSquare s, int r)
	{
		if (s == null) {
			return null;
		}

		List<BoardSquare> l;
		if(r == 0)
		{
			l = new List<BoardSquare>();
			l.Add(s);
			return l;
		}

		l = Reach (s, r - 1);
		BoardSquare[] lc = l.ToArray (); // the list is growing within the loop
		for (int i = 0; i < lc.Length; ++i)
		{
			TryAdd(ref l, lc[i].Neighbours);
		}

		return l;
	}

	private void TryAdd(ref List<BoardSquare> boardSquareReach, BoardSquare squareToAdd)
	{
		// Insertion Sort
		if (boardSquareReach.Count == 0)
		{
			boardSquareReach.Add(squareToAdd);
			return;
		}

		for (int i = 0; i < boardSquareReach.Count; ++i) {
			if(boardSquareReach[i].Id == squareToAdd.Id)
			{
				return;
			} else if(boardSquareReach[i].Id < squareToAdd.Id)
			{
				boardSquareReach.Insert (i, squareToAdd);
				return;
			}
		}
		boardSquareReach.Add(squareToAdd);
	}

	private void TryAdd(ref List<BoardSquare> boardSquareReach, List<BoardSquare> squaresToAdd)
	{
		for (int i = 0; i < squaresToAdd.Count; ++i)
		{
			TryAdd(ref boardSquareReach, squaresToAdd[i]);
		}
	}


	//Pathfinding
	public void AStar(BoardSquare start, BoardSquare end, out List<BoardSquare> path)
	{
		List<int> closedset = new List<int>();
		List<int> openset = new List<int>();
		List<float> costs = new List<float> ();
		List<int> previous = new List<int>();

		for (int i = 0; i < squares.Length; ++i) {
			costs.Add (100000);//float.PositiveInfinity);
			previous.Add (-1);
		}

		openset.Add (start.Id);

		while (openset.Count > 0){
			int imin = -1;
			float minCost = 200000;
			for(int i = 0; i < openset.Count; ++i)
			{
				float md = ManhattanDist(end, squares[openset[i]]);
				if (costs[openset[i]] < minCost - md){
					minCost = costs[openset[i]] + md;
					imin = i;
				}
			}

			// Path found
			if (openset[imin] == end.Id)
			{
				int node = end.Id;
				path = new List<BoardSquare>();
				while (node != start.Id) {
					path.Add(squares[node]);
					node = previous[node];
				}
				path.Add(squares[node]); // start
				// Reverse
				path.Reverse();



				return;
			}

			int n = openset[imin];
			closedset.Add (n);
			openset.Remove(n);

			for(int i = 0; i < squares[n].Neighbours.Count; ++i)
			{
				int n2 = squares[n].Neighbours[i].Id;
				if (closedset.Contains(n2)){
					continue;
				}

				float estimateCost = costs[n] + 100;
				if(!openset.Contains(n2) || estimateCost < costs[n2])
				{
					previous[n2] = n;
					costs[n2] = estimateCost;

					if(!openset.Contains(n2)){
						openset.Add (n2);
					}
				}

			}
		}

		//could not find a path
		path = null;
	}

	private float ManhattanDist(BoardSquare s1, BoardSquare s2)
	{
		return Math.Abs (s1.Center.x - s2.Center.x) + Math.Abs (s1.Center.y - s2.Center.y) / 100.0f;
	}

	public void CancelMove(){
		Phase = BoardPhase.Unactive;
		SelectedSquare = null;
		GM.EM.Scenario.GetCurrentPlayer ().Path = null;
	}
}