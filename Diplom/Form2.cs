using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Form2 : Form
    {

        public static int N;
        public static int V;
        public static double d;
        public static double s;
        public static bool flag;
        public static List<Data> data;
        public static Random rnd;

        int V_current;
        double F_current;

        double V_best = int.MaxValue;
        double F_best = int.MaxValue;

        public Form2() { }

        public Form2(List<Data> dataList, int a, int b, double c, double q, bool e)
        {
            InitializeComponent();
            data = dataList;
            N = a;
            V = b;
            d = c;
            s = q;
            flag = e;

            rnd = new Random();

            int[] currentStrategy = new int[data.Count];
            int[] bestStrategy = new int[data.Count];
            StringBuilder str = new StringBuilder();
            double temp = 0;

            var startTime = System.Diagnostics.Stopwatch.StartNew();
            for (int iteration = 0; iteration < N; iteration++)
            {
                V_current = 0;
                F_current = 0;

                

                for (int i = 0; i < data.Count; i++)
                {
                    currentStrategy[i] = (int)Math.Round(rnd.NextDouble());
                    V_current += (1- currentStrategy[i]) * data[i].arraySize;
                }

                F_current = 0;

                for (int i = 0; i < data.Count; i++)
                {
                    F_current += currentStrategy[i] * data[i].resizeCount * (data[i].arraySize + d);
                }

                if (V_current > V)
                {
                    temp = F_current * d;
                    if (flag)
                    {
                        this.dataGridView1.Rows.Add(iteration + 1 + ")", "[" + string.Join(", ", currentStrategy) + "]", temp.ToString("0.########"), V_current, "не удовлетворяет ограничению");
                    }
                }
                else if (F_current < F_best)
                {
                    F_best = F_current;
                    V_best = V_current;

                    Array.Copy(currentStrategy, bestStrategy, currentStrategy.Length);

                    temp = F_current * d;
                    if (flag)
                    {
                        this.dataGridView1.Rows.Add(iteration + 1 + ")", "[" + string.Join(", ", currentStrategy) + "]", temp.ToString("0.########"), V_current, "новое лучшее решение");
                    }
                }
                else
                {
                    temp = F_current * d;
                    if (flag)
                    {
                        this.dataGridView1.Rows.Add(iteration + 1 + ")", "[" + string.Join(", ", currentStrategy) + "]", temp.ToString("0.########"), V_current, "хуже текущего лучшего решения");
                    }
                }
            }
            startTime.Stop();
            temp = F_best * d;

            textBox3.Text = "[" + string.Join(", ", bestStrategy) + "]";
            textBox10.Text = V_best.ToString("0.########");
            textBox11.Text = temp.ToString("0.########");
            textBox12.Text = startTime.Elapsed.TotalMilliseconds.ToString() + "мс";
            textBox4.Text = (startTime.Elapsed.TotalMilliseconds/N).ToString() + "мс";
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // если в таблицу добавлена новая строка, то изменить высоту таблицы
            ChangeHeight();
        }

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
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
    }

}
