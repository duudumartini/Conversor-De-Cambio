using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Conversor_De_Cambio.Classes;

internal class Api
{
    public static async Task<decimal> ObterTaxaDeCambio(string moedaBase, string moedaAlvo)
    {
        using (var client = new HttpClient())
        {
            string URL = "https://api.freecurrencyapi.com/v1/latest?";
            string ApiKey = "fca_live_9dzaIpvp3nVZyDoO8OVK7MWcKudPKGeFL0LSwfKB";
            string apiUrl = $"{URL}apikey={ApiKey}&currencies={moedaBase}&base_currency={moedaAlvo}";
            try
            {
                string resposta = await client.GetStringAsync(apiUrl);
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resposta);
                decimal valor = json.data[moedaBase];
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível obter as taxas de câmbio.");
                return 0;
            }
        }
    }
}
        
    

