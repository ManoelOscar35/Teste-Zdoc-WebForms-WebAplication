using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoPessoalWebForms
{
    // Modelo local para desserialização
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public string Status { get; set; }
    }

    public partial class Funcionarios : Page
    {
        // HttpClient único para toda a aplicação
        private static readonly HttpClient _httpClient = new HttpClient();
        // Base da API na porta 5117
        private const string ApiBaseUrl = "http://localhost:5117/api/Funcionarios";

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CarregarFuncionariosAsync();
            }
        }

        private async Task CarregarFuncionariosAsync()
        {
            lblErro.Visible = false;
            try
            {
                var response = await _httpClient.GetAsync(ApiBaseUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var lista = JsonSerializer.Deserialize<List<Funcionario>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                gvFuncionarios.DataSource = lista;
                gvFuncionarios.DataBind();
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao carregar funcionários: " + ex.Message;
                lblErro.Visible = true;
            }
        }

        protected async void btnSalvar_Click(object sender, EventArgs e)
        {
            lblErro.Visible = false;

            // Obtém o id (0 para cadastro, >0 para edição)
            int.TryParse(hfFuncionarioId.Value, out int id);

            // Validação mínima
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtCargo.Text) ||
                string.IsNullOrWhiteSpace(txtDataAdmissao.Text) ||
                string.IsNullOrWhiteSpace(txtSalario.Text))
            {
                lblErro.Text = "Preencha todos os campos.";
                lblErro.Visible = true;
                return;
            }

            // Tenta converter DataAdmissao e Salario
            if (!DateTime.TryParse(txtDataAdmissao.Text, out DateTime dataAdm))
            {
                lblErro.Text = "Data de admissão inválida.";
                lblErro.Visible = true;
                return;
            }
            if (!decimal.TryParse(txtSalario.Text, out decimal salario))
            {
                lblErro.Text = "Salário inválido.";
                lblErro.Visible = true;
                return;
            }

            var func = new Funcionario
            {
                Nome = txtNome.Text.Trim(),
                Cargo = txtCargo.Text.Trim(),
                DataAdmissao = dataAdm,
                Salario = salario,
                Status = ddlStatus.SelectedValue
            };

            try
            {
                if (id > 0)
                {
                    // Edição
                    func.Id = id;
                    var jsonPut = JsonSerializer.Serialize(func);
                    var contentPut = new StringContent(jsonPut, Encoding.UTF8, "application/json");
                    var responsePut = await _httpClient.PutAsync($"{ApiBaseUrl}/{id}", contentPut);
                    responsePut.EnsureSuccessStatusCode();
                }
                else
                {
                    // Cadastro
                    var jsonPost = JsonSerializer.Serialize(func);
                    var contentPost = new StringContent(jsonPost, Encoding.UTF8, "application/json");
                    var responsePost = await _httpClient.PostAsync(ApiBaseUrl, contentPost);
                    responsePost.EnsureSuccessStatusCode();
                }

                LimparFormulario();
                await CarregarFuncionariosAsync();
            }
            catch (Exception ex)
            {
                lblErro.Text = "Falha ao salvar funcionário: " + ex.Message;
                lblErro.Visible = true;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparFormulario();
        }

        private void LimparFormulario()
        {
            hfFuncionarioId.Value = "0";
            txtNome.Text = "";
            txtCargo.Text = "";
            txtDataAdmissao.Text = "";
            txtSalario.Text = "";
            ddlStatus.SelectedValue = "Ativo";
            btnSalvar.Text = "Salvar";
        }

        protected async void gvFuncionarios_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblErro.Visible = false;
            int id = Convert.ToInt32(gvFuncionarios.DataKeys[e.NewEditIndex].Value);
            e.Cancel = true;
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var func = JsonSerializer.Deserialize<Funcionario>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (func != null)
                {
                    hfFuncionarioId.Value = func.Id.ToString();
                    txtNome.Text = func.Nome;
                    txtCargo.Text = func.Cargo;
                    txtDataAdmissao.Text = func.DataAdmissao.ToString("yyyy-MM-dd");
                    txtSalario.Text = func.Salario.ToString();
                    ddlStatus.SelectedValue = func.Status;
                    btnSalvar.Text = "Atualizar";
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao carregar funcionário: " + ex.Message;
                lblErro.Visible = true;
            }
        }

        protected async void gvFuncionarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblErro.Visible = false;
            int id = Convert.ToInt32(gvFuncionarios.DataKeys[e.RowIndex].Value);

            try
            {
                var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                await CarregarFuncionariosAsync();
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao excluir funcionário: " + ex.Message;
                lblErro.Visible = true;
            }
        }
    }
}
