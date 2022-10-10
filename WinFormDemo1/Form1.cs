using MiniSoftware;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinFormDemo1
{

    public partial class Form1 : Form
    {

        //集_变量 
        //加个默认值
        public string huahenIp = "127.0.0.1";
        public string huahenPort = "80";

        public string yinjiaoIp = "127.0.0.1";
        public string yinjiaoPort = "81";

        int index = 0;
        string[] path1;
        string[] path2;
        string[] paths;
        private Point mouseOff;//鼠标移动位置变量

        private bool leftFlag;//鼠标是否为左键



        //读写配置的dll方法
        [DllImport("kernel32")]// 读配置文件方法的6个参数：所在的分区（section）、 键值、     初始缺省值、   StringBuilder、  参数长度上限 、配置文件路径
        public static extern long GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]//写入配置文件方法的4个参数：  所在的分区（section）、  键值、     参数值、       配置文件路径
        public static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        


        //窗口初始化
        public static Form1 frm1;
        public Form1()
        {
            frm1 = this;
            InitializeComponent();
            MyInit();
            initListView();
        }

        private void MyInit()
        {

            CheckForIllegalCrossThreadCalls = false;//false后不检查线程访问ui（线程任务可访问ui）

            //读取本地配置
            huahenIp = GetConfigValue("HuaHen","Ip");
            huahenPort = GetConfigValue("HuaHen", "Port");
            yinjiaoIp = GetConfigValue("YinJiao", "Ip");
            yinjiaoPort = GetConfigValue("YinJiao", "Port");


            label9.Text = Convert.ToString(trackBar1.Value);
            label9.Text = Convert.ToString(Convert.ToDouble(label9.Text) / 100);
            //取自身目录 是否存在dll
            //string str = System.Environment.CurrentDirectory;
            //str = str + @"\myDll.dll";
            //if (File.Exists(str))
            //{
            //    MessageBox.Show("ok");
            //}
            //else
            //{
            //    MessageBox.Show("没有找到myDll.dll");
            //}


        }


        //列表框初始化
        private void initListView()
        {

            //详情模式
            listView1.View = View.Details;
            //整行选择
            listView1.FullRowSelect = true;

            //设置列头
            listView1.Columns.Add("文件名", 175, HorizontalAlignment.Left);
            listView1.Columns.Add("置信度", 55, HorizontalAlignment.Left);
            listView1.Columns.Add("分类", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("路径",0, HorizontalAlignment.Left);//隐藏该列

            //数据项
            //ListViewItem listitem = new ListViewItem("123123.jpg", 0);
            //子数据
            //listitem.SubItems.Add("png");
            //listitem.SubItems.Add("不合格");
            //添加进列表框
            // listView1.Items.Add(listitem);




        }



        //获取图片
        private void GetPath_btn_Click_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "请选择图片文件路径";//对话框描述
            dialog.ShowNewFolderButton = false;//不允许新建文件夹

            if (dialog.ShowDialog() == DialogResult.OK)//选中确定按钮
            {
                //获取文件夹路径
                string defaultPath = dialog.SelectedPath;
                textBox1.Text = defaultPath;


                //获取文件夹下的所有文件
                path1 = Directory.GetFiles(defaultPath, "*.png");
                path2 =Directory.GetFiles(defaultPath, "*.jpg");

                //合并图片数组
                paths = new string[path1.Length + path2.Length];
                Array.Copy(path1, 0, paths, 0, path1.Length);
                Array.Copy(path2, 0, paths,path1.Length, path2.Length);


                if (paths.Length == 0)
                {
                    MessageBox.Show("该目录下没有图片");
                    return;

                }

                index = 0;//初始化索引，防止当前图片索引超出范围

                label1.Text = "共有[" + paths.Length + "]张图片";

                //开始读取图片 
                Stream s = File.Open(paths[index], FileMode.Open);
                Image img = Bitmap.FromStream(s);
                pictureBox1.Image = img;
                s.Close();
                s.Dispose();

                //pictureBox1.Load(path[index]);
                label2.Text = "当前是第[1]张图";

            }

        }

        //下一张图
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (paths == null)
            {
                return;
            }


            if (index >= paths.Length - 1)
            {
                MessageBox.Show("最后一张图了！");
                return;
            }

            index = index + 1;

            //开始读取图片 
            Stream s = File.Open(paths[index], FileMode.Open);
            Image img = Bitmap.FromStream(s);
            pictureBox1.Image = img;
            s.Close();
            s.Dispose();


            
            label2.Text = "当前是第[" + (index + 1) + "]张图";
        }

        //上一张图
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (paths == null)
            {
                return;
            }

            if (index == 0)
            {
                MessageBox.Show("第一张图了！");
                return;
            }
            index = index - 1;

            //开始读取图片 
            Stream s = File.Open(paths[index], FileMode.Open);
            Image img = Bitmap.FromStream(s);
            pictureBox1.Image = img;
            s.Close();
            s.Dispose();

            label2.Text = "当前是第[" + (index + 1) + "]张图";
        }



        //鼠标按下
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//获得当前鼠标的坐标
                leftFlag = true;
            }
        }

        //鼠标移动
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;//获得移动后鼠标的坐标
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的位置
                Location = mouseSet;//窗口为移动值
            }
        }

        //鼠标放开
        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;
            }
        }


        //鼠标进入最小化键
        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.BackColor = Color.Gray;
        }

        //鼠标离开最小化键
        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(0, 120, 215);
        }

        //鼠标点击最小化键
        private void label4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//窗口最小化
        }

        //鼠标进入关闭键
        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.BackColor = Color.Red;
        }

        //鼠标离开关闭键
        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = Color.FromArgb(0, 120, 215);
        }

        //鼠标点击关闭键
        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //插入列表
        public void insertIntoListView(string picPath,string zhixindu,string fenlei,string FileMuluPath)
        {
            //数据项

            ListViewItem listitem = new ListViewItem(picPath, 0);
            //子数据
            listitem.SubItems.Add(zhixindu);
            listitem.SubItems.Add(fenlei);
            listitem.SubItems.Add(FileMuluPath);
            //添加进列表框
            listView1.Items.Add(listitem);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();//清空
        }

        //鼠标进入设置按钮
        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Gray;
        }
        //鼠标离开置按钮
        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.FromArgb(0, 120, 215);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            new settingForm().Show();
        }

        //检测单张图片
        private void button5_Click(object sender, EventArgs e)
        {
            //多线程send
            if (paths == null)
            {
                return;
            }

            //判断是否设置服务地址
            if (radioButton1.Checked)
            {
                if (huahenIp == "")
                {
                    MessageBox.Show("请先设置SDK启动服务地址");
                    return;

                }
            }

            if (radioButton2.Checked)
            {
                if (yinjiaoIp == "")
                {
                    MessageBox.Show("请先设置SDK启动服务地址");
                    return;
                }
            }



            //检查端口是否开启
            string ip = "";
            string port = "";
            if (radioButton1.Checked)
            {
                ip = huahenIp;
                port = huahenPort;
            }

            if (radioButton2.Checked)
            {
                ip = yinjiaoIp;
                port = yinjiaoPort;
            }
            if (port == "")
            {
                port = "80";
            }

            IPAddress ipa = IPAddress.Parse(ip);
            IPEndPoint point = new IPEndPoint(ipa, Convert.ToInt32(port));
            TcpClient tcp = null;

            try
            {
                tcp = new TcpClient();
                tcp.Connect(point);
                //MessageBox.Show("端口打开");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查SDK服务是否开启，错误消息：\n\n" + ex.Message);
                return;
            }
            finally
            {
                if (tcp != null)
                {
                    tcp.Close();
                }
            }



            ThreadStart threadStart = new ThreadStart(() =>
            {
                 sendPicture(paths[index]);
            });
            Thread thread = new Thread(threadStart);
            thread.Start();
            
        }

        

        //[DllImport(@"C:\Users\zwl\Desktop\mydll.dll")]
        //public static extern void msgbox(string text);


        //生成报告Word
        public bool creatrWord()
        {
            //string templePath = @"D:\temp.docx";
            //这里填写和软件放在一起的word模板文件，可以修改里面的内容 或者改名
            string templePath = "./template.docx";

            //ToUniversalTime()转换为标准时区的时间,去掉的话直接就用北京时间
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            //得到精确到毫秒的时间戳（长度13位）
            long time = (long)ts.TotalMilliseconds;
            string docxTitle = ""; 

            if (radioButton1.Checked) {
                docxTitle = "图像分类报告";
            }

            if (radioButton2.Checked)
            {
                docxTitle = "物体检测报告";
            }

            string ouputWordPath = textBox4.Text + @"\" + docxTitle+ time + ".docx";


            List<Dictionary<string, object>> lis = new List<Dictionary<string, object>> { };

            //读取listview1行
            for (int i = 0; i < listView1.Items.Count; i++)
            {

                lis.Add(new Dictionary<string, object> {
                    { "picture", listView1.Items[i].SubItems[0].Text},
                    { "zxd", listView1.Items[i].SubItems[1].Text} ,
                    { "fenlei", listView1.Items[i].SubItems[2].Text}

                });
            }

            string WordIntitle = "";
            if (radioButton1.Checked)
            {
                WordIntitle = "图像分类报告";
            }

            if (radioButton2.Checked)
            {
                WordIntitle = "物体检测报告";
            }

            var value = new Dictionary<string, object>()
            {
                ["title"] = WordIntitle,
                ["time"] = DateTime.Now,
                ["yuzhi"] = label9.Text,
                ["picPath"] = textBox1.Text,
                ["My"] = "霖哥 - WX：ZWL10203040",
                //["jc"] = new List<Dictionary<string, object>> {
                //new Dictionary<string, object> { { "picture", "45678453.png" }, { "isok", "合格" }}
                //new Dictionary<string,object>{ { "picture","45678453.png"}, { "isok","合格"} },
                // new Dictionary<string,object>{ { "picture", "4534534.png" }, { "isok","不合格"} },
                //  new Dictionary<string,object>{ { "picture", "456346876.png" }, { "isok","合格"} },
                //   new Dictionary<string,object>{ { "picture", "124537893sa.png" }, { "isok", "不合格" } },
                //}
                ["jc"] = lis
            };

            try
            {
                MiniWord.SaveAsByTemplate(ouputWordPath, templePath, value);
                
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("保存报告失败，请关闭已打开报告文档");
                return false;
                //throw;
            }

        }




        //开始检测
        public string sendPicture(string picPath)
        {

            FileStream fs = new FileStream(picPath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] img = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();


            string url = "http://127.0.0.1:80?threshold="+ label9.Text;
            if (radioButton1.Checked)
            {
                if (huahenIp == "")
                {
                    MessageBox.Show("请先设置SDK启动服务地址");
                    return null;

                }
                url = "http://"+huahenIp+":"+huahenPort+ "?threshold="+ label9.Text;

            }

            if (radioButton2.Checked)
            {
                if (yinjiaoIp == "")
                {
                    MessageBox.Show("请先设置SDK启动服务地址");
                    return null;

                }
                url = "http://" + yinjiaoIp + ":" + yinjiaoPort + "?threshold="+ label9.Text;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            try
            {
            Stream stream = request.GetRequestStream();
             stream.Write(img, 0, img.Length);
             stream.Close();
             WebResponse response = request.GetResponse();
             StreamReader sr = new StreamReader(response.GetResponseStream());
             string jsonstr = sr.ReadToEnd();//返回文本
             string zhixinfu = readJsonZhixindu(jsonstr);//读取置信度
                if (zhixinfu==null)
                {
                    zhixinfu = "未知";
                }
             string fenlei = readJsonFenlei(jsonstr);
                if (fenlei == null)
                {
                    fenlei = "未知";
                }


             insertIntoListView(Path.GetFileName(picPath), zhixinfu, fenlei, picPath);//插入列表
             Console.WriteLine(sr.ReadToEnd());
             sr.Close();
             response.Close();
             
             return jsonstr;
            }
            catch (Exception)
            {
                //MessageBox.Show("检测失败，请检查SDK服务启动情况");
                insertIntoListView(Path.GetFileName(picPath), "未知", "未知", picPath);

            }

            return null;

            

        }





        //读取置信度
        public string readJsonZhixindu(string json)
        {
            try
            {
            JObject jobj = JObject.Parse(json);
            string str = jobj["results"][0]["confidence"].ToString();
            str = Convert.ToString(decimal.Round(decimal.Parse(str), 2));
            return str;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //读取json分类哦
        public string readJsonFenlei(string json)
        {
            try
            {
                JObject jobj = JObject.Parse(json);
                string str = jobj["results"][0]["label"].ToString();
                return str;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "请选择报告保存路径";//对话框描述
            dialog.ShowNewFolderButton = true;//允许新建文件夹

            if (dialog.ShowDialog() == DialogResult.OK)//选中确定按钮
            {
                textBox4.Text = dialog.SelectedPath;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

            if (textBox4.Text =="")
            {
                MessageBox.Show("请选择报告输出路径！");
                return;
            }

            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("还没有检测哦！");
                return;
            }

            bool res = creatrWord();

            if (res) {
                MessageBox.Show("报告输出完成！");
            }
        }



        /*读配置文件*/
        public static string GetConfigValue(string section, string key)
        {
            // ▼ 获取当前程序启动目录
            // string strPath = Application.StartupPath + @"/config.ini"; 这里是绝对路径
            string strPath = "./config.ini";  //这里是相对路径
            if (File.Exists(strPath))  //检查是否有配置文件，并且配置文件内是否有相关数据。
            {
                StringBuilder sb = new StringBuilder(255);
                GetPrivateProfileString(section, key, "配置文件不存在，读取未成功!", sb, 255, strPath);

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /*写配置文件*/
        public static void SetConfigValue(string section, string key, string value)
        {
            // ▼ 获取当前程序启动目录
            // string strPath = Application.StartupPath + @"/config.ini";  这里是绝对路径
            string strPath = "./config.ini";      //这里是相对路径，
            WritePrivateProfileString(section, key, value, strPath);
        }

        //批量检测
        private void button4_Click(object sender, EventArgs e)
        {
            label10.Text = "";
            //判断是否已有图片
            if (paths == null)
            {
                return;
            }


            //判断是否设置服务地址
            if (radioButton1.Checked)
            {
                if (huahenIp == "")
                {
                    MessageBox.Show("请先设置SDK启动服务地址");
                    return;

                }
            }

            if (radioButton2.Checked)
            {
                if (yinjiaoIp == "")
                {
                    MessageBox.Show("请先设置SDK启动服务地址");
                    return;
                }
            }


            //检查端口是否开启
            string ip = "";
            string port = "";
            if (radioButton1.Checked)
            {
                ip = huahenIp;
                port = huahenPort;
            }

            if (radioButton2.Checked)
            {
                ip = yinjiaoIp;
                port = yinjiaoPort;
            }
            if (port=="")
            {
                port = "80";
            }

            IPAddress ipa = IPAddress.Parse(ip);
            IPEndPoint point = new IPEndPoint(ipa, Convert.ToInt32(port));
            TcpClient tcp = null;

            try
            {
                tcp = new TcpClient();
                tcp.Connect(point);
                //MessageBox.Show("端口打开");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查SDK服务是否开启，错误消息：\n\n" + ex.Message);
                return;
            }
            finally
            {
                if (tcp != null)
                {
                    tcp.Close();
                }
            }



            label10.Text = "检测中";
            button5.Enabled = false;
            button4.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            GetPath_btn_Click.Enabled = false;
            trackBar1.Enabled = false;
            ThreadStart threadStart = new ThreadStart(() =>
            {
                for (int i = 0; i < paths.Length; i++)
                {

                    sendPicture(paths[i]);
                    if (i == paths.Length-1)
                    {
                        button5.Enabled = true;
                        button4.Enabled = true;
                        radioButton2.Enabled = true;
                        radioButton1.Enabled = true;
                        GetPath_btn_Click.Enabled = true;
                        trackBar1.Enabled = true;
                        label10.Text = "检测完成！";
                    }
                }
            });
            Thread thread = new Thread(threadStart);
            thread.Start();


        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            label9.Text = Convert.ToString(trackBar1.Value);

            label9.Text = Convert.ToString(Convert.ToDouble(label9.Text) / 100);

        }

        //列表框选中项改变事件
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            else
            {
                //选中点击那一行的第一列的值，索引值必须是0，而且无论点这一行的第几列，选中的都是这一行第一列的值 ，如果想获取这一行除第一列外的值，则用subitems获取，[]中为索引，从1开始。
                string picPath = listView1.SelectedItems[0].SubItems[3].Text;

                //开始读取图片 
                Stream s = File.Open(picPath, FileMode.Open);
                Image img = Bitmap.FromStream(s);
                pictureBox1.Image = img;
                s.Close();
                s.Dispose();
            }

        }
    }


}
