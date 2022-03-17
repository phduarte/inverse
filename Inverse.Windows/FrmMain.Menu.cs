using Inverse.Domain.Models;
using System;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public partial class FrmMain
    {
        private string _currentFilename = string.Empty;

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FrmNewConnection(this);
            form.ShowDialog();
            ToggleMenuButtons();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var exportables = _databaseService.GetCompatiblesFileTypes();
            var dialog = new OpenFileDialog
            {
                Filter = string.Join("|", exportables)
            };
            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.FileName))
            {
                _currentFilename = dialog.FileName;

                _database = _databaseService.OpenFile(_currentFilename);
                panel1.Invalidate();
            }

            ToggleMenuButtons();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_database.IsEmpty)
                return;

            if (string.IsNullOrEmpty(_currentFilename))
            {
                var exportables = _databaseService.GetCompatiblesFileTypes();
                var dialog = new SaveFileDialog
                {
                    Filter = string.Join("|", exportables),
                    FileName = _database.Name + Constants.FileManager.FILTER_EXTENSION
                };

                var res = dialog.ShowDialog();

                _currentFilename = res != DialogResult.Cancel ? dialog.FileName : null;
            }

            if (string.IsNullOrEmpty(_currentFilename))
                return;

            _databaseService.SaveFile(_database, _currentFilename);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString))
                return;

            var database = _databaseService.LoadDatabase(_provider, _connectionString);

            UseDatabase(database);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _database = new Database(_provider);
            _currentFilename = null;
            ResetPanelSize();
            ToggleMenuButtons();
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

            MessageBox.Show("Script exportado com sucesso.");
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

        private void addColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var activeTable = GetActiveTable();
            var frm = new FrmAddColumn();
            frm.AddColumn(activeTable);
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
    }
}
