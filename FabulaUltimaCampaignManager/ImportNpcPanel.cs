using Godot;
using System;

public partial class ImportNpcPanel : PopupPanel
{
	public void HandleStartImportButtonClicked()
	{
		this.Visible = true;
	}

    public void HandleBeastTemplateCreated()
    {
        this.Visible = false;
    }
}
