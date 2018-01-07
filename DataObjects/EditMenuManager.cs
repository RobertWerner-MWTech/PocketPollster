using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace DataObjects
{
	/// <summary>
	/// The EditMenuManager implements an edit menu for a Framework program.
	/// 
	/// To use it, reference it from your project, create an instance and 
	/// pass a top level "Edit" MenuItem to the ConnectMenus() method.  If you 
	/// have custom controls that implement editing functionality, 
	/// implementing the ISupportsEdit interface on those controls enables 
	/// the EditMenuManager to work with them.
	/// </summary>
	public class EditMenuManager
	{
		// Declare the MenuItems to insert under the Edit menu
		private MenuItem m_miEdit;
		private MenuItem m_miUndo;
		private MenuItem m_miRedo;
		private MenuItem m_miCut;
		private MenuItem m_miCopy;
		private MenuItem m_miPaste;
		private MenuItem m_miDelete;
		private MenuItem m_miDividerRedoCut;
		private MenuItem m_miDividerPasteDelete;
		private MenuItem m_miSelectAll;
		//private MenuItem m_miDividerPropertiesSelectAll;
		//private MenuItem m_miProperties;

		// Declare an enumeration to represent the visible/invisible enabled/disabled
		// state of the menu.
		[Flags]
		private enum EditState
		{
			None = 0x0,
			UndoVisible = 0x1,
			RedoVisible = 0x2,
			UndoEnabled = 0x4,
			RedoEnabled = 0x8,
			CutEnabled = 0x10,
			CopyEnabled = 0x20,
			PasteEnabled = 0x40,
			SelectAllEnabled = 0x80,
			DeleteEnabled = 0x100,
			RenameEnabled = 0x200,
			PropertiesEnabled = 0x400
		}

		// Declare an enumeraction corresponding to the menu item positions.  This
		// makes is a bit easier to respond to menu commands.
		private enum MenuIndex
		{
			Undo = 0,
			Redo = 1, 
			Divider1 = 2,
			Cut = 3,
			Copy = 4,
			Paste = 5,
			Divider2 = 6,
			Delete = 7,
			SelectAll = 8,
			Divider3 = 9,
			Properties = 10
		}

		/// <summary>
		/// Nothing needs to be done in the constructor.
		/// </summary>
		public EditMenuManager() {}

		// The main entry point for this module.  Used for initializing
		// the Edit menu of the Windows Forms application.
		public void ConnectMenus(MenuItem miEdit)
		{
			// Only the initial call does anything.
			if( m_miEdit == null )
			{
				CreateMenus();
				m_miEdit = miEdit;
				m_miEdit.MenuItems.AddRange(
          new MenuItem[] {
                           m_miUndo,
                           m_miRedo,
                           m_miDividerRedoCut,
                           m_miCut,
                           m_miCopy,
                           m_miPaste,
                           m_miDividerPasteDelete,
                           m_miDelete,
                           m_miSelectAll});
                           //									   m_miDividerPropertiesSelectAll,
                           //									   m_miProperties});

				m_miEdit.Popup += new System.EventHandler(Edit_Popup);
				m_miUndo.Click += new System.EventHandler(Menu_Click);
				m_miRedo.Click += new System.EventHandler(Menu_Click);
				m_miCut.Click += new System.EventHandler(Menu_Click);
				m_miCopy.Click += new System.EventHandler(Menu_Click);
				m_miPaste.Click += new System.EventHandler(Menu_Click);
				m_miDelete.Click += new System.EventHandler(Menu_Click);
				m_miSelectAll.Click += new System.EventHandler(Menu_Click);
//				m_miProperties.Click += new System.EventHandler(Menu_Click);
			}
		}	
	
		/// <summary>
		/// Create the menu items and initialize them as desired.
		/// </summary>
		private void CreateMenus()
		{
			// Create
			m_miUndo = new MenuItem();
			m_miRedo = new MenuItem();
			m_miCut = new MenuItem();
			m_miCopy = new MenuItem();
			m_miPaste = new MenuItem();
			m_miDelete = new MenuItem();
			m_miDividerRedoCut = new MenuItem();
			m_miDividerPasteDelete = new MenuItem();
			m_miSelectAll = new MenuItem();
			//m_miDividerPropertiesSelectAll = new MenuItem();
			//m_miProperties = new MenuItem();

			// Initialize
			m_miUndo.Index = (int)MenuIndex.Undo;
			m_miUndo.Text = "&Undo";
			m_miUndo.Shortcut = Shortcut.CtrlZ;
			m_miRedo.Index = (int)MenuIndex.Redo;
			m_miRedo.Text = "&Redo";
			m_miRedo.Shortcut = Shortcut.CtrlY;
			m_miDividerRedoCut.Index = (int)MenuIndex.Divider1;
			m_miDividerRedoCut.Text = "-";
			m_miCut.Index = (int)MenuIndex.Cut;
			m_miCut.Text = "Cu&t";
			m_miCut.Shortcut = Shortcut.CtrlX;
			m_miCopy.Index = (int)MenuIndex.Copy;
			m_miCopy.Text = "&Copy";
			m_miCopy.Shortcut = Shortcut.CtrlC;
			m_miPaste.Index = (int)MenuIndex.Paste;
			m_miPaste.Text = "&Paste";
			m_miPaste.Shortcut = Shortcut.CtrlV;
			m_miDividerPasteDelete.Index = (int)MenuIndex.Divider2;
			m_miDividerPasteDelete.Text = "-";
			m_miDelete.Index = (int)MenuIndex.Delete;
			m_miDelete.Text = "&Delete";
			m_miDelete.Shortcut = Shortcut.Del;
			m_miSelectAll.Index = (int)MenuIndex.SelectAll;
			m_miSelectAll.Text = "Select A&ll";
			m_miSelectAll.Shortcut = Shortcut.CtrlA;
//			m_miDividerPropertiesSelectAll.Index = (int)MenuIndex.Divider3;
//			m_miDividerPropertiesSelectAll.Text = "-";
//			m_miProperties.Index = (int)MenuIndex.Properties;
//			m_miProperties.Text = "Pr&operties";
//			m_miProperties.Shortcut = Shortcut.F4;
		}

		// When the user clicks on the Edit menu, determine the focused control,
		// call a method to get state flags, then setup the edit menu based 
		// on the flags.
		private void Edit_Popup(object sender, System.EventArgs e)
		{
			IntPtr hFocus = Win32API.GetFocus();
			Control ctlFocus = Win32API.GetFrameworkControl(hFocus);
			EditState eEditState = EditState.None;

			if( ctlFocus is TextBoxBase )
				eEditState = GetTextBoxEditState((TextBoxBase)ctlFocus);
			else if( ctlFocus is ComboBox && ((ComboBox)ctlFocus).DropDownStyle == ComboBoxStyle.DropDown )
				eEditState = GetComboBoxEditState(hFocus, (ComboBox)ctlFocus);
			else
			{ 
				// If this is not a simple control, search up the parent chain for 
				// a custom editable control.
				ISupportsEdit ctlEdit = GetISupportsEditControl(ctlFocus);
				if( ctlEdit != null )
					eEditState = GetISupportsEditState(ctlEdit);
			}

			// Show or hide and enable or disable menu controls according to eEditState.
			m_miUndo.Visible		= (eEditState & EditState.UndoVisible) != 0;
			m_miRedo.Visible		= (eEditState & EditState.RedoVisible) != 0;
			m_miDividerRedoCut.Visible = (m_miUndo.Visible == true || m_miRedo.Visible == true);
			m_miUndo.Enabled		= (eEditState & EditState.UndoEnabled) != 0;
			m_miRedo.Enabled		= (eEditState & EditState.RedoEnabled) != 0;
			m_miCut.Enabled			= (eEditState & EditState.CutEnabled) != 0;
			m_miCopy.Enabled		= (eEditState & EditState.CopyEnabled) != 0;
			m_miPaste.Enabled		= (eEditState & EditState.PasteEnabled) != 0;
			m_miDelete.Enabled		= (eEditState & EditState.DeleteEnabled) != 0;
			m_miSelectAll.Enabled	= (eEditState & EditState.SelectAllEnabled) != 0;
			//m_miProperties.Enabled	= (eEditState & EditState.PropertiesEnabled) != 0;
		}

		// Obtain EditState flags for TextBox and RichTextBox controls.
		private EditState GetTextBoxEditState(TextBoxBase textbox)
		{
			// Set Booleans defining the textbox state.
			bool bWritable = (textbox.ReadOnly == false && textbox.Enabled == true);
			bool bTextSelected = (textbox.SelectionLength > 0);
			// Cannot use textbox.TextLength, because that wipes out Undo/Redo buffer in RichTextBox
			bool bHasText = Win32API.GetTextLength(textbox.Handle) > 0;
			bool bIsRichText = (textbox is RichTextBox);

			// Use the Booleans to set the EditState flags.
			EditState eState = EditState.UndoVisible;
			if( bIsRichText )
			{
				eState |= EditState.RedoVisible;
				if( ((RichTextBox)textbox).CanRedo )
					eState |= EditState.RedoEnabled;
			}
			if( textbox.CanUndo )
				eState |= EditState.UndoEnabled;
			if( textbox.CanSelect )
				eState |= EditState.SelectAllEnabled;
			if( bTextSelected )
				eState |= EditState.CopyEnabled;
			if( bWritable )
			{
				if( bTextSelected )
				{
					eState |= EditState.CutEnabled;
					eState |= EditState.DeleteEnabled;
				}
				if( bIsRichText )
				{
					if( Win32API.CanPasteAnyFormat(textbox.Handle) )
						eState |= EditState.PasteEnabled;
				}
				else	// TextBox
				{
					if( Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) ) 
						eState |= EditState.PasteEnabled;
				}
			}
			return eState;
		}

		// Obtain EditState flags for ComboBox controls.
		private EditState GetComboBoxEditState(IntPtr hEdit, ComboBox combobox)
		{
			// Set Booleans defining the ComboBox state.
			bool bWritable =		combobox.Enabled;
			bool bClipboardText =	Clipboard.GetDataObject().GetDataPresent(DataFormats.Text);
			bool bTextSelected =	combobox.SelectionLength > 0;
			bool bHasText =			combobox.Text.Length > 0;

			// Use the Booleans to set the EditState flags.
			EditState eState = EditState.UndoVisible;
			if( Win32API.CanUndo(Win32API.GetFocus()) )
				eState |= EditState.UndoEnabled;

			if( bWritable )
			{
				if( bTextSelected )
				{
					eState |= EditState.CutEnabled;
					eState |= EditState.DeleteEnabled;
				}
				if( bClipboardText )
					eState |= EditState.PasteEnabled;
			}
			if( bTextSelected )
				eState |= EditState.CopyEnabled;
			if( bHasText )
				eState |= EditState.SelectAllEnabled;

			return eState;
		}

		// Set EditState flags for ISupportsEdit controls.
		private EditState GetISupportsEditState(ISupportsEdit control)
		{
			EditState eState = EditState.None;
 			if( control.UndoVisible )		eState |= EditState.UndoVisible;
			if( control.CanUndo )			eState |= EditState.UndoEnabled;
			if( control.RedoVisible )		eState |= EditState.RedoVisible;
			if( control.CanRedo )			eState |= EditState.RedoEnabled;
			if( control.CanCut )			eState |= EditState.CutEnabled;
			if( control.CanCopy )			eState |= EditState.CopyEnabled;
			if( control.CanPaste )			eState |= EditState.PasteEnabled;
			if( control.CanSelectAll )		eState |= EditState.SelectAllEnabled;
			if( control.CanDelete )			eState |= EditState.DeleteEnabled;
			if( control.CanShowProperties ) eState |= EditState.PropertiesEnabled;
			return eState;
		}

		// Takes a control and traverses the parent chain until it finds a control
		// that supports the ISupportsEdit interface.  Returns that control, or null
		// if none is found.
		private ISupportsEdit GetISupportsEditControl(Control ctlFocus)
		{
			while( !(ctlFocus is ISupportsEdit) && ctlFocus != null )
				ctlFocus = ctlFocus.Parent;
			return (ISupportsEdit)ctlFocus;
		}


		// Click handler for all edit menus.  Determine the focused window
		// and framework control, then call a method to take the appropriate 
		// action, based on the type of the control.
		private void Menu_Click(object sender, System.EventArgs e)
		{
			MenuItem miClicked = sender as MenuItem;

			IntPtr hFocus = Win32API.GetFocus();
			Control ctlFocus = Win32API.GetFrameworkControl(hFocus);
			MenuIndex menuIndex = (MenuIndex)miClicked.Index;
			if(	ctlFocus is TextBoxBase )
				DoTextBoxCommand((TextBoxBase)ctlFocus, menuIndex);
			else if( ctlFocus is ComboBox && ((ComboBox)ctlFocus).DropDownStyle == ComboBoxStyle.DropDown ) 
				DoComboBoxCommand(hFocus, (ComboBox)ctlFocus, menuIndex);
			else
			{
				ISupportsEdit ctlEdit = GetISupportsEditControl(ctlFocus);
				if (ctlEdit != null )
					DoISupportsEditCommand(ctlEdit, menuIndex);
			}
		}

		// Perform the command associated with MenuIndex on the specified TextBoxBase.
		private void DoTextBoxCommand(TextBoxBase textbox, MenuIndex menuIndex)
		{
			switch(menuIndex)
			{
				case MenuIndex.Undo:		textbox.Undo();				break;
				case MenuIndex.Redo:
					if( textbox is RichTextBox )
					{
						RichTextBox rt = (RichTextBox)textbox;
						rt.Redo();
					}
					break;
				case MenuIndex.Cut:			textbox.Cut();				break;
				case MenuIndex.Copy:		textbox.Copy();				break;
				case MenuIndex.Paste:		textbox.Paste();			break;
				case MenuIndex.Delete:		textbox.SelectedText = "";	break;
				case MenuIndex.SelectAll:	textbox.SelectAll();		break;
				case MenuIndex.Properties:								break;
			}
		}

		// Perform the command associated with MenuIndex on the specified ComboBox.
		private void DoComboBoxCommand(IntPtr hEdit, ComboBox combobox, MenuIndex menuIndex)
		{
			switch(menuIndex)
			{
				case MenuIndex.Undo:		Win32API.Undo(hEdit); break;
				case MenuIndex.Cut:			Win32API.Cut(hEdit); break;
				case MenuIndex.Copy:		Win32API.Copy(hEdit); break;
				case MenuIndex.Paste:		Win32API.Paste(hEdit); break;
				case MenuIndex.SelectAll:	combobox.SelectAll(); break;
				case MenuIndex.Delete:		combobox.SelectedText = ""; break;
			}
		}

		// Perform the command associated with MenuIndex on the specified ISupportsEdit control.
		private void DoISupportsEditCommand(ISupportsEdit control, MenuIndex menuIndex)
		{
			switch(menuIndex)
			{
				case MenuIndex.Undo:		control.Undo();				break;
				case MenuIndex.Redo:		control.Redo();				break;
				case MenuIndex.Cut:			control.Cut();				break;
				case MenuIndex.Copy:		control.Copy();				break;
				case MenuIndex.Paste:		control.Paste();			break;
				case MenuIndex.SelectAll:	control.SelectAll();		break;
				case MenuIndex.Delete:		control.Delete();			break;
				case MenuIndex.Properties:	control.ShowProperties();	break;
			}
		}
	}

	// Define the public interface for user defined editable controls.
	public interface ISupportsEdit
	{
		bool UndoVisible { get;}
		bool CanUndo { get; }
		void Undo();
		bool RedoVisible {get;}
		bool CanRedo { get; }
		void Redo();
		bool CanCut { get;}
		void Cut();
		bool CanCopy { get; }
		void Copy();
		bool CanPaste { get; }
		void Paste();
		bool CanSelectAll { get; }
		void SelectAll();
		bool CanDelete { get; }
		void Delete();
		bool CanShowProperties { get; }
		void ShowProperties();
	}
}
