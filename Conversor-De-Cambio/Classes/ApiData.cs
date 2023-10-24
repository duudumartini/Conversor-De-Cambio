using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Conversor_De_Cambio.Classes
{
    internal class ApiData
    {
        public static async Task<decimal> Data(string moedaBase, string moedaAlvo, int mes)
        {
            using (var client = new HttpClient())
            {
                string apiUrl = $"https://economia.awesomeapi.com.br/json/daily/{moedaBase}-{moedaAlvo}/365";
                try
                {
                    string resposta = await client.GetStringAsync(apiUrl);
                    var json = JsonConvert.DeserializeObject<List<ExchangeRateData>>(resposta);

                    // Converter os carimbos de data/hora Unix Epoch em datas legíveis
                    var dataWithDates = json.Select(data => new
                    {
                        Data = data,
                        Date = DateTimeOffset.FromUnixTimeSeconds(data.Timestamp).DateTime
                    });

                    // Filtrar os dados para pegar apenas o mês 5 (maio)
                    var mayData = dataWithDates.Where(data => data.Date.Month == mes);

                    // Encontrar o valor mais alto (high) para o mês de maio
                    decimal highestValue = mayData.Max(data => data.Data.High);

                    return highestValue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível obter as taxas de câmbio.");
                    return 0;
                }
            }
        }
    }

    public class ExchangeRateData
    {
        public decimal High { get; set; }
        public long Timestamp { get; set; }
    }
}
