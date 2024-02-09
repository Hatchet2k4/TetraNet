using Godot;
using System;
using TetraNet;

public partial class BlockData : Resource
{
    [Export] public Data.BlockType blockType;
    [Export] public Vector2 spawnPosition;
    [Export] Texture blockTexture;
}
