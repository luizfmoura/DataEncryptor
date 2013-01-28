using DataEncryptor.Model;
using DataEncryptor.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataEncryptor
{
    public partial class MainForm : Form
    {
        private string FileName;
        private List<Entry> entries;

        public MainForm()
        {
            InitializeComponent();
            textBoxFileName.Text = Resources.NoFileSelected;
            this.Text = Assembly.GetExecutingAssembly().FullName;
        }

        #region Events

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fileName = PromptDialog(new OpenFileDialog());
                if (!string.IsNullOrEmpty(fileName))
                {
                    OpenFile(fileName);
                    textBoxKey.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                Decrypt();
                decryptButton.Enabled = false;
                textBoxDescription.ReadOnly = false;
                textBoxUser.ReadOnly = false;
                textBoxPassword.ReadOnly = false;
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FileName) && entries != null)
            {
                SaveFile(FileName);
            }
            else if (entries != null)
            {
                SaveAs();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
            New();
        }

        private void buttonAddEntry_Click(object sender, EventArgs e)
        {
            var obj = new Entry(textBoxDescription.Text, textBoxUser.Text, textBoxPassword.Text);
            entries.Add(obj);
            UpdateGrid();
        }

        private void buttonRemoveEntry_Click(object sender, EventArgs e)
        {
            RemoveEntry();         
        }

        #endregion

        #region Methods

        private void RemoveEntry()
        {
            var selectedRow = dataGridViewEntry.SelectedRows[0];
            if (selectedRow != null)
            {
                var obj = selectedRow.DataBoundItem as Entry;
                entries.Remove(obj);
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            dataGridViewEntry.DataSource = null;
            dataGridViewEntry.DataSource = entries;
        }

        private void Decrypt()
        {
            entries = LoadEntries(FileName, new CryptoKey(textBoxKey.Text, textBoxIV.Text));
            dataGridViewEntry.DataSource = entries;
        }

        private void SaveAs()
        {
            var fileName = PromptDialog(new SaveFileDialog());
            if (!string.IsNullOrEmpty(fileName))
                SaveFile(fileName);
        }

        private string PromptDialog(FileDialog dialog)
        {
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            return null;
        }

        private void ShowException(Exception ex)
        {
            MessageBox.Show(ex.Message, Assembly.GetExecutingAssembly().FullName);
        }

        private List<Entry> LoadEntries(string fileName, CryptoKey crypto)
        {
            return Serializer.DeserializeFromFile<List<Entry>>(fileName, crypto);
        }

        private void CloseFile()
        {
            FileName = string.Empty;
            entries = null;
            dataGridViewEntry.DataSource = null;
            textBoxKey.ReadOnly = true;
            textBoxIV.ReadOnly = true;
            decryptButton.Enabled = false;
            textBoxKey.Text = string.Empty;
            textBoxIV.Text = string.Empty;
            textBoxFileName.Text = Resources.NoFileSelected;
            textBoxDescription.ReadOnly = true;
            textBoxUser.ReadOnly = true;
            textBoxPassword.ReadOnly = true;
            textBoxDescription.Text = string.Empty;
            textBoxUser.Text = string.Empty;
            textBoxPassword.Text = string.Empty;
        }

        private void OpenFile(string fileName)
        {
            entries = null;
            textBoxFileName.Text = fileName;
            FileName = fileName;
            textBoxKey.ReadOnly = false;
            textBoxIV.ReadOnly = false;
            decryptButton.Enabled = true;
        }

        private void SaveFile(string fileName)
        {
            Serializer.SerializeToFile<List<Entry>>(fileName, entries, new CryptoKey(textBoxKey.Text, textBoxIV.Text));
            FileName = fileName;
            textBoxFileName.Text = fileName;
        }

        private void New()
        {
            entries = new List<Entry>();
            dataGridViewEntry.DataSource = entries;
            textBoxKey.ReadOnly = false;
            textBoxIV.ReadOnly = false;
            textBoxDescription.ReadOnly = false;
            textBoxUser.ReadOnly = false;
            textBoxPassword.ReadOnly = false;
        }

        #endregion


    }
}