using Godot;
using System;
using System.Threading;
using System.Timers;

public partial class Tick : Sprite2D
{
	Vector2 InitialPosition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitialPosition = Position;
	}

	public void _on_animation_player_animation_finished(StringName AnimationName)
	{
		if (AnimationName == "Death")
		{
			QueueFree();
		}
	}
}
