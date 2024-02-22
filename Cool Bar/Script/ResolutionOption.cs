using Godot;
using System;

public partial class ResolutionOption : OptionButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        AddItem("1920x1080", 0);
        AddItem("1600x900", 1);
        AddItem("1366x768", 2);
        Select(1);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void _on_item_selected(int index)
	{
		if (index == 0) 
		{
            Vector2I size = new Vector2I(1920, 1080);
            DisplayServer.WindowSetSize(size);
        }
		else if (index == 1)
        {
            Vector2I size = new Vector2I(1600, 900);
            DisplayServer.WindowSetSize(size);
        }
    
		else if (index == 2)
        {
            Vector2I size = new Vector2I(1366, 768);
            DisplayServer.WindowSetSize(size);
        }
    }
}
