using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversor_De_Cambio.Classes;

internal class MoedasDisponiveis
{
    public static Dictionary<string, string> moedas = new Dictionary<string, string>
    {
    { "EUR", "Euro" },
    { "USD", "Dólar Americano" },
    { "JPY", "Iene Japonês" },
    { "BGN", "Lev Búlgaro" },
    { "CZK", "Coroa Tcheca" },
    { "DKK", "Coroa Dinamarquesa" },
    { "GBP", "Libra Esterlina" },
    { "HUF", "Florim Húngaro" },
    { "PLN", "Zloty Polonês" },
    { "RON", "Leu Romeno" },
    { "SEK", "Coroa Sueca" },
    { "CHF", "Franco Suíço" },
    { "ISK", "Coroa Islandesa" },
    { "NOK", "Coroa Norueguesa" },
    { "HRK", "Kuna Croata" },
    { "RUB", "Rublo Russo" },
    { "TRY", "Lira Turca" },
    { "AUD", "Dólar Australiano" },
    { "BRL", "Real Brasileiro" },
    { "CAD", "Dólar Canadense" },
    { "CNY", "Yuan Chinês" },
    { "HKD", "Dólar de Hong Kong" },
    { "IDR", "Rupia Indonésia" },
    { "ILS", "Novo Sheqel Israelense" },
    { "INR", "Rúpia Indiana" },
    { "KRW", "Won Sul-Coreano" },
    { "MXN", "Peso Mexicano" },
    { "MYR", "Ringgit Malaio" },
    { "NZD", "Dólar Neozelandês" },
    { "PHP", "Peso Filipino" },
    { "SGD", "Dólar de Singapura" },
    { "THB", "Baht Tailandês" },
    { "ZAR", "Rand Sul-Africano" }
    };

}
