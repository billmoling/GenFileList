using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace GenFileNameList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("input folder path");
                return;
            }

            DirectoryInfo di = new DirectoryInfo(textBox1.Text);
            

            FileInfo[] fis = di.GetFiles("*.*",SearchOption.AllDirectories);
            List<string> nameList = new List<string>();
           
            for (int i = 0; i < fis.Length; i++)
            {
                nameList.Add(fis[i].Name);
            }

            CultureInfo StrokCi = new CultureInfo(133124);
            System.Threading.Thread.CurrentThread.CurrentCulture = StrokCi;
            
            nameList.Sort(new AlphanumComparatorFast());


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nameList.Count; i++)
            {
                sb.Append(nameList[i] + "\r\n");
                //sb.Append(string.Format("insert into PictureSet(PictureName,pictureDescription) values('{0}','{1}')", "gallary" + i, nameList[i]));

                //sb.Append("\r\n");



            }

            textBox2.Text = sb.ToString();


        }
    }
}
