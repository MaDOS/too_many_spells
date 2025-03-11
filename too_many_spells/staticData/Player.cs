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

    private const string LEVELDEFS = "res://staticData/leveldefs.json";
    private const string SAVEFILE = "user://player.json";

    public override void _Ready()
    {
        Instance = this;

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
        
        if (FileAccess.FileExists(SAVEFILE))
        {
            using (FileAccess file = FileAccess.Open(SAVEFILE, FileAccess.ModeFlags.Read))
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
        using (FileAccess file = FileAccess.Open(SAVEFILE, FileAccess.ModeFlags.Write))
        {
            string json = JsonSerializer.Serialize(Data);
            file.StoreString(json);
        }
    }    

    public void AddExperience(int experience)
    {
        GD.Print($"Current level: {Level}");
        GD.Print($"Current experience: {Experience}");
        GD.Print($"Adding {experience} experience to player");

        Data.Experience += experience;

        LevelDefintion? levelDefintion = LevelDefintions.FirstOrDefault(lvlDef => lvlDef.Level == Level);

        if(levelDefintion is null)
        {
            GD.Print("Player is max level");
            return;
        }

        if (levelDefintion != null && Experience >= levelDefintion.Experience)
        {
            Data.Level++;
            Data.Experience = 0;

            this.SavePlayer();

            GD.Print($"Player leveled up to {Level}");

            EmitSignal(nameof(LevelUp), Level);
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
    }
}
