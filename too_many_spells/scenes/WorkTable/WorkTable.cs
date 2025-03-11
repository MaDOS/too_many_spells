using Godot;
using System;

public partial class WorkTable : Node2D
{
    public void _on_BtnNextSession_pressed()
    {
        GD.Print("Next session");
        GetTree().ChangeSceneToFile("res://scenes/GameTable/game_table.tscn");
    }
}
