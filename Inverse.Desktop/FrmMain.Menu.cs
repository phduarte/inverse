using Inverse.Domain;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class FrmMain
    {
        private string _currentFilename = string.Empty;
        private bool isSavePending = false;

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FrmNewConnection(this);
            form.ShowDialog();
            ToggleMenuButtons();
            isSavePending = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = GetCompatibleFilesFilter()
            };

            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.FileName))
            {
                _currentFilename = dialog.FileName;

                _database = _databaseService.OpenFile(_currentFilename);
                _connectionString = _database.ConnectionString;
                panel1.Invalidate();

                editToolStripMenuItem1.Visible = diagramToolStripMenuItem.Visible = true;
            }

            ToggleMenuButtons();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_database.IsEmpty)
                return;

            if (string.IsNullOrEmpty(_currentFilename))
            {
                var dialog = new SaveFileDialog
                {
                    Filter = GetCompatibleFilesFilter(),
                    FileName = _database.Name + Constants.FileManager.FILTER_EXTENSION
                };

                var res = dialog.ShowDialog();

                _currentFilename = res != DialogResult.Cancel ? dialog.FileName : null;
            }

            if (string.IsNullOrEmpty(_currentFilename))
                return;

            _databaseService.SaveFile(_database, _currentFilename);
            isSavePending = false;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString) || _database.IsEmpty)
                return;

            _tablePositions.Clear();

            foreach (var t in _database.Tables)
            {
                _tablePositions.Add(new TableViewStatus
                {
                    Table = t.Name,
                    Left = t.Left,
                    Top = t.Top,
                    Visible = !t.IsHidden
                });
            }

            var database = _databaseService.LoadDatabase(_provider, _connectionString);

            UseDatabase(database);

            foreach (var t in _database.Tables)
            {
                if (_tablePositions.FirstOrDefault(a => a.Table.Equals(t.Name)) is TableViewStatus view)
                {
                    if (!view.Visible)
                    {
                        t.Hide();
                    }

                    t.MoveTo(view.Left, view.Top);
                }
            }

            _tablePositions.Clear();
            isSavePending = true;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isSavePending || UserWantsClose())
            {
                _database = new Database(_provider);
                _currentFilename = null;
                ResetPanelSize();
                ToggleMenuButtons();
                editToolStripMenuItem1.Visible = diagramToolStripMenuItem.Visible = false;
            }
        }

        private void scriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_database.IsEmpty)
                return;

            var dialog = new SaveFileDialog();

            var exportables = _databaseService.GetCompatiblesScriptings();

            dialog.Filter = string.Join("|", exportables);
            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            _databaseService.Export(_database, dialog.FileName);

            UpdateStatus("Script exportado com sucesso.");
        }

        private void showHiddenTablesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastFilename = _currentFilename;
            _currentFilename = null;

            saveToolStripMenuItem_Click(sender, e);

            _currentFilename = string.IsNullOrEmpty(_currentFilename) ? lastFilename : _currentFilename;
        }

        private void arrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Arrange().GetAwaiter();
        }

        private void ToggleMenuButtons()
        {
            saveAsToolStripMenuItem.Enabled
                = saveToolStripMenuItem.Enabled
                = refreshToolStripMenuItem.Enabled
                = scriptToolStripMenuItem.Enabled
                = closeToolStripMenuItem.Enabled
                = exportToolStripMenuItem.Enabled
                = arrangeToolStripMenuItem.Enabled = !_database.IsEmpty;
        }

        private string GetCompatibleFilesFilter()
        {
            var exportables = _databaseService.GetCompatiblesFileTypes().ToList();
            var extensions = exportables.Select(x => x.Split("|")[1]);
            var todos = "All files|" + string.Join(";", extensions);
            exportables.Insert(0, todos);
            return string.Join("|", exportables);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var t in _database.Tables)
            {
                _selectedTables.Add(t);
            }
            panel1.Invalidate();
        }

        private void releaseTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentTable = null;
            _selectedTables.Clear();
            panel1.Invalidate();
        }
    }
}
