using Godot;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
		FileInfo file = new FileInfo("res://components/book/pages.json");

		if(file.Exists)
		{
			GD.Print("Loading pages from file");

			string json = File.ReadAllText(file.FullName);
			_pages = JsonSerializer.Deserialize<List<PageSet>>(json)!;
		}
		else
		{		
			GD.Print("Loading pages from resources");
			
			bool firstRun = true;

			for (int i = 0; firstRun || ResourceLoader.Exists($"res://components/book/pages/page{i}.png"); i += 2)
			{
				Texture2D? leftTexture = LoadTexture($"res://components/book/pages/page{i}.png");
				Texture2D? rightTexture = LoadTexture($"res://components/book/pages/page{i + 1}.png");

				_pages.Add(new PageSet(leftTexture, rightTexture));

				firstRun = false;
			}

			File.WriteAllText(file.FullName, JsonSerializer.Serialize(_pages));
		}
	}

	private Texture2D? LoadTexture(string path)
	{
		return ResourceLoader.Exists(path) ? GD.Load<Texture2D>(path) : null;
	}

	public void OnAnimationFinished()
	{
		if (_currentTurn >= 1 && _currentTurn <= _pages.Count + 1)
			_leftPageSprite.Texture = _pages[_currentTurn - 1].LeftPageTexture;
		_rightPageSprite.Texture = _pages[_currentTurn - 1].RightPageTexture;
	}

	private void GotoTurn(int turn)
	{
		// GD.Print($"CurrentTurn {_currentTurn}");
		// GD.Print($"Turn {turn}");
		// GD.Print($"Pagecount {_pages.Count}");

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

		GD.Print($"Goto {turn} done");

		this._currentTurn = turn;
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
	public string LeftPageTextureFile { get; set; }
	public string RightPageTextureFile { get; set; }
	public Texture2D? LeftPageTexture { get; set; }
	public Texture2D? RightPageTexture { get; set; }

	public PageSet(Texture2D? leftPageTexture, Texture2D? rightPageTexture)
	{
		LeftPageTexture = leftPageTexture;
		RightPageTexture = rightPageTexture;
	}
}
