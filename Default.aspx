<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GestaoPessoalWebForms.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home ‒ Gestão de Pessoal</title>
    <style>
        /* Apenas um estilo simples para o menu */
        .nav {
            margin: 20px;
        }
        .nav a {
            margin-right: 15px;
            text-decoration: none;
            font-weight: bold;
            color: #333;
        }
        .nav a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="nav">
            <a href="Default.aspx">Home</a>
            <a href="Funcionarios.aspx">Funcionários</a>
            <a href="Ferias.aspx">Férias</a>
            <a href="Relatorio.aspx">Relatorio</a>
        </div>
        <h2>Bem‐vindo ao Sistema de Gestão de Pessoal</h2>
        <p>Use o menu acima para navegar até a página de <strong>Funcionários</strong>, onde você poderá visualizar, cadastrar, editar e excluir funcionários.</p>
    </form>
</body>
</html>
