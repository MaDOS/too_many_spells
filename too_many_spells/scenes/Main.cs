using System;
using System.Reflection;
using Godot;

public partial class Main : Node2D
{
	public enum GameState
	{
		Menu,
		WorkTable,
		GameTable
	}

	public GameState CurrentGameState { get; private set; }

	private PackedScene _gameTableScene = GD.Load<PackedScene>("res://scenes/GameTable/game_table.tscn");
	private PackedScene _workTableScene = GD.Load<PackedScene>("res://scenes/WorkTable/work_table.tscn");
	private PackedScene _menuScene = GD.Load<PackedScene>("res://scenes/Menu/top_node_ui_main_menu.tscn");
	private PackedScene _pauseMenuScene = GD.Load<PackedScene>("res://scenes/PauseMenu/pause_menu.tscn");

	private Node2D? activeGameScene = null;

	private Node2D _pauseMenuSceneNode = null!;

	public override void _Ready()
	{
		_pauseMenuSceneNode = _pauseMenuScene.Instantiate<Node2D>();
		_pauseMenuSceneNode.Visible = false;
		AddChild(_pauseMenuSceneNode);

		this.SetGameState(GameState.Menu);

		GameStateManager.Instance.GamePauseToggled += GamePauseToggled;
	}

	public void GamePauseToggled(bool paused)
	{
		_pauseMenuSceneNode.Visible = paused;
		_pauseMenuSceneNode.SetProcessInput(paused);
	}

	public void SetGameState(GameState gameState)
	{
		GD.Print($"SetGameState({gameState})");

		if(activeGameScene is not null && this.CurrentGameState != gameState)
		{
			RemoveChild(activeGameScene);
			activeGameScene = null;
		}

		switch (gameState)
		{
			case GameState.Menu:
				this.ActivateMenuScene();
				break;
			case GameState.WorkTable:
				this.ActivateWorkTableScene();
				break;
			case GameState.GameTable:
				this.ActivateGameTableScene();
				break;
		}

		AddChild(activeGameScene);
		this.CurrentGameState = gameState;

		GD.Print("SetGameState done");
	}

	private void ActivateMenuScene()
	{
		var menuScene = (Node2D)_menuScene.Instantiate();

		menuScene.Connect("main_menu_play_clicked", Callable.From(() => this.SetGameState(GameState.WorkTable)));

		this.activeGameScene = menuScene;
	}

	private void ActivateGameTableScene()
	{
		var gameTableScene = _gameTableScene.Instantiate<GameTable>();

		gameTableScene.GoHome += () => this.SetGameState(GameState.WorkTable);

		this.activeGameScene = gameTableScene;
	}

	private void ActivateWorkTableScene()
	{
		var workTableScene = _workTableScene.Instantiate<WorkTable>();

		workTableScene.NextSession += () => this.SetGameState(GameState.GameTable);

		this.activeGameScene = workTableScene;
	}
}
