using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

		// Link all squares to the Game Manager
		for (int i = 0; i < squares.Length; ++i) {
			squares[i].GM = GM;
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



	}

	// Update is called once per frame
	void Update ()
	{

	}
}