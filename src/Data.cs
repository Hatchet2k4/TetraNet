namespace TetraNet;

using Godot;
using System;
using System.Collections.Generic;

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

    public static List<List<Vector2>> wall_kicks_i = new List<List<Vector2>>
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

	public static List<List<Vector2>> wall_kicks_jlostz = new List<List<Vector2>> {
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1,1), new Vector2(0,-2), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1,0), new Vector2(1, -1), new Vector2(0,2), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1, 0), new Vector2(1,-1), new Vector2(0,2), new Vector2(1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1,0), new Vector2(1, 1), new Vector2(0,-2), new Vector2(1, -2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(-1,0), new Vector2(-1,-1), new Vector2(0, 2), new Vector2(-1, 2)},
		new List<Vector2> {new Vector2(0,0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0,-2), new Vector2(1, -2)}
	};

}
