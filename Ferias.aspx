<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="FeriasPage.aspx.cs" Inherits="GestaoPessoalWebForms.FeriasPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de Férias</title>
    <style>
        .form-table {
            width: 100%;
            max-width: 500px;
            border-collapse: collapse;
        }
        .form-table td {
            padding: 6px;
            vertical-align: middle;
        }
        .form-table label {
            font-weight: bold;
        }
        .form-table input,
        .form-table select {
            width: 100%;
            box-sizing: border-box;
            padding: 4px;
        }
        .button-row {
            margin-top: 10px;
        }
        .button-row input {
            margin-right: 8px;
            padding: 6px 12px;
        }
        .error-label {
            color: red;
            margin-bottom: 8px;
            display: block;
        }
    </style>
</head>
<body>
    <form id="formFerias" runat="server">
        <h2>Férias</h2>

        <!-- Mensagem de erro -->
        <asp:Label ID="lblErroFerias" CssClass="error-label" runat="server" Visible="false" />

        <!-- Campo oculto para o ID -->
        <asp:HiddenField ID="hfFeriasId" runat="server" Value="0" />

        <!-- Layout em tabela semelhante ao formulário de Funcionários -->
        <table class="form-table">
            <tr>
                <td>
                    <asp:Label ID="lblFuncionario" runat="server" AssociatedControlID="ddlFuncionario" Text="Funcionário:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlFuncionario" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDataInicio" runat="server" AssociatedControlID="txtDataInicio" Text="Data de Início:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDataInicio" runat="server" TextMode="Date" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDataTermino" runat="server" AssociatedControlID="txtDataTermino" Text="Data de Término:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDataTermino" runat="server" TextMode="Date" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatusFerias" runat="server" AssociatedControlID="ddlStatusFerias" Text="Status:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatusFerias" runat="server">
                        <asp:ListItem Text="Pendente" Value="Pendente" />
                        <asp:ListItem Text="Aprovada" Value="Aprovada" />
                        <asp:ListItem Text="Recusada" Value="Recusada" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <!-- Botões Salvar e Cancelar, alinhados como no formulário de Funcionários -->
        <div class="button-row">
            <asp:Button ID="btnSalvarFerias" runat="server" Text="Salvar" OnClick="btnSalvarFerias_Click" />
            <asp:Button ID="btnCancelarFerias" runat="server" Text="Cancelar" OnClick="btnCancelarFerias_Click" />
        </div>

        <br />

        <!-- Grid de Férias (igual ao layout dos funcionários, mas com colunas específicas) -->
        <asp:GridView ID="gvFerias" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
            OnRowEditing="gvFerias_RowEditing" OnRowDeleting="gvFerias_RowDeleting" Width="100%">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" ItemStyle-Width="50px" />
                <asp:BoundField DataField="NomeFuncionario" HeaderText="Funcionário" ItemStyle-Width="200px" />
                <asp:BoundField DataField="DataInicio" HeaderText="Data Início" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="120px" />
                <asp:BoundField DataField="DataTermino" HeaderText="Data Término" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="120px" />
                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="100px" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="100px" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
