using Godot;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class Book : AnimatedSprite2D
{
	private const string SAVEFILE = "user://book.json";

	private int _currentPage = -1;

	private TextureRect _leftPage = null!;
	private TextureRect _rightPage = null!;

	private List<Page> _pages = new List<Page>();

	public override void _Ready()
	{
		_leftPage = GetNode<TextureRect>("PageLeft");
		_rightPage = GetNode<TextureRect>("PageRight");

		this.LoadPages();

		this.GotoPage(-1);
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
				_pages = JsonSerializer.Deserialize<List<Page>>(json)!;
			}
		}
		else
		{
			for (int i = 1; ResourceLoader.Exists($"res://components/book/pages/page{i}.png"); i += 1)
			{
				_pages.Add(new Page($"res://components/book/pages/page{i}.png"));
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
		if(_currentPage == 0)
		{
			this._leftPage.Texture = null;
			this._rightPage.Texture = this._pages[0].Texture;
		}
		else
		{
			this._leftPage.Texture = this._pages[_currentPage - 1].Texture;
			this._rightPage.Texture = this._pages[_currentPage].Texture;
		}
	}

	private void GotoPage(int page)
	{
		GD.Print($"Goto turn {page}");
		GD.Print($"PageCount {page}");

		_leftPage.Texture = null;
		_rightPage.Texture = null;

		if (page < -1)
			page = -1;
		else if (page > _pages.Count + 1)
			page = _pages.Count + 1;

		if (_currentPage == -1 && page == 0)
			Play("open_to_first");
		else if (_currentPage == 0 && page == -1)
			Play("close_from_first");
		else if (_currentPage == 0 && page == 2)
			Play("next_from_first");
		else if (_currentPage == 2 && page == 0)
			Play("previous_to_first");
		else if (_currentPage == _pages.Count - 2 && page == _pages.Count)
			Play("next_to_last");
		else if (_currentPage == _pages.Count && page == _pages.Count + 1)
			Play("close_from_last");
		else if (_currentPage == _pages.Count + 1 && page == _pages.Count)
			Play("open_to_last");
		else
		{
			if (_currentPage < page)
				Play("next_page");
			else if (_currentPage > page)
				Play("previous_page");
		}

		_currentPage = page;
	}

	public void OnNextPage()
	{
		GD.Print("Next page");

		if(_currentPage == -1)
		{
			this.GotoPage(0);
		}
		else if(_currentPage == _pages.Count)
		{
			this.GotoPage(_currentPage + 1);
		}
		else
		{
			this.GotoPage(_currentPage + 2);
		}
	}

	public void OnPreviousPage()
	{
		GD.Print("Previous page");

		if(_currentPage == _pages.Count + 1)
		{
			this.GotoPage(_currentPage - 1);
		}
		else if (_currentPage == 0)
		{
			this.GotoPage(-1);
		}
		else
		{
			this.GotoPage(_currentPage - 2);
		}
	}

	#region dragging page

	private Page? draggingPage = null;

	private bool mouseOnLeftSide = false;
	private bool mouseOnRightSide = false;

	public void PageReleased()
	{
		//Insert page at new position
		if(draggingPage is not null)
		{
			if (this.mouseOnLeftSide)
			{
				if(_currentPage == 0)
				{
					return;
				}
				else
				{
					this._pages.Remove(draggingPage);
					this._pages.Insert(_currentPage - 1, this.draggingPage!);
					this.OnAnimationFinished();
				}
			}
			else if (this.mouseOnRightSide)
			{
				if(_currentPage == _pages.Count)
				{
					return;
				}
				else
				{
					this._pages.Remove(draggingPage);
					this._pages.Insert(_currentPage, this.draggingPage!);
					this.OnAnimationFinished();
				}
			}
		}
	}

	public void _on_gui_input_leftPage(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed)
			{
				this.draggingPage = this.GetLeftPage();
				this.BlankLeftPage();
			}

			if(!mouseButton.Pressed)
			{
				this.UnBlankLeftPage();
				this.PageReleased();
			}

			GD.Print($"Mouse {(mouseButton.Pressed ? "pressed" : "released")} on left page");
		}
	}

	public void _on_gui_input_rightPage(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed)
			{
				this.draggingPage = this.GetRightPage();
				this.BlankRightPage();
			}

			if(!mouseButton.Pressed)
			{
				this.UnBlankRightPage();
				this.PageReleased();
			}

			GD.Print($"Mouse {(mouseButton.Pressed ? "pressed" : "released")} on right page");
		}
	}

	public void _on_mouse_entered_leftPage()
	{
		this.mouseOnLeftSide = true;
	}

	public void _on_mouse_exited_leftPage()
	{
		this.mouseOnLeftSide = false;
	}

	public void _on_mouse_entered_rightPage()
	{
		this.mouseOnRightSide = true;
	}

	public void _on_mouse_exited_rightPage()
	{
		this.mouseOnRightSide = false;
	}

	public Book.Page GetLeftPage()
	{
		return _pages[_currentPage - 1];
	}

	public Book.Page GetRightPage()
	{
		return _pages[_currentPage];
	}

	public void BlankLeftPage()
	{
		if (_currentPage > 1 && _currentPage < _pages.Count + 1)
		{
			_leftPage.Texture = null;
		}
	}

	public void UnBlankLeftPage()
	{		
		if (_currentPage > 1 && _currentPage < _pages.Count + 1)
		{
			_leftPage.Texture = _pages[_currentPage - 1].Texture;
		}
	}

	public void BlankRightPage()
	{
		if (_currentPage > 0 && _currentPage < _pages.Count)
		{
			_rightPage.Texture = null;
		}
	}

	public void UnBlankRightPage()
	{
		if (_currentPage > 0 && _currentPage < _pages.Count)
		{
			_rightPage.Texture = _pages[_currentPage].Texture;
		}
	}

	#endregion

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
