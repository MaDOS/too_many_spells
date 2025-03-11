using Godot;

public partial class GameTable : Node2D
{
    Dialogbox _dialogbox = null!;

    public override void _Ready() 
    {
        _dialogbox = GetNode<Dialogbox>("Dialogbox");

        var gmPrompt = GameMaster.Instance.GetRandomPrompt();

        _dialogbox.Talk(gmPrompt.TalkingPoints, "Game Master");      
    }
}
