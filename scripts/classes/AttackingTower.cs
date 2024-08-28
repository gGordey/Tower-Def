using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AttackingTower : TowerClass
{
	[Export] private int damage;
	[Export] public float AttackTime;
	[Export] public int AttackRange = 1;

	public Area3D AttackingArea;

	private List<EnemyClass> CurrentEnemeis = new();

	public override void InhereitReady()
	{
		AttackingArea = GetNode<Area3D>("AttackArea");
		AttackingArea.BodyEntered += Entr;
		AttackingArea.BodyExited += Exit;
		AttackingArea.Scale = new Vector3 (AttackRange*0.3f, 1, AttackRange*0.3f);
		Timer AttackTimer = new Timer();
		AttackTimer.WaitTime = AttackTime;
		AddChild(AttackTimer);
		AttackTimer.Timeout += Attack;
		AttackTimer.Start();
	}

	public void Attack()
	{
		if (CurrentEnemeis.Count > 0)
		{
			CurrentEnemeis[0].CurentHp -= damage;
			if (CurrentEnemeis[0].CurentHp <= 0)
			{
				CurrentEnemeis.Remove(CurrentEnemeis[0]);
			}
		}
	}

	public void Entr(Node3D Body)
	{
		if (Body.IsInGroup("enemy"))
		{
			CurrentEnemeis.Add((EnemyClass)Body);
		}
	}
	public void Exit(Node3D Body)
	{
		if (Body.IsInGroup("enemy"))
		{
			CurrentEnemeis.Remove((EnemyClass)Body);
		}
	}
}
