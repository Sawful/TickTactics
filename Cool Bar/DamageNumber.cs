using Godot;
using System;

public partial class DamageNumber : Label
{
    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        Enemy enemy = (Enemy)GetParent();

        base._Ready();
        Scale = new Vector2(1, 1) / enemy.Scale;
    }

    public void _on_animation_player_animation_finished(StringName anim_name)
	{
		QueueFree();
	}
}
