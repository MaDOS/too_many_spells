using Godot;

public partial class WorkBook : Book
{
	private Page? draggingPage = null;

	private bool mouseOnLeftSide = false;
	private bool mouseOnRightSide = false;

	public void PageReleased()
	{
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
					this._pages.Insert(_currentPage - 1, this.draggingPage);
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
					this._pages.Insert(_currentPage, this.draggingPage);
				}
			}
		}
	}

	public override void _on_gui_input_leftPage(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed)
			{
				this.draggingPage = this.GetLeftPage();
				this._pages.Remove(this.draggingPage);
				this.OnAnimationFinished();
			}

			if(!mouseButton.Pressed)
			{
				this.PageReleased();
				this.OnAnimationFinished();
			}

			GD.Print($"Mouse {(mouseButton.Pressed ? "pressed" : "released")} on left page");
		}
	}

	public override void _on_gui_input_rightPage(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
		{
			if(mouseButton.Pressed)
			{
				this.draggingPage = this.GetRightPage();
				this._pages.Remove(this.draggingPage);
				this.OnAnimationFinished();
			}

			if(!mouseButton.Pressed)
			{
				this.PageReleased();
				this.OnAnimationFinished();
			}

			GD.Print($"Mouse {(mouseButton.Pressed ? "pressed" : "released")} on right page");
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
