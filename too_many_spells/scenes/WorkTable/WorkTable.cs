using Godot;
using System;

public partial class WorkTable : Node2D
{
    [Signal]
    public delegate void NextSessionEventHandler();

    private WorkBook _workBook = null!;
    private Label _lblSwapsRemaining = null!;

    private int _swapsRemaining = 5;

    public override void _Ready()
    {
        base._Ready();

        _workBook = GetNode<WorkBook>("Book");
        _lblSwapsRemaining = GetNode<Label>("LblSwapsRemaining");

        _workBook.PageSwap += this.OnPageSwap;
    }

    private void _on_BtnNextSession_pressed()
    {
        EmitSignal(nameof(NextSession));
    }

    private void OnPageSwap()
    {
        this._swapsRemaining--;

        if(this._swapsRemaining == 0)
        {
            this._workBook.DisableSwapping();
        }
        _lblSwapsRemaining.Text = $"Swaps remaining: {_swapsRemaining}";
    }
}
