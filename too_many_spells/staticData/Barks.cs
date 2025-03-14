using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class Barks : Node
{
    public static Barks Instance { get; private set; } = null!;

    private const string BARKSFILE = "res://staticData/barks.json";

    private List<Bark> AllBarks { get; set; } = new List<Bark>();

    public override void _Ready()
    {
        Instance = this;

        this.LoadBarks();
    }

    public Bark GetRandomBark()
    {
        return AllBarks[GD.RandRange(0, AllBarks.Count - 1)];
    }

    public void LoadBarks()
    {
        if (FileAccess.FileExists(BARKSFILE))
        {
            using (FileAccess file = FileAccess.Open(BARKSFILE, FileAccess.ModeFlags.Read))
            {
                string json = file.GetAsText();
                AllBarks = JsonSerializer.Deserialize<List<Bark>>(json)!;

                GD.Print($"Loaded Barks ({AllBarks.Count})");
            }
        }
        else
        {
            GD.Print("Barks file does not exist");
        }
    }

    public record Bark
    {
        public string SpeakerName { get; init; } = string.Empty;
        public string[] TalkingPoints { get; init; } = Array.Empty<string>();
    }
}
