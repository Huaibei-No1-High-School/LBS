using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LBS
{
    public partial class Form1 : Form
    {
        string path = @"pic";
        string indexpath = @"index.ho";
        string cntpath = @"cnt.ho";
        string APP_ID = "17942189";
        string API_KEY = "dD4bDfmKj9QGvGz84XvVLwaV";
        string SECRET_KEY = "7siy7rVkth8UKsB3dO3syZ8zXj1fmgRc";
        List<string> al;
        List<string> cont;
        Baidu.Aip.Ocr.Ocr client;
        public Form1()
        {
            InitializeComponent();
            client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            al = new List<string>();
            cont = new List<string>();
            updatelist();
            updatelv();
        }
        private void updatelist()
        {
            cont.Clear();
            al.Clear();
            string []tss = File.ReadAllLines(indexpath, Encoding.GetEncoding("GBK"));
            foreach(string s in tss)
            {
                if(s.Trim() != "")
                {
                    cont.Add(s.Trim());
                }
            }
            Console.WriteLine(cont.Count);
            foreach(string s in cont)
            {
                al.Add(Regex.Split(s, "@*@")[0]);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            updatelist();
            Checknewfile();
            textBox1.Select();
            textBox1.Focus();
            pictureBox2.Image = Resource1.no;
        }
        private bool Checknewfile()
        {
            var files = Directory.GetFiles(path, "*.*");
            string has = File.ReadAllText(cntpath).Trim();
            has = (Convert.ToInt32(has) + 1).ToString();
            string havnt = files.Count().ToString();
            label1.Text = "/ " + havnt;
            label2.Text = has;
            if(has != havnt)
            {
                label2.ForeColor = Color.Red;
                return true;
            }
            else
            {
                label2.ForeColor = Color.Black;
                return false;
            }
        }
        int getcnt()
        {
            string has = File.ReadAllText(cntpath).Trim();
            return Convert.ToInt32(has);
        }
        private void addimg(string tpath)
        {
            //读取编号
            int tcnt = getcnt()+1;
            string filename = "";
            //OCR 添加index
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(indexpath,true,Encoding.GetEncoding("GBK")))
            {
                var res = client.General(File.ReadAllBytes(tpath));
                string st = tcnt.ToString() + "." + tpath.Split('.')[1];
                filename = st;
                st += "@*@";
                foreach (var str in res["words_result"])
                {
                    st += str["words"].ToString() + "@*@";
                }
                Console.WriteLine(st);
                file.WriteLine(st);
            }
            //添加图片
            FileStream fs = new FileStream(path + "/" + filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(File.ReadAllBytes(tpath), 0, File.ReadAllBytes(tpath).Length);
            bw.Close();
            fs.Close();
            //写入编号
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(cntpath,false, Encoding.GetEncoding("GBK")))
            {
                file.WriteLine(tcnt);
            }
            //更新List
            updatelist();
            updatelist();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //frcnt();
            
        }


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;                                                              //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            Console.WriteLine("!");
            string tpath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            addimg(tpath);
            Checknewfile();
        }
        private void updatenewfiles()
        {
            var files = Directory.GetFiles(path, "*.*");
            foreach (string s in files)
            {
                string ts = Path.GetFileName(s);
                if (!al.Contains(ts))
                {
                    addimg(path + "/" + ts);
                    File.Delete(path + "/" + ts);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            updatelist();
            if (Checknewfile())
            {
                updatenewfiles();
            }
            Checknewfile();
            updatelv();
        }
        private void updatelv()
        {
            listBox1.Items.Clear();
            foreach (string s in cont)
            {
                if(s.ToLower().Contains(textBox1.Text.ToLower()))
                {
                    listBox1.Items.Add(s);
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updatelv();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItems.Count > 0)
            {
                pictureBox1.Load(path + "/" + Regex.Split(listBox1.SelectedItem.ToString(), "@*@")[0]);
                pictureBox2.Image = Resource1.no;
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox1.Image);
            pictureBox2.Image = Resource1.ok;
        }
    }
}
