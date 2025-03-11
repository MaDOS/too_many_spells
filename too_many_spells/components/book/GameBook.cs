using Godot;

public partial class GameBook : Book
{
    [Signal]
    public delegate void SpellCastEventHandler(string spellName);

    public bool AllowedToCast = false;

    private string? draggingSpell = null;
    private bool castOnMouseUp = false;

    private PackedScene _spellTrailScene = GD.Load<PackedScene>("res://components/spelltrail/SpellTrail.tscn");
    private Node2D _spellTrail = null!;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if(this.draggingSpell is not null)
        {
            _spellTrail.GlobalPosition = GetGlobalMousePosition();
        }
    }

    public void ActiveSpellTrail()
    {
        _spellTrail = _spellTrailScene.Instantiate<Node2D>();
        AddChild(_spellTrail);
    }

    public void DeactivateSpellTrail()
    {
        _spellTrail.QueueFree();
        RemoveChild(_spellTrail);
    }

    public override void _on_gui_input_leftPage(InputEvent @event)
    {
        base._on_gui_input_leftPage(@event);

        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed && AllowedToCast)
			{
				this.draggingSpell = this.GetLeftPage().SpellName;
                this.ActiveSpellTrail();
			}
            else
            {
                if(this.castOnMouseUp)
                {
                    AllowedToCast = false;
                    EmitSignal(nameof(SpellCast), this.draggingSpell!);
                }
                else
                {
                    this.draggingSpell = null;
                }

                this.DeactivateSpellTrail();
            }
        }
    }

    public override void _on_gui_input_rightPage(InputEvent @event)
    {
        base._on_gui_input_rightPage(@event);

        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed && AllowedToCast)
			{
                this.draggingSpell = this.GetRightPage().SpellName;
                this.ActiveSpellTrail();
            }
            else
            {
                if(this.castOnMouseUp)
                {
                    AllowedToCast = false;
                    EmitSignal(nameof(SpellCast), this.draggingSpell!);
                }
                else
                {
                    this.draggingSpell = null;
                }

                this.DeactivateSpellTrail();
            }
        }
    }

    public override void _on_mouse_entered_leftPage()
    {
        if(this.draggingSpell is not null)
        {
            this.castOnMouseUp = false;
        }
    }

    public override void _on_mouse_exited_leftPage()
    {
        if (this.draggingSpell is not null)
        {
            this.castOnMouseUp = true;
        }
    }

    public override void _on_mouse_entered_rightPage()
    {
        if(this.draggingSpell is not null)
        {
            this.castOnMouseUp = false;
        }
    }

    public override void _on_mouse_exited_rightPage()
    {
        if (this.draggingSpell is not null)
        {
            this.castOnMouseUp = true;
        }
    }
}
