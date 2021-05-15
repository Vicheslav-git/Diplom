using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static int N;
        public static int V;
        public static double d;
        public static double s;
        public static bool flag;

        string[] fileText;
        Dictionary<string, int[]> map = new Dictionary<string, int[]>();
        public static List<Data> dataList;


        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // если в таблицу добавлена новая строка, то изменить высоту таблицы
            ChangeHeight();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // если в таблице удалена строка, то изменить высоту таблицы
            ChangeHeight();
        }

        private void ChangeHeight()
        {
            // меняем высоту таблицу по высоте всех строк
            dataGridView1.Height = dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible) +
                               dataGridView1.ColumnHeadersHeight;
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {

            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            this.dataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);

            

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            fileText = System.IO.File.ReadAllLines(filename);

            //////////////////////////////////////////////////////////////////////////////////
            Regex regexWithSize = new Regex(@"vector<(\w+)> (\w+)(\s?)\((\d+)\)");
            Regex regex2WithoutSize = new Regex(@"vector<(\w+)> (\w+)");
            Regex regexForResize = new Regex(@"(\w+).resize\((\d+)\)");
            Regex regexCycle = new Regex(@"for\s?\(int \w+ = (\d+);\si < (\d+);");

            int c = 1;
            int count = 0;
            bool flag = false;
            String str = "";
            for (int i = 0; i < fileText.Length; i++)
            {
                str = fileText[i];
                Match matchWithSize = regexWithSize.Match(str);
                Match matchWithoutSize = regex2WithoutSize.Match(str);
                Match matchCycle = regexCycle.Match(str);
                Match matchResize = regexForResize.Match(str);
                if (matchCycle.Success)
                {
                    flag = matchCycle.Success;
                    GroupCollection groupCycle = matchCycle.Groups;
                    c = Int16.Parse(groupCycle[2].Value);
                    if (str.Contains("{")) count++;
                    continue;
                }

                if (flag)
                {
                    if (str.Contains("{")) count++;
                    if (str.Contains("}")) count--;

                    if (count == 0)
                    {
                        c = 1;
                        flag = false;
                    }

                }

                if (matchWithSize.Success)
                {
                    GroupCollection groupWithSize = matchWithSize.Groups;
                    string name = groupWithSize[2].Value;
                    string typeName = groupWithSize[1].Value;
                    int type = 0;
                    switch (typeName)
                    {
                        case "double":
                            type = 8;
                            break;
                        case "int":
                            type = 4;
                            break;
                    }

                    int size = Int32.Parse(groupWithSize[4].Value);
                    int resizeCount = c;
                    int[] arr = new int[] { size, type, resizeCount };

                    map.Add(name, arr);
                }
                else if (matchWithoutSize.Success)
                {

                    GroupCollection groupWithoutSize = matchWithoutSize.Groups;
                    string name = groupWithoutSize[2].Value;
                    string typeName = groupWithoutSize[1].Value;
                    int type = 0;
                    switch (typeName)
                    {
                        case "double":
                            type = 8;
                            break;
                        case "int":
                            type = 4;
                            break;
                    }
                    int size = 0;
                    int resizeCount = c;

                    int[] arr = new int[] { size, type, resizeCount };
                    map.Add(name, arr);
                }
                else if (matchResize.Success)
                {
                    GroupCollection groupResize = matchResize.Groups;
                    map[groupResize[1].Value][0] = Int32.Parse(groupResize[2].Value);
                    map[groupResize[1].Value][2] = c;
                }
            }

            Console.WriteLine(map);
            dataGridView1.Visible = true;
            //int  qwer = AddNumbers(structs, structs.Length);

            foreach (Data data in mapToMyStructs(map))
            {
                this.dataGridView1.Rows.Add(data.name, data.count, data.size, data.arraySize, data.resizeCount);
            }

        }

        private static List<Data> mapToMyStructs(Dictionary<string, int[]> map)
        {

            dataList = new List<Data>();
            foreach (KeyValuePair<string, int[]> kvp in map)
            {
                Data data = new Data();

                data.name = kvp.Key;
                data.count = kvp.Value[0];
                data.size = kvp.Value[1];
                data.arraySize = kvp.Value[0] * kvp.Value[1];
                data.resizeCount = kvp.Value[2];
                dataList.Add(data);
            }


            return dataList;
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            flag = ribbonCheckBox1.Checked;
            N = Convert.ToInt32(ribbonTextBox1.TextBoxText);
            V = Convert.ToInt32(ribbonTextBox5.TextBoxText);
            //d = Convert.ToDouble(ribbonTextBox2.TextBoxText);
            d = double.Parse(ribbonTextBox2.TextBoxText, CultureInfo.InvariantCulture);
            s = double.Parse(ribbonTextBox4.TextBoxText, CultureInfo.InvariantCulture);

            Form2 form2 = new Form2(dataList, N, V, d, s, flag);
            form2.Show();
        }
    }

    
}
