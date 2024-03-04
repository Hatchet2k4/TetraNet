namespace TetraNet;

using Godot;
using static Data;

public partial class HoldGrid : Control
{
	[Export] private TextureRect _holdGrid;
	public Block _holdBlock;

	public override void _Ready()
	{
		SetProcess(false);
	}

	public bool HasBlock()
	{
		return _holdBlock != null;
	}

	public void ClearBlock()
	{
		_holdBlock = null;
	}

	public void SetBlock(Block block)
	{
		if (HasBlock()) RemoveChild(_holdBlock);
		_holdBlock = block;
		_holdBlock.DefaultOrientation();
		AddChild(_holdBlock);
		Populate();
	}

	public Block GetBlock()
	{

		return _holdBlock;
	}

	public void Populate()
	{
		if (HasBlock())
		{
			_holdBlock.Position = _holdGrid.Position + nextSpawnPositions[_holdBlock.BlockType] * GRID_SIZE;
		}
	}

}
