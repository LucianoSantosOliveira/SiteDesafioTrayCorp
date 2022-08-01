using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteDesafioTrayCorp.Models;
using SiteDesafioTrayCorp.Requests;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Globalization;

namespace SiteDesafioTrayCorp.Controllers
{
    public class ProdutosController : Controller
    {

        private readonly HttpClient Request;
        private ProdutoViewModel produtoViewModel;
        private List<ProdutoViewModel> produtoViewModels;
        

        public ProdutosController()
        {
            Request = new HttpClient();
            Request.BaseAddress = new Uri("https://localhost:7298/");
            produtoViewModel = new ProdutoViewModel();
            produtoViewModels = new List<ProdutoViewModel>();
        }

        private async Task GetAllProdutos()
        {
            var ProdutoJson = await Request.GetStringAsync("api/Produtos");
            
             produtoViewModels = JsonConvert.DeserializeObject<List<ProdutoViewModel>>(ProdutoJson);
             return;
        }

        private async Task GetProdutosById(Guid produtoId)
        {
            var ProdutoJson = await Request.GetStringAsync("api/Produtos/" + produtoId.ToString());

            produtoViewModel = JsonConvert.DeserializeObject<ProdutoViewModel>(ProdutoJson);
            return;
        }

        // GET: ProdutoController
        public async Task<ActionResult> IndexAsync()
        {

            try
            {
                await GetAllProdutos();

                if (!produtoViewModels.Any())
                    return View(new ProdutoViewModel()
                    {
                        nome = "Não há produtos cadastrados"
                    });

                return View(produtoViewModels);
            }

            catch (Exception ex)
            {
                return View();
            }
            
        }

        // GET: ProdutoController/Details/5
        public async Task<ActionResult> Details(Guid produtoId)
        {
            await GetProdutosById(produtoId);
            return View(produtoViewModel);
        }

        // GET: ProdutoController/Create
        public ActionResult Create()
        {
            ProdutoViewModel produtoViewModel = new ProdutoViewModel() { id = Guid.NewGuid() };
            return View(produtoViewModel);
        }

        private float ConvertValorToFloat(string valor)
        {

            CultureInfo formato = null;
            formato = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            formato.NumberFormat.NumberDecimalSeparator = ".";
            formato.NumberFormat.NumberGroupSeparator = ",";
            return float.Parse(valor.Replace("R$", ""),formato);
                    
        }

        // POST: ProdutoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid produtoId,ProdutoViewModel collection)
        {
            try
            {
                if (collection.estoque < 0)
                {
                    ModelState.AddModelError("estoque", "Estoque não pode ser número negativo");                                     
                }
                if (String.IsNullOrEmpty(collection.nome))
                {
                    ModelState.AddModelError("nome", "nome não pode ser nulo");
                }

                if(!ModelState.IsValid)
                {
                    return View();
                }

                collection.value = ConvertValorToFloat(collection.Valor);
                var ProdutoJson = JsonConvert.SerializeObject(collection);
                var requestContent = new StringContent(ProdutoJson, Encoding.UTF8, "application/json");
                await Request.PostAsync("api/Produtos/", requestContent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProdutoController/Edit/5
        public async Task<ActionResult> Edit(Guid produtoId)
        {
            await GetProdutosById(produtoId);
            produtoViewModel.Valor = produtoViewModel.value.ToString();
            return View(produtoViewModel);
        }

        // POST: ProdutoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid produtoId, ProdutoViewModel collection)
        {
            try
            {
                if (collection.estoque < 0)
                {
                    ModelState.AddModelError("estoque", "Estoque não pode ser número negativo");
                }
                if (String.IsNullOrEmpty(collection.nome))
                {
                    ModelState.AddModelError("nome", "nome não pode ser nulo");
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }

                var url = "api/Produtos/";
                collection.value = ConvertValorToFloat(collection.Valor);
                var ProdutoJson = JsonConvert.SerializeObject(collection);                
                var requestContent = new StringContent(ProdutoJson, Encoding.UTF8, "application/json");
                await Request.PutAsync(url + collection.id, requestContent);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProdutoController/Delete/5
        public async Task<ActionResult> Delete(Guid produtoId)
        {
            await GetProdutosById(produtoId);
            
            return View(produtoViewModel);
        }

        // POST: ProdutoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid produtoId, ProdutoViewModel collection)
        {
            try
            {
                await Request.DeleteAsync("api/Produtos/" + collection.id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
