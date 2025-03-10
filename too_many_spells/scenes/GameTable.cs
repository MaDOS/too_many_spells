using Godot;
using System;

public partial class GameTable : Node2D
{
    private DmPrompt _prompt = null!;

    public override void _Ready()
    {
        _prompt = GetNode<DmPrompt>("DmPrompt");
        _prompt.OKClicked += OnPromptOKClicked;        
    }
    
    private void OnPromptOKClicked()
    {
        GD.Print("OK clicked");
        _prompt.Hide();
    }
}
