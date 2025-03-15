using Godot;
using System;

public partial class Dialogbox : Control
{
	[Signal]
	public delegate void TalkingPointsFinishedEventHandler();

	private string[] _talkingPoints;
	private string _speakerName;

	private int _currentTalkingPointIndex = 0;

	private Label _dialogLabel = null!;
	private Label _speakerNameLabel = null!;

	public override void _Ready()
	{
		_dialogLabel = GetNode<Label>("lblDialogText");
		_speakerNameLabel = GetNode<Label>("lblSpeekerName");

		this.Hide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("advance_talking_points"))
		{
			if (this.Visible)
			{
				this.AdvanceTalkingPoints();
			}
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
	}

	public void AdvanceTalkingPoints()
	{
		_currentTalkingPointIndex++;

		if (_currentTalkingPointIndex < _talkingPoints.Length)
		{
			_dialogLabel.Text = _talkingPoints[_currentTalkingPointIndex];
		}
		else
		{
			this.Hide();

			EmitSignal(nameof(TalkingPointsFinished));
		}
	}
}
