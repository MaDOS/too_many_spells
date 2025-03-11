using Godot;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class Book : AnimatedSprite2D
{
	private const string SAVEFILE = "user://book.json";

	private TextureRect _leftPage = null!;
	private TextureRect _rightPage = null!;

	protected int _currentPage = -1;
	protected List<Page> _pages = new List<Page>();

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
			Spells.Instance.AllSpells.ForEach(spell => _pages.Add(new Page(spell.Name, spell.Artwork)));

			this.SavePages();
		}
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
		if (_currentPage == 0)
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
		if (_currentPage == -1)
		{
			this.GotoPage(0);
		}
		else if (_currentPage == _pages.Count)
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
		if (_currentPage == _pages.Count + 1)
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

	public virtual void _on_gui_input_leftPage(InputEvent @event)
	{ }

	public virtual void _on_gui_input_rightPage(InputEvent @event)
	{ }

	public virtual void _on_mouse_entered_leftPage()
	{ }

	public virtual void _on_mouse_exited_leftPage()
	{ }

	public virtual void _on_mouse_entered_rightPage()
	{ }

	public virtual void _on_mouse_exited_rightPage()
	{ }

	public Book.Page GetLeftPage()
	{
		GD.Print($"{nameof(GetLeftPage)}(idx: {_currentPage - 1})");

		return _pages[_currentPage - 1];
	}

	public Book.Page GetRightPage()
	{
		GD.Print($"{nameof(GetRightPage)}(idx: {_currentPage})");

		return _pages[_currentPage];
	}

	public class Page
	{
		public string TextureFile { get; set; }

		public string SpellName { get; set; }

		[JsonIgnore]
		public Texture2D? Texture { get; set; }

		[JsonConstructor]
		public Page(string spellName, string textureFile)
		{
			SpellName = spellName;
			TextureFile = textureFile;

			Texture = ResourceLoader.Exists(textureFile) ? GD.Load<Texture2D>(textureFile) : null;
		}
	}
}
