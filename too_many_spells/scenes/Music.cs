using Godot;
using System;

public partial class Music : Node2D
{
    private Node2D _musicEmitter = null!;

    public override void _Ready()
    {
        _musicEmitter = GetNode<Node2D>("MusicEmitter");

        _musicEmitter.Set("SceneChange", "Menu");
        _musicEmitter.Call("play");
        
        GameStateManager.Instance.MusicChange += OnMusicChange;        
    }

    public void OnMusicChange(string parameter)
    {
        _musicEmitter.Set("SceneChange", parameter);
    }
}
