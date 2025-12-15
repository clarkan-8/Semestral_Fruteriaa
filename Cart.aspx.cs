using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Semestral_Fruteriaa
{
    public partial class WebForm1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarCarrito();
        }

        private void CargarCarrito()
        {
            var carrito = ObtenerCarrito();

            pnlVacio.Visible = (carrito.Count == 0);
            pnlCarrito.Visible = (carrito.Count > 0);

            gvCarrito.DataSource = carrito;
            gvCarrito.DataBind();

            lblTotal.Text = carrito.Sum(x => x.Subtotal).ToString("0.00");

            // actualizar contador del header
            Session["CART_COUNT"] = carrito.Sum(x => x.Cantidad);
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            if (Session["CART"] == null)
                Session["CART"] = new List<CarritoItem>();

            return (List<CarritoItem>)Session["CART"];
        }

        protected void gvCarrito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                var carrito = ObtenerCarrito();
                var item = carrito.FirstOrDefault(x => x.Id == id);
                if (item != null)
                    carrito.Remove(item);

                Session["CART"] = carrito;
                CargarCarrito();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // Por ahora: simulación checkout (luego hacemos dirección/pago)
            Session["CART"] = new List<CarritoItem>();
            Session["CART_COUNT"] = 0;

            Response.Redirect("~/Default.aspx");
        }

        [Serializable]
        public class CarritoItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string ImagenUrl { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }

            public decimal Subtotal => Precio * Cantidad;
        }
    }
}
