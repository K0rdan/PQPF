using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;


public class BoardSquare : MonoBehaviour// BoardGameItem
{
	public int Id;

	public Vector2[] vertices2D;
	public Vector2 Center;

	public Board GameBoard;
	public GameManager GM;

	public Material matEnter;
	public Material matExit;

	public ResourceFindingRate[] resourceFindingRates;

	public MeshRenderer mrenderer;

	public List<BoardSquare> Neighbours = new List<BoardSquare> ();
	public List<GamePlayer> Players = new List<GamePlayer> ();
	public List<GameEnemy> Enemies = new List<GameEnemy> ();


	/*
	Character[] 					_personnages;
	BoardSquareLink[]				_neighbours;
	Property[] 						_effect;
	*/
	PolygonCollider2D			Collider2D;
	LineRenderer				Border;

	public void Awake ()
	{	
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D);
		int[] indices = tr.Triangulate();
		
		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices2D.Length];
		for (int i = 0; i<vertices.Length; i++) {
			vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
		}
		
		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();

		Vector2[] uvs = new Vector2[vertices.Length];
		for(int i = 0; i < uvs.Length; i++) {
			uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
		}
		msh.uv = uvs;

		// Set up game object with mesh;
		//MeshRenderer mrenderer
		mrenderer = gameObject.AddComponent<MeshRenderer>();
		//mrenderer.material.color = Color.red;

		Debug.Log("Material create");
		matExit = new Material (Shader.Find ("Transparent/Diffuse"));
		matExit.color = new Color (1f, 1f, 1f, 0.1f);
		matEnter = new Material (Shader.Find ("Transparent/Diffuse"));//"Self-Illumin/Diffuse"));
		matEnter.color = new Color (1f, 1f, 1f, 0.5f);

		mrenderer.material = matExit;


		MeshFilter mfilter = gameObject.AddComponent<MeshFilter>();
		mfilter.mesh = msh;
		

		float s = 30f;
		Transform tf = gameObject.GetComponent<Transform> ();
		tf.localPosition = new Vector3(-4.0f*s,2.5f*s,0);
		tf.localScale = new Vector3 (s,s,s);

		Collider2D = gameObject.AddComponent<PolygonCollider2D> ();
		Collider2D.SetPath (0, vertices2D);


		Vector2[] vertices2D2 = new Vector2[vertices2D.Length * 3];
		int altBeginEnd = 1; // begin
		for (int i = 0; i < vertices2D2.Length; i++) {
			int iBegin = i/3;
			if (i%3 == 0) {
				vertices2D2[i] = new Vector2(vertices2D[iBegin].x, vertices2D[iBegin].y);
				altBeginEnd = 1;
			} else {
				int iEnd = iBegin + 1;
				if(iEnd == vertices2D.Length){
					iEnd = 0;
				}

				float dx = vertices2D[iEnd].x - vertices[iBegin].x;
				float dy = vertices2D[iEnd].y - vertices[iBegin].y;

				float p = s*0.0001f;
				if(altBeginEnd < 0){
					vertices2D2[i] = new Vector2(vertices2D[iEnd].x - p * dx, vertices2D[iEnd].y - p * dy);
				}else{
					vertices2D2[i] = new Vector2(vertices2D[iBegin].x + p * dx, vertices2D[iBegin].y + p * dy);
				}

				altBeginEnd = -1;
			}
		}

	/*	
	//GameObject o = (GameObject) Instantiate (gameObject);
		Border = gameObject.AddComponent<LineRenderer> ();
		Border.useWorldSpace = true;
			Border.material = new Material (Shader.Find ("Diffuse"));
			//Border.material.SetFloat("_Outline", 0.01F);
			//Border.material.SetColor("_OutlineColor", Color.red);
			Border.SetWidth(0.05f, 0.05f);
			Border.SetVertexCount (vertices2D2.Length+1);

			for (int i=0; i<vertices2D2.Length; i++) {
				Border.SetPosition (i, tf.TransformVector(vertices2D2 [i] + new Vector2(-4.0f,2.5f)));
			}
			Border.SetPosition (vertices2D2.Length, tf.TransformVector(vertices2D2 [0]  + new Vector2(-4.0f,2.5f)));
			for (int i=vertices2D2.Length-1; i>-1; i--) {
				Border.SetPosition (i, tf.TransformVector(vertices2D2 [i] + new Vector2(-4.0f,2.5f)));
			}
			Border.SetPosition (vertices2D2.Length, tf.TransformVector(vertices2D2 [0] + new Vector2(-4.0f,2.5f)));
		*/

			/*
			float s2 = 1.4666f;
			for (int i=0; i<vertices2D2.Length; i++) {
				Border.SetPosition (i, new Vector3 ((vertices2D2 [i].x - 4.0f) / s2, (vertices2D2 [i].y + 2.5f) / s2, 1f));
			}
			Border.SetPosition (vertices2D2.Length, new Vector3 ((vertices2D2 [0].x - 4.0f) / s2, (vertices2D2 [0].y + 2.5f) / s2, 1f));
			for (int i=vertices2D2.Length-1; i>-1; i--) {
				Border.SetPosition (i, new Vector3 ((vertices2D2 [i].x - 4.0f) / s2, (vertices2D2 [i].y + 2.5f) / s2, 1f));
			}
			Border.SetPosition (vertices2D2.Length, new Vector3 ((vertices2D2 [0].x - 4.0f) / s2, (vertices2D2 [0].y + 2.5f) / s2, 1f));
			*/

	}

	public void Update ()
	{

	}

	public void SetMaterial(Material mat, Color color)
	{
		mrenderer.material = mat;
		mrenderer.material.color = color;
	}

	public void OnMouseEnter()
	{
		//mrenderer.material = matEnter;
		//mrenderer.material.color = new Color(0F, 1F, 0F);

		// TODO Display informations about this square

	}
	public void OnMouseExit()
	{
		//mrenderer.material = matExit;
		//mrenderer.material.color -= new Color(0, 1F, 0);

		//for(int i=0; i<Neighbours.Count; i++){
		//	Neighbours[i].mrenderer.material = Neighbours[i].matExit;
		//}

		/*List<BoardSquare> l = GameBoard.Reach (this, GM.EM.Scenario.GetCurrentPlayer ().Liveliness);
		for (int i=0; i < l.Count; i++) {
			l [i].mrenderer.material = l [i].matExit;
		}*/
		
	}
	public void OnMouseUpAsButton()
	{
		GamePlayer p;
		if(GameBoard.Phase == Board.BoardPhase.PlayerMoving || GameBoard.Phase == Board.BoardPhase.PlayerHasSelectedSquare) {
			p = GM.EM.Scenario.GetCurrentPlayer ();
			if(p.Reach != null && p.Reach.Contains(this) && p.CurrentSquare != this) // Click on a Reachable square different of the current one
			{
				//TODO factorize and put in board or DM;
				// Uncolorize all //previous path
				/*if(p.Path != null){
					for(int i = 0; i < p.Path.Count; ++i)
					{
						p.Path[i].SetMaterial(matExit, new Color (1F, 1F, 1F, 0.0f));
					}
				}*/
				Color col = new Color (1F, 1F, 1F, 0.0f);
				for(int i = 0; i < GameBoard.squares.Length; ++i)
				{
					GameBoard.squares[i].SetMaterial(matExit, col);
				}

				// Colorize Reach in blue
				Color blue = new Color (0F, 0F, 1F, 0.35f);
				for(int i=0; i < p.Reach.Count; i++){
					p.Reach[i].SetMaterial(p.Reach[i].matEnter, blue);
				}

				Debug.Log ("Square Id is "+ this.Id.ToString());
				GameBoard.AStar(p.CurrentSquare, this, out p.Path);

				if(p.Path != null){
					int i = p.Path.Count-1;
					p.Path[i].SetMaterial(p.Path[i].matEnter, new Color(1,0.5f,0,0.3f));
					while(i > 0){
						--i;
						p.Path[i].SetMaterial(p.Path[i].matEnter, new Color (0F, 1F, 0F, 0.25f));
					}
					GameBoard.SelectedSquare = this;
					GameBoard.Phase = Board.BoardPhase.PlayerHasSelectedSquare;
				}
			}
		}
	}

	public Resource digUpResource()
	{
		int nbRes = resourceFindingRates.Length;
		float[] cumulativeRates;
		float rand;

		if (nbRes > 0) {
			cumulativeRates = new float[nbRes];
			cumulativeRates[0] = resourceFindingRates[0].rate;
			for (int i = 1; i < nbRes; i++) {
				cumulativeRates[i] = cumulativeRates[i - 1] + resourceFindingRates[i].rate;
			}

			rand = UnityEngine.Random.Range (0f, cumulativeRates[nbRes - 1]);

			int j = 0;
			while(cumulativeRates[j] < rand){
				j++;
			}

			return resourceFindingRates[j].resource;
		}

		return null;
	}

	public void SetId (int id)
	{
		Id = id;
	}

	// TODO extend this with aligment
	// TODO only count if seen (eg Tirette)
	public bool IsThreatened (GameEnemy ge){
		return Players.Count > 0;
	}
	public List<GamePlayer> ThreatList(GameEnemy ge)
	{
		// TODO Extend
		return Players;
	}
	public bool IsThreatened (GamePlayer gp){
		return Enemies.Count > 0;	
	}
	public List<GameEnemy> ThreatList(GamePlayer gp)
	{
		// TODO Extend
		//List<GameEnemy> enemies = new List<GameEnemy> ();
		/*for (int i = 0; i < Enemies.Count; ++i) {
			
		}*/
		return Enemies;
	}
}
	
[System.Serializable]
public class ResourceFindingRate
{
	public Resource resource;
	public float rate;
}

public class Triangulator
	{
		private List<Vector2> m_points = new List<Vector2>();
		
		public Triangulator (Vector2[] points) {
			m_points = new List<Vector2>(points);
		}
		
		public int[] Triangulate() {
			List<int> indices = new List<int>();
			
			int n = m_points.Count;
			if (n < 3)
				return indices.ToArray();
			
			int[] V = new int[n];
			if (Area() > 0) {
				for (int v = 0; v < n; v++)
					V[v] = v;
			}
			else {
				for (int v = 0; v < n; v++)
					V[v] = (n - 1) - v;
			}
			
			int nv = n;
			int count = 2 * nv;
			for (int m = 0, v = nv - 1; nv > 2; ) {
				if ((count--) <= 0)
					return indices.ToArray();
				
				int u = v;
				if (nv <= u)
					u = 0;
				v = u + 1;
				if (nv <= v)
					v = 0;
				int w = v + 1;
				if (nv <= w)
					w = 0;
				
				if (Snip(u, v, w, nv, V)) {
					int a, b, c, s, t;
					a = V[u];
					b = V[v];
					c = V[w];
					indices.Add(a);
					indices.Add(b);
					indices.Add(c);
					m++;
					for (s = v, t = v + 1; t < nv; s++, t++)
						V[s] = V[t];
					nv--;
					count = 2 * nv;
				}
			}
			
			indices.Reverse();
			return indices.ToArray();
		}
		
		private float Area () {
			int n = m_points.Count;
			float A = 0.0f;
			for (int p = n - 1, q = 0; q < n; p = q++) {
				Vector2 pval = m_points[p];
				Vector2 qval = m_points[q];
				A += pval.x * qval.y - qval.x * pval.y;
			}
			return (A * 0.5f);
		}
		
		private bool Snip (int u, int v, int w, int n, int[] V) {
			int p;
			Vector2 A = m_points[V[u]];
			Vector2 B = m_points[V[v]];
			Vector2 C = m_points[V[w]];
			if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
				return false;
			for (p = 0; p < n; p++) {
				if ((p == u) || (p == v) || (p == w))
					continue;
				Vector2 P = m_points[V[p]];
				if (InsideTriangle(A, B, C, P))
					return false;
			}
			return true;
		}
		
		private bool InsideTriangle (Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
			float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
			float cCROSSap, bCROSScp, aCROSSbp;
			
			ax = C.x - B.x; ay = C.y - B.y;
			bx = A.x - C.x; by = A.y - C.y;
			cx = B.x - A.x; cy = B.y - A.y;
			apx = P.x - A.x; apy = P.y - A.y;
			bpx = P.x - B.x; bpy = P.y - B.y;
			cpx = P.x - C.x; cpy = P.y - C.y;
			
			aCROSSbp = ax * bpy - ay * bpx;
			cCROSSap = cx * apy - cy * apx;
			bCROSScp = bx * cpy - by * cpx;
			
			return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
		}
	}

