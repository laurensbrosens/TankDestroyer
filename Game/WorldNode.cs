using System;
using Godot;
using Godot.Collections;
using TankDestroyer.API;
using TankDestroyer.Engine;

namespace TankDestroyer;

public partial class WorldNode : Node3D
{
	[Export] public GridMap GridMap { get; set; }
	[Export] public PackedScene TreeScene { get; set; }
	[Export] public Array<Mesh> Buildings { get; set; } = new();
	[Export] public Material BuildingMaterial { get; set; }

	public World World { get; set; }


	public override void _Ready()
	{
		base._Ready();
	}

	public void InitializeWorld()
	{
		GridMap.Clear();
		for (int y = 0; y < World.Height; y++)
		{
			for (int x = 0; x < World.Width; x++)
			{
				GridMap.SetCellItem(new Vector3I(x, 0, y), (int)World.GetTile(x, y).TileType);
			}
		}

		SpawnTrees();
		SpawnBuildings();
	}

	private void SpawnBuildings()
	{
		Vector3[] positions =
		[
			new Vector3(0.5f, 0, 0.5f),
			new Vector3(0.5f, 0, 0f),
			new Vector3(-0.5f, 0, -0.5f)
		];
		Random rand = new();
		float[] rotations = [0, 90, 180, 270];
		for (int y = 0; y < World.Height; y++)
		{
			for (int x = 0; x < World.Width; x++)
			{
				if (World.GetTile(x, y).TileType == TileType.Building)
				{
					for (int i = 0; i < 3; i++)
					{
						var node = new MeshInstance3D();
						node.MaterialOverride = BuildingMaterial;
						node.Mesh = Buildings[rand.Next(0, Buildings.Count)];
						GridMap.AddChild(node);
						node.GlobalPosition = new Vector3(x * 2f + 1f, 1.5f,
							y * 2f + 1f) + positions[i];
						node.GlobalRotationDegrees = new Vector3(0, rotations[rand.Next(0, rotations.Length)], 0);
						node.Scale *= 0.02f;
					}
				}
			}
		}
	}

	private void SpawnTrees()
	{
		Vector3[] positions =
		[
			new Vector3(0.5f, 0, 0.5f),
			new Vector3(0.5f, 0, 0f),
			new Vector3(-0.5f, 0, -0.5f)
		];
		Random rand = new();
		for (int y = 0; y < World.Height; y++)
		{
			for (int x = 0; x < World.Width; x++)
			{
				if (World.GetTile(x, y).TileType == TileType.Tree)
				{
					for (int i = 0; i < positions.Length; i++)
					{
						var node = TreeScene.Instantiate<Node3D>();
						GridMap.AddChild(node);
						node.GlobalPosition = new Vector3(x * 2f + 1f, 1.5f,
							y * 2f + 1f) + positions[i];
						node.GlobalRotationDegrees = new Vector3(0, (float)rand.NextDouble() * 360f, 0);
						var scale = (float)rand.NextDouble() + 0.5f;
						node.Scale *= scale;
					}
				}
			}
		}
	}
}
