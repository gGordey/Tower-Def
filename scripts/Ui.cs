using System;
using Godot;

public partial class Ui : Control
{
	public Control LoseScrean;
	public Control InGame;

	public override void _Ready()
	{
		InGame = GetNode<Control>("InGame");
		LoseScrean = GetNode<Control>("LoseScrean");
	}
}
