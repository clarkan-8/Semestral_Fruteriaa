<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="Semestral_Fruteriaa.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row justify-content-center mt-4">
        <div class="col-md-6">
            <div class="card shadow-sm" style="border-radius:18px;">
                <div class="card-body p-4">

                    <h2 class="fw-bold mb-3">Iniciar sesión</h2>

                    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger"></asp:Label>

                    <div class="mb-3">
                        <label class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Contraseña</label>
                        <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
                    </div>

                    <asp:Button ID="btnLogin" runat="server"
                        CssClass="btn btn-success w-100 fw-semibold"
                        Text="Entrar"
                        OnClick="btnLogin_Click" />

                    <div class="mt-3 text-center">
                        <a runat="server" href="~/Register.aspx">Crear cuenta</a>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
