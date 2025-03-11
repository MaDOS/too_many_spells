using Godot;
using System;

public partial class WorkTable : Node2D
{
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
        GD.Print("Next session");
        GetTree().ChangeSceneToFile("res://scenes/GameTable/game_table.tscn");
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
