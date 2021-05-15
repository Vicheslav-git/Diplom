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

        int V_current;

        public Form2(List<Data> dataList, int a, int b, double c, double d, bool e)
        {
            InitializeComponent();
            data = dataList;
            N = a;
            V = b;
            d = c;
            s = d;
            flag = e;

            int[] currentStrategy = new int[data.Count];


            for (int iteration = 0; iteration < N; iteration++)
            {
                V_current = 0;

                Random rnd = new Random(DateTime.Now.Second);

                for (int i = 0; i < data.Count; i++)
                {
                    currentStrategy[i] = (int)Math.Round(rnd.NextDouble());
                    V_current += (1- currentStrategy[i]) * data[i].m_size;
                }


            }

        }
    }
}
