using Xunit;
using SiteDesafioTrayCorp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TestesUnitariosSiteTrayCorp
{
    public class Index
    {
        [Fact]
        public void WhenCallIndexSholdReturnNotNull()
        {
            var controller = new ProdutosController();
            var result = controller.IndexAsync();
            Assert.NotNull(result);
        }
    }
}