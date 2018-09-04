using CQCM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CQCMWin
{
    public partial class Form1 : Form
    {
        private string dbFile = "";

        private string siteFile = "";

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
        }

        private void btnDb_Click(object sender, EventArgs e)
        {
            dbFile = GetPath();
            txtDb.Text = dbFile;
        }

        private void btnSite_Click(object sender, EventArgs e)
        {
            siteFile = GetPath();
            txtSite.Text = siteFile;
        }

        /// <summary>
        /// 提取文件路径
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            OpenFileDialog fileLog = new OpenFileDialog();
            fileLog.Filter = "excel file(*.xlsx)|*.xlsx|excel file(*.xls)|*.xls";
            fileLog.Multiselect = false;

            if (fileLog.ShowDialog() == DialogResult.OK)
            {
                return fileLog.FileName;
            }
            return "";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dbFile == "" || siteFile == "")
            {
                MessageBox.Show("请选择输入文件");
                return;
            }
            if (!(File.Exists(dbFile) && File.Exists(siteFile)))
            {
                MessageBox.Show("请确定输入文件是否存在");
                return;
            }

            if (IsFileInUse(dbFile) || IsFileInUse(siteFile))
            {
                MessageBox.Show("请确定输入文件是否处于关闭状态");
                return;
            }

            richTxt.Clear();
            RichBoxAddMessage("开始扩容...");
            //ExpSite();

            Task.Run(() =>
            {
                try
                {
                    CheckForIllegalCrossThreadCalls = false;
                    ExpSite();                                       
                }
                 catch
                {
                    RichBoxAddMessage("扩容失败");
                }
            });
        }

        /// <summary>
        /// 扩容
        /// </summary>
        public void ExpSite()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParseCMSite siteExcel = new ParseCMSite(siteFile);
            DataTable dt = siteExcel.Translate("扩容站点");

            ParseDatabase.siteLst = siteExcel.GetSite(dt);
            ParseDatabase db = new ParseDatabase(dbFile);
            DataTable dbDt = db.Translate("database input");

            CmProducer cm = new CmProducer();
            DataTable cmDt = cm.Produce(ParseDatabase.siteLst, dbDt, dt);

            Neighbor neighbor = new Neighbor();
            DataTable result = neighbor.GetNeighbor(cmDt);

            SaveExcel excel = new SaveExcel(GetSavePath());
            excel.SaveToExcel(cmDt, "扩容输出", result, "自邻区");
            RichBoxAddMessage("扩容成功");
            sw.Stop();
            richTxt.AppendText(string.Format("耗时：{0} s", sw.ElapsedMilliseconds/1000));
        }

        /// <summary>
        /// 生成保存路径
        /// </summary>
        /// <returns></returns>
        public string GetSavePath()
        {
            string nowTime = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH-mm-ss");
            string savePath = Path.GetDirectoryName(siteFile) + string.Format(@"\output\{0}{1}.xlsx", Path.GetFileNameWithoutExtension(siteFile), nowTime);
            string saveDir = Path.GetDirectoryName(siteFile) + @"\output";
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            return savePath;
        }

        /// <summary>
        /// 判断文件是否被打开
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>文件打开状态</returns>
        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                FileShare.None);
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用    
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 给richbox文本框添加数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="param"></param>
        public void RichBoxAddMessage(string message, params string[] param)
        {
            if (richTxt.InvokeRequired)
            {
                richTxt.BeginInvoke(new Action(() =>
                {
                    richTxt.AppendText(String.Format(message, param) + Environment.NewLine);
                }));
            }
            else
            {
                richTxt.AppendText(String.Format(message, param) + Environment.NewLine);
            }
        }
    }
}
