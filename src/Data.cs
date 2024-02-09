namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;


public partial class Data : Node
{
	public enum BlockType
	{
		I, O, T, J, L, S, Z
	}

	public static List<Vector2> I = new() { new Vector2(-1, 0), new Vector2(0,0), new Vector2(1, 0), new Vector2(2,0) };
	public static List<Vector2> J = new() { new Vector2(-1, -1), new Vector2(-1,0), new Vector2(0, 0), new Vector2(1,0) };
	public static List<Vector2> L = new() { new Vector2(1, 1), new Vector2(-1,0), new Vector2(0, 0), new Vector2(1,0) };
	public static List<Vector2> O = new() { new Vector2(0, 1), new Vector2(1,1), new Vector2(0, 0), new Vector2(1,0) };
	public static List<Vector2> S = new() { new Vector2(0, 1), new Vector2(1,1), new Vector2(-1, 0), new Vector2(0,0) };
	public static List<Vector2> T = new() { new Vector2(0, 1), new Vector2(-1,0), new Vector2(0, 0), new Vector2(1,0) };
	public static List<Vector2> Z = new() { new Vector2(-1, 1), new Vector2(0, 1), new Vector2(0, 0), new Vector2(1,0) };

	public static Dictionary<BlockType, List<Vector2>> Cells = new()
	{
		{BlockType.I, I},
		{BlockType.J, J},
		{BlockType.L, L},
		{BlockType.O, O},
		{BlockType.S, S},
		{BlockType.T, T},
		{BlockType.Z, Z}
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

	public static Dictionary<BlockType, Resource> blockResources = new()
	{
		{BlockType.I, GD.Load("res://resources/I.tres")},
		{BlockType.J, GD.Load("res://resources/J.tres")},
		{BlockType.L, GD.Load("res://resources/L.tres")},
		{BlockType.O, GD.Load("res://resources/O.tres")},
		{BlockType.S, GD.Load("res://resources/S.tres")},
		{BlockType.T, GD.Load("res://resources/T.tres")},
		{BlockType.Z, GD.Load("res://resources/Z.tres")}  		
	};

}
