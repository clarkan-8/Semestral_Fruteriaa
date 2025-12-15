<%@ Page Title="Carrito" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Cart.aspx.cs"
    Inherits="Semestral_Fruteriaa.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex align-items-center justify-content-between mb-3">
        <h2 class="fw-bold mb-0">🛒 Tu carrito</h2>
        <asp:HyperLink ID="hlSeguir" runat="server" NavigateUrl="~/Default.aspx"
            CssClass="btn btn-outline-secondary">
            Seguir comprando
        </asp:HyperLink>
    </div>

    <asp:Panel ID="pnlVacio" runat="server" Visible="false" CssClass="alert alert-warning">
        Tu carrito está vacío. Agrega frutas desde la página principal.
    </asp:Panel>

    <asp:Panel ID="pnlCarrito" runat="server">
        <div class="table-responsive">
            <asp:GridView ID="gvCarrito" runat="server"
                CssClass="table table-hover align-middle"
                AutoGenerateColumns="False"
                DataKeyNames="Id"
                OnRowCommand="gvCarrito_RowCommand">
                <Columns>

                    <asp:TemplateField HeaderText="Producto">
                        <ItemTemplate>
                            <div class="d-flex align-items-center gap-3">
                                <img src="<%%>" alt="img"
                                     style="width:72px;height:52px;object-fit:cover;border-radius:12px;" />
                                <div>
                                    <div class="fw-semibold"><%# Eval("Nombre")%></div>
                                    <div class="text-muted small"><%# Eval("Categoria") %></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="Precio" DataField="Precio" DataFormatString="${0:0.00}" />
                    <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" />
                    <asp:BoundField HeaderText="Subtotal" DataField="Subtotal" DataFormatString="${0:0.00}" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-danger"
                                CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'>
                                Eliminar
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

        <div class="d-flex justify-content-end mt-3">
            <div class="card shadow-sm" style="min-width:320px;border-radius:18px;">
                <div class="card-body">
                    <div class="d-flex justify-content-between">
                        <span class="text-muted">Total</span>
                        <span class="fw-bold">
                            $<asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label>
                        </span>
                    </div>

                    <asp:Button ID="btnCheckout" runat="server"
                        Text="Finalizar compra"
                        CssClass="btn btn-success w-100 mt-3 fw-semibold"
                        OnClick="btnCheckout_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>

</asp:Content>
