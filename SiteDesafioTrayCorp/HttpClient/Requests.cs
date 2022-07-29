using System.Threading.Tasks;
using System.Net.Http;

namespace SiteDesafioTrayCorp.Requests
{
    public class Requests
    {

        public static async Task Get()
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync("api/Produtos");

            return;
        }
    }
}
