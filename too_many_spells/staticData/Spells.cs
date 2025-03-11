using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class Spells : Node
{
    public static Spells Instance { get; private set; } = null!;

    private const string PROMPTSFILE = "res://staticData/spells.json";

    public List<Spell> AllSpells { get; set; } = new List<Spell>();

    public override void _Ready()
    {
        Instance = this;

        this.LoadSpells();
    }

    public void LoadSpells()
    {
        GD.Print("Loading spells");

        if (FileAccess.FileExists(PROMPTSFILE))
        {
            using (FileAccess file = FileAccess.Open(PROMPTSFILE, FileAccess.ModeFlags.Read))
            {
                string json = file.GetAsText();
                AllSpells = JsonSerializer.Deserialize<List<Spell>>(json)!;                

                GD.Print($"Loaded Game Master prompts ({AllSpells.Count})");
            }
        }
        else
        {
            GD.Print("Spells file does not exist");
        }
    }

    public record Spell
    {
        public string Name { get; set; } = string.Empty;
        public string Artwork { get; set; } = string.Empty;
        public bool IsTrash { get; set; } = false;
        public string[] SpellTags { get; set; } = Array.Empty<string>();
    }

}