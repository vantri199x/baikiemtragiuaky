<%@ Page Title="Quản lý sinh viên" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanLySinhVien.aspx.cs" Inherits="baikiemtragiuaky.QuanLyNhanVien" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="container-fluid px-0">
        <div class="card shadow-sm mb-3">
            <div class="card-body">
                <div class="row g-2 align-items-center">
                    <div class="col-12 col-lg-5"><div class="input-group"><span class="input-group-text">⌕</span><asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Tìm theo mã, tên SV..." /></div></div>
                    <div class="col-12 col-sm-5 col-lg-3"><asp:DropDownList ID="ddlFilterLop" runat="server" CssClass="form-select" /></div>
                    <div class="col-6 col-sm-3 col-lg-2"><asp:Button ID="btnSearch" runat="server" Text="Lọc" CssClass="btn btn-success w-100" OnClick="btnSearch_Click" /></div>
                    <div class="col-6 col-lg-2"><asp:Button ID="btnNew" runat="server" Text="＋ Thêm sinh viên" CssClass="btn btn-success w-100" OnClick="btnNew_Click" CausesValidation="false" /></div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="studentModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered"><div class="modal-content">
        <asp:Panel ID="pnlStudentForm" runat="server" DefaultButton="btnSave">
            <div class="modal-header bg-success text-white"><h5 class="modal-title">Thông tin sinh viên</h5><button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Đóng"></button></div>
            <div class="modal-body"><asp:Label ID="lblMessage" runat="server" CssClass="small d-block mb-2" /><div class="row g-3">
                <div class="col-md-6 col-lg"><asp:Label ID="lblMaSV" runat="server" AssociatedControlID="txtMaSV" Text="Mã sinh viên" CssClass="form-label" /><asp:TextBox ID="txtMaSV" runat="server" CssClass="form-control" /></div>
                <div class="col-md-6 col-lg"><asp:Label ID="lblHoTen" runat="server" AssociatedControlID="txtHoTen" Text="Họ và tên" CssClass="form-label" /><asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" /></div>
                <div class="col-md-6 col-lg"><asp:Label ID="lblNgaySinh" runat="server" AssociatedControlID="txtNgaySinh" Text="Ngày sinh" CssClass="form-label" /><asp:TextBox ID="txtNgaySinh" runat="server" CssClass="form-control" TextMode="Date" /></div>
                <div class="col-md-6 col-lg"><asp:Label ID="lblSDT" runat="server" AssociatedControlID="txtSDT" Text="Số điện thoại" CssClass="form-label" /><asp:TextBox ID="txtSDT" runat="server" CssClass="form-control" /></div>
                <div class="col-md-6 col-lg"><asp:Label ID="lblMaLop" runat="server" AssociatedControlID="txtMaLop" Text="Mã lớp" CssClass="form-label" /><asp:TextBox ID="txtMaLop" runat="server" CssClass="form-control" /></div>
                <div class="col-12 d-flex gap-2"><asp:Button ID="btnSave" runat="server" Text="Lưu thông tin" CssClass="btn btn-success" OnClick="btnSave_Click" /><asp:Button ID="btnDelete" runat="server" Text="Xóa" CssClass="btn btn-danger" OnClick="btnDelete_Click" CausesValidation="false" /><asp:Button ID="btnClear" runat="server" Text="Làm mới" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" /></div>
            </div></div>
        </asp:Panel>
            </div></div>
        </div>

        <div class="card shadow-sm border-0">
            <div class="table-responsive">
                <asp:GridView ID="gvSinhVien" runat="server" AutoGenerateColumns="false" DataKeyNames="MaSV" AllowPaging="false" EmptyDataText="Chưa có dữ liệu sinh viên." CssClass="table table-hover align-middle mb-0" HeaderStyle-CssClass="table-light text-uppercase small" GridLines="None" OnRowCommand="gvSinhVien_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="MÃ SV"><ItemTemplate><asp:LinkButton ID="btnSelect" runat="server" CommandName="Chon" CommandArgument='<%# Container.DataItemIndex %>' Text='<%# Eval("MaSV") %>' CssClass="link-success fw-semibold text-decoration-none" /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="ẢNH"><ItemTemplate><span class="rounded-circle bg-success text-white d-inline-flex align-items-center justify-content-center p-2">SV</span></ItemTemplate></asp:TemplateField>
                        <asp:BoundField DataField="HoTenSV" HeaderText="HỌ VÀ TÊN" />
                        <asp:BoundField DataField="NgaySinh" HeaderText="NGÀY SINH" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="SDT" HeaderText="SỐ ĐIỆN THOẠI" />
                        <asp:TemplateField HeaderText="MÃ LỚP"><ItemTemplate><span class="badge text-bg-danger"><%# Eval("MaLop") %></span></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="THAO TÁC"><ItemTemplate><asp:LinkButton ID="btnEdit" runat="server" CommandName="Chon" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-sm btn-outline-warning me-1">✎ Sửa</asp:LinkButton><asp:LinkButton ID="btnRemove" runat="server" CommandName="Xoa" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-sm btn-outline-danger" CausesValidation="false" OnClientClick="return confirm('Bạn có chắc muốn xóa sinh viên này không?');">✕ Xóa</asp:LinkButton></ItemTemplate></asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>