using Godot;
using System;

public partial class EnemySpewner : Node3D
{
	private PackedScene[] PackedEnemyList = 
	{
		ResourceLoader.Load<PackedScene>("res://scenes/classes/enemy_class.tscn"),
	};

	public Vector3 spawnPos;

	private Global Global;

	public override void _Ready()
	{
		Global = GetNode<Global>("/root/Global");
		Global.EnemySpawner = this;
	}

	public override void _Process(double delta)
	{
		if (spawnPos == new Vector3(0,0,0) && Global.CurrentGrid != null)
		{
			InitPos();
		}
	}

	private void InitPos()
	{
		GD.Print(Global.CurrentGrid.PathTiles[0]);
		Vector2 zeroTile = Global.CurrentGrid.PathTiles[0];
		float tile_size = Global.CurrentGrid.tile_size;
		float tiles_offset = Global.CurrentGrid.tiles_offset;
		spawnPos = new Vector3(-2.2f ,0 ,(zeroTile.Y-1)*tile_size+(zeroTile.Y-1)*tiles_offset) + Global.CurrentGrid.GlobalPosition;
		SpawnEnemy();
	}

	public void SpawnEnemy()
	{
		EnemyClass NewEnemy = PackedEnemyList[Global.RandiRange(0, PackedEnemyList.Length-1)].Instantiate<EnemyClass>();
		AddChild(NewEnemy);
		NewEnemy.Position = spawnPos;
	}
}
