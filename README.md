Front-End Web Forms (ASP.NET)
Descrição
Aplicação Web Forms serve de interface clássica (server-side) para:

Cadastrar, editar e excluir Funcionários

Cadastrar, editar e excluir Férias

Visualizar Relatório de Funcionários (em uma GridView)

O Web Forms consome a API via HttpClient no code-behind (C#), chamando os endpoints descritos na seção anterior.

2.2. Pré-requisitos
Visual Studio 2019/2022 (com workload de desenvolvimento ASP.NET)

[.NET 7 Runtime] (o projeto Web Forms pode estar direcionado para .NET Framework ou .NET Core; verifique o TargetFramework no .csproj)

A API rodando (localhost:5117)

Configuração inicial
Clone o repositório Web Forms:

bash
Copiar
Editar
git clone https://github.com/ManoelOscar35/Teste-Zdoc-WebForms-WebAplication.git
cd Teste-Zdoc-WebForms-WebAplication
Abra no Visual Studio:

File → Open → Project/Solution

Selecione o arquivo .sln ou .csproj

Confirme o endpoint da API nos code-behind:

Em FeriasPage.aspx.cs, FuncionariosPage.aspx.cs e Relatorio.aspx.cs:

csharp
Copiar
Editar
private const string ApiBaseFuncionarios = "http://localhost:5117/api/Funcionarios";
private const string ApiBaseFerias = "http://localhost:5117/api/Ferias";
Caso a API esteja em outra porta, ajuste as URLs.

Executando o Web Forms
No Visual Studio, selecione Zdoc-WebForms como projeto de inicialização (Startup).

Pressione F5 ou clique em IIS Express ▶️ para rodar.

A aplicação abrirá em http://localhost:{porta}/Ferias.aspx (ou Funcionarios.aspx, Relatorio.aspx).

Navegue pelas abas:

Funcionários: CRUD funcionários

Férias: CRUD férias

Relatório: lista de funcionários

Observações
O Web Forms utiliza GridView, DropDownList e formulários reativos no code-behind para validações e chamadas HTTP.

Para exibir o nome do funcionário em “Férias” e “Relatório”, o Web Forms mantém um modelo local (FeriasModel, Funcionario) que espelha o JSON da API (incluindo o campo nomeFuncionario quando retornado).
