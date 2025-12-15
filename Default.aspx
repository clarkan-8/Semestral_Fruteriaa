<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Semestral_Fruteriaa._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- HERO / BANNER (tipo Amazon) -->
    <div class="fru-hero mt-3 mb-4">
        <div id="heroCarousel" class="carousel slide" data-bs-ride="carousel">
            <div class="carousel-inner rounded-4 overflow-hidden shadow-sm">
                <div class="carousel-item active">
                    <div class="p-5 fru-hero-slide">
                        <h1 class="display-6 fw-bold mb-2">Frutas frescas directo de la granja</h1>
                        <p class="lead mb-3">Entrega rápida • Calidad premium • Precios de temporada</p>
                        <a href="#productos" class="btn btn-warning fw-semibold">Ver ofertas</a>
                    </div>
                </div>

                <div class="carousel-item">
                    <div class="p-5 fru-hero-slide">
                        <h2 class="fw-bold mb-2">Combos familiares</h2>
                        <p class="mb-3">Ahorra con paquetes listos para la semana</p>
                        <a href="#productos" class="btn btn-warning fw-semibold">Explorar combos</a>
                    </div>
                </div>

                <div class="carousel-item">
                    <div class="p-5 fru-hero-slide">
                        <h2 class="fw-bold mb-2">Orgánico y de temporada</h2>
                        <p class="mb-3">Seleccionado a mano, recién cosechado</p>
                        <a href="#productos" class="btn btn-warning fw-semibold">Comprar ahora</a>
                    </div>
                </div>
            </div>

            <button class="carousel-control-prev" type="button" data-bs-target="#heroCarousel" data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Anterior</span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#heroCarousel" data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Siguiente</span>
            </button>
        </div>
    </div>

    <!-- CATEGORÍAS (tipo Amazon “departamentos”) -->
    <div class="row g-3 mb-4">
        <div class="col-6 col-md-3">
            <div class="card fru-cat shadow-sm h-100">
                <div class="card-body">
                    <div class="fw-bold">Frutas</div>
                    <div class="text-muted small">Mango, piña, banano…</div>
                </div>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card fru-cat shadow-sm h-100">
                <div class="card-body">
                    <div class="fw-bold">Combos</div>
                    <div class="text-muted small">Ahorra en paquetes</div>
                </div>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card fru-cat shadow-sm h-100">
                <div class="card-body">
                    <div class="fw-bold">Orgánico</div>
                    <div class="text-muted small">Seleccionado premium</div>
                </div>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card fru-cat shadow-sm h-100">
                <div class="card-body">
                    <div class="fw-bold">Ofertas</div>
                    <div class="text-muted small">Descuentos del día</div>
                </div>
            </div>
        </div>
    </div>

    <!-- LISTA DE PRODUCTOS (cards) -->
    <div class="d-flex align-items-center justify-content-between mb-2" id="productos">
        <h2 class="h4 mb-0 fw-bold">Productos destacados</h2>

        <!-- mini filtro (simple) -->
        <asp:DropDownList ID="ddlOrden" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true"
            OnSelectedIndexChanged="ddlOrden_SelectedIndexChanged" style="max-width: 220px;">
            <asp:ListItem Text="Ordenar: Recomendados" Value="reco" Selected="True" />
            <asp:ListItem Text="Precio: menor a mayor" Value="asc" />
            <asp:ListItem Text="Precio: mayor a menor" Value="desc" />
        </asp:DropDownList>
    </div>

    <asp:Label ID="lblInfo" runat="server" CssClass="text-success fw-semibold"></asp:Label>

    <div class="row g-3 mt-1">
        <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand">
            <ItemTemplate>
                <div class="col-6 col-md-4 col-lg-3">
                    <div class="card shadow-sm h-100 fru-card">
                        <img class="card-img-top fru-img" src="<%# Eval("ImagenUrl") %>" alt="<%# Eval("Nombre") %>" />
                        <div class="card-body d-flex flex-column">
                            <div class="d-flex justify-content-between align-items-start gap-2">
                                <div class="fw-bold"><%# Eval("Nombre") %></div>
                                <span class="badge bg-success-subtle text-success border">Granja</span>
                            </div>

                            <div class="text-muted small mb-2"><%# Eval("Categoria") %></div>

                            <div class="mt-auto">
                                <div class="d-flex align-items-center justify-content-between">
                                    <div class="h5 mb-0 fw-bold text-dark">
                                        $<%# string.Format("{0:0.00}", Eval("Precio")) %>
                                    </div>
                                    <div class="small text-warning">
                                        ★ <%# Eval("Rating") %>
                                    </div>
                                </div>

                                <asp:Button runat="server"
                                    Text="Agregar al carrito"
                                    CssClass="btn btn-warning w-100 mt-2 fw-semibold"
                                    CommandName="Agregar"
                                    CommandArgument='<%# Eval("Id") %>' />
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>
