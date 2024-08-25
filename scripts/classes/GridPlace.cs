using Godot;
using Godot.NativeInterop;
using System;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Collections.Generic;
using System.Linq;

public partial class GridPlace : Node3D
{
	[Export] public Vector2 tiles_count { get; set; }
	[Export] public PackedScene TileScene { get; set; }
	[Export] public float tile_size = 0.34f;
	[Export] public float tiles_offset = 0.05f;

	public bool IsTowerPicked
	{
		set
		{
			foreach (Node i in GetChildren())
			{
				if (i.IsInGroup("tile"))
				{
					Tile tile = (Tile)i;
					if (value)
					{
						tile.TowerPickedUp();
					}
					else
					{
						tile.TowerPlaced();
					}
				}
			}
		}
		get { return IsTowerPicked;}
	}

	public List<Vector2> FreeTiles = new ();
	public List<Vector2> PathTiles = new ();
	public List<Vector2> ActivatedTiles = new();

 	private Global Global;

	private PackedScene EnemyRotater = ResourceLoader.Load<PackedScene>("res://scenes/classes/enemy_rotater.tscn");

	public override void _Ready()
	{
		Global = GetNode<Global>("/root/Global");
		Global.CurrentGrid = this;
		PathGenerator(tiles_count);
		Global.GameSpace = GetParent<Node3D>();
	}

	public override void _Process(double delta)
	{
		if (GetChildCount()==0 && tiles_count != new Vector2 (0,0)){
			FillFreeTiles();
			CreateGrid();
			ClearPath();
		}
	}

	public void FillFreeTiles()
	{
		for (int x = 0; x < tiles_count.X; x++)
		{
			for (int y = 0; y < tiles_count.Y; y++)
			{
				FreeTiles.Add(new Vector2(x, y));
			}
		}
	}
	
	public void CreateGrid()
	{
		for (int x = 0; x < tiles_count.X; x++)
		{
			for (int y = 0; y < tiles_count.Y; y++)
			{
				Tile NewTile = (Tile)TileScene.Instantiate();
				AddChild(NewTile);
				NewTile.Position = new Vector3(x*tile_size+x*tiles_offset,0,y*tile_size+y*tiles_offset);
				NewTile.ind = new Vector2 (x+1, y+1);	
			}
		}
	}

	public bool IsTileFree(Vector2 tile)
	{
		foreach (Vector2 i in FreeTiles)
		{
			if (tile == i) { return true; }
		}
		return false;
	}

	public void TileActivated(Vector2 ind)
	{
		ActivatedTiles.Add(ind);
	}

	public void TileDeactivated(Vector2 ind)
	{
		ActivatedTiles.Remove(ind);
	}

	public void Place(StaticBody3D TowerBody)
	{
		TowerClass Tower = (TowerClass)TowerBody;
		foreach (Tile i in GetChildren())
		{
			if (!i.IsCovered && i.CheckCollision()) {TileActivated(i.ind);}
		}
		Vector2 smallest_tile = new Vector2 (0,0);
		float pos_sum = 1000.0f;
		for (int i = 0; i<ActivatedTiles.Count; i++)
		{
			GD.Print(ActivatedTiles[i]);
			if ((ActivatedTiles[i].X + ActivatedTiles[i].Y + 2) < pos_sum)
			{
				pos_sum = ActivatedTiles[i].X+ActivatedTiles[i].Y;
				smallest_tile = ActivatedTiles[i];
			}
		}
		smallest_tile -= new Vector2(1,1);
		Vector2 biggest_tile = new Vector2 (smallest_tile.X+(Tower.size.X-1), smallest_tile.Y+(Tower.size.Y-1));
		List<Vector2> CoveredTiles = new();
		for (float x = smallest_tile.X; x <= biggest_tile.X; x++)
		{
			for (float y = smallest_tile.Y; y <= biggest_tile.Y; y++)
			{
				if (!IsTileFree(new Vector2(x, y))) 
				{ 
					Tower.CouldNotPlace(); 
					ActivatedTiles.Clear();
					return; 
				}
				CoveredTiles.Add(new Vector2(x,y));
			}
		}
		foreach (Vector2 i in CoveredTiles)
		{
			FreeTiles.Remove(i);
		}
		foreach (Tile i in GetChildren())
		{
			foreach (Vector2 cvr in CoveredTiles)
			{
				if (i.ind == cvr) { i.IsCovered = true; }
			}
			TileDeactivated(i.ind);
		}
		Tower.is_placed = true;
		Tower.Position = Position;
		Tower.Position += new Vector3 (
			(biggest_tile.X + smallest_tile.X)/2 * tile_size + (biggest_tile.X + smallest_tile.X)/2 *  tiles_offset, 0.2f, (biggest_tile.Y + smallest_tile.Y)/2 * tile_size + (biggest_tile.Y + smallest_tile.Y)/2 *  tiles_offset
		);
	}

	public void PathGenerator(Vector2 BoardSize)
	{
		Vector2 currentPos;
		Dictionary<string, Vector2> dirs = new()
		{
			{"up", new Vector2(0,-1)},
			{"down", new Vector2(0,1)},
			{"right", new Vector2(1,0)}
		};
		string prevDir = "right";
		string currentDir = "right";
		string RandomDir()
		{
			string[] dirsStr = {"up", "down", "right"};
			if (prevDir == "up" || prevDir == "down") { return "right"; }
			if (currentPos.Y >= BoardSize.Y/2+1) { return "up"; }
			return "down";
		}
		currentPos = new Vector2 (1, Global.RandiRange(1, (int)BoardSize.Y-1));
		while (currentPos.X <= BoardSize.X)
		{
			int steps = Global.RandiRange(2, (int)BoardSize.Y/2+1);
			for (int i = 0; i < steps; i++)
			{
				PathTiles.Add(currentPos);
				if ((currentPos.Y + dirs[currentDir].Y <= 0) || (currentPos.Y + dirs[currentDir].Y)>=BoardSize.Y-1) { break; }
				currentPos += dirs[currentDir];
			}
			prevDir = currentDir;
			while (currentDir == prevDir)
			{
				currentDir = RandomDir();
				if (currentPos.X <= BoardSize.X)
				{
					NewRotater(currentPos, currentDir);
				}
			}
		}
		ClearPath();
	}

	public void NewRotater(Vector2 Tile, string rot)
	{
		EnemyRotater NewRot = EnemyRotater.Instantiate<EnemyRotater>();
		NewRot.Position = GlobalPosition + new Vector3((Tile.X-1)*tile_size+(Tile.X-1)*tiles_offset, 0, (Tile.Y-1)*tile_size+(Tile.Y-1)*tiles_offset);
		NewRot.rot = rot;
		GetParent().GetNode("EnemyRotaters").AddChild(NewRot);
	}

	public void ClearPath()
	{
		foreach (Vector2 i in PathTiles)
		{
			FreeTiles.Remove(new Vector2(i.X-1, i.Y-1));
			foreach (Tile Child in GetChildren())
			{
				if (Child.ind == i)
				{
					Child.QueueFree();
				}
			}
		}
	}
}
