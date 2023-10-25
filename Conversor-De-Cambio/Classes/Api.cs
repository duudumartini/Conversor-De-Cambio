using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Conversor_De_Cambio.Classes
{
    internal class Api
    {
        public static async Task<decimal> ObterTaxaDeCambio(string moedaBase, string moedaAlvo)
        {
            using (var client = new HttpClient())
            {
                string apiUrl = $"https://economia.awesomeapi.com.br/json/daily/{moedaBase}-{moedaAlvo}";
                try
                {
                    string resposta = await client.GetStringAsync(apiUrl);
                    var json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic[]>(resposta);

                    // Verifique se há pelo menos um item no array
                    if (json.Length > 0)
                    {
                        decimal valor = json[0].high;
                        return valor;
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter as taxas de câmbio.");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível obter as taxas de câmbio.");
                    return 0;
                }
            }
        }
    }
}
