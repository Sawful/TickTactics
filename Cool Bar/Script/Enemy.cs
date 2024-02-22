using Godot;
using System;

public partial class Enemy : AnimatedSprite2D
{
	StringName LastAnimation;
    public AnimationPlayer Player;
    public AudioStreamPlayer AudioStreamPlayer;
    
    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        Player = GetNode<AnimationPlayer>("AnimationPlayer");
        AudioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        base._Ready();
    }

    public void _on_animation_changed()
    {
		LastAnimation = Animation;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _on_animation_finished()
    {
		if(LastAnimation == "Attack")
		{
            Play("Idle");
        }

        else if (LastAnimation == "TakeHit")
		{
            Play("Idle");
        }

        else if (LastAnimation == "Death")
        {
            QueueFree();
        }
	}
}
