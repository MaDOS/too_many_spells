using Godot;

public partial class Main : Node2D
{
	public enum GameState
	{
		Paused,
		Menu,
		WorkTable,
		GameTable
	}

	public GameState CurrentGameState { get; private set; }

	private PackedScene _gameTableScene = GD.Load<PackedScene>("res://scenes/GameTable/game_table.tscn");
	private PackedScene _workTableScene = GD.Load<PackedScene>("res://scenes/WorkTable/work_table.tscn");
	private PackedScene _menuScene = GD.Load<PackedScene>("res://scenes/Menu/ui_main_menu.tscn");

	private Node2D? activeGameScene = null!;

	public override void _Ready()
	{
		this.SetGameState(GameState.Menu);
	}

	public void SetGameState(GameState gameState)
	{
		GD.Print($"SetGameState({gameState})");

		if(this.CurrentGameState != gameState)
		{
			RemoveChild(activeGameScene);
			activeGameScene = null;
		}

		switch (gameState)
		{
			case GameState.Paused:
				break;
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
		var menuScene = _menuScene.Instantiate<UI_Main_Menu>();

		menuScene.PlayClicked += () => this.SetGameState(GameState.WorkTable);

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
