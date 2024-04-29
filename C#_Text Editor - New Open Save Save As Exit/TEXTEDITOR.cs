using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEXTEDITOR
{
    public partial class TEXTEDITOR : Form
    {
        private string _currentFile; // Untuk melacak file yang sedang dibuka

        public TEXTEDITOR()
        {
            InitializeComponent();
            richTextBox1.Text = ""; // Memastikan TextBox kosong
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return; // Keluar jika pengguna membatalkan perubahan yang dibuang
            }

            richTextBox1.Clear();
            _currentFile = null; // Menandakan tidak ada file yang dibuka
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return; // Keluar jika pengguna membatalkan perubahan yang dibuang
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "File Teks (*.txt)|*.txt";
            openFileDialog.Title = "Buka File Teks";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    richTextBox1.Text = File.ReadAllText(openFileDialog.FileName);
                    _currentFile = openFileDialog.FileName; // Perbarui file yang sedang dibuka
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentFile == null)
            {
                saveAsToolStripMenuItem_Click(sender, e); // Gunakan saveAs jika tidak ada file yang dibuka
                return;
            }

            try
            {
                File.WriteAllText(_currentFile, richTextBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "File Teks (*.txt)|*.txt";
            saveFileDialog.Title = "Simpan File Teks";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                    _currentFile = saveFileDialog.FileName; // Perbarui file yang sedang dibuka
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return; // Keluar jika pengguna membatalkan perubahan yang dibuang
            }

            Application.Exit();
        }

        private bool ConfirmDiscardChanges()
        {
            if (richTextBox1.Text.Length > 0)
            {
                DialogResult result = MessageBox.Show("Apakah Anda ingin menyimpan perubahan?", "Konfirmasi", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(null, null); // Simpan dan lanjutkan
                        return true;
                    case DialogResult.No:
                        return true; // Buang dan lanjutkan
                    case DialogResult.Cancel:
                        return false; // Batalkan operasi
                    default:
                        return false; // Tangani hasil yang tidak terduga
                }
            }
            else
            {
                return true; // Tidak ada perubahan yang dibuang
            }
        }
    }
}
