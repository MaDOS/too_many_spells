using Godot;

public partial class WorkBook : Book
{
	[Signal]
	public delegate void PageSwapEventHandler();

	[Signal]
	public delegate void PageDeletedEventHandler();

	public bool DeletePage { get; set; } = false;

	private Page? draggingPage = null;
	private int draggingFromIndex = 1;

	private bool mouseOnLeftSide = false;
	private bool mouseOnRightSide = false;

	private bool _swappingDisabled = false;

	public void DisableSwapping()
	{
		this._swappingDisabled = true;
	}

	public void PageReleased()
	{
		if (draggingPage is not null)
		{
			GD.Print("Page released");
			GD.Print(DeletePage);

			if (!this.DeletePage)
			{
				if (_currentPage == 0 || _currentPage == _pages.Count)
				{
					this._pages.Insert(draggingFromIndex, this.draggingPage);
				}
				else
				{
					if (this.mouseOnLeftSide)
					{
						this._pages.Insert(_currentPage - 1, this.draggingPage);

						if (_currentPage - 1 != draggingFromIndex)
						{
							this.SavePages();
							EmitSignal(nameof(PageSwap));
						}
					}
					else if (this.mouseOnRightSide)
					{
						this._pages.Insert(_currentPage, this.draggingPage);

						if (_currentPage != draggingFromIndex)
						{
							this.SavePages();
							EmitSignal(nameof(PageSwap));
						}
					}
					else
					{
						this._pages.Insert(draggingFromIndex, this.draggingPage);
					}
				}
			}
			else
			{
				GD.Print("Page deleted");
				EmitSignal(nameof(PageDeleted));
			}

			this.CleanBook();
			this.OnAnimationFinished();
		}
	}

	public void PageDragged()
	{
		this._pages.Remove(this.draggingPage!);
		this.EnsureEvenPages();
		this.OnAnimationFinished();
	}

	public override void _on_gui_input_leftPage(InputEvent @event)
	{
		if (_swappingDisabled)
		{ return; }

		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			GD.Print($"Mouse {(mouseButton.Pressed ? "pressed" : "released")} on left page");

			if (mouseButton.Pressed)
			{
				//workaround do not drag if we have a buffer page
				var leftPage = this.GetLeftPage();
				if (leftPage.SpellName != string.Empty)
				{
					this.draggingPage = leftPage;
					this.draggingFromIndex = _currentPage - 1;
					PageDragged();
				}
			}

			if (!mouseButton.Pressed && draggingPage != null)
			{
				this.PageReleased();
			}
		}
	}

	public override void _on_gui_input_rightPage(InputEvent @event)
	{
		if (_swappingDisabled)
		{ return; }

		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			GD.Print($"Mouse {(mouseButton.Pressed ? "pressed" : "released")} on right page");

			if (mouseButton.Pressed)
			{
				this.draggingPage = this.GetRightPage();
				this.draggingFromIndex = _currentPage;
				PageDragged();
			}

			if (!mouseButton.Pressed && draggingPage != null)
			{
				this.PageReleased();
			}
		}
	}

	public override void _on_mouse_entered_leftPage()
	{
		this.mouseOnLeftSide = true;
	}

	public override void _on_mouse_exited_leftPage()
	{
		this.mouseOnLeftSide = false;
	}

	public override void _on_mouse_entered_rightPage()
	{
		this.mouseOnRightSide = true;
	}

	public override void _on_mouse_exited_rightPage()
	{
		this.mouseOnRightSide = false;
	}
}
