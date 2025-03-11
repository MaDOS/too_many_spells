using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class GameMaster : Node
{
    public static GameMaster Instance { get; private set; } = null!;

    private const string PROMPTSFILE = "res://staticData/gamemasterprompts.json";

    private List<GameMasterPrompt> _prompts = new List<GameMasterPrompt>();

    public override void _Ready()
    {
        Instance = this;

        this.LoadPrompts();
    }

    public void LoadPrompts()
    {
        if (FileAccess.FileExists(PROMPTSFILE))
        {
            using (FileAccess file = FileAccess.Open(PROMPTSFILE, FileAccess.ModeFlags.Read))
            {
                string json = file.GetAsText();
                _prompts = JsonSerializer.Deserialize<List<GameMasterPrompt>>(json)!;

                GD.Print($"Loaded Game Master prompts ({_prompts.Count})");
            }
        }
        else
        {
            GD.Print("GameMaster file does not exist");
        }
    }

    public GameMasterPrompt GetRandomPrompt()
    {
        return _prompts[GD.RandRange(0, _prompts.Count - 1)];
    }

    public record GameMasterPrompt
    {
        public string ScenarioName { get; set; } = string.Empty;
        public string[] TalkingPoints { get; set; } = Array.Empty<string>();
        public string[] SpellTags { get; set; } = Array.Empty<string>();
    }
}
