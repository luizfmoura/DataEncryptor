using DataEncryptor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataEncryptor
{
    public partial class PasswordGenerationForm : Form
    {
        public PasswordGenerationForm()
        {
            InitializeComponent();
            textBoxAllowableChars.Text = KeyGeneration.DefaultChars;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            var gen = new KeyGeneration((int)numericUpDownLength.Value, textBoxAllowableChars.Text);
            textBoxPassword.Text = gen.GeneratePassword();
        }
    }
}
