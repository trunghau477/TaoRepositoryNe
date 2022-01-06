using QLKTX.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX
{
    public partial class frmAddBuilding : Form
    {
        //int Sum = 0;
        private readonly KTXDBContext context;
        public frmAddBuilding()
        {
            context = new KTXDBContext();
            InitializeComponent();
        }
        private void BindGrid(List<TOANHA> listBuilding)
        {
            dgvBuilding.Rows.Clear();
            foreach (var item in listBuilding)
            {
                int index = dgvBuilding.Rows.Add();
                dgvBuilding.Rows[index].Cells[0].Value = item.MATOA;
                dgvBuilding.Rows[index].Cells[1].Value = item.TENTOA;
                //Sum++;
                //lblTong.Text = Sum.ToString();
            }
        }
        private void setNull()
        {
            txtBuildingID.Text = "";
            txtBuildingName.Text = "";
        }
        private int GetSelectedRow(string buildingID)
        {
            for (int i = 0; i < dgvBuilding.Rows.Count; i++)
            {
                if (dgvBuilding.Rows[i].Cells[0].Value.ToString() == buildingID)
                {
                    return i;
                }
            }
            return -1;
        }
        private bool CheckValidate()
        {
            if (txtBuildingID.Text == "" || txtBuildingName.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống dữ liệu", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            else return true;
        }
        private void reloadDGV()
        {
            List<TOANHA> listBuilding = context.TOANHA.ToList();
            BindGrid(listBuilding);
        }
        private void frmAddBuilding_Load(object sender, EventArgs e)
        {
            try
            {
                List<TOANHA> listBuilding = context.TOANHA.ToList();
                BindGrid(listBuilding);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvBuilding_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvBuilding.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvBuilding.CurrentRow.Selected = true;
                    txtBuildingID.Text = dgvBuilding.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtBuildingName.Text = dgvBuilding.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
            {
                if (GetSelectedRow(txtBuildingID.Text) == -1)
                {
                    TOANHA toa = new TOANHA()
                    {
                        MATOA = txtBuildingID.Text,
                        TENTOA = txtBuildingName.Text,
                    };
                    context.TOANHA.Add(toa);
                    context.SaveChanges();
                    reloadDGV();
                    setNull();
                    MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK);
                    //Sum++;
                    //lblTong.Text = Sum.ToString();
                }
                else
                {
                    var idtoa = txtBuildingID.Text;
                    TOANHA dbUpdate = context.TOANHA.FirstOrDefault(p => p.MATOA == idtoa);
                    if (dbUpdate != null)
                    {
                        dbUpdate.MATOA = txtBuildingID.Text;
                        dbUpdate.TENTOA = txtBuildingName.Text;
                        context.SaveChanges(); //Lưu thay đổi
                        reloadDGV();
                        setNull();
                        MessageBox.Show("Cập nhật dữ liệu thành công!”.", "Thông báo", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tòa cần sửa!", "Thông báo", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            TOANHA dbDelete = context.TOANHA.FirstOrDefault(p => p.MATOA.ToString() == txtBuildingID.Text);
            if (dbDelete != null)
            {
                DialogResult dr = MessageBox.Show("Bạn có muốn xoá?", "Yes/No", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    KTXDBContext ctext = new KTXDBContext();
                    List<PHONGSV> listRoom = ctext.PHONGSV.ToList();
                    PHONGSV DeleteRoom = ctext.PHONGSV.FirstOrDefault(p => p.TOANHA.TENTOA == txtBuildingName.Text);
                    foreach (var item in listRoom)
                    {
                        if (DeleteRoom != null)
                        {
                            if (item.TOANHA.TENTOA == txtBuildingName.Text)
                                ctext.PHONGSV.Remove(item);
                        }
                    }
                    ctext.SaveChanges();
                    context.TOANHA.Remove(dbDelete);
                    context.SaveChanges(); //Lưu thay dổi
                    reloadDGV();
                    setNull();
                    MessageBox.Show("Xóa tòa thành công!", "Thông báo", MessageBoxButtons.OK);
                    //Sum--;
                    //lblTong.Text = Sum.ToString();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy tòa cần xóa!", "Thông báo", MessageBoxButtons.OK);
            }
        }
        //private void Sum()
        //{
        //    int Tong = 0;
        //    for (int i = 0; i < dgvBuilding.Rows.Count; i++)
        //    {
        //        if (dgvBuilding.Rows[i].Cells[1].Value != null)
        //        {
        //            Tong++;
        //        }
        //    }
        //    lblTong.Text = Tong.ToString();
        //}
    }
}
