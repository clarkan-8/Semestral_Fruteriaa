<%@ Page Title="Admin - Agregar producto" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminAgregarProducto.aspx.cs"
    Inherits="Semestral_Fruteriaa.AdminAgregarProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="fw-bold mb-3">🛠️ Admin: Agregar producto</h2>

    <asp:Label ID="lblMsg" runat="server"></asp:Label>

    <div class="row g-3">
        <div class="col-md-6">
            <label class="form-label">Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        </div>

        <div class="col-md-6">
            <label class="form-label">Categoría</label>
            <asp:TextBox ID="txtCategoria" runat="server" CssClass="form-control" placeholder="Frutas / Combos / Orgánico" />
        </div>

        <div class="col-md-4">
            <label class="form-label">Precio</label>
            <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" placeholder="1.25" />
        </div>

        <div class="col-md-4">
            <label class="form-label">Rating</label>
            <asp:TextBox ID="txtRating" runat="server" CssClass="form-control" placeholder="4.5" />
        </div>

        <div class="col-md-4 d-flex align-items-end">
            <div class="form-check">
                <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
                <label class="form-check-label">Activo</label>
            </div>
        </div>

        <div class="col-md-12">
            <label class="form-label">Imagen (jpg/png)</label>
            <asp:FileUpload ID="fuImagen" runat="server" CssClass="form-control" />
            <div class="text-muted small mt-1">Se guardará en ~/Images/Productos/</div>
        </div>

        <div class="col-md-12">
            <asp:Button ID="btnGuardar" runat="server"
                Text="Guardar producto"
                CssClass="btn btn-success fw-semibold"
                OnClick="btnGuardar_Click" />
        </div>
    </div>

</asp:Content>
