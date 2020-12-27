using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
	private bool _isExit;
	public List<Transform> FloorList;
	public List<LineRenderer> LineRenderers;

	private BasicMazeGenerator mMazeGenerator = null;

	void Start () {
		if (!FullRandom) {
			Random.seed = RandomSeed;
		}
		switch (Algorithm) {
		case MazeGenerationAlgorithm.PureRecursive:
			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveTree:
			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RandomTree:
			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.OldestTree:
			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveDivision:
			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
			break;
		}
		var lineRender = gameObject.AddComponent<LineRenderer>();

		mMazeGenerator.GenerateMaze ();
		for (int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++){
				float x = column*(CellWidth+(AddGaps?.2f:0));
				float z = row*(CellHeight+(AddGaps?.2f:0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				GameObject tmp;
				tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;

				tmp.transform.parent = transform;

				FloorList.Add(tmp.transform);

				DrawLineRenderer(tmp, cell);

				if (cell.IsGoal && GoalPrefab != null)
				{
					tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
					tmp.transform.parent = transform;
					tmp.transform.localRotation = GoalPrefab.transform.localRotation;
				}

				if(cell.WallRight)
				{
					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
					tmp.tag = "RightWall";
					tmp.transform.parent = transform;
				}
				if(cell.WallFront){
					tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
					tmp.tag = "FrontWall";
					tmp.transform.parent = transform;
				}
				if(cell.WallLeft){
					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
					tmp.tag = "LeftWall";
					tmp.transform.parent = transform;
				}
				if(cell.WallBack)
				{
					tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
					tmp.tag = "BackWall";
					tmp.transform.parent = transform;
					if (column == Columns - 1 && !_isExit)
					{
						_isExit = true;
						tmp.GetComponent<MeshRenderer>().enabled = false;
						tmp.GetComponent<MeshCollider>().isTrigger = true;
					}
				}
			}
		}
		if(Pillar != null){
			for (int row = 0; row < Rows+1; row++) {
				for (int column = 0; column < Columns+1; column++) {
					float x = column*(CellWidth+(AddGaps?.2f:0));
					float z = row*(CellHeight+(AddGaps?.2f:0));
					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
	}

	private void DrawLineRenderer(GameObject tmp, MazeCell cell)
	{
		var colliderComponent = tmp.GetComponent<Collider>();
		var line = new GameObject("line").AddComponent<LineRenderer>();

		line.alignment = LineAlignment.TransformZ;

		line.startWidth = 0.2f;
		line.endWidth = 0.2f;

		var bounds = colliderComponent.bounds;
		var boundsMin = bounds.min;
		var boundsMax = bounds.max;
		var boundsCenter = bounds.center;

		var points = new List<Vector3>();

		if (cell.WallBack && cell.WallFront)
		{
			var startPosition = new Vector3(boundsMin.x, boundsMin.y, boundsCenter.z);
			var endPosition =  new Vector3(boundsMax.x, boundsMax.y, boundsCenter.z);
			points.Add(startPosition);
			points.Add(endPosition);
		}
		else if (cell.WallRight && cell.WallLeft)
		{
			var startPosition = new Vector3(boundsCenter.x, boundsMin.y, boundsMin.z);
			var endPosition =  new Vector3(boundsCenter.x + 0.1f, boundsMax.y, boundsMax.z);
			points.Add(startPosition);
			points.Add(endPosition);
		}
		else if (cell.WallRight && cell.WallBack)
		{
			var startPosition = new Vector3(boundsMin.x, boundsMin.y, boundsCenter.z);
			var endPosition = new Vector3(boundsCenter.x + 0.1f, boundsMax.y, boundsMax.z);
			points.Add(startPosition);
			points.Add(boundsCenter);
			points.Add(endPosition);
		}
		else if (cell.WallRight && cell.WallFront)
		{
			var startPosition = new Vector3(boundsMin.x, boundsMin.y, boundsCenter.z);
			var endPosition = new Vector3(boundsCenter.x + 0.1f, boundsMin.y, boundsMin.z);
			points.Add(startPosition);
			points.Add(boundsCenter);
			points.Add(endPosition);
		}
		else if (cell.WallLeft && cell.WallFront)
		{
			var startPosition = new Vector3(boundsMax.x, boundsMax.y, boundsCenter.z);
			var endPosition = new Vector3(boundsCenter.x, boundsMin.y, boundsMin.z);
			points.Add(startPosition);
			points.Add(boundsCenter);
			points.Add(endPosition);
		}
		else if (cell.WallBack && cell.WallLeft)
		{
			var startPosition = new Vector3(boundsMax.x, boundsMax.y, boundsCenter.z);
			var endPosition = new Vector3(boundsCenter.x, boundsMax.y, boundsMax.z);
			points.Add(startPosition);
			points.Add(boundsCenter);
			points.Add(endPosition);
		}
		else if (cell.WallFront)
		{
			var startPosition = new Vector3(boundsMin.x, boundsMin.y, boundsCenter.z);
			var endPosition =  new Vector3(boundsMax.x, boundsMax.y, boundsCenter.z);
			points.Add(startPosition);
			points.Add(endPosition);
		}
		else
		{
			points.Add(boundsMin);
			points.Add(boundsMax);
		}

		line.positionCount = points.Count;
		line.SetPositions(points.ToArray());

		LineRenderers.Add(line);
	}

}
