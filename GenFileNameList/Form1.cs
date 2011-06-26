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
using System.Data.SQLite;
using System.Data.Common;

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
            Dictionary<string, string> fileNameWithDesciption = new Dictionary<string, string>();


            List<string> nameList = new List<string>();
           
            for (int i = 0; i < fis.Length; i++)
            {
                string newfileName=txt_prefix.Text + i;

                fileNameWithDesciption.Add(newfileName, fis[i].Name);

                RenameFile(fis[i], newfileName);
                
            }
            AddFileToDB(fileNameWithDesciption);


            dataGridView1.DataSource = ReadData();

            //CultureInfo StrokCi = new CultureInfo(133124);
            //System.Threading.Thread.CurrentThread.CurrentCulture = StrokCi;
            
            //nameList.Sort(new AlphanumComparatorFast());


            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < nameList.Count; i++)
            //{
            //    sb.Append(nameList[i] + "\r\n");
            //    //sb.Append(string.Format("insert into PictureSet(PictureName,pictureDescription) values('{0}','{1}')", "gallary" + i, nameList[i]));

            //    //sb.Append("\r\n");



            //}

            //textBox2.Text = sb.ToString();


        }

        private void RenameFile(FileInfo fileInfo,string newName)
        {
            string path = fileInfo.DirectoryName;
            string ext = fileInfo.Extension;

            fileInfo.CopyTo(Path.Combine(path,newName+ext));

        }

        private DataTable ReadData()
        {
            SQLiteDataAdapter ad;
            SQLiteConnection sqlite = new SQLiteConnection("Data Source=ROC.db3");
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();  //Initiate connection to the db
                cmd = sqlite.CreateCommand();
                cmd.CommandText = "select * from NameDescription";  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }
            sqlite.Close();
            return dt;
        }


        public static void AddFileToDB(Dictionary<string, string> contenet)
        {
            
            DbProviderFactory factory = SQLiteFactory.Instance;
            using (DbConnection conn = factory.CreateConnection())
            {
                // 连接数据库
                conn.ConnectionString = "Data Source=ROC.db3";
                conn.Open();

                // 创建数据表
               
                DbCommand cmd = conn.CreateCommand();
                
                // 添加参数
                //cmd.Parameters.Add(cmd.CreateParameter());



                DbTransaction trans = conn.BeginTransaction(); // <-------------------
                try
                {
                    // 连续插入1000条记录
                    foreach (var item in contenet)
	                {
                        cmd.CommandText = string.Format("insert into NameDescription(FileName,Description) values ('{0}','{1}')",item.Key,item.Value);
                       

                        cmd.ExecuteNonQuery();
	                }

                    trans.Commit(); // <-------------------
                }
                catch
                {
                    trans.Rollback(); // <-------------------
                    throw; // <-------------------
                }

            }
       
        }

    }
}
