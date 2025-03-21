using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public partial class Player : Node
{
    [Signal]
    public delegate void LevelUpEventHandler(int level);

    public static Player Instance { get; private set; } = null!;
    public List<LevelDefintion> LevelDefintions = new List<LevelDefintion>();

    public PlayerData Data { get; private set; } = new PlayerData();

    public int Level => Data.Level;
    public int Experience => Data.Experience;
    public bool IsInTutorial => Data.PromptsPlayed < 2;

    private const string LEVELDEFS = "res://staticData/leveldefs.json";

    public override void _Ready()
    {
        Instance = this;

        GameStateManager.Instance.GameSaved += this.SavePlayer;
        GameStateManager.Instance.ReloadGameFiles += this.LoadPlayer;

        this.LoadPlayer();
    }

    private void LoadPlayer()
    {
        if (FileAccess.FileExists(LEVELDEFS))
        {
            using (FileAccess file = FileAccess.Open(LEVELDEFS, FileAccess.ModeFlags.Read))
            {
                string json = file.GetAsText();
                LevelDefintions = JsonSerializer.Deserialize<List<LevelDefintion>>(json)!.OrderBy(lvlDef => lvlDef.Level).ToList();

                GD.Print($"Loaded level defintions ({LevelDefintions.Count})");
            }
        }
        else
        {
            GD.Print("Leveldefs file does not exist");
        }

        if (FileAccess.FileExists(GameStateManager.PLAYERSAVEFILE))
        {
            using (FileAccess file = FileAccess.Open(GameStateManager.PLAYERSAVEFILE, FileAccess.ModeFlags.Read))
            {
                string json = file.GetAsText();
                Data = JsonSerializer.Deserialize<PlayerData>(json)!;

                GD.Print($"Loaded player data");
            }
        }
        else
        {
            GD.Print("Playerdata file does not exist");
            this.SavePlayer();
        }
    }

    public void SavePlayer()
    {
        using (FileAccess file = FileAccess.Open(GameStateManager.PLAYERSAVEFILE, FileAccess.ModeFlags.Write))
        {
            string json = JsonSerializer.Serialize(Data);
            file.StoreString(json);
        }
    }

    public void AddExperience(int experience)
    {
        if(this.IsInTutorial)
        { 
            GD.Print($"Player still in tutorial. Skipping exp calc. Prompts ({this.Data.PromptsPlayed}/2)");
            return; 
        }

        GD.Print($"Current level: {Level}");
        GD.Print($"Current experience: {Experience}");
        GD.Print($"Adding {experience} experience to player");

        var totalLevelUpExp = this.Experience + experience;

        List<LevelDefintion> levelsToLevelUp = new();
        List<LevelDefintion> levelsAbovePlayerLevel = LevelDefintions.Where(levelDef => levelDef.Level > this.Level).ToList();

        LevelDefintion? nextLevelUp;

        while ((nextLevelUp = levelsAbovePlayerLevel.FirstOrDefault()) is not null && totalLevelUpExp >= nextLevelUp.Experience)
        {
            levelsToLevelUp.Add(nextLevelUp);
            levelsAbovePlayerLevel.Remove(nextLevelUp);

            totalLevelUpExp -= nextLevelUp.Experience;
        }

        GD.Print($"Levelups count: {levelsToLevelUp.Count}");

        this.Data.Experience = totalLevelUpExp;

        foreach (var levelToLevelUp in levelsToLevelUp)
        {
            this.Data.Level++;
            EmitSignal(nameof(LevelUp), this.Data.Level);
        }
    }

    public record LevelDefintion
    {
        public int Level { get; set; }
        public int Experience { get; set; }
        public List<string> AddedSpells { get; set; } = new List<string>();
    }

    public record PlayerData
    {
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int PromptsPlayed { get; set; } = 0;
    }
}
