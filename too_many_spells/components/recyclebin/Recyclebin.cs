using Godot;

public partial class Recyclebin : Node2D
{
    [Signal]
    public delegate void OnRecyclebinEnteredEventHandler();

    [Signal]
    public delegate void OnRecyclebinExitedEventHandler();

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
