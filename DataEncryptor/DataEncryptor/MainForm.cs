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
        private BindingList<Entry> entries;

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
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CloseFile();
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAs();
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CloseFile();
                New();
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        #endregion

        #region Methods

        private void Decrypt()
        {
            LoadEntries(FileName, new CryptoKey(textBoxKey.Text));
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
            MessageBox.Show("Error: " + ex.Message, Assembly.GetExecutingAssembly().FullName);
        }

        private void LoadEntries(string fileName, CryptoKey crypto)
        {
            entries = Serializer.DeserializeFromFile<BindingList<Entry>>(fileName, crypto);
            dataGridViewEntry.DataSource = entries;

            dataGridViewEntry.Columns[0].MinimumWidth = 215;
            dataGridViewEntry.Columns[1].MinimumWidth = 215;
            dataGridViewEntry.Columns[2].MinimumWidth = 125;
        }

        private void CloseFile()
        {
            entries = null;
            dataGridViewEntry.DataSource = null;
            FileName = string.Empty;
            textBoxKey.ReadOnly = true;
            decryptButton.Enabled = false;
            textBoxKey.Text = string.Empty;
            textBoxFileName.Text = Resources.NoFileSelected;
        }

        private void OpenFile(string fileName)
        {
            entries = null;
            dataGridViewEntry.DataSource = null;
            textBoxFileName.Text = fileName;
            FileName = fileName;
            textBoxKey.ReadOnly = false;
            decryptButton.Enabled = true;
        }

        private void SaveFile(string fileName)
        {
            Serializer.SerializeToFile<BindingList<Entry>>(fileName, entries, new CryptoKey(textBoxKey.Text));
            FileName = fileName;
            textBoxFileName.Text = fileName;
        }

        private void New()
        {
            entries = new BindingList<Entry>();
            dataGridViewEntry.DataSource = entries;
            textBoxKey.ReadOnly = false;
        }

        #endregion
    }
}