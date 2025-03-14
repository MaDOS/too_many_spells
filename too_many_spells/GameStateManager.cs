using Godot;

public partial class GameStateManager : Node
{
    [Signal]
    public delegate void GamePauseToggledEventHandler(bool paused);

    [Signal]
    public delegate void GameSavedEventHandler();

    [Signal]
    public delegate void ReloadGameFilesEventHandler();
    
    public const string PLAYERSAVEFILE = "user://player.json";
	public const string BOOKSAVEFILE = "user://book.json";

    public static GameStateManager Instance = null!;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_cancel"))
        {
            EmitSignal(nameof(GamePauseToggled));

            GD.Print("Game pause toggled. Actually puasing is commented out though because it halts the game.");

            // var tree = GetTree();
            // tree.Paused = true;
        }
    }

    public void SaveGame()
    {
        GD.Print("Game saved.");

        EmitSignal(nameof(GameSaved));
    }

    public void ResetSave()
    {
        GD.Print("Resetting save files.");

        if (FileAccess.FileExists(GameStateManager.PLAYERSAVEFILE))
        {            
            string absPath = "";

            using (FileAccess file = FileAccess.Open(GameStateManager.PLAYERSAVEFILE, FileAccess.ModeFlags.Read))
            {
                absPath = file.GetPathAbsolute();

                GD.Print(absPath);
            }

            DirAccess.RemoveAbsolute(absPath);
        }

        if (FileAccess.FileExists(GameStateManager.BOOKSAVEFILE))
        {            
            string absPath = "";

            using (FileAccess file = FileAccess.Open(GameStateManager.BOOKSAVEFILE, FileAccess.ModeFlags.Read))
            {
                absPath = file.GetPathAbsolute();
            }

            DirAccess.RemoveAbsolute(absPath);
        }

        EmitSignal(nameof(ReloadGameFiles));
    }
}
