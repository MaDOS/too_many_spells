using Godot;
using System;

public partial class Torchlight : Node2D
{
	private Light2D _light = null!;
	private Timer _timer = null!;

	private float _randomLightEnergy = 0.5f;

	public override void _Ready()
	{
		_light = GetNode<Light2D>("PointLight2D");
		_timer = GetNode<Timer>("Timer");

		_timer.Timeout += OnTimerTimeout;
		_timer.Start(0.15f);
	}

	public override void _Process(double delta)
	{
		_light.Energy = Mathf.Lerp(_light.Energy, _randomLightEnergy, 0.05f);
	}

	private void OnTimerTimeout()
	{
		_randomLightEnergy = (float)GD.Randfn(0.7, 0.4);

		GD.Print($"Random light energy: {_randomLightEnergy}");
	}
}
