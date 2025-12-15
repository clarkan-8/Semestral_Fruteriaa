using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Semestral_Fruteriaa
{
    public partial class _Default : Page
    {
        // “DB” temporal en memoria (después lo conectamos a SQL si quieres)
        private static List<Producto> ProductosBase = new List<Producto>
        {
            new Producto{ Id=1, Nombre="Mango Ataulfo", Categoria="Frutas", Precio=1.25m, Rating=4.8m, ImagenUrl="https://picsum.photos/seed/mango/600/400" },
            new Producto{ Id=2, Nombre="Piña Golden", Categoria="Frutas", Precio=2.10m, Rating=4.6m, ImagenUrl="https://picsum.photos/seed/pina/600/400" },
            new Producto{ Id=3, Nombre="Banano", Categoria="Frutas", Precio=0.45m, Rating=4.7m, ImagenUrl="https://picsum.photos/seed/banano/600/400" },
            new Producto{ Id=4, Nombre="Sandía", Categoria="Frutas", Precio=3.50m, Rating=4.5m, ImagenUrl="https://picsum.photos/seed/sandia/600/400" },
            new Producto{ Id=5, Nombre="Combo Familiar", Categoria="Combos", Precio=9.99m, Rating=4.9m, ImagenUrl="https://picsum.photos/seed/combo/600/400" },
            new Producto{ Id=6, Nombre="Manzana Roja", Categoria="Frutas", Precio=0.80m, Rating=4.4m, ImagenUrl="https://picsum.photos/seed/manzana/600/400" },
            new Producto{ Id=7, Nombre="Aguacate", Categoria="Frutas", Precio=1.60m, Rating=4.3m, ImagenUrl="https://picsum.photos/seed/aguacate/600/400" },
            new Producto{ Id=8, Nombre="Fresas", Categoria="Orgánico", Precio=2.75m, Rating=4.8m, ImagenUrl="https://picsum.photos/seed/fresas/600/400" },
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductos();
                lblInfo.Text = "";
            }
        }

        protected void ddlOrden_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductos();
        }

        private void BindProductos()
        {
            var lista = ProductosBase.ToList();

            switch (ddlOrden.SelectedValue)
            {
                case "asc":
                    lista = lista.OrderBy(x => x.Precio).ToList();
                    break;
                case "desc":
                    lista = lista.OrderByDescending(x => x.Precio).ToList();
                    break;
                default:
                    // recomendados: por rating
                    lista = lista.OrderByDescending(x => x.Rating).ToList();
                    break;
            }

            rptProductos.DataSource = lista;
            rptProductos.DataBind();
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Agregar")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                // Carrito básico: solo contador
                int carrito = (Session["CART_COUNT"] == null) ? 0 : (int)Session["CART_COUNT"];
                carrito++;
                Session["CART_COUNT"] = carrito;

                // Guardamos último producto agregado (opcional)
                var prod = ProductosBase.FirstOrDefault(p => p.Id == id);
                if (prod != null)
                {
                    lblInfo.Text = $"✅ Agregaste: {prod.Nombre} al carrito.";
                }
                else
                {
                    lblInfo.Text = "✅ Agregado al carrito.";
                }
            }
        }

        // Modelo simple
        public class Producto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public decimal Precio { get; set; }
            public decimal Rating { get; set; }
            public string ImagenUrl { get; set; }
        }
    }
}
