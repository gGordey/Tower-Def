using Godot;
using System;

public partial class EnemyRotater : Area3D
{
	public string rot; // {set { if (value != "right") { RotateY(90); } rot = value; } get { return rot; }}

	public override void _Ready()
	{
		BodyEntered += RotateEname;
		if (rot == "right")
		{
			Rotation = new Vector3 (0, 1.57f, 0);
		}
		if (Position.X < -2) { QueueFree(); }
	}

	public void RotateEname(Node3D Body)
	{
		if (Body.IsInGroup("enemy"))
		{
			EnemyClass Enemy = (EnemyClass)Body;
			Enemy.MovingDir = rot; 
		}
	}
}
