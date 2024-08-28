using System;
using System.Threading;
using Godot;

public partial class Castle : StaticBody3D
{
	private Global Global;
	private Area3D EnterArea;

	private int _hp = 5;
	public int HP 
	{
		get {return _hp;}
		set 
		{
			_hp = value;
			if (_hp <= 0)
			{
				GD.Print("Damge");
			}
		}
	}


	public override void _Ready()
	{
		Global = GetNode<Global>("/root/Global");
		EnterArea = GetNode<Area3D>("Area3D");
        EnterArea.BodyEntered += Entr;
		Position = new Vector3(Global.CurrentGrid.tiles_count.X*Global.CurrentGrid.tile_size,0,0);
	}
	public void Entr(Node3D Body)
	{
		if (Body.IsInGroup("enemy"))
		{
			HP--;
			Body.QueueFree();
		}
	}
}
