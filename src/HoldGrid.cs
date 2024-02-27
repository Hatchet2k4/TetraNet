namespace TetraNet;

using Godot;


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
		_holdBlock = block;
	}

	public Block GetBlock()
	{
		return _holdBlock;
	}

	public Block SwapBlock(Block block)
	{
		Block returnBlock = _holdBlock;
		_holdBlock = block;
		return returnBlock;
	}

}
