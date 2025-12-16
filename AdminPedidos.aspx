<%@ Page Title="Admin - Pedidos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminPedidos.aspx.cs"
    Inherits="Semestral_Fruteriaa.AdminPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex align-items-center justify-content-between mb-3">
        <h2 class="fw-bold mb-0">📦 Admin: Pedidos</h2>
    </div>

    <asp:Label ID="lblMsg" runat="server"></asp:Label>

    <div class="table-responsive">
        <asp:GridView ID="gvPedidosAdmin" runat="server"
            CssClass="table table-hover align-middle"
            AutoGenerateColumns="False"
            DataKeyNames="Id"
            OnRowCommand="gvPedidosAdmin_RowCommand">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Pedido" />
                <asp:BoundField DataField="UsuarioId" HeaderText="UsuarioId" />
                <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="${0:0.00}" />

                <asp:TemplateField HeaderText="Estado actual">
                    <ItemTemplate>
                        <span class="badge bg-secondary"><%# Eval("Estado") %></span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Cambiar estado">
                    <ItemTemplate>
                        <div class="d-flex gap-2 align-items-center">
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select form-select-sm" style="max-width:180px;">
                                <asp:ListItem Text="Procesando" Value="Procesando" />
                                <asp:ListItem Text="Enviado" Value="Enviado" />
                                <asp:ListItem Text="Entregado" Value="Entregado" />
                                <asp:ListItem Text="Cancelado" Value="Cancelado" />
                            </asp:DropDownList>

                            <asp:LinkButton runat="server"
                                CssClass="btn btn-sm btn-warning fw-semibold"
                                CommandName="GuardarEstado"
                                CommandArgument='<%# Eval("Id") %>'>
                                Guardar
                            </asp:LinkButton>

                            <a class="btn btn-sm btn-outline-secondary"
                               href='PedidoDetalle.aspx?id=<%# Eval("Id") %>'>
                                Ver detalle
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
