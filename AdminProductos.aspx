<%@ Page Title="Admin - Productos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminProductos.aspx.cs"
    Inherits="Semestral_Fruteriaa.AdminProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex align-items-center justify-content-between mb-3">
        <h2 class="fw-bold mb-0">🛠️ Admin: Productos</h2>
        <a runat="server" href="~/AdminAgregarProducto.aspx" class="btn btn-success fw-semibold">
            + Agregar producto
        </a>
    </div>

    <asp:Label ID="lblMsg" runat="server"></asp:Label>

    <div class="table-responsive">
        <asp:GridView ID="gvProductos" runat="server"
            CssClass="table table-hover align-middle"
            AutoGenerateColumns="False"
            DataKeyNames="Id"
            OnRowEditing="gvProductos_RowEditing"
            OnRowCancelingEdit="gvProductos_RowCancelingEdit"
            OnRowUpdating="gvProductos_RowUpdating"
            OnRowCommand="gvProductos_RowCommand">

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="true" />

                <asp:TemplateField HeaderText="Imagen">
                    <ItemTemplate>
                        <img src='<%# ResolveUrl(Eval("ImagenUrl").ToString()) %>'
                             style="width:70px;height:50px;object-fit:cover;border-radius:12px;" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="Rating" HeaderText="Rating" DataFormatString="{0:0.0}" />

                <asp:TemplateField HeaderText="Activo">
                    <ItemTemplate>
                        <span class='badge <%# (Convert.ToBoolean(Eval("Activo")) ? "bg-success" : "bg-secondary") %>'>
                            <%# (Convert.ToBoolean(Eval("Activo")) ? "Sí" : "No") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="True" EditText="Editar" UpdateText="Guardar" CancelText="Cancelar" />

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-primary"
                            CommandName="ToggleActivo" CommandArgument='<%# Eval("Id") %>'>
                            <%# (Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar") %>
                        </asp:LinkButton>

                        <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-danger ms-2"
                            CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                            OnClientClick="return confirm('¿Seguro que quieres eliminar este producto?');">
                            Eliminar
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
