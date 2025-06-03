+ <%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Funcionarios.aspx.cs" Inherits="GestaoPessoalWebForms.Funcionarios" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Funcionários ‒ Gestão de Pessoal</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        .form‐box {
            margin‐bottom: 30px;
            border: 1px solid #ccc;
            padding: 15px;
            width: 400px;
            border‐radius: 5px;
        }
        .form‐box label {
            display: block;
            margin‐bottom: 5px;
            font-weight: bold;
        }
        .form‐box input,
        .form‐box select {
            width: 100%;
            padding: 6px;
            margin‐bottom: 15px;
            border: 1px solid #aaa;
            border‐radius: 3px;
        }
        .form‐box button {
            padding: 8px 16px;
            margin‐right: 10px;
            border: none;
            border‐radius: 3px;
            background‐color: #28a745;
            color: white;
            cursor: pointer;
        }
        .form‐box button:hover {
            background‐color: #218838;
        }
        .grid‐container {
            margin‐top: 40px;
        }
        .grid‐container table {
            width: 100%;
            border‐collapse: collapse;
        }
        .grid‐container th,
        .grid‐container td {
            border: 1px solid #ccc;
            padding: 8px;
            text‐align: left;
        }
        .grid‐container th {
            background‐color: #f2f2f2;
        }
        .grid‐actions button {
            padding: 4px 8px;
            margin‐right: 5px;
            border: none;
            border‐radius: 3px;
            cursor: pointer;
        }
        .btn-edit {
            background‐color: #007bff;
            color: white;
        }
        .btn-edit:hover {
            background‐color: #0069d9;
        }
        .btn-delete {
            background‐color: #dc3545;
            color: white;
        }
        .btn-delete:hover {
            background‐color: #c82333;
        }
        .error‐message {
            color: red;
            margin‐bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Funcionários</h2>

        <!-- Área para exibir mensagens de erro/get de API -->
        <asp:Label ID="lblErro" runat="server" CssClass="error‐message" Visible="false" Text=""></asp:Label>

        <!-- Formulário de cadastro/edição -->
        <div class="form‐box">
            <asp:HiddenField ID="hfFuncionarioId" runat="server" />

            <label for="txtNome">Nome:</label>
            <asp:TextBox ID="txtNome" runat="server" />

            <label for="txtCargo">Cargo:</label>
            <asp:TextBox ID="txtCargo" runat="server" />

            <label for="txtDataAdmissao">Data de Admissão:</label>
            <asp:TextBox ID="txtDataAdmissao" runat="server" TextMode="Date" />

            <label for="txtSalario">Salário:</label>
            <asp:TextBox ID="txtSalario" runat="server" />

            <label for="ddlStatus">Status:</label>
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Text="Ativo" Value="Ativo" />
                <asp:ListItem Text="Inativo" Value="Inativo" />
            </asp:DropDownList>

            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
        </div>

        <!-- GridView de listagem de funcionários -->
        <div class="grid‐container">
            <asp:GridView ID="gvFuncionarios"
                          runat="server"
                          AutoGenerateColumns="False"
                          DataKeyNames="Id"
                          OnRowEditing="gvFuncionarios_RowEditing"
                          OnRowDeleting="gvFuncionarios_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="Nome" HeaderText="Nome" />
                    <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                    <asp:BoundField DataField="DataAdmissao" HeaderText="Data de Admissão" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Salario" HeaderText="Salário" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />

                    <asp:TemplateField HeaderText="Ações">
                        <ItemTemplate>
                            <div class="grid‐actions">
                                <asp:Button runat="server"
                                            CommandName="Edit"
                                            Text="Editar"
                                            CssClass="btn‐edit" />

                                <asp:Button runat="server"
                                            CommandName="Delete"
                                            Text="Excluir"
                                            CssClass="btn‐delete"
                                            OnClientClick="return confirm('Deseja realmente excluir?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
