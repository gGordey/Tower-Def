using Godot;
using System;

public partial class Tile : StaticBody3D
{
	public RayCast3D RayCast;
	public TileInterface Interface;
	public Vector2 ind;

 	public bool IsCovered;

	public override void _Ready()
	{
		IsCovered = false;
		RayCast = GetNode<RayCast3D>("RayCast");
		Interface = GetNode<TileInterface>("TileInterface");
		Interface.Position += new Vector3 (0, 0.05f, 0);
	}

	public void TowerPickedUp()
	{
		ProcessMode = ProcessModeEnum.Always;
	}
	public void TowerPlaced()
	{
		ProcessMode = ProcessModeEnum.Disabled;
	}

	public override void _Process(double delta)
	{
		if (CheckCollision())
		{
			Interface.Activate();
		}
		else { Interface.Deactivate(); }
	}

	public bool CheckCollision()
	{
		if (!RayCast.IsColliding()) { return false; }
		Node3D Collision = (Node3D)RayCast.GetCollider();
		if (Collision != null && Collision.IsInGroup("tower"))  { return true; } 
		return false;
	}

	public void Cover()
	{
		RayCast.Enabled = false;
	}
}
