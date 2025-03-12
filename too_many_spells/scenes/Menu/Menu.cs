using Godot;
using System;

public partial class Menu : Node2D
{
    [Signal]
    public delegate void PlayClickedEventHandler();

    private void _on_BtnPlay_Clicked()
    {
        EmitSignal(nameof(PlayClicked));
    }
}
