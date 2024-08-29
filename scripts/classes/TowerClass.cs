using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

[GlobalClass]
public partial class TowerClass : StaticBody3D
{ 
	// export varriables
	[Export] public Vector2 size { get; set; }
	[Export] public float life_time { get; set; }
	// node links
	public Global Global;
	public Timer LifeTimer;
	public DragAndDrop Dragger;
	public GridPlace CurrentGrid;
	public TowerClass Self;
	// changable varriables
	public int level;
	public bool is_placed;
	public bool is_ready_to_place;
	public override void _Ready()
	{
		Self = GetChild(0).GetParent<TowerClass>();
		Dragger = new DragAndDrop ();
		AddChild (Dragger);
		LifeTimer = GetNode<Timer>("life_timer");
		LifeTimer.Timeout += Die;
		is_placed = false;
		is_ready_to_place = false;
		Global = GetNode<Global>("/root/Global");
		Global.CurrentGrid.IsTowerPicked = false;
		InhereitReady();
	}

	public virtual void InhereitReady()
	{

	}


	public override void _Process(double delta)
	{
		if (Global.IsLosed) { return; }
		if (Input.IsActionPressed("mouse") && !is_placed)
		{
			Global.CurrentGrid.IsTowerPicked = true;
			is_ready_to_place = true;
			Dragger.is_activated = true;
		}
		if (Input.IsActionJustReleased("mouse") && is_ready_to_place)//&& Global.CurrentGrid.IsTowerPicked)
		{
			Place();
			Dragger.is_activated = false;
			Global.CurrentGrid.IsTowerPicked = true;
			is_ready_to_place = false;
		}
	}

	public void WhenPlaced()
	{
		LifeTimer.Start();
	}

	public void LevelUp()
	{
		level++;
		LifeTimer.Stop();
		LifeTimer.WaitTime = life_time;
		LifeTimer.Start();
	}

	public void Die() 
	{
	}

	public void Place()
	{
		is_placed = true;
		Global.CurrentGrid.Place(this);
	}
	public void CouldNotPlace()
	{
		QueueFree();
	}
}
