using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TotalCommander
{
    public partial class Form1 : Form
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static HookProc proc = HookCallback;
        private static IntPtr hook = IntPtr.Zero;
        public Form1()
        {
            InitializeComponent();
            hook = SetHook(proc);
            Application.Run();
            UnhookWindowsHookEx(hook);

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
        private static IntPtr SetHook(HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (wParam == (IntPtr)WM_KEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (((Keys)vkCode == Keys.Enter))
                {
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(hook, nCode, wParam, lParam);
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
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
                // Отображение окна ввода имени папки
                string folderName = Interaction.InputBox("Введите имя новой папки:", "Создание папки", "");

                if (!string.IsNullOrEmpty(folderName))
                {
                    // Создание полного пути к новой папке
                    string newFolderPath = Path.Combine(textBox1.Text, folderName);

                    try
                    {
                        // Попытка создать новую папку
                        Directory.CreateDirectory(newFolderPath);

                        MessageBox.Show($"Папка '{folderName}' успешно создана.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Обновление содержимого DataGridView после создания новой папки
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
                // Отображение окна ввода имени папки
                string folderName = Interaction.InputBox("Введите имя новой папки:", "Создание папки", "");

                if (!string.IsNullOrEmpty(folderName))
                {
                    // Создание полного пути к новой папке
                    string newFolderPath = Path.Combine(textBox2.Text, folderName);

                    try
                    {
                        // Попытка создать новую папку
                        Directory.CreateDirectory(newFolderPath);

                        MessageBox.Show($"Папка '{folderName}' успешно создана.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Обновление содержимого DataGridView после создания новой папки
                    loadFolder2();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выделенные строки
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем тип элемента (Файл или Папка)
                string itemType = dataGridView1.SelectedRows[0].Cells["ItemType"].Value.ToString();
                // Получаем имя элемента
                string itemName = dataGridView1.SelectedRows[0].Cells["ItemName"].Value.ToString();
                // Получаем полный путь к элементу
                string itemPath = Path.Combine(textBox1.Text, itemName);

                // Проверяем, что элемент существует
                if ((itemType == "Файл" && File.Exists(itemPath)) || (itemType == "Папка" && Directory.Exists(itemPath)))
                {
                    try
                    {
                        if (itemType == "Файл")
                        {
                            // Копируем файл
                            File.Copy(itemPath, Path.Combine(textBox2.Text, itemName), true); // true - перезаписывать, если файл уже существует
                           
                        }
                        else if (itemType == "Папка")
                        {
                            // Копируем папку и её содержимое
                            CopyDirectory(itemPath, Path.Combine(textBox1.Text, itemName));
                        }
                        loadFolder2();
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            // Получаем значение из ячейки "ItemName"
                            string rowItemName = row.Cells["ItemName"].Value.ToString();

                            if (rowItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                            {
                                // Выделяем строку
                                row.Selected = true;

                                // Прокручиваем к выделенной строке (если она не видна)
                                dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;

                                // Выход из цикла после первого совпадения
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
            // Создаем папку, если её нет
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Копируем файлы
            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string destFile = Path.Combine(destinationPath, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            // Рекурсивно копируем подпапки
            foreach (string subDirectory in Directory.GetDirectories(sourcePath))
            {
                string destSubDirectory = Path.Combine(destinationPath, Path.GetFileName(subDirectory));
                CopyDirectory(subDirectory, destSubDirectory);
            }
        }
    }
}
