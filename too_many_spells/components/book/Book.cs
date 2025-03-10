using Godot;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class Book : AnimatedSprite2D
{
	private const string SAVEFILE = "user://book.json";

	private int _currentTurn = 0;

	private TextureRect _leftPage = null!;
	private TextureRect _rightPage = null!;

	private List<PageSet> _pages = new List<PageSet>();

	public override void _Ready()
	{
		_leftPage = GetNode<TextureRect>("PageLeft");
		_rightPage = GetNode<TextureRect>("PageRight");

		this.LoadPages();

		this.GotoTurn(0);
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

	private void LoadPages()
	{
		if (FileAccess.FileExists(SAVEFILE))
		{
			using (FileAccess file = FileAccess.Open(SAVEFILE, FileAccess.ModeFlags.Read))
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

		GD.Print($"PageCount {_pages.Count}");
	}

	private void SavePages()
	{
		using (FileAccess file = FileAccess.Open(SAVEFILE, FileAccess.ModeFlags.Write))
		{
			var json = JsonSerializer.Serialize(_pages);
			file.StoreString(json);
		}
	}

	public void OnAnimationFinished()
	{
		if (_currentTurn >= 1 && _currentTurn <= _pages.Count + 1)
			_leftPage.Texture = _pages[_currentTurn - 1].LeftPage.Texture;
		_rightPage.Texture = _pages[_currentTurn - 1].RightPage.Texture;
	}

	private void GotoTurn(int turn)
	{
		GD.Print($"Goto turn {turn}");
		GD.Print($"PageCount {turn}");

		_leftPage.Texture = null;
		_rightPage.Texture = null;

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

	public void OnNextPage()
	{
		GD.Print("Next page");
		this.GotoTurn(_currentTurn + 1);
	}

	public void OnPreviousPage()
	{
		GD.Print("Previous page");
		this.GotoTurn(_currentTurn - 1);
	}

	#region dragging page

	public void _on_gui_input_leftPage(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed)
			{
				this.BlankLeftPage();
			}

			if(!mouseButton.Pressed)
			{
				this.UnBlankLeftPage();
			}
		}
	}

	public void _on_gui_input_rightPage(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed)
			{
				this.BlankRightPage();
			}

			if(!mouseButton.Pressed)
			{
				this.UnBlankRightPage();
			}
		}
	}

	public Book.Page GetLeftPage()
	{
		return _pages[_currentTurn].LeftPage;
	}

	public Book.Page GetRightPage()
	{
		return _pages[_currentTurn].RightPage;
	}

	public void BlankLeftPage()
	{
		GD.Print($"Blank left page");
		if (_currentTurn > 1 && _currentTurn < _pages.Count + 1)
		{
			_leftPage.Texture = null;
		}
	}

	public void UnBlankLeftPage()
	{		
		GD.Print($"Unblank left page");
		if (_currentTurn > 1 && _currentTurn < _pages.Count + 1)
		{
			_leftPage.Texture = _pages[_currentTurn - 1].LeftPage.Texture;
		}
	}

	public void BlankRightPage()
	{
		GD.Print($"Blank right page");
		if (_currentTurn > 0 && _currentTurn < _pages.Count)
		{
			_rightPage.Texture = null;
		}
	}

	public void UnBlankRightPage()
	{
		GD.Print($"Unblank right page");
		if (_currentTurn > 0 && _currentTurn < _pages.Count)
		{
			_rightPage.Texture = _pages[_currentTurn - 1].RightPage.Texture;
		}
	}

	#endregion

	public class PageSet
	{
		public Page LeftPage { get; set; }
		public Page RightPage { get; set; }

		[JsonConstructor]
		public PageSet()
		{ }

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

		[JsonConstructor]
		public Page(string textureFile)
		{
			TextureFile = textureFile;

			Texture = ResourceLoader.Exists(textureFile) ? GD.Load<Texture2D>(textureFile) : null;
		}
	}
}
