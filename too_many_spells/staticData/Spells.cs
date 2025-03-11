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
        if (FileAccess.FileExists(PROMPTSFILE))
        {
            using (FileAccess file = FileAccess.Open(PROMPTSFILE, FileAccess.ModeFlags.Read))
            {
                string json = file.GetAsText();
                AllSpells = JsonSerializer.Deserialize<List<Spell>>(json)!;                

                GD.Print($"Loaded Spells ({AllSpells.Count})");
            }
        }
        else
        {
            GD.Print("Spells file does not exist");
        }
    }

    public Spell? GetSpell(string spellName)
    {
        return AllSpells.Find(spell => spell.Name == spellName);
    }

    public record Spell
    {
        public string Name { get; set; } = string.Empty;
        public string Artwork { get; set; } = string.Empty;
        public bool IsTrash { get; set; } = false;
        public string[] SpellTags { get; set; } = Array.Empty<string>();
    }

}