using Godot;

public partial class GameTable : Node2D
{
    enum State
    {
        GMPrompt,
        SpellCast,        
        GMPromptScoring
    }

    private State _state = State.GMPrompt;

    private Dialogbox _dialogbox = null!;
    private GameBook _book = null!;

    private GameMaster.GameMasterPrompt _gmPrompt = null!;
    private string _lastSpellCast = string.Empty;

    public override void _Ready() 
    {
        _dialogbox = GetNode<Dialogbox>("Dialogbox");
        _book = GetNode<GameBook>("Book");

        _dialogbox.TalkingPointsFinished += Dialogbox_TalkingPointsFinished;
        _book.SpellCast += Book_SpellCast;

        _gmPrompt = GameMaster.Instance.GetRandomPrompt();

        DialoogBoxTalk(_gmPrompt.PromptTexts, "Game Master");      
    }

    private void Book_SpellCast(string spellName)
    {
        this._lastSpellCast = spellName;
        DialoogBoxTalk(new [] {$"You cast {spellName}!"}, "");
    }

    private void DialoogBoxTalk(string[] talkingPoints, string speaker)
    {
        _book.SetProcessInput(false);
        _dialogbox.Talk(talkingPoints, speaker);
    }

    private void HandleSpellCast()
    {
        string[] answers = GameMaster.Instance.GetAnswer(_gmPrompt, Spells.Instance.GetSpell(_lastSpellCast)!);

        GD.Print($"Answers: {string.Join(", ", answers)}");

        DialoogBoxTalk(answers, "Game Master");
    }

    private void Dialogbox_TalkingPointsFinished()
    {
        switch(_state)
        {
            case State.GMPrompt:
                _state = State.SpellCast;
                _book.SetProcessInput(true);
                _book.AllowedToCast = true;
                break;
            case State.SpellCast:
                _state = State.GMPromptScoring;
                HandleSpellCast();
                break;
            case State.GMPromptScoring: //todo scene end
                break;
        }
    }
}
