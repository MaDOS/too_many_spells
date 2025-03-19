using Godot;
using System;

public partial class Bark : Node2D
{
	private Dialogbox _dialogbox = null!;
	private Timer _barkTimer = null!;

	public override void _Ready()
	{
		_dialogbox = GetNode<Dialogbox>("Barkbox");
		_barkTimer = GetNode<Timer>("BarkTimer");

		_dialogbox.TalkingPointsFinished += OnTalkingPointsFinished;

		_barkTimer.OneShot = true;
		_barkTimer.Timeout += OnBarkTimerTimeout;
	}

	public void Enable()
	{
		GD.Print("Bark enabled");

		var timerInterval = GD.RandRange(10, 35);
		GD.Print($"Bark timer interval: {timerInterval}");
		_barkTimer.Start(timerInterval);
	}

	public void Disable()
	{
		GD.Print("Bark disabled");
		_barkTimer.Stop();
	}

	private void OnBarkTimerTimeout()
	{
		GD.Print("Bark timer timeout");

		Barks.Bark bark = Barks.Instance.GetRandomBark();

		_dialogbox.Talk(bark.TalkingPoints, bark.SpeakerName);
	}

	private void OnTalkingPointsFinished()
	{
		this.Enable();
	}
}
