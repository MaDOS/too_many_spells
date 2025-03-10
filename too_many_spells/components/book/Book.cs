using Godot;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class Book : AnimatedSprite2D
{
	private Sprite2D _leftPageSprite = null!;
	private Sprite2D _rightPageSprite = null!;

	private List<PageSet> _pages = new List<PageSet>();

	private int _currentTurn = 0;

	public override void _Ready()
	{
		_leftPageSprite = GetNode<Sprite2D>("PageLeft");
		_rightPageSprite = GetNode<Sprite2D>("PageRight");

		this.LoadPages();

		this.GotoTurn(0);
	}

	private void LoadPages()
	{
		if (FileAccess.FileExists("user://book.json"))
		{
			using (FileAccess file = FileAccess.Open("user://book.json", FileAccess.ModeFlags.Read))
			{
				string json = file.GetAsText();
				_pages = JsonSerializer.Deserialize<List<PageSet>>(json)!;
			}
		}
		else
		{
			for (int i = 0; i == 0 || ResourceLoader.Exists($"res://components/book/pages/page{i}.png"); i += 2)
			{
				_pages.Add(new PageSet($"res://components/book/pages/page{i}.png", $"res://components/book/pages/page{i + 1}.png"));
			}
			
			this.SavePages();
		}
	}

	private void SavePages()
	{
		using (FileAccess file = FileAccess.Open("user://book.json", FileAccess.ModeFlags.Write))
		{
			var json = JsonSerializer.Serialize(_pages);
			file.StoreString(json);
		}
	}

	public void OnAnimationFinished()
	{
		if (_currentTurn >= 1 && _currentTurn <= _pages.Count + 1)
			_leftPageSprite.Texture = _pages[_currentTurn - 1].LeftPage.Texture;
			_rightPageSprite.Texture = _pages[_currentTurn - 1].RightPage.Texture;
	}

	private void GotoTurn(int turn)
	{
		_leftPageSprite.Texture = null;
		_rightPageSprite.Texture = null;

		if (turn < 0)
			turn = 0;
		else if (turn > _pages.Count + 1)
			turn = _pages.Count + 1;

		if (_currentTurn == 0 && turn == 1)
			Play("open_to_first");
		else if (_currentTurn == 1 && turn == 0)
			Play("close_from_first");
		else if (_currentTurn == 1 && turn == 2)
			Play("next_from_first");
		else if (_currentTurn == 2 && turn == 1)
			Play("previous_to_first");
		else if (_currentTurn == _pages.Count - 1 && turn == _pages.Count)
			Play("next_to_last");
		else if (_currentTurn == _pages.Count && turn == _pages.Count + 1)
			Play("close_from_last");
		else if (_currentTurn == _pages.Count + 1 && turn == _pages.Count)
			Play("open_to_last");
		else
		{
			if (_currentTurn < turn)
				Play("next_page");
			else if (_currentTurn > turn)
				Play("previous_page");
		}

		this._currentTurn = turn;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("next_page"))
		{
			this.OnNextPage();
		}
		else if (@event.IsActionPressed("previous_page"))
		{
			this.OnPreviousPage();
		}
	}

	public void OnNextPage()
	{
		this.GotoTurn(_currentTurn + 1);
	}

	public void OnPreviousPage()
	{
		this.GotoTurn(_currentTurn - 1);
	}
}

public class PageSet
{
	public Page LeftPage { get; set; }
	public Page RightPage { get; set; }

	public PageSet(string leftPageTextureFile, string rightPageTextureFile)
	{
		LeftPage = new Page(leftPageTextureFile);
		RightPage = new Page(rightPageTextureFile);
	}
}

public class Page
{
	public string TextureFile { get; set; }
	
	[JsonIgnore]
	public Texture2D? Texture { get; set; }

	public Page(string textureFile)
	{
		TextureFile = textureFile;

		Texture = ResourceLoader.Exists(textureFile) ? GD.Load<Texture2D>(textureFile) : null;
	}
}
