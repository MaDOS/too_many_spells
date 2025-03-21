using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
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

	private void LoadPrompts()
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

	public float ScoreSpell(GameMasterPrompt prompt, Spells.Spell spell)
	{
		return prompt.PromptTags.Intersect(spell.SpellTags).Count() / (float)prompt.PromptTags.Length;
	}

	public string[] GetAnswer(GameMasterPrompt prompt, Spells.Spell spell)
	{
		Player.Instance.Data.PromptsPlayed++;

		if (spell.IsTrash)
		{
			return new[] { prompt.TrashAnswer };
		}

		GD.Print($"Player played {Player.Instance.Data.PromptsPlayed} prompts now.");
		GD.Print($"Spell tags: {string.Join(", ", spell.SpellTags)}");
		GD.Print($"Prompt tags: {string.Join(", ", prompt.PromptTags)}");

		List<GameMasterPrompt.Answer> answers = prompt.Answers
			.Where(answer => spell.SpellTags.Intersect(answer.FilterForIncludedSpellTags).Count() == answer.FilterForIncludedSpellTags.Length)
			.Where(answer => !spell.SpellTags.Intersect(answer.FilterForExcludedSpellTags).Any())
			.ToList();

		GD.Print($"Found {answers.Count} answers");

		var answer = answers[GD.RandRange(0, answers.Count - 1)];

		GameStateManager.Instance.FirePlayEffect(answer.AnswerSound);

		return answer.AnswerTexts;
	}

	public record GameMasterPrompt
	{
		public string ScenarioName { get; set; } = string.Empty;
		public int MaxExperience { get; set; }
		public string[] PromptTexts { get; set; } = Array.Empty<string>();
		public string[] PromptTags { get; set; } = Array.Empty<string>();

		public string TrashAnswer { get; set; } = string.Empty;

		public List<Answer> Answers { get; set; } = new List<Answer>();

		public record Answer
		{
			public string[] AnswerTexts { get; set; } = Array.Empty<string>();
			public string[] FilterForIncludedSpellTags { get; set; } = Array.Empty<string>();
			public string[] FilterForExcludedSpellTags { get; set; } = Array.Empty<string>();
			public float MinScore { get; set; }
			public float MaxScore { get; set; }
			public string AnswerSound { get; set; } = string.Empty;
		}
	}
}
