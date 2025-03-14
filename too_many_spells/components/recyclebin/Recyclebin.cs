using Godot;

public partial class Recyclebin : Node2D
{
    [Signal]
    public delegate void OnRecyclebinEnteredEventHandler();

    [Signal]
    public delegate void OnRecyclebinExitedEventHandler();

    private AnimatedSprite2D _trashCanSprite = null!;

    public override void _Ready()
    {
        base._Ready();

        _trashCanSprite = GetNode<AnimatedSprite2D>("TrashCanSprite");
    }

    public void NomNomNm()
    {
        GD.Print("Nom nom nm");
        _trashCanSprite.Play("nomnomnom");
    }

    public void OnRecyclebinAreaEntered()
    {
        GD.Print("Recyclebin entered");
        EmitSignal(nameof(OnRecyclebinEntered));
    }

    public void OnRecyclebinAreaExited()
    {
        GD.Print("Recyclebin exited");
        EmitSignal(nameof(OnRecyclebinExited));}
}
