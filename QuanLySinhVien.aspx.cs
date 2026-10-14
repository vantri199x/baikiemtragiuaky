using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace baikiemtragiuaky
{
    public partial class QuanLyNhanVien : System.Web.UI.Page
    {
        private readonly KetNoi ketNoi = new KetNoi();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                NapLop();
                NapDanhSach();
            }
        }

        private void NapLop()
        {
            DataTable data = ketNoi.LayDuLieu("SELECT DISTINCT MaLop FROM SinhVien WHERE MaLop IS NOT NULL AND MaLop <> '' ORDER BY MaLop");
            ddlFilterLop.DataSource = data;
            ddlFilterLop.DataTextField = "MaLop";
            ddlFilterLop.DataValueField = "MaLop";
            ddlFilterLop.DataBind();
            ddlFilterLop.Items.Insert(0, new ListItem("-- Tất cả lớp --", ""));
        }

        private void NapDanhSach()
        {
            string keyword = txtSearch.Text.Trim();
            string maLop = ddlFilterLop.SelectedValue;
            const string query = "SELECT MaSV, HoTenSV, NgaySinh, SDT, MaLop FROM SinhVien WHERE (@Keyword = '' OR MaSV LIKE '%' + @Keyword + '%' OR HoTenSV LIKE '%' + @Keyword + '%') AND (@MaLop = '' OR MaLop = @MaLop) ORDER BY MaSV";
            gvSinhVien.DataSource = ketNoi.LayDuLieu(query, new[]
            {
                new SqlParameter("@Keyword", keyword),
                new SqlParameter("@MaLop", maLop)
            });
            gvSinhVien.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            NapDanhSach();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime ngaySinh;
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                HienThiThongBao("Vui lòng nhập mã và họ tên sinh viên.", false);
                return;
            }
            if (!DateTime.TryParse(txtNgaySinh.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh) &&
                !DateTime.TryParse(txtNgaySinh.Text, new CultureInfo("vi-VN"), DateTimeStyles.None, out ngaySinh))
            {
                HienThiThongBao("Ngày sinh không hợp lệ.", false);
                return;
            }

            const string query = "IF EXISTS (SELECT 1 FROM SinhVien WHERE MaSV = @MaSV) UPDATE SinhVien SET HoTenSV=@HoTenSV, NgaySinh=@NgaySinh, SDT=@SDT, MaLop=@MaLop WHERE MaSV=@MaSV ELSE INSERT INTO SinhVien(MaSV, HoTenSV, NgaySinh, SDT, MaLop) VALUES(@MaSV, @HoTenSV, @NgaySinh, @SDT, @MaLop)";
            ketNoi.ThucThi(query, ThamSoSinhVien());
            HienThiThongBao("Đã lưu thông tin sinh viên.", true);
            NapLop();
            NapDanhSach();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                HienThiThongBao("Chọn sinh viên cần xóa trước.", false);
                return;
            }
            ketNoi.ThucThi("DELETE FROM SinhVien WHERE MaSV=@MaSV", new[] { new SqlParameter("@MaSV", txtMaSV.Text.Trim()) });
            XoaTrang();
            HienThiThongBao("Đã xóa sinh viên.", true);
            NapLop();
            NapDanhSach();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            XoaTrang();
            lblMessage.Text = string.Empty;
            HienThiForm();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            XoaTrang();
            lblMessage.Text = string.Empty;
            HienThiForm();
        }

        protected void gvSinhVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Chon" && e.CommandName != "Xoa") return;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            string maSv = gvSinhVien.DataKeys[rowIndex].Value.ToString();
            if (e.CommandName == "Xoa")
            {
                ketNoi.ThucThi("DELETE FROM SinhVien WHERE MaSV=@MaSV", new[] { new SqlParameter("@MaSV", maSv) });
                NapDanhSach();
                return;
            }
            DataTable data = ketNoi.LayDuLieu("SELECT MaSV, HoTenSV, NgaySinh, SDT, MaLop FROM SinhVien WHERE MaSV=@MaSV", new[] { new SqlParameter("@MaSV", maSv) });
            if (data.Rows.Count == 0) return;
            DataRow row = data.Rows[0];
            txtMaSV.Text = row["MaSV"].ToString();
            txtHoTen.Text = row["HoTenSV"].ToString();
            txtNgaySinh.Text = row["NgaySinh"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["NgaySinh"]).ToString("yyyy-MM-dd");
            txtSDT.Text = row["SDT"] == DBNull.Value ? string.Empty : row["SDT"].ToString();
            txtMaLop.Text = row["MaLop"].ToString();
            lblMessage.Text = "Đang chỉnh sửa: " + maSv;
            HienThiForm();
        }

        private void HienThiForm()
        {
            ClientScript.RegisterStartupScript(GetType(), "showStudentModal", "showStudentModal();", true);
        }

        private List<SqlParameter> ThamSoSinhVien()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@MaSV", txtMaSV.Text.Trim()),
                new SqlParameter("@HoTenSV", txtHoTen.Text.Trim()),
                new SqlParameter("@NgaySinh", DateTime.Parse(txtNgaySinh.Text, CultureInfo.InvariantCulture)),
                new SqlParameter("@SDT", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text.Trim()),
                new SqlParameter("@MaLop", txtMaLop.Text.Trim())
            };
        }

        private void XoaTrang()
        {
            txtMaSV.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            txtNgaySinh.Text = string.Empty;
            txtSDT.Text = string.Empty;
            txtMaLop.Text = string.Empty;
        }

        private void HienThiThongBao(string message, bool success)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(success ? "#168443" : "#d94a4a");
        }
    }
}