using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	Label HealthLabel;
	public override void _Ready()
	{
		HealthLabel = GetNode<Label>("Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HealthLabel.Text = "Health: " + Value.ToString() + " / " + MaxValue.ToString();
    }
}
