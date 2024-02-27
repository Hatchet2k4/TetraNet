namespace TetraNet;

using Godot;
using System.Collections.Generic;

public partial class Data : Node
{
	public static readonly int GRID_SIZE = 48;
	public static readonly int GRID_W = 10;
	public static readonly int GRID_H = 20;
	public static readonly Vector2 DOWN = Vector2.Down;
	public static readonly Vector2 UP = Vector2.Up;
	public static readonly Vector2 LEFT = Vector2.Left;
	public static readonly Vector2 RIGHT = Vector2.Right;


	public enum BlockType
	{
		I, O, T, J, L, S, Z
	}

	public static Dictionary<BlockType, List<Vector2>> Cells = new Dictionary<BlockType, List<Vector2>>
	{
		{BlockType.I, new List<Vector2> { new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0) }},
		{BlockType.J, new List<Vector2> { new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0) }},
		{BlockType.L, new List<Vector2> { new Vector2(1, 1), new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0) }},
		{BlockType.O, new List<Vector2> { new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }},
		{BlockType.S, new List<Vector2> { new Vector2(0, 1), new Vector2(1, 1), new Vector2(-1, 0), new Vector2(0, 0) }},
		{BlockType.T, new List<Vector2> { new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0) }},
		{BlockType.Z, new List<Vector2> { new Vector2(-1, 1), new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0) }}
	};

	public static List<List<Vector2>> wallKicksI = new List<List<Vector2>>
	{
		new List<Vector2> {new Vector2(0,0), new Vector2(-2,0), new Vector2(1,0), new Vector2(-2,-1), new Vector2(1,2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(2,0), new Vector2(-1, 0), new Vector2(2,1), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1, 0), new Vector2(2,0), new Vector2(-1,2), new Vector2(2, -1)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1,0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1)},
		new List<Vector2> {new Vector2(0,0), new Vector2(2,0), new Vector2(-1, 0), new Vector2(2,1), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-2,0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1,0), new Vector2(-2,0), new Vector2(1, -2), new Vector2(-2,1)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1,2), new Vector2(2, -1)}
	};

	public static List<List<Vector2>> wallKicks = new List<List<Vector2>> {
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1,1), new Vector2(0,-2), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1,0), new Vector2(1, -1), new Vector2(0,2), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1, 0), new Vector2(1,-1), new Vector2(0,2), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1,0), new Vector2(1, 1), new Vector2(0,-2), new Vector2(1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1,-1), new Vector2(0, 2), new Vector2(-1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0,-2), new Vector2(1, -2)}
	};

	public static Dictionary<BlockType, BlockData> blockResources = new()
	{
		{BlockType.I, GD.Load("res://resources/I.tres") as BlockData},
		{BlockType.J, GD.Load("res://resources/J.tres") as BlockData},
		{BlockType.L, GD.Load("res://resources/L.tres") as BlockData},
		{BlockType.O, GD.Load("res://resources/O.tres") as BlockData},
		{BlockType.S, GD.Load("res://resources/S.tres") as BlockData},
		{BlockType.T, GD.Load("res://resources/T.tres") as BlockData},
		{BlockType.Z, GD.Load("res://resources/Z.tres") as BlockData}
	};

	public static Dictionary<BlockType, Vector2> spawnPositions = new()
	{
		{BlockType.I, new Vector2(0,1)},
		{BlockType.J, new Vector2(0.5f,1)},
		{BlockType.L, new Vector2(0.5f,2)},
		{BlockType.O, new Vector2(0, 1)},
		{BlockType.S, new Vector2(0.5f,1)},
		{BlockType.T, new Vector2(0.5f,1)},
		{BlockType.Z, new Vector2(0.5f,1)}
	};



	public static List<Vector2> clockwiseRotation = new()
	{
		new Vector2(0, -1),
		new Vector2(1, 0)
	};

	public static List<Vector2> counterClockwiseRotation = new()
	{
		new Vector2(0, 1),
		new Vector2(-1, 0)
	};

}
