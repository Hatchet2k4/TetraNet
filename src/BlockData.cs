namespace TetraNet;

using Godot;
using static Data;

public partial class BlockData : Resource
{
    [Export] public BlockType blockType;
    [Export] public Vector2 spawnPosition;
    [Export] Texture blockTexture;
}
