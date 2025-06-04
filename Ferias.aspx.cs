// ---------------------------------------------------------------------------
// FeriasPage.aspx.cs (code-behind do Web Forms)
// ---------------------------------------------------------------------------
using GestaoPessoalWebForms.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoPessoalWebForms
{

	public class FeriasModel
	{
		public int Id { get; set; }
		public DateTime DataInicio { get; set; }
		public DateTime DataTermino { get; set; }
		public string Status { get; set; } = default;

		// O JSON da API já traz “nomeFuncionario” como string,
		// então recebemos diretamente aqui, em vez de um objeto Funcionario.
		public int FuncionarioId { get; set; }

		// Propriedade mapeada do campo "nomeFuncionario" do DTO no Swagger
		[JsonPropertyName("NomeFuncionario")]
		public string NomeFuncionario { get; set; } = default;
	}


	public partial class FeriasPage : Page
	{
		private static readonly HttpClient _httpClient = new HttpClient();

		// URLs da API (ajuste a porta se necessário)
		private const string ApiBaseFerias = "http://localhost:5117/api/Ferias";
		private const string ApiBaseFuncionarios = "http://localhost:5117/api/Funcionarios";

		protected async void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				await CarregarFuncionariosDropDownAsync();
				await CarregarFeriasAsync();
			}
		}

		private async Task CarregarFuncionariosDropDownAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync(ApiBaseFuncionarios);
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				var listaFunc = JsonSerializer.Deserialize<List<Funcionario>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}) ?? new List<Funcionario>();

				ddlFuncionario.Items.Clear();
				ddlFuncionario.Items.Add(new ListItem("-- Selecione --", "0"));

				foreach (var f in listaFunc)
				{
					ddlFuncionario.Items.Add(new ListItem(f.Nome, f.Id.ToString()));
				}
			}
			catch (Exception ex)
			{
				lblErroFerias.Text = "Erro ao carregar funcionários: " + ex.Message;
				lblErroFerias.Visible = true;
			}
		}

		private async Task CarregarFeriasAsync()
		{
			lblErroFerias.Visible = false;
			try
			{
				var response = await _httpClient.GetAsync(ApiBaseFerias);
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				var listaFerias = JsonSerializer.Deserialize<List<FeriasModel>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}) ?? new List<FeriasModel>();

				// Agora a lista contém NomeFuncionario (em vez de Funcionario.Nome aninhado)
				gvFerias.DataSource = listaFerias;
				gvFerias.DataBind();
			}
			catch (Exception ex)
			{
				lblErroFerias.Text = "Erro ao carregar férias: " + ex.Message;
				lblErroFerias.Visible = true;
			}
		}

		protected async void btnSalvarFerias_Click(object sender, EventArgs e)
		{
			lblErroFerias.Visible = false;

			int.TryParse(hfFeriasId.Value, out int idFerias);

			if (!int.TryParse(ddlFuncionario.SelectedValue, out int idFunc) || idFunc <= 0)
			{
				lblErroFerias.Text = "Selecione um funcionário.";
				lblErroFerias.Visible = true;
				return;
			}

			if (!DateTime.TryParse(txtDataInicio.Text, out DateTime dataInicio))
			{
				lblErroFerias.Text = "Data de Início inválida.";
				lblErroFerias.Visible = true;
				return;
			}

			if (!DateTime.TryParse(txtDataTermino.Text, out DateTime dataTermino))
			{
				lblErroFerias.Text = "Data de Término inválida.";
				lblErroFerias.Visible = true;
				return;
			}

			var statusEscolhido = ddlStatusFerias.SelectedValue;
			if (string.IsNullOrWhiteSpace(statusEscolhido))
			{
				lblErroFerias.Text = "Selecione um status para as férias.";
				lblErroFerias.Visible = true;
				return;
			}

			// Prepara o objeto para envio (note que NomeFuncionario não é enviado no PUT/POST)
			var feriasObj = new FeriasModel
			{
				Id = idFerias,
				DataInicio = dataInicio,
				DataTermino = dataTermino,
				FuncionarioId = idFunc,
				Status = statusEscolhido
				// NomeFuncionario NÃO deve ser preenchido aqui, pois só a API define esse campo no GET
			};

			try
			{
				string json = JsonSerializer.Serialize(feriasObj);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				if (idFerias > 0)
				{
					// Edição
					var responsePut = await _httpClient.PutAsync($"{ApiBaseFerias}/{idFerias}", content);
					responsePut.EnsureSuccessStatusCode();
				}
				else
				{
					// Criação
					var responsePost = await _httpClient.PostAsync(ApiBaseFerias, content);
					responsePost.EnsureSuccessStatusCode();
				}

				LimparFormularioFerias();
				await CarregarFeriasAsync();
			}
			catch (Exception ex)
			{
				lblErroFerias.Text = "Falha ao salvar férias: " + ex.Message;
				lblErroFerias.Visible = true;
			}
		}

		protected void btnCancelarFerias_Click(object sender, EventArgs e)
		{
			LimparFormularioFerias();
		}

		private void LimparFormularioFerias()
		{
			hfFeriasId.Value = "0";
			ddlFuncionario.SelectedValue = "0";
			txtDataInicio.Text = "";
			txtDataTermino.Text = "";
			ddlStatusFerias.SelectedValue = "Pendente";
			btnSalvarFerias.Text = "Salvar";
		}

		protected async void gvFerias_RowEditing(object sender, GridViewEditEventArgs e)
		{
			lblErroFerias.Visible = false;
			int id = Convert.ToInt32(gvFerias.DataKeys[e.NewEditIndex].Value);
			e.Cancel = true;

			try
			{
				var response = await _httpClient.GetAsync($"{ApiBaseFerias}/{id}");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				var ferias = JsonSerializer.Deserialize<FeriasModel>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (ferias != null)
				{
					hfFeriasId.Value = ferias.Id.ToString();

					// Seleciona o dropdown pelo FuncionarioId
					ddlFuncionario.SelectedValue = ferias.FuncionarioId.ToString();

					txtDataInicio.Text = ferias.DataInicio.ToString("yyyy-MM-dd");
					txtDataTermino.Text = ferias.DataTermino.ToString("yyyy-MM-dd");
					ddlStatusFerias.SelectedValue = ferias.Status;
					btnSalvarFerias.Text = "Atualizar";
				}
			}
			catch (Exception ex)
			{
				lblErroFerias.Text = "Erro ao carregar férias: " + ex.Message;
				lblErroFerias.Visible = true;
			}
		}

		protected async void gvFerias_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			lblErroFerias.Visible = false;
			int id = Convert.ToInt32(gvFerias.DataKeys[e.RowIndex].Value);

			try
			{
				var response = await _httpClient.DeleteAsync($"{ApiBaseFerias}/{id}");
				response.EnsureSuccessStatusCode();
				await CarregarFeriasAsync();
			}
			catch (Exception ex)
			{
				lblErroFerias.Text = "Erro ao excluir férias: " + ex.Message;
				lblErroFerias.Visible = true;
			}
		}
	}
}
