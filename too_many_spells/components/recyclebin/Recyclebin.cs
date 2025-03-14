using Godot;

public partial class Recyclebin : Node2D
{
    [Signal]
    public delegate void OnRecyclebinEnteredEventHandler();

    [Signal]
    public delegate void OnRecyclebinExitedEventHandler();

    private AnimatedSprite2D _trashCanSprite = null!;
    private Sprite2D _glowSprite = null!;

    public override void _Ready()
    {
        base._Ready();

        _trashCanSprite = GetNode<AnimatedSprite2D>("TrashCanSprite");
        _glowSprite = GetNode<Sprite2D>("GlowSprite");
    }

    public void NomNomNm()
    {
		GameStateManager.Instance.FirePlayEffect(SoundEffects.Trashcan);
        GD.Print("Nom nom nm");
        _trashCanSprite.Play("nomnomnom");
    }

    public void OnRecyclebinAreaEntered()
    {
        GD.Print("Recyclebin entered");
        EmitSignal(nameof(OnRecyclebinEntered));
        _glowSprite.Visible = true;
    }

    public void OnRecyclebinAreaExited()
    {
        GD.Print("Recyclebin exited");
        EmitSignal(nameof(OnRecyclebinExited));
        _glowSprite.Visible = false;
    }
}
