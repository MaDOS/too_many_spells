using Godot;
using System;

public partial class GameStateManager : Node
{
    [Signal]
    public delegate void GamePauseToggledEventHandler(bool paused);

    public static GameStateManager Instance = null!;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_cancel"))
        {
            EmitSignal(nameof(GamePauseToggled));

            GD.Print("Game pause toggled. Actually puasing is commented out though because it halts the game.");

            // var tree = GetTree();
            // tree.Paused = true;
        }
    }
}
