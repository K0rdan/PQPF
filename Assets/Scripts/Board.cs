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

	}

	// Update is called once per frame
	void Update ()
	{

	}
}