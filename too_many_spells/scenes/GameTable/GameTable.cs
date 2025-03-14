using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameTable : Node2D
{
    [Signal]
    public delegate void GoHomeEventHandler();

    enum State
    {
        GMPrompt,
        SpellCast,
        GMPromptScoring,
        AddExperience,
        SessionEnd
    }

    private State _state = State.GMPrompt;

    private Dialogbox _dialogbox = null!;
    private GameBook _book = null!;
    private Button _btnGoHome = null!;
    private Bark _bark = null!;

    private GameMaster.GameMasterPrompt _gmPrompt = null!;

    private string _lastSpellCast = string.Empty;
    private int promptsThisSession = Player.Instance.IsInTutorial ? 1 : GD.RandRange(1,3);

    public override void _Ready()
    {
        _dialogbox = GetNode<Dialogbox>("Dialogbox");
        _book = GetNode<GameBook>("Book");
        _btnGoHome = GetNode<Button>("BtnGoHome");
        _bark = GetNode<Bark>("Bark");

        _btnGoHome.Visible = false;

        _dialogbox.TalkingPointsFinished += AddvanceState;
        _book.SpellCast += Book_SpellCast;
        Player.Instance.LevelUp += OnPlayerLevelUp;

        this.BeginNewPrompt();
    }

    private void BeginNewPrompt()
    {
        _state = State.GMPrompt;

        _gmPrompt = GameMaster.Instance.GetRandomPrompt();

        DialogBoxTalk(_gmPrompt.PromptTexts, "Game Master");
    }

    private void Book_SpellCast(string spellName)
    {
        _bark.Disable();
        this._lastSpellCast = spellName;
        DialogBoxTalk(new[] { $"You cast {spellName}!" }, "");
    }

    private void DialogBoxTalk(string[] talkingPoints, string speaker)
    {
        _book.SetProcessInput(false);
        _dialogbox.Talk(talkingPoints, speaker);
    }

    private void HandleSpellCast()
    {
        Spells.Spell spell = Spells.Instance.GetSpell(_lastSpellCast)!;

        //float score = GameMaster.Instance.ScoreSpell(_gmPrompt, spell);
        string[] answers = GameMaster.Instance.GetAnswer(_gmPrompt, spell);

        GD.Print($"Answers: {string.Join(", ", answers)}");

        DialogBoxTalk(answers, "Game Master");
    }

    private void OnPlayerLevelUp(int level)
    {
        Player.LevelDefintion levelDefintion = Player.Instance.LevelDefintions.Single(lvlDef => lvlDef.Level == level);
        List<Spells.Spell> spellsAdded = Spells.Instance.AllSpells
            .Where(spell => levelDefintion.AddedSpells.Contains(spell.Name))
            .Where(spell => !spell.IsTrash)
            .ToList();

        string[] talkingPoints = 
        {
            "Congratulations you leveled up!",
            $"Spell added: {string.Join(" ,", spellsAdded.Select(spell => spell.Name).ToList())}"
        };

        DialogBoxTalk(talkingPoints, "Game Master");
    }

    private void HandleAddExperience()
    {
        var level = Player.Instance.Level;

        promptsThisSession--;
        Player.Instance.AddExperience(_gmPrompt.MaxExperience);

        if(level == Player.Instance.Level) //No level up happened
        {
            AddvanceState();
        }
    }

    private void HandleSessionContinuation()
    {
        GD.Print($"There are {promptsThisSession} prompts in this session left");

        if(promptsThisSession == 0)
        {
            _state = State.SessionEnd;
            AddvanceState();
        }
        else
        {
            BeginNewPrompt();
        }
    }

    private void AddvanceState()
    {
        switch (_state)
        {
            case State.GMPrompt:
                _state = State.SpellCast;
                _book.SetProcessInput(true);
                _book.AllowedToCast = true;
                _bark.Enable();
                break;
            case State.SpellCast:
                _state = State.GMPromptScoring;
                HandleSpellCast();
                break;
            case State.GMPromptScoring:
                _state = State.AddExperience;
                HandleAddExperience();
                break;
            case State.AddExperience:
                HandleSessionContinuation();
                break;
            case State.SessionEnd:
                _btnGoHome.Visible = true;
                break;
        }
    }

    public void _on_BtnGoHome_pressed()
    {
        EmitSignal(nameof(GoHome));
    }
}
