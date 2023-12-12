using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Windows.Forms;

namespace TotalCommander
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBox1.Text = "C:\\";
            textBox2.Text = "C:\\Users";

            dataGridView1.Columns.Add("ItemType", "Тип");
            dataGridView1.Columns.Add("ItemName", "Имя");
            dataGridView1.Columns.Add("ItemSize", "Размер (байт)");
            dataGridView1.Columns.Add("LastModified", "Дата последнего изменения");

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.ReadOnly = true;
            }

            dataGridView2.Columns.Add("ItemType", "Тип");
            dataGridView2.Columns.Add("ItemName", "Имя");
            dataGridView2.Columns.Add("ItemSize", "Размер (байт)");
            dataGridView2.Columns.Add("LastModified", "Дата последнего изменения");

            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.ReadOnly = true;
            }

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView2.SelectionChanged += DataGridView2_SelectionChanged;

            loadFolder1();
            loadFolder2();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string fileName = dataGridView1.SelectedRows[0].Cells["ItemName"].Value.ToString();
                string itemType = dataGridView1.SelectedRows[0].Cells["ItemType"].Value.ToString();
                string selectedFilePath = Path.Combine(textBox1.Text, fileName);
                textBox3.Text = selectedFilePath;
                button11.Enabled = (itemType == "Папка");
            }
            else
            {
                button11.Enabled = false;
            }
        }

        private void DataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                string fileName = dataGridView2.SelectedRows[0].Cells["ItemName"].Value.ToString();
                string itemType = dataGridView2.SelectedRows[0].Cells["ItemType"].Value.ToString();
                string selectedFilePath = Path.Combine(textBox2.Text, fileName);
                textBox4.Text = selectedFilePath;
                button12.Enabled = (itemType == "Папка");
            }
            else
            {
                button12.Enabled = false;
            }
        }

        private void loadFolder1()
        {
            string folderPath = textBox1.Text;

            if (Directory.Exists(folderPath))
            {
                string[] items = Directory.GetFileSystemEntries(folderPath);
                dataGridView1.Rows.Clear();
                foreach (string item in items)
                {
                    FileInfo fileInfo = new FileInfo(item);
                    DirectoryInfo dirInfo = new DirectoryInfo(item);

                    string itemName;
                    long itemSize;

                    if (fileInfo.Exists)
                    {
                        itemName = fileInfo.Name;
                        itemSize = fileInfo.Length;
                    }
                    else if (dirInfo.Exists)
                    {
                        itemName = dirInfo.Name;
                        itemSize = -1;
                    }
                    else
                    {
                        continue;
                    }

                    dataGridView1.Rows.Add(fileInfo.Exists ? "Файл" : "Папка", itemName, itemSize, fileInfo.Exists ? fileInfo.LastWriteTime : dirInfo.LastWriteTime);
                }
            }
            else
            {
                MessageBox.Show("Указанный путь не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadFolder2()
        {
            string folderPath = textBox2.Text;

            if (Directory.Exists(folderPath))
            {
                string[] items = Directory.GetFileSystemEntries(folderPath);
                dataGridView2.Rows.Clear();
                foreach (string item in items)
                {
                    FileInfo fileInfo = new FileInfo(item);
                    DirectoryInfo dirInfo = new DirectoryInfo(item);

                    string itemName;
                    long itemSize;

                    if (fileInfo.Exists)
                    {
                        itemName = fileInfo.Name;
                        itemSize = fileInfo.Length;
                    }
                    else if (dirInfo.Exists)
                    {
                        itemName = dirInfo.Name;
                        itemSize = -1;
                    }
                    else
                    {
                        continue;
                    }

                    dataGridView2.Rows.Add(fileInfo.Exists ? "Файл" : "Папка", itemName, itemSize, fileInfo.Exists ? fileInfo.LastWriteTime : dirInfo.LastWriteTime);
                }
            }
            else
            {
                MessageBox.Show("Указанный путь не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadFolder1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadFolder2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
            {
                string folderName = Interaction.InputBox("Введите имя новой папки:", "Создание папки", "");

                if (!string.IsNullOrEmpty(folderName))
                {
                    string newFolderPath = Path.Combine(textBox1.Text, folderName);

                    try
                    {
                        Directory.CreateDirectory(newFolderPath);

                        MessageBox.Show($"Папка '{folderName}' успешно создана.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    loadFolder1();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string currentFolderPath = textBox1.Text;
            try
            {
                string parentFolderPath = Path.GetDirectoryName(currentFolderPath);

                if (!string.IsNullOrEmpty(parentFolderPath))
                {
                    textBox1.Text = parentFolderPath;
                    loadFolder1();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переходе в папку уровнем выше: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string currentFolderPath = textBox2.Text;
            try
            {
                string parentFolderPath = Path.GetDirectoryName(currentFolderPath);

                if (!string.IsNullOrEmpty(parentFolderPath))
                {
                    textBox2.Text = parentFolderPath;
                    loadFolder2();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переходе в папку уровнем выше: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string selectedFolderPath = Path.Combine(textBox1.Text, dataGridView1.SelectedRows[0].Cells["ItemName"].Value.ToString());
                textBox1.Text = selectedFolderPath;
                loadFolder1();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                string selectedFolderPath = Path.Combine(textBox2.Text, dataGridView2.SelectedRows[0].Cells["ItemName"].Value.ToString());
                textBox2.Text = selectedFolderPath;
                loadFolder2();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox2.Text))
            {
                string folderName = Interaction.InputBox("Введите имя новой папки:", "Создание папки", "");

                if (!string.IsNullOrEmpty(folderName))
                {
                    string newFolderPath = Path.Combine(textBox2.Text, folderName);

                    try
                    {
                        Directory.CreateDirectory(newFolderPath);

                        MessageBox.Show($"Папка '{folderName}' успешно создана.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    loadFolder2();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string itemType = dataGridView1.SelectedRows[0].Cells["ItemType"].Value.ToString();
                string itemName = dataGridView1.SelectedRows[0].Cells["ItemName"].Value.ToString();
                string itemPath = Path.Combine(textBox1.Text, itemName);
                if ((itemType == "Файл" && File.Exists(itemPath)) || (itemType == "Папка" && Directory.Exists(itemPath)))
                {
                    try
                    {
                        if (itemType == "Файл")
                        {
                            File.Copy(itemPath, Path.Combine(textBox2.Text, itemName), true);
                           
                        }
                        else if (itemType == "Папка")
                        {
                            CopyDirectory(itemPath, Path.Combine(textBox1.Text, itemName));
                        }
                        loadFolder2();
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            string rowItemName = row.Cells["ItemName"].Value.ToString();

                            if (rowItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                            {
                                row.Selected = true;
                                dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                                break;
                            }
                        }
                        MessageBox.Show($"{itemType} '{itemName}' успешно скопировано.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при копировании: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void CopyDirectory(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string destFile = Path.Combine(destinationPath, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }
            foreach (string subDirectory in Directory.GetDirectories(sourcePath))
            {
                string destSubDirectory = Path.Combine(destinationPath, Path.GetFileName(subDirectory));
                CopyDirectory(subDirectory, destSubDirectory);
            }
        }
    }
}
