<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Relatorio.aspx.cs" Inherits="GestaoPessoalWebForms.Relatorio" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relatório de Funcionários</title>
    <style>
        .error-label {
            color: red;
            margin-bottom: 8px;
            display: block;
        }
        .button-row {
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <form id="formRelatorio" runat="server">
        <h2>Relatório de Funcionários</h2>

        <!-- Mensagem de erro -->
        <asp:Label ID="lblErroRelatorio" CssClass="error-label" runat="server" Visible="false" />

        <!-- Botão para gerar relatório -->
        <div class="button-row">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click" />
        </div>

        <!-- Grid para exibir o relatório de funcionários -->
        <asp:GridView ID="gvRelatorio" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" Width="100%">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" ItemStyle-Width="50px" />
                <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-Width="200px" />
                <asp:BoundField DataField="Cargo" HeaderText="Cargo" ItemStyle-Width="150px" />
                <asp:BoundField DataField="DataAdmissao" HeaderText="Data de Admissão" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="120px" />
                <asp:BoundField DataField="Salario" HeaderText="Salário" DataFormatString="{0:C}" ItemStyle-Width="100px" />
                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="100px" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
