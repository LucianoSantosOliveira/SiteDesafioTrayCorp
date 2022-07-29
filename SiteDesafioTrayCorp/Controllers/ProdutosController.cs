using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteDesafioTrayCorp.Models;
using SiteDesafioTrayCorp.Requests;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

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
            produtoViewModel = new ProdutoViewModel();
            produtoViewModels = new List<ProdutoViewModel>();
        }

        private async Task GetAllProdutos()
        {
            var ProdutoJson = await Request.GetStringAsync("https://localhost:7298/api/Produtos");
            
             produtoViewModels = JsonConvert.DeserializeObject<List<ProdutoViewModel>>(ProdutoJson);
             return;
        }

        private async Task GetProdutosById(Guid produtoId)
        {
            var ProdutoJson = await Request.GetStringAsync("https://localhost:7298/api/Produtos/" + produtoId.ToString());

            produtoViewModel = JsonConvert.DeserializeObject<ProdutoViewModel>(ProdutoJson);
            return;
        }

        // GET: ProdutoController
        public async Task<ActionResult> IndexAsync()
        {
            await GetAllProdutos();

            if (!produtoViewModels.Any())
                return View(new ProdutoViewModel()
                {
                    nome = "Não há produtos cadastrados"
                });
            
            return View(produtoViewModels);
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

        // POST: ProdutoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid produtoId,ProdutoViewModel collection)
        {
            try
            {
                var ProdutoJson = JsonConvert.SerializeObject(collection);
                var requestContent = new StringContent(ProdutoJson, Encoding.UTF8, "application/json");

                if (collection.estoque < 0)
                    ModelState.AddModelError("estoque", "Estoque não pode ser número negativo");

                await Request.PostAsync("https://localhost:7298/api/Produtos/", requestContent);

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
            return View(produtoViewModel);
        }

        // POST: ProdutoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid produtoId, ProdutoViewModel collection)
        {
            try
            {
                var url = "https://localhost:7298/api/Produtos/";
                var ProdutoJson = JsonConvert.SerializeObject(collection);
                var requestContent = new StringContent(ProdutoJson, Encoding.UTF8, "application/json");

                if (collection.estoque < 0)
                {
                    ModelState.AddModelError("estoque", "Estoque não pode ser número negativo");

                    return View();
                }
                    

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
                await Request.DeleteAsync("https://localhost:7298/api/Produtos/" + collection.id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
