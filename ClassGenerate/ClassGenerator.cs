using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ClassGenerate.Proc;

namespace ClassGenerate
{
    public partial class ClassGenerator : Form
    {
        /// <summary>
        /// 生成路径
        /// </summary>
        string path = "";
        /// <summary>
        /// 数据库访问
        /// </summary>
        public static DataAccess da;
        /// <summary>
        /// 表处理
        /// </summary>
        ProcTable pt ;
        /// <summary>
        /// 文件操作对象
        /// </summary>
        Generator g ;

        public ClassGenerator(DBConnectType ConnectType)
        {
            InitializeComponent();
            da = new DataAccess(ConnectType);
            pt = new ProcTable();
            g = new Generator();
            pt.SetConnectionString(LoadServer.cn);
            if (TestAndLoad())
            {
                btnGenerate.Enabled = true;
            } 
        }

        #region 初始化
        private void ClassGenerator_Load(object sender, EventArgs e)
        {
            DataTable dtDatabase = pt.GetAllDataBase(false);
            foreach (DataRow dr in dtDatabase.Rows)
            {
                treeView1.Nodes.Add(dr["name"].ToString());
            }

            lblMsg.Text = "请在左边选择数据库";
        } 
        #endregion
        
        #region 退出程序
        private void ClassGenerator_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        } 
        #endregion

        #region 点击树设置数据库
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            LoadServer.cn.Database = e.Node.Text;
            gbOpition.Text = e.Node.Text + ":生成选项";
            pt.SetConnectionString(LoadServer.cn);

            if (TestAndLoad())
            {
                btnGenerate.Enabled = true;
            } 

            lblMsg.Text = "";
        } 
        #endregion

        #region 测试连接
        private void btnTest_Click(object sender, EventArgs e)
        {
            TestAndLoad();
        }
        /// <summary>
        /// 测试,加载数据
        /// </summary>
        private bool TestAndLoad()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(LoadServer.cn.ConnectionString))
            {
                MessageBox.Show("请输入连接字符串,测试通过后才可使用");
                lblMsg.Text = "代码生成器:连接失败或没有表";
                ret = false;
            }
            da.strConnection = LoadServer.cn.ConnectionString;
            if (da.TestConnect())
            {
                lblMsg.Text = "代码生成器:连接成功";
                BindAllTables();
                ret = true;
            }
            return ret;
        } 
        #endregion

        #region 绑定当前数据库中所有用户表
        /// <summary>
        /// 绑定所有用户表
        /// </summary>
        void BindAllTables()
        {
            try
            {
                DataTable dt = pt.GetAllTable();

                lvTables.Items.Clear();
                for(int i=0; i<dt.Rows.Count; i ++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, dt.Rows[i][0].ToString()));
                    //item.SubItems.Add(new ListViewItem.ListViewSubItem(item, pt.GetTableDesciption(dt.Rows[i][0].ToString())));
                    //item.Checked =!Convert.ToBoolean( dt.Rows[i]["IsDeleted"]);
                    lvTables.Items.Add(item);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        void BindAllTables(DataTable dt, bool boolChecked)
        {
            try
            {
                lvTables.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = boolChecked;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, dt.Rows[i][0].ToString()));
                    //item.SubItems.Add(new ListViewItem.ListViewSubItem(item, pt.GetTableDesciption(dt.Rows[i][0].ToString())));
                    //item.Checked =!Convert.ToBoolean( dt.Rows[i]["IsDeleted"]);
                    lvTables.Items.Add(item);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        } 
        #endregion
        
        #region 设置生成属性
        /// <summary>
        /// 设置生成属性
        /// </summary>
        /// <param name="g"></param>
        void SetGenerator(Generator g)
        {
            g.strNameSpace = txtNamespace.Text.Trim();
            g.strNameSpace = "CommonLib.Entity";
            g.strPrefix = txtPrefix.Text.Trim();
            g.path = path;
        } 
        #endregion

        #region 获取用户选中的生成选项
        /// <summary>
        /// 获取用户选中的生成选项
        /// </summary>
        /// <returns></returns>
        List<string> GetChecked()
        {
            List<string> list = new List<string>();
            foreach (Control c in gbOpition.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox ck = c as CheckBox;
                    if (ck.Checked && ck.Text != "所有表")
                    {
                        list.Add(ck.Text);
                    }
                }
            }
            return list;
        } 
        #endregion

        void Go(bool isWrite)
        {
            if (lvTables.Items.Count <= 0)
            {
                MessageBox.Show("连不上数据库");
                return;
            }
            List<string> option = GetChecked();
            if (option.Count <= 0)
            {
                MessageBox.Show("请选择至少一项生成选项");
                return;
            }
            txtPreview.Text = "";
            
            SetGenerator(g);
            if (lvTables.CheckedItems.Count > 0)
            {
                List<string> tablesName = pt.GetTables(lvTables);
                //lsj--获取全部表名
                StringBuilder ret = new StringBuilder();
                ProcString ps = new ProcString();
                ret.AppendLine("/*");
                for (int i = 0; i < tablesName.Count; i++)
                {
                    ret.AppendLine(tablesName[i].ToString());
                }
                ret.AppendLine("*/");
                //txtPreview.Text = ret.ToString();
                if (ckModel.Checked)
                {
                    if (isWrite)
                    {
                        g.ToModels(isWrite, tablesName);
                    }
                    else
                    {
                        txtPreview.Text += "//实体类\r\n";
                        txtPreview.Text += g.ToModels(isWrite, tablesName);
                        
                    }
                }
                
                
               
                if (!string.IsNullOrEmpty(path) && ckDocument.Checked)
                {
                    g.ToWord(tablesName);
                }
            }
            else
            {
                lblMsg.Text = "请选择表!";
                MessageBox.Show(lblMsg.Text);
            }          
        }

        #region 预览效果
        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Go(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败:" + ex.Message);
                lblMsg.Text = "代码生成器:生成失败";
            }
        }
        #endregion

        #region 生成
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                path = "";
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                    Go(true);
                    MessageBox.Show("生成成功！");
                }
                else
                {
                    fbd.SelectedPath = "";
                    lblMsg.Text = "重新选择存储路径";
                    MessageBox.Show("重新选择存储路径");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败:" + ex.Message); 
                lblMsg.Text = "代码生成器:生成失败";
            }
        } 
        #endregion

        #region 表选择(全选/全不选)
        private void ckSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (ckSelect.Checked)
            {
                foreach (ListViewItem item in lvTables.Items)
                {
                    item.Checked = true;
                }
            }
            else
            {
                foreach (ListViewItem item in lvTables.Items)
                {
                    item.Checked = false;
                }
            }
        } 
        #endregion

        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
