<%@ Page Title="Detalle de pedido" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PedidoDetalle.aspx.cs"
    Inherits="Semestral_Fruteriaa.PedidoDetalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <a runat="server" href="~/Pedidos.aspx" class="btn btn-outline-secondary mb-3">← Volver</a>

    <asp:Panel ID="pnlLogin" runat="server" Visible="false" CssClass="alert alert-warning">
        Debes iniciar sesión para ver este pedido.
    </asp:Panel>

    <asp:Panel ID="pnlContenido" runat="server" Visible="true">
        <h2 class="fw-bold mb-1">Pedido #<asp:Label ID="lblPedidoId" runat="server" /></h2>
        <div class="text-muted mb-3">
            Estado actual: <span class="badge bg-secondary"><asp:Label ID="lblEstado" runat="server" /></span>
            • Total: <strong>$<asp:Label ID="lblTotal" runat="server" /></strong>
        </div>

        <h4 class="fw-bold mt-4">🧾 Productos</h4>
        <asp:GridView ID="gvItems" runat="server" CssClass="table table-striped align-middle"
            AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField HeaderText="Producto" DataField="Nombre" />
                <asp:BoundField HeaderText="Precio" DataField="Precio" DataFormatString="${0:0.00}" />
                <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" />
                <asp:BoundField HeaderText="Subtotal" DataField="Subtotal" DataFormatString="${0:0.00}" />
            </Columns>
        </asp:GridView>

        <h4 class="fw-bold mt-4">📍 Seguimiento</h4>
        <asp:Repeater ID="rptTracking" runat="server">
            <ItemTemplate>
                <div class="card shadow-sm mb-2" style="border-radius:14px;">
                    <div class="card-body py-2">
                        <div class="d-flex justify-content-between">
                            <span class="fw-semibold"><%# Eval("Estado") %></span>
                            <span class="text-muted small"><%# Eval("Fecha", "{0:yyyy-MM-dd HH:mm}") %></span>
                        </div>
                        <div class="text-muted small"><%# Eval("Comentario") %></div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>

</asp:Content>
