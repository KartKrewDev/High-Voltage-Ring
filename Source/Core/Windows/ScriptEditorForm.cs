
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Controls;

#endregion

#if NO_SCINTILLA
using CodeImp.DoomBuilder.Data.Scripting;
using CodeImp.DoomBuilder.Windows;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Compilers;
using CodeImp.DoomBuilder.Controls.Scripting;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CodeImp.DoomBuilder.Controls
{
	internal class ScriptEditorPanel : UserControl
	{
		public ScriptDocumentTab ActiveTab { get { return null; } }
		public bool ShowWhitespace { get { return false; } }
		public bool WrapLongLines { get { return false; } }
		public void Initialize(ScriptEditorForm form) { }
		public void ApplySettings() { }
		public void SaveSettings() { }
	        public int FindReplace(FindReplaceOptions options) { return 0; }
		public bool FindNext(FindReplaceOptions options) { return false; }
		public bool FindNext() { return false; }
		public bool FindPrevious(FindReplaceOptions options) { return false; }
		public bool FindPrevious() { return false; }
		public bool FindNextWrapAround(FindReplaceOptions options) { return false; }
		public bool FindPreviousWrapAround(FindReplaceOptions options) { return false; }
		public bool Replace(FindReplaceOptions options) { return false; }
		public bool FindUsages(FindReplaceOptions options, ScriptType scripttype) { return false; }
		public void CloseFindReplace(bool closing) { }
		public void OpenFindAndReplace() { }
		public void GoToLine() { }
		public void RefreshSettings() { }
		public void ClearErrors() { }
		public void ShowErrors(IEnumerable<CompilerError> errors, bool combine) { }
		public void WriteOpenFilesToConfiguration() { }
		public bool AskSaveAll() { return true; }
		public bool CheckImplicitChanges() { return false; }
		public void ForceFocus() { }
		public void ImplicitSave() { }
		public ScriptFileDocumentTab OpenFile(string filename, ScriptType scripttype) { return null; }
		public void ExplicitSaveCurrentTab() { }
		public void OpenBrowseScript() { }
		public bool LaunchKeywordHelp() { return false; }
		public void OnClose() { }
		
		internal ScriptIconsManager Icons { get; private set; }
		internal ScriptResourceDocumentTab OpenResource(ScriptResource resource) { return null; }
		internal ScriptResourcesControl ScriptResourcesControl { get; private set; }
		
		public void DisplayStatus(ScriptStatusType type, string message) { }
		public void ShowError(TextResourceErrorItem error) { }
		
		public void OnReloadResources() { }
	}
	
	internal class ScriptEditorControl : UserControl
	{
		private static Encoding encoding = Encoding.GetEncoding(1251); //mxd. ASCII with cyrillic support...
		internal static Encoding Encoding { get { return encoding; } }
	
		public delegate void ExplicitSaveTabDelegate();
		public delegate void OpenScriptBrowserDelegate();
		public delegate void OpenFindReplaceDelegate();
		public delegate bool FindNextDelegate();
		public delegate bool FindPreviousDelegate();
		public delegate bool FindNextWrapAroundDelegate(FindReplaceOptions options);
		public delegate bool FindPreviousWrapAroundDelegate(FindReplaceOptions options);
		public delegate void GoToLineDelegate();
		public delegate void CompileScriptDelegate();

		public event ExplicitSaveTabDelegate OnExplicitSaveTab;
		public event OpenScriptBrowserDelegate OnOpenScriptBrowser;
		public event OpenFindReplaceDelegate OnOpenFindAndReplace;
		public event FindNextDelegate OnFindNext;
		public event FindPreviousDelegate OnFindPrevious;
		public event FindNextWrapAroundDelegate OnFindNextWrapAround;
		public event FindPreviousWrapAroundDelegate OnFindPreviousWrapAround;
		public new event EventHandler OnTextChanged;
		public event EventHandler OnFunctionBarDropDown;
		public event GoToLineDelegate OnGoToLine;
		public event CompileScriptDelegate OnCompileScript;
		
		public bool IsChanged { get { return false; } }
		public int SelectionStart { get; set; }
		public int SelectionEnd { get; set; }
		public new string Text { get; set; }
		public string SelectedText { get; set; }
		public bool ShowWhitespace { get; set; }
		public bool WrapLongLines { get; set;}
		
		public bool LaunchKeywordHelp() { return false; }
		public void ReplaceSelection(string replacement) { }
		public void MoveToLine(int linenumber) { }
		public void EnsureLineVisible(int linenumber) { }
		public int LineFromPosition(int position) { return 0; }
		public void ClearMarks() { }
		public void AddMark(int linenumber) { }
		public void RefreshStyle() { }
		public void SetupStyles(ScriptConfiguration config) { }
		public string GetCurrentWord() { return ""; }
		public string GetWordAt(int position) { return ""; }
		public void Undo() { }
		public void Redo() { }
		public void ClearUndoRedo() { }
		public void SetSavePoint() { }
		public void Cut() { }
		public void Copy() { }
		public void Paste() { }
		public void GrabFocus() { }
		public byte[] GetText() { return null; }
		public void SetText(byte[] text) { }
		public void InsertSnippet(string[] lines) { }
		public bool FindNext(FindReplaceOptions options, bool useselectionstart) { return false; }
		public bool FindPrevious(FindReplaceOptions options) { return false; }
		public void IndentSelection(bool indent) { }
		public void DuplicateLine() { }
	}
	
	internal class ScriptEditorPreviewControl : UserControl
	{
		public string FontName { set; private get; }
		public int FontSize { set; private get; }
		public bool FontBold { set; private get; }
		public int TabWidth { set; private get; }
		public bool ShowLineNumbers { set; private get; }
		public bool ShowFolding { set; private get; }
		public Color ScriptBackground { set; private get; }
		public Color FoldForeColor { set; private get; }
		public Color FoldBackColor { set; private get; }
		public Color LineNumbers { set; private get; }
		public Color PlainText { set; private get; }
		public Color Comments { set; private get; }
		public Color Keywords { set; private get; }
		public Color Properties { set; private get; }
		public Color Literals { set; private get; }
		public Color Constants { set; private get; }
		public Color Strings { set; private get; }
		public Color Includes { set; private get; }
		public Color SelectionForeColor { set; private get; }
		public Color SelectionBackColor { set; private get; }
		public Color WhitespaceColor { set; private get; }
		public Color BraceHighlight { set; private get; }
		public Color BadBraceHighlight { set; private get; }
		public Color ScriptIndicator { set; private get; }
	}
}
#endif

namespace CodeImp.DoomBuilder.Windows
{
	internal partial class ScriptEditorForm : DelayedForm
	{
		#region ================== Variables

		// Closing?
		private bool appclose;
		
		#endregion
		
		#region ================== Properties
		
		public ScriptEditorPanel Editor { get { return editor; } }
		
		#endregion
		
		#region ================== Constructor
		
		// Constructor
		public ScriptEditorForm()
		{
			InitializeComponent();
			editor.Initialize(this);
		}
		
		#endregion
		
		#region ================== Methods
		
		// This asks to save files and returns the result
		// Also does implicit saves
		// Returns false when cancelled by the user
		public bool AskSaveAll()
		{
			// Implicit-save the script lumps
			editor.ImplicitSave();
			
			// Save other scripts
			return editor.AskSaveAll();
		}
		
		// Close the window
		new public void Close()
		{
			appclose = true;
			base.Close();
		}

		//mxd
		internal void OnReloadResources()
		{
			editor.OnReloadResources();
		}

		//mxd
		internal void DisplayError(TextResourceErrorItem error)
		{
			editor.ShowError(error);
		}

		//mxd
		/*internal void DisplayError(TextFileErrorItem error)
		{
			editor.ShowError(error);
		}*/

		#endregion
		
		#region ================== Events

		// Window is loaded
		private void ScriptEditorForm_Load(object sender, EventArgs e)
		{
			// Apply panel settings
			editor.ApplySettings();
		}
		
		// Window is shown
		private void ScriptEditorForm_Shown(object sender, EventArgs e)
		{
			// Focus to script editor
			editor.ForceFocus();
		}
		
		// Window is closing
		private void ScriptEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			editor.SaveSettings();
			
			// Only when closed by the user
			if(!appclose && (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.FormOwnerClosing))
			{
				// Remember if scipts are changed
				General.Map.ApplyScriptChanged();
				
				// Ask to save scripts
				if(AskSaveAll())
				{
					// Let the general call close the editor
					General.Map.CloseScriptEditor(true);
				}
				else
				{
					// Cancel
					e.Cancel = true;
				}
			}

			// Not cancelling?
			if(!e.Cancel) editor.OnClose();
		}

		// Help
		private void ScriptEditorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			if(!editor.LaunchKeywordHelp())	General.ShowHelp("w_scripteditor.html"); //mxd
			hlpevent.Handled = true;
		}
		
		#endregion
	}
}