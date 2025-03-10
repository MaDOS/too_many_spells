using Godot;

public partial class DmPrompt : Node2D
{
    [Signal] public delegate void OKClickedEventHandler();

    Label _promptLabel = null!;

    public override void _Ready()
    {
        this._promptLabel = GetNode<Label>("Background/Prompt");
        this._promptLabel.Text = "Are you sure?";
    }

    public void OnOkClicked()
    {
        EmitSignal(nameof(OKClicked));
    }
}
