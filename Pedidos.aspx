<%@ Page Title="Mis pedidos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs"
    Inherits="Semestral_Fruteriaa.Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex align-items-center justify-content-between mb-3">
        <h2 class="fw-bold mb-0">📦 Mis pedidos</h2>
        <a runat="server" href="~/Default.aspx" class="btn btn-outline-secondary">Seguir comprando</a>
    </div>

    <asp:Panel ID="pnlLogin" runat="server" Visible="false" CssClass="alert alert-warning">
        Debes iniciar sesión para ver tus pedidos. <a runat="server" href="~/Login.aspx">Ir a login</a>
    </asp:Panel>

    <asp:Panel ID="pnlVacio" runat="server" Visible="false" CssClass="alert alert-info">
        Aún no tienes pedidos realizados.
    </asp:Panel>

    <asp:GridView ID="gvPedidos" runat="server"
        CssClass="table table-hover align-middle"
        AutoGenerateColumns="False"
        DataKeyNames="Id">
        <Columns>
            <asp:BoundField HeaderText="N° Pedido" DataField="Id" />
            <asp:BoundField HeaderText="Fecha" DataField="FechaCreacion" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:BoundField HeaderText="Total" DataField="Total" DataFormatString="${0:0.00}" />

            <asp:TemplateField HeaderText="Estado">
                <ItemTemplate>
                    <span class="badge bg-secondary"><%# Eval("Estado") %></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <a class="btn btn-sm btn-warning fw-semibold"
                       href='PedidoDetalle.aspx?id=<%# Eval("Id") %>'>
                        Ver seguimiento
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
