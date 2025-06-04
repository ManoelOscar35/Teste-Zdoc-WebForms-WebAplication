// Relatorio.aspx.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.UI;
using GestaoPessoalWebForms.Models;

namespace GestaoPessoalWebForms
{
    public partial class Relatorio : Page
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseFuncionarios = "http://localhost:5117/api/Funcionarios";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Nada a fazer na carga inicial; aguarda clique no botão
        }

        protected async void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            lblErroRelatorio.Visible = false;
            try
            {
                var response = await _httpClient.GetAsync(ApiBaseFuncionarios);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var listaFunc = JsonSerializer.Deserialize<List<Funcionario>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Funcionario>();

                gvRelatorio.DataSource = listaFunc;
                gvRelatorio.DataBind();
            }
            catch (Exception ex)
            {
                lblErroRelatorio.Text = "Erro ao gerar relatório: " + ex.Message;
                lblErroRelatorio.Visible = true;
            }
        }
    }
}
