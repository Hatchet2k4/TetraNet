namespace TetraNet;

using Godot;
using static Data;

 [GlobalClass]
public partial class BlockData : Resource
{
    [Export] public BlockType blockType { get; set; }
    [Export] public Vector2 spawnPosition { get; set; }
    [Export] public Texture2D blockTexture { get; set; }
}
