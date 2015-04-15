using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;

public class PhysicalBoardManager : MonoBehaviour {

	public GameManager GM;
	//IntPtr physicaloid;
	int i = 0;
	const int imax = 100;
	SerialPort SP;
	List<SerialPort> LSP = new List<SerialPort>();

	string list;

	// Use this for initialization
	void Start () {
		
		/*Debug.Log ("Ports:");
        string[] ports = SerialPort.GetPortNames ();
        string s = "";
        for (int i = ports.Length - 1; i > -1; --i) {
            s += (i+1).ToString() + ports[i].ToString() + "\n";
			LSP.Add(new SerialPort(ports[i].ToString()));
		}
        if (ports.Length > 0) {
            //t.text = s;
			Debug.Log (s);
			list =s;
            SP = new SerialPort (ports [0].ToString ());
            SP.BaudRate = 9600;

            SP.ReadTimeout = 100;
            SP.Open ();
        } else {
            //t.text = "NO COM PORT DETECTED...";
			Debug.Log ("NO COM PORT DETECTED...");
		}*/

		//AndroidJNI.NewByteArray (6);
		
		//physicaloid = AndroidJNI.FindClass ("com.physicaloid.lib.Physicaloid");
		//using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.physicaloid.lib.Physicaloid"));
		//AndroidJavaObject physicaloidClass = new AndroidJavaClass ("com.physicaloid.lib.Physicaloid");

		#if UNITY_ANDROID && !UNITY_EDITOR
		//AndroidJavaObject physicaloidObject = new AndroidJavaObject("com/physicaloid/lib/Physicaloid");
		//physicaloidObject.Call ("open");

		string physicaloidObject = "test";
		/*
		GM.NarrationScreen.transform.FindChild("CharactersTextAnchor").Find("CharactersBubble").gameObject.GetComponent<BGOfTextScenario>().SetBgForScenario("Narration");
		GM.DM.SetNarrationScreenActive(true);
		GM.NarrationScreen.transform.FindChild("NarratorTextAnchor").Find("NarratorText").GetComponent<Text>().text = physicaloidObject.ToString ();
		*/
		//IntPtr physicaloidObject = AndroidJNI.AllocObject (physicaloid);
		//IntPtr openMethod = AndroidJNI.GetMethodID (physicaloidObject, "open", "()");
		
		//jvalue[] args = new jvalue[0];
		//bool b = AndroidJNI.CallBooleanMethod(physicaloidObject, openMethod, args);
		
		/*bool b = true;
		DM.SetMapActive (b);
		if (b) {
			AndroidJNIHelper.ConvertToJNIArray();
			public boolean setBaudrate(int baudrate) throws RuntimeException{
				synchronized (LOCK) {
					if(mSerial == null) return false;
					return mSerial.setBaudrate(baudrate);
				}
			}
			
			public int write(byte[] buf) throws RuntimeException {
			    if(mSerial == null) return 0;
			    return write(buf, buf.length);
			}

			public int read(byte[] buf) throws RuntimeException {
				if(mSerial == null) return 0;
				return read(buf, buf.length);
			}
			args[0].b = 1;
			physicaloidObject.Call("setBaudrate", 9400);
			
			string str = "get";
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			physicaloidObject.Call("write", bytes);
			
			byte[] buffer = {0,0,0,0,0,0};
			physicaloidObject.Call("read", buffer);
			
			
			Debug.Log ("Yay");
			Debug.Log (buffer);
		} else {
			Debug.Log ("Nope");
		}
		Debug.Log (physicaloid);
		//Debug.Log (physicaloidjava);*/
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	/*	if (i == 0) {
			//SP.Write("get");
			for(int j=0; j < LSP.Count; ++j){
				LSP[j].Write("get");
			}
			Debug.Log(list);
		}
		++i;
		if (i >= imax) {
			i = 0;
		}*/
	}

	string s  = "";
	void OnGUI()
	{
		Debug.Log (GM.GameBoard.Phase);
		if (GM.GameBoard.Phase == Board.BoardPhase.PlayerMoving || GM.GameBoard.Phase == Board.BoardPhase.PlayerHasSelectedSquare) {
			Debug.Log("in condition");
			if (Event.current.Equals (Event.KeyboardEvent ("Keypad0")) || Event.current.Equals (Event.KeyboardEvent ("Alpha0"))) {
				s += "0";
			} else if (Event.current.Equals (Event.KeyboardEvent ("Keypad1")) || Event.current.Equals (Event.KeyboardEvent ("Alpha1"))) {
				s += "1";
			} else if (Event.current.keyCode == KeyCode.E){//KeyCode.At){
				//|| Event.current.Equals (Event.KeyboardEvent ("Return")) || Event.current.Equals (Event.KeyboardEvent ("KeypadEnter")) || Event.current.Equals (Event.KeyboardEvent ("Enter")) ) {
				Debug.Log (s);

				// Reverse
				/*char[] arr = s.ToCharArray();
				Array.Reverse(arr);
				s = new string(arr);*/

				int physicalSquare = -1;
				switch(s)
				{
				case "011111":
					physicalSquare = 1;
					break;
				case "001111":
					physicalSquare = 2;
				break;
				case "011011":
					physicalSquare = 3;
				break;
				case "001011":
					physicalSquare = 4;
				break;
				case "000011":
					physicalSquare = 5;
				break;
				case "110100":
					physicalSquare = 6;
				break;
				case "110110":
					physicalSquare = 7;
				break;
				case "110000":
					physicalSquare = 8;
				break;
				case "110011":
					physicalSquare = 9;
				break;
				case "110010":
					physicalSquare = 10;
				break;
				case "110001":
					physicalSquare = 11;
				break;
				case "111101":
					physicalSquare = 12;
				break;
				case "111100":
					physicalSquare = 13;
				break;
				case "111000":
					physicalSquare = 14;
				break;
				case "111110":
					physicalSquare = 15;
				break;
				case "111010":
					physicalSquare = 16;
				break;
				case "111011":
					physicalSquare = 17;
				break;
				case "010011":
					physicalSquare = 18;
				break;
				case "010111":
					physicalSquare = 19;
				break;
				case "011010":
					physicalSquare = 20;
				break;
				case "010110":
					physicalSquare = 21;
				break;
				case "001110":
					physicalSquare = 22;
				break;
				case "011110":
					physicalSquare = 23;
				break;
				case "001100":
					physicalSquare = 24;
				break;
				case "101111":
					physicalSquare = 25;
				break;
				case "100111":
					physicalSquare = 26;
				break;
				case "000111":
					physicalSquare = 27;
				break;
				case "010101":
					physicalSquare = 28;
				break;
				case "010001":
					physicalSquare = 29;
				break;
				case "100001":
					physicalSquare = 30;
				break;
				case "110101":
					physicalSquare = 31;
				break;
				case "110111":
					physicalSquare = 32;
				break;
				case "011001":
					physicalSquare = 33;
				break;
				case "100100":
					physicalSquare = 34;
				break;
				case "010100":
					physicalSquare = 35;
				break;
				
				default:
					s = "";
					return;
				}


				GamePlayer p = GM.EM.Scenario.GetCurrentPlayer();

				// Uncolorize board
				Color col = new Color(1F, 1F, 1F, 0.0f);
				for (int i = 0; i < GM.GameBoard.squares.Length; ++i)
				{
					GM.GameBoard.squares[i].SetMaterial(GM.GameBoard.squares[i].matExit, col);
				}
				
				// Colorize Reach in green
				p = GM.EM.Scenario.GetCurrentPlayer();
				if (p.Reach != null)
				{
					for (int i = 0; i < p.Reach.Count; i++)
					{
						p.Reach[i].SetMaterial(p.Reach[i].matExit, new Color(0, 0, 0, 0));
					}
				}
				Color green = new Color(0.19f, 0.78f, 0.45f);
				p.Reach = GM.GameBoard.Reach(p.CurrentSquare, p.Liveliness);
				
				if (p.Reach != null)
				{
					for (int i = 0; i < p.Reach.Count; i++)
					{
						p.Reach[i].SetMaterial(p.Reach[i].matEnter, green);
					}
				}
				

				BoardSquare square = GM.GameBoard.squares[physicalSquare - 1];
				if(square == p.CurrentSquare || !p.Reach.Contains(square))
				{
					return;
				}

				GM.GameBoard.AStar(p.CurrentSquare, square, out p.Path);
				
				// Colorize path
				Color orange = new Color (1, 0.5f, 0);
				Color blue = new Color (0.19f, 0.45f, 0.78f);
				if(p.Path != null){
					int i = 0;//p.Path.Count-1;
					while(i < p.Path.Count){
						// Stop if square is threatened
						if (p.Path[i].IsThreatened(p) && !(p.Fleeing && i == 0)) {
							p.Path[i].SetMaterial(p.Path[i].matEnter, new Color(1,0,0));
							GM.GameBoard.SelectedSquare = p.Path[i];
							GM.GameBoard.Phase = Board.BoardPhase.PlayerHasSelectedSquare;
							return;
						}
						p.Path[i].SetMaterial(p.Path[i].matEnter, orange);
						++i;
					}
					
					square.SetMaterial(square.matEnter, blue);
					GM.GameBoard.SelectedSquare = square;
					GM.GameBoard.Phase = Board.BoardPhase.PlayerHasSelectedSquare;
				}


				GM.GameBoard.Phase = Board.BoardPhase.PlayerHasSelectedSquare;

				/*GM.NarrationScreen.transform.FindChild ("CharactersTextAnchor").Find ("CharactersBubble").gameObject.GetComponent<BGOfTextScenario> ().SetBgForScenario ("Narration");
				GM.DM.SetNarrationScreenActive (true);
				GM.NarrationScreen.transform.FindChild ("NarratorTextAnchor").Find ("NarratorText").GetComponent<Text> ().text = s;
				*/

				s = "";




			}
		}
	}
}
