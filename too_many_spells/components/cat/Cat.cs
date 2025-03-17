using Godot;
using System;

public partial class Cat : Node2D
{
    private AnimatedSprite2D _catSprite = null!;
    private Timer _timer = null!;

    private bool _isMoving = false;
    private bool _enabled = false;

    public override void _Ready()
    {
        _catSprite = GetNode<AnimatedSprite2D>("CatSprite");
        _timer = GetNode<Timer>("Timer");

        //_timer.Start(2); //debugging

        _catSprite.Play("default");

        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        GD.Print("Cat starts moving");

        GameStateManager.Instance.FirePlayEffect(SoundEffects.Cat);

        _isMoving = true;
    }

    public void Enable()
    {
        _enabled = true;
    }

    public void Disable()
    {
        _enabled = false;
        _isMoving = false;
        _catSprite.Position = new Vector2(2560, 0);
        _timer.Stop();
    }

    public void Reset()
    {
        GD.Print("Resetting cat position");

        _isMoving = false;
        _catSprite.Position = new Vector2(2560, 0);
        _timer.Start(GD.RandRange(30, 60));
        //_timer.Start(2); //debugging
    }

    public override void _Process(double delta)
    {
        if(_isMoving)
        {
            _catSprite.Position = new Vector2(_catSprite.Position.X - 3, _catSprite.Position.Y);  
        }

        if(_catSprite.Position.X < -2560)
        {
            Reset();
        }
    }
}
