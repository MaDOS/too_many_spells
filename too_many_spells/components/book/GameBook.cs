using Godot;

public partial class GameBook : Book
{
    [Signal]
    public delegate void SpellCastEventHandler(string spellName);

    public bool AllowedToCast = false;

    private string? draggingSpell = null;
    private bool castOnMouseUp = false;

    public override void _on_gui_input_leftPage(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed && AllowedToCast)
			{
				this.draggingSpell = this.GetLeftPage().SpellName;
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
            }
        }
    }

    public override void _on_gui_input_rightPage(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed && AllowedToCast)
			{
                this.draggingSpell = this.GetRightPage().SpellName;
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
