using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameBook : Book
{
    [Signal]
    public delegate void SpellCastEventHandler(string spellName);

    public bool AllowedToCast = false;

    private string? _draggingSpell = null;
    private bool castOnMouseUp = false;

    private PackedScene _spellTrailScene = GD.Load<PackedScene>("res://components/spelltrail/SpellTrail.tscn");
    private Node2D? _spellTrail = null;

    public override void _Ready()
    {
        base._Ready();

		Player.Instance.LevelUp += OnPlayerLevelUp;

        var GameTableNode = GetNode("GameTable");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if(this._draggingSpell is not null && this._spellTrail is not null)
        {
            _spellTrail.GlobalPosition = GetGlobalMousePosition();
        }
    }    

	private void OnPlayerLevelUp(int level)
	{
		GD.Print("OnPlayerLevelUp");

		var levelDef = Player.Instance.LevelDefintions
			.FirstOrDefault(lvlDef => lvlDef.Level == level);

		foreach (var spellname in levelDef?.AddedSpells ?? new List<string>())
		{
			GD.Print($"Adding spell {spellname}");

			Spells.Spell spell = Spells.Instance.GetSpell(spellname)!;

			_pages.Insert(GD.RandRange(0, _pages.Count - 1), new Page(spell.Name, spell.Artwork));
		}

		this.CleanBook();
		this.SavePages();
	}

    public void ActiveSpellTrail()
    {
        _spellTrail = _spellTrailScene.Instantiate<Node2D>();
        AddChild(_spellTrail);
    }

    public void DeactivateSpellTrail()
    {
        if(_spellTrail is not null)
        {
            RemoveChild(_spellTrail);
            _spellTrail = null;
        }
    }

    public override void _on_gui_input_leftPage(InputEvent @event)
    {
        base._on_gui_input_leftPage(@event);

        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed && AllowedToCast)
			{
				this._draggingSpell = this.GetLeftPage().SpellName;
                this.ActiveSpellTrail();
			}
            else
            {
                if(this.castOnMouseUp)
                {
                    AllowedToCast = false;
                    EmitSignal(nameof(SpellCast), this._draggingSpell!);
                }
                else
                {
                    this._draggingSpell = null;
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
                this._draggingSpell = this.GetRightPage().SpellName;
                this.ActiveSpellTrail();
            }
            else
            {
                if(this.castOnMouseUp)
                {
                    AllowedToCast = false;
                    EmitSignal(nameof(SpellCast), this._draggingSpell!);
                }
                else
                {
                    this._draggingSpell = null;
                }

                this.DeactivateSpellTrail();
            }
        }
    }

    public override void _on_mouse_entered_leftPage()
    {
        if(this._draggingSpell is not null)
        {
            this.castOnMouseUp = false;
        }
    }

    public override void _on_mouse_exited_leftPage()
    {
        if (this._draggingSpell is not null)
        {
            this.castOnMouseUp = true;
        }
    }

    public override void _on_mouse_entered_rightPage()
    {
        if(this._draggingSpell is not null)
        {
            this.castOnMouseUp = false;
        }
    }

    public override void _on_mouse_exited_rightPage()
    {
        if (this._draggingSpell is not null)
        {
            this.castOnMouseUp = true;
        }
    }
}
