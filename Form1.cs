using DevExpress.DashboardCommon;
using DevExpress.DataAccess.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuFromDB
{
    public partial class Form1 : Form
    {
        public string groupname { get;  set; }
        public string groupDispalyName { get;  set; }
        public string groupsequence { get; set; }

       

        public string itemname { get; set; }
        public string itemDispalyName { get;  set; }
        public string itemsequence { get; set; }
        private bool linklabelsVisible = true;
        SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=WingsInfonet;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Load_Data();    

        }

        public void Load_Data()
        {
            try
            {
                //SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=WingsInfonet;Integrated Security=True");

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT GroupSequence,GroupName,GroupDisplayName,ItemSequence,ItemName,ItemDisplayName From DynamicMenu order by GroupName", con);
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                int j;
                while (dr.Read())

                {
                    groupname = dr["GroupName"].ToString();
                    groupDispalyName = dr["GroupDisplayName"].ToString() ;
                    groupsequence = dr["GroupSequence"].ToString();
                    itemsequence = dr["ItemSequence"].ToString();
                    itemname = dr["ItemName"].ToString();
                    itemDispalyName = dr["ItemDisplayName"].ToString();
                  
                    int GSequence = Convert.ToInt32(groupsequence);
                    j = GSequence;
                    if(GSequence > 0)
                    {
                        if(GSequence==i)
                        {
                            System.Windows.Forms.Label lGroup = new System.Windows.Forms.Label()
                            {
                                Text = groupname,
                                Tag = groupDispalyName,
                                AutoSize = false,
                                Size = new System.Drawing.Size(250, 28),
                                Padding = new System.Windows.Forms.Padding(0, 0, 0, 0),
                                Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))),


                            };
                            lGroup.Click += LGroup_Click;
                            lGroup.MouseDoubleClick += LGroup_MouseDoubleClick;
                            flowLayoutPanel1.Controls.Add(lGroup);
                            i++;
                        }
                      
                    }
                    LinkLabel linkLabel = new LinkLabel()
                    {
                        Text = groupname == itemname ? groupDispalyName : itemDispalyName,
                            Tag= itemname,              
                            AutoSize=true,
                            Padding=new Padding(10,5,5,5),
                            Font=new Font("Segoe UI",12F,FontStyle.Regular,GraphicsUnit.Point,(byte)(0))
                            
                    };
                    linkLabel.Click += LinkLabel_Click;
                    flowLayoutPanel1.Controls.Add(linkLabel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }
        

        private void LinkLabel_Click(object sender, EventArgs e)
        {
            LinkLabel linkLabel = (LinkLabel)sender;
            string itemName = linkLabel.Tag.ToString();
            string FileName = itemName + ".xml";
            string FolderPath = "D:\\DevExpress";

            using (OpenFileDialog openfiledialog = new OpenFileDialog())
            {
                try
                {
                    string[] XmlFiles = Directory.GetFiles(FolderPath, FileName);
                    if (XmlFiles.Length > 0)
                    {
                        string selectedFilePath = XmlFiles[0];

                        dashboardViewer1.LoadDashboard(selectedFilePath);
                        dashboardViewer1.Dashboard.Title.Text = itemName;
                        dashboardViewer1.Dock = DockStyle.Fill;
                        panelControl2.Controls.Add(dashboardViewer1);
                        //dashboardViewer1.ResetText();
                    }
                    else
                    {
                        MessageBox.Show("file not found", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //  MessageBox.Show(FileName);

            //try
            //{
            //    // Retrieve the XML file path from the Tag property of the clicked link label
            //    string xmlFilePath = (sender as LinkLabel).Tag.ToString();

            //    Dashboard dashboard = new Dashboard();
            //    if (File.Exists(xmlFilePath))
            //    {
            //        // Create a new dashboard
            //         dashboard = new Dashboard();

            //        // Load the XML file into the dashboard
            //        dashboard.LoadFromXml(xmlFilePath);

            //        // Bind the dashboard to the viewer
            //        dashboardViewer1.Dashboard = dashboard;

            //        // Update the viewer to display the dashboard
            //        dashboardViewer1.UpdateDashboardTitle();
            //    }
            //    else
            //    {
            //        dashboard.LoadFromXml("D:\\DevExpress\\CashFlow");

            //        // Bind the dashboard to the viewer
            //        dashboardViewer1.Dashboard = dashboard;

            //        // Update the viewer to display the dashboard
            //        dashboardViewer1.UpdateDashboardTitle();
            //    }
            //}
            //catch (UnauthorizedAccessException ex)
            //{
            //    MessageBox.Show($"Access denied: {ex.Message}");
            //}  catch (Exception ex)
            //{
            //    MessageBox.Show($"Error loading dashboard: {ex.Message}");
            //}
        }



        private void LGroup_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LGroup_Click(object sender, EventArgs e)
        {
            linklabelsVisible =! linklabelsVisible;
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if(control is LinkLabel linkLabel)
                {
                    linkLabel.Visible = linklabelsVisible;
                }
            }
           // throw new NotImplementedException();
        }

        private void dashboardViewer1_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
