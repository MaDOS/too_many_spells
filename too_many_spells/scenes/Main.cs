using Godot;
using System;

public partial class Main : Node2D
{
    private PackedScene _spellTrailScene = GD.Load<PackedScene>("res://components/spelltrail/SpellTrail.tscn");

    private Node2D _spellTrail = null!;

    public override void _Ready()
    {
        _spellTrail = _spellTrailScene.Instantiate<Node2D>();
        AddChild(_spellTrail);
    }

    public override void _PhysicsProcess(double delta)
    {
        _spellTrail.GlobalPosition = GetGlobalMousePosition();
    }
}
