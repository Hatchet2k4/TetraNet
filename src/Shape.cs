namespace TetraNet;

using System.Collections.Generic;
using Godot;
using static Data;

public class Shape
{
	public int w;
	public int h;
	public int numrotations;
	public int orientation = 0;
	public int[][,] data;

	public int GetBlock(int x, int y)
	{
		return data[orientation][y, x];
	}

	public void Rotate(Vector2 direction)
	{
		if (direction == Data.RIGHT)
		{
			orientation++;
			if (orientation == numrotations) orientation = 0;
		}
		else
		{
			orientation--;
			if (orientation < 0) orientation = numrotations - 1;
		}
	}

	public List<Vector2> GetCells()
	{
		List<Vector2> cells = new();
		for (int y = 0; y < h; y++)
		{
			for (int x = 0; x < w; x++)
			{
				if (GetBlock(x, y) > 0) cells.Add(new Vector2(x, y));
			}
		}
		return cells;
	}

	public Shape(BlockType t)
	{
		if (t == BlockType.O)
		{
			data = new int[][,] { new int[,] { { 1, 1 }, { 1, 1 } } };
			numrotations = 1;
		}

		if (t == BlockType.I)
		{
			numrotations = 4;
			data = new int[][,] {
				new int[,] { { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } },
				new int[,] { { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 } },
				new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 } },
				new int[,] { { 0, 1, 0, 0 }, { 0, 1, 0, 0 }, { 0, 1, 0, 0 }, { 0, 1, 0, 0 } }
			};
		}

		if (t == BlockType.T)
		{
			numrotations = 4;
			data = new int[][,] {
				new int[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } },
				new int[,] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 1, 0 } },
				new int[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 1, 0 } },
				new int[,] { { 0, 1, 0 }, { 1, 1, 0 }, { 0, 1, 0 } }
			};
		}

		if (t == BlockType.L)
		{
			numrotations = 4;
			data = new int[][,] {
				new int[,] { { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } },
				new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 1 } },
				new int[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 0 } },
				new int[,] { { 1, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } }
			};
		}
		if (t == BlockType.J)
		{
			numrotations = 4;
			data = new int[][,] {
				new int[,] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } },
				new int[,] { { 0, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } },
				new int[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 } },
				new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 1, 1, 0 } }
			};
		}

		if (t == BlockType.S)
		{
			numrotations = 4;
			data = new int[][,] {
				new int[,] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } },
				new int[,] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 0, 1 } },
				new int[,] { { 0, 0, 0 }, { 0, 1, 1 }, { 1, 1, 0 } },
				new int[,] { { 1, 0, 0 }, { 1, 1, 0 }, { 0, 1, 0 } }
			};
		}

		if (t == BlockType.Z)
		{
			numrotations = 4;
			data = new int[][,] {
				new int[,] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } },
				new int[,] { { 0, 0, 1 }, { 0, 1, 1 }, { 0, 1, 0 } },
				new int[,] { { 0, 0, 0 }, { 1, 1, 0 }, { 0, 1, 1 } },
				new int[,] { { 0, 1, 0 }, { 1, 1, 0 }, { 1, 0, 0 } }
			};
		}

		w = data[0].GetLength(0); //set width and height based on shape data
		h = data[0].GetLength(1);
	}
}
