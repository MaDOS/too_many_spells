using Godot;
using System;

public partial class Dialogbox : Control
{
    private string[] _talkingPoints;
    private string _speakerName;

    private int _currentTalkingPointIndex = 0;

    private Label _dialogLabel = null!;
    private Label _speakerNameLabel = null!;

    public override void _Ready()
    {
        _dialogLabel = GetNode<Label>("lblDialogText");
        _speakerNameLabel = GetNode<Label>("lblSpeekerName");
    }

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("advance_talking_points"))
        {
            this.AdvanceTalkingPoints();
        }
	}

    public void Talk(string[] talkingPoints, string speekerName)
    {
        _talkingPoints = talkingPoints;
        _speakerName = speekerName;

        _currentTalkingPointIndex = 0;

        _speakerNameLabel.Text = _speakerName;
        _dialogLabel.Text = _talkingPoints[_currentTalkingPointIndex];

        this.Show();

        GD.Print("Dialogbox talking");
    }

    public void AdvanceTalkingPoints()
    {
        GD.Print("Dialogbox advancing talking points");

        _currentTalkingPointIndex++;

        if (_currentTalkingPointIndex < _talkingPoints.Length)
        {
            _dialogLabel.Text = _talkingPoints[_currentTalkingPointIndex];

            GD.Print("Dialogbox advanced talking points");
        }
        else
        {
            this.Hide();

            GD.Print("Dialogbox hidden");
        }
    }
}
