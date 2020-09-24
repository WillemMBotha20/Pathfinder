using Broos_Gerrit_PRG282_prj.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Broos_Gerrit_PRG282_prj
{
    public partial class frmSim : Form
    {
        PictureBox selectedObs;
        bool selected = false;
        List<Obstacle> obstacles = new List<Obstacle>();
        List<FileInfo> files = new List<FileInfo>();
        List<PictureBox> obsPics = new List<PictureBox>();

        int height = 0;
        int x = 0, y = 3, goalX = 14, goalY = 3;
        List<Node> nodes = new List<Node>();
        List<Node> selectedNodes = new List<Node>();
        int timerCount;

        public frmSim()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvGame.DefaultCellStyle.NullValue = null;
            for (int i = 0; i < 7; i++)
            {
                dgvGame.Rows.Add();
            }

            GetPictureBoxes();
            LoadImages();

            Obstacle obs;
            DataGridViewImageCell imgCell = new DataGridViewImageCell();
            imgCell.Value = Image.FromFile(@"Assets\1Tank.PNG");
            imgCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dgvGame[x, y] = imgCell;

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("1"))
                {
                    height = 1000;
                }
                else if (file.Name.Contains("2"))
                {
                    height = 2000;
                }
                else
                {
                    height = 3000;
                }

                obs = new Obstacle(height, Image.FromFile(file.FullName), files.IndexOf(file));
                obstacles.Add(obs);
            }
        }

        void LoadImages()
        {
            DirectoryInfo assets = new DirectoryInfo(@"Assets");
            files = assets.GetFiles("*.png").ToList();
            int count = 0;

            foreach (PictureBox pic in obsPics)
            {
                pic.Image = Image.FromFile(files[count].FullName);
                count++;
            }
        }

        private void dgvGame_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (selected)
            {
                DataGridViewImageCell imgCell = new DataGridViewImageCell();
                imgCell.Value = selectedObs.Image;
                imgCell.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGame[e.ColumnIndex, e.RowIndex] = imgCell;
            }
        }

        void ObsSelect(PictureBox Obs)
        {
            if (selected)
            {
                foreach (PictureBox pic in obsPics)
                {
                    pic.BorderStyle = BorderStyle.None;
                    pbxEraser.BorderStyle = BorderStyle.None;
                }
            }

            selectedObs = Obs;
            Obs.BorderStyle = BorderStyle.Fixed3D;
            selected = true;
        }

        void GetPictureBoxes()
        {
            foreach (TabPage page in tabCObs.TabPages)
            {
                foreach (Control con in page.Controls)
                {
                    if (con is PictureBox)
                    {
                        obsPics.Add((PictureBox)con);
                    }
                }
            }
        }

        private void ObsLow_Click(object sender, EventArgs e)
        {
            ObsSelect(ObsLow1);
        }

        private void ObsMed_Click(object sender, EventArgs e)
        {
            ObsSelect(ObsLow2);
        }

        private void pbxEraser_Click(object sender, EventArgs e)
        {
            ObsSelect(pbxEraser);
        }

        private void tmrAnim_Tick(object sender, EventArgs e)
        {
            if (timerCount == selectedNodes.Count)
            {
                tmrAnim.Stop();
                return;
            }

            DataGridViewImageCell imgCell = new DataGridViewImageCell();
            imgCell.Value = Image.FromFile(@"Assets\index.PNG");
            imgCell.ImageLayout = DataGridViewImageCellLayout.Stretch;

            dgvGame[selectedNodes[timerCount].x, selectedNodes[timerCount].y] = imgCell;
            if (timerCount > 0)
            {
                dgvGame[selectedNodes[timerCount - 1].x, selectedNodes[timerCount - 1].y].Value = null;
            }
            timerCount++;
        }

        private void ObsHigh_Click(object sender, EventArgs e)
        {
            ObsSelect(ObsLow3);
        }

        private void btnSim_Click(object sender, EventArgs e)
        {
            ClearGrid();
            nodes.Clear();
            selectedNodes.Clear();
            x = 0;
            y = 3;
            Node nodeInst;
            int minF;
            int minH;

            while (x != goalX || y != goalY)
            {
                nodes.Clear();

                if (y - 1 >= 0)
                {
                    if (dgvGame[x, y - 1].Value == null)
                    {
                        nodeInst = new Node(0, Math.Abs(x - goalX) + Math.Abs((y - 1) - goalY), x, y - 1);
                        nodes.Add(nodeInst);
                    }
                }

                if (y - 1 >= 0 && x + 1 < dgvGame.Columns.Count)
                {
                    if (dgvGame[x + 1, y - 1].Value == null)
                    {
                        nodeInst = new Node(0, Math.Abs((x + 1) - goalX) + Math.Abs((y - 1) - goalY), x + 1, y - 1);
                        nodes.Add(nodeInst);
                    }
                }

                if (x + 1 < dgvGame.Columns.Count)
                {
                    if (dgvGame[x + 1, y].Value == null)
                    {
                        nodeInst = new Node(0, Math.Abs((x + 1) - goalX) + Math.Abs(y - goalY), x + 1, y);
                        nodes.Add(nodeInst);
                    }
                }

                if (y + 1 < dgvGame.Rows.Count && x + 1 < dgvGame.Columns.Count)
                {
                    if (dgvGame[x + 1, y + 1].Value == null)
                    {
                        nodeInst = new Node(0, Math.Abs((x + 1) - goalX) + Math.Abs((y + 1) - goalY), x + 1, y + 1);
                        nodes.Add(nodeInst);
                    }
                }

                if (y + 1 < dgvGame.Rows.Count)
                {
                    if (dgvGame[x, y + 1].Value == null)
                    {
                        nodeInst = new Node(0, Math.Abs(x - goalX) + Math.Abs((y + 1) - goalY), x, y + 1);
                        nodes.Add(nodeInst);
                    }
                }

                nodes.RemoveAll(x => selectedNodes.Any(y => y.x == x.x && y.y == x.y));

                try
                {
                    minF = nodes[0].f;
                    minH = nodes[0].h;

                    foreach (Node i in nodes)
                    {
                        if (i.f < minF)
                        {
                            minF = i.f;
                            minH = i.h;
                        }
                    }

                    foreach (Node i in nodes)
                    {
                        if (i.f == minF && i.h < minH)
                        {
                            minH = i.h;
                        }
                    }

                    foreach (Node i in nodes)
                    {
                        if (i.f == minF && i.h == minH)
                        {
                            selectedNodes.Add(i);
                            x = i.x;
                            y = i.y;
                            dgvGame[i.x, i.y].Style.BackColor = Color.Red;
                            break;
                        }
                    }                    
                }
                catch
                {
                    MessageBox.Show("Dead");
                    break;
                }
            }

            tmrAnim.Interval = 400;
            timerCount = 0;
            tmrAnim.Start();
        }

        void ClearGrid()
        {
            foreach (DataGridViewRow row in dgvGame.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;

                    if ((Image) cell.Value == Image.FromFile(@"Assets\index.PNG"))
                    {
                        cell.Value = null;
                    }
                }
            }
        }
    }

    class Node
    {
        public int g, h, f, x, y;

        public Node(int _g, int _h, int _x, int _y)
        {
            g = _g;
            h = _h;
            x = _x;
            y = _y;
            f = h + g;
        }

        public string ToString()
        {
            return String.Format("X: {0}, Y: {1} G: {2}, H: {3}, F: {4}", x, y, g, h, f);
        }
    }
}
