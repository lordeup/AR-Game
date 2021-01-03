using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    public Material material;
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
    public GameObject GoalPrefab;
    private bool _isExit;
    public Dictionary<Transform, List<LineRenderer>> FloorsWithLines = new Dictionary<Transform, List<LineRenderer>>();
    private const float LineWidth = 0.05f;
    private const float PointY = 0.05f;

    private BasicMazeGenerator mMazeGenerator;

    private void Start()
    {
        if (!FullRandom)
        {
            Random.InitState(RandomSeed);
        }

        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        mMazeGenerator.GenerateMaze();
        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                var x = column * (CellWidth + (AddGaps ? .2f : 0));
                var z = row * (CellHeight + (AddGaps ? .2f : 0));
                var cell = mMazeGenerator.GetMazeCell(row, column);
                var tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0));
                tmp.transform.parent = transform;

                DrawLineRenderer(tmp, cell);

                if (cell.IsGoal && GoalPrefab != null)
                {
                    tmp = Instantiate(GoalPrefab, new Vector3(x, 0.1f, z), Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = transform;
                    tmp.transform.localRotation = GoalPrefab.transform.localRotation;
                }

                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position,
                        Quaternion.Euler(0, 90, 0));
                    tmp.transform.parent = transform;
                }

                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position,
                        Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = transform;
                }

                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position,
                        Quaternion.Euler(0, 270, 0));
                    tmp.transform.parent = transform;
                }

                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position,
                        Quaternion.Euler(0, 180, 0));
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

        if (Pillar != null)
        {
            for (var row = 0; row < Rows + 1; row++)
            {
                for (var column = 0; column < Columns + 1; column++)
                {
                    var x = column * (CellWidth + (AddGaps ? .2f : 0));
                    var z = row * (CellHeight + (AddGaps ? .2f : 0));
                    var tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2),
                        Quaternion.identity);
                    tmp.transform.parent = transform;
                }
            }
        }
    }

    private void DrawLineRenderer(GameObject tmp, MazeCell cell)
    {
        const float offset = 0f;
        var colliderComponent = tmp.GetComponent<Collider>();
        var line = CreateLineRenderer();

        var bounds = colliderComponent.bounds;
        var boundsMin = bounds.min;
        var boundsMax = bounds.max;
        var boundsCenter = bounds.center;

        boundsCenter.y = PointY;

        var lines = new List<LineRenderer>();

        var points = new List<Vector3>();

        if (cell.WallBack && cell.WallFront)
        {
            const float outBounds = 0.3f;
            var startPosition = new Vector3(boundsMin.x, PointY, boundsCenter.z);
            var endPosition = new Vector3(boundsMax.x, PointY, boundsCenter.z);

            if (cell.WallLeft)
            {
                startPosition.x += outBounds;
            }
            else if (cell.WallRight)
            {
                endPosition.x -= outBounds;
            }

            points.Add(startPosition);
            points.Add(endPosition);
        }
        else if (cell.WallRight && cell.WallLeft)
        {
            var startPosition = new Vector3(boundsCenter.x, PointY, boundsMin.z);
            var endPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMax.z);
            points.Add(startPosition);
            points.Add(endPosition);
        }
        else if (cell.WallRight && cell.WallBack)
        {
            var startPosition = new Vector3(boundsMin.x, PointY, boundsCenter.z);
            var endPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMax.z);
            points.Add(startPosition);
            points.Add(boundsCenter);
            points.Add(endPosition);
        }
        else if (cell.WallRight && cell.WallFront)
        {
            var startPosition = new Vector3(boundsMin.x, PointY, boundsCenter.z);
            var endPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMin.z);
            points.Add(startPosition);
            points.Add(boundsCenter);
            points.Add(endPosition);
        }
        else if (cell.WallLeft && cell.WallFront)
        {
            var startPosition = new Vector3(boundsMax.x, PointY, boundsCenter.z);
            var endPosition = new Vector3(boundsCenter.x, PointY, boundsMin.z);
            points.Add(startPosition);
            points.Add(boundsCenter);
            points.Add(endPosition);
        }
        else if (cell.WallBack && cell.WallLeft)
        {
            var startPosition = new Vector3(boundsMax.x, PointY, boundsCenter.z);
            var endPosition = new Vector3(boundsCenter.x, PointY, boundsMax.z);
            points.Add(startPosition);
            points.Add(boundsCenter);
            points.Add(endPosition);
        }
        else if (cell.WallFront)
        {
            var startPosition = new Vector3(boundsMin.x, PointY, boundsCenter.z);
            var endPosition = new Vector3(boundsMax.x, PointY, boundsCenter.z);
            points.Add(startPosition);
            points.Add(endPosition);

            var bottomPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMin.z);
            var newPoints = new List<Vector3> {boundsCenter, bottomPosition};
            CreateNewLine(newPoints, lines);
        }
        else
        {
            if (cell.WallLeft)
            {
                var startPosition = new Vector3(boundsMax.x, PointY, boundsCenter.z);
                points.Add(boundsCenter);
                points.Add(startPosition);

                var bottomPosition = new Vector3(boundsCenter.x, PointY, boundsMin.z);
                var topPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMax.z);
                var newPoints = new List<Vector3> {bottomPosition, topPosition};

                CreateNewLine(newPoints, lines);
            }
            else if (cell.WallRight)
            {
                var startPosition = new Vector3(boundsMin.x, PointY, boundsCenter.z);
                points.Add(startPosition);
                points.Add(boundsCenter);

                var bottomPosition = new Vector3(boundsCenter.x, PointY, boundsMin.z);
                var topPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMax.z);

                var newPoints = new List<Vector3> {bottomPosition, topPosition};

                CreateNewLine(newPoints, lines);
            }
            else if (cell.WallBack)
            {
                var leftPosition = new Vector3(boundsMin.x, PointY, boundsCenter.z);
                var rightPosition = new Vector3(boundsMax.x, PointY, boundsCenter.z);

                points.Add(leftPosition);
                points.Add(rightPosition);

                var topPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMax.z);

                var newPoints = new List<Vector3> {boundsCenter, topPosition};

                CreateNewLine(newPoints, lines);
            }
            else
            {
                var leftPosition = new Vector3(boundsCenter.x, PointY, boundsMin.z);
                var rightPosition = new Vector3(boundsCenter.x + offset, PointY, boundsMax.z);
                points.Add(leftPosition);
                points.Add(rightPosition);

                var bottomPosition = new Vector3(boundsMin.x, PointY, boundsCenter.z);
                var topPosition = new Vector3(boundsMax.x, PointY, boundsCenter.z);

                var newPoints = new List<Vector3> {bottomPosition, topPosition};

                CreateNewLine(newPoints, lines);
            }
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
        lines.Add(line);

        FloorsWithLines.Add(tmp.transform, lines);
    }

    private LineRenderer CreateLineRenderer()
    {
        var line = new GameObject("line").AddComponent<LineRenderer>();

        //   line.alignment = LineAlignment.TransformZ;

        line.startWidth = LineWidth;
        line.endWidth = LineWidth;

        //  line.numCapVertices = 5;
        //  line.numCornerVertices = 5;

        line.enabled = false;
        line.material = material;

        return line;
    }

    private void CreateNewLine(List<Vector3> points, ICollection<LineRenderer> lines)
    {
        var line = CreateLineRenderer();

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());

        lines.Add(line);
    }
}
