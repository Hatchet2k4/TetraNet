namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


public partial class Data : Node
{
	public static readonly int GRID_SIZE = 48;
	public static readonly int GRID_W = 10;
	public static readonly int GRID_H = 20;
	public static readonly Vector2 DOWN = Vector2.Down;
	public static readonly Vector2 UP = Vector2.Up;
	public static readonly Vector2 LEFT = Vector2.Left;
	public static readonly Vector2 RIGHT = Vector2.Right;

	public static readonly int NUM_ITEMS = 12;

	public enum ItemType
	{
		//defense
		C, //clear line
		D, //defense shield - block attacks/line adds for X seconds
		G, //gravity
		N, //nuke field - clear all blocks		
		   //attack
		A, //add random line
		B, //remove special blocks	
		L, //lock rotation
		H, //lock hold button			
		O, //explode special blocks
		Q, //quake
		R, //random 
		S //switch field
	}

	public static Dictionary<ItemType, string> toolTips = new()
	{
		{ItemType.C, "Clear Line - Removes a line from bottom of field."},
		{ItemType.D, "Defense - Block incoming attacks for 6 seconds. (Not implemented)"},
		{ItemType.G, "Gravity - Pull all blocks down, filling any gaps."},
		{ItemType.N, "Nuke - Destroys all blocks in field."},
		{ItemType.A, "Add Line - Adds a junk line to bottom of field."},
		{ItemType.B, "Block Clear - Remove all item blocks from field."},
		{ItemType.L, "Lock - Locks rotation of blocks for 6 seconds. (Not implemented)"},
		{ItemType.H, "Hold - Prevents use of hold box for 6 seconds. (Not implemented)"},
		{ItemType.O, "Block Bomb - Destroys O blocks in field, scattering up to 8 blocks randomly. (Not implemented)"},
		{ItemType.Q, "Quake - Randomly shift all field rows."},
		{ItemType.R, "Random - Removes up to 10 random blocks from field."},
		{ItemType.S, "Switch Field - Switch your field with an opponent's. (Not implemented)"}
	};

	public static Dictionary<int, string> TeamMappings = new()
	{
		{0, "None"},
		{1, "Red"},
		{2, "Green"},
		{3, "Blue"},
		{4, "Yellow"},
		{5, "Orange"},
		{6, "Purple"}
	};

	public enum BlockType
	{
		I, O, T, J, L, S, Z
	}

	public static List<List<Vector2>> wallKicksI = new List<List<Vector2>>
	{
		new List<Vector2> {new Vector2(-2,0), new Vector2(1,0), new Vector2(-2,-1), new Vector2(1,2)},
		new List<Vector2> {new Vector2(2,0), new Vector2(-1, 0), new Vector2(2,1), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(-1, 0), new Vector2(2,0), new Vector2(-1,2), new Vector2(2, -1)},
		new List<Vector2> {new Vector2(1,0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1)},
		new List<Vector2> {new Vector2(2,0), new Vector2(-1, 0), new Vector2(2,1), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(-2,0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(1,0), new Vector2(-2,0), new Vector2(1, -2), new Vector2(-2,1)},
		new List<Vector2> {new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1,2), new Vector2(2, -1)}
	};

	public static List<List<Vector2>> wallKicks = new List<List<Vector2>> {
		new List<Vector2> {new Vector2(-1,0), new Vector2(-1,1), new Vector2(0,-2), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(1,0), new Vector2(1, -1), new Vector2(0,2), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(1, 0), new Vector2(1,-1), new Vector2(0,2), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(-1,0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(1,0), new Vector2(1, 1), new Vector2(0,-2), new Vector2(1, -2)},
		new List<Vector2> {new Vector2(-1,0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2)},
		new List<Vector2> {new Vector2(-1,0), new Vector2(-1,-1), new Vector2(0, 2), new Vector2(-1, 2)},
		new List<Vector2> {new Vector2(1, 0), new Vector2(1, 1), new Vector2(0,-2), new Vector2(1, -2)}
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

	public static Dictionary<BlockType, Vector2> nextSpawnPositions = new()
	{
		{BlockType.I, new Vector2(0.5f,0)},
		{BlockType.J, new Vector2(1,0.5f)},
		{BlockType.L, new Vector2(1,0.5f)},
		{BlockType.O, new Vector2(1.5f, 0.5f)},
		{BlockType.S, new Vector2(1,0.5f)},
		{BlockType.T, new Vector2(1,0.5f)},
		{BlockType.Z, new Vector2(1,0.5f)}
	};

	public static Dictionary<BlockType, Vector2> gridSpawnPositions = new()
	{
		{BlockType.I, new Vector2(-1f,-1f)},
		{BlockType.J, new Vector2(0f,0f)},
		{BlockType.L, new Vector2(0f,0f)},
		{BlockType.O, new Vector2(1f,0f)},
		{BlockType.S, new Vector2(0f,0f)},
		{BlockType.T, new Vector2(0f,0f)},
		{BlockType.Z, new Vector2(0f,0f)}
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

	public static Dictionary<Vector2, List<Vector2>> rotations = new()
	{
		{LEFT, counterClockwiseRotation},
		{RIGHT, clockwiseRotation}
	};

	public static Dictionary<BlockType, sbyte> colorIndexMapping = new()
	{
		{BlockType.I, 0},
		{BlockType.J, 1},
		{BlockType.L, 2},
		{BlockType.O, 3},
		{BlockType.S, 4},
		{BlockType.T, 5},
		{BlockType.Z, 6}
	};


}



