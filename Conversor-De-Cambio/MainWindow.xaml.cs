using Conversor_De_Cambio.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System.Globalization;

namespace Conversor_De_Cambio
{
    public partial class MainWindow : Window
    {
        public string simboloMonetario;
        public string valor;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AtualizaHistorico()
        {
            List<Cambio> historicoItens = Database.RecuperaHistorico();
            foreach (var historicoItem in historicoItens)
            {
                Historico.Items.Add(historicoItem);
            }

        }

        private void Btn_Converter_Click(object sender, RoutedEventArgs e)
        {
            if (Txt_Valor.Text == "Valor" || Txt_Valor.Text =="")
            {
                MessageBox.Show("Favor preencher o valor de conversão.");
            }
            else if (Cbx_MoedaBase.Text == "Moeda Base")
            {
                MessageBox.Show("Favor selecionar a moeda base");
            }
            else if(Cbx_MoedaAlvo.Text == "Moeda Alvo")
            {
                MessageBox.Show("Favor selecionar a moeda alvo");
            }
            else if(Cbx_MoedaAlvo.Text == Cbx_MoedaBase.Text)
            {
                MessageBox.Show("Favor selecionar moedas diferentes para conversão de cambio");
            }
            else
            {
                _ = ConverteAsync();
            }
        }

        public async Task ConverteAsync()
        {
            string texto = Txt_Valor.Text;
            decimal valorDecimal = decimal.TryParse(texto, out decimal result) ? result : 0.0m;

            string siglaMoedaBase = Cbx_MoedaBase.Text.Substring(0, 3);
            string siglaMoedaAlvo = Cbx_MoedaAlvo.Text.Substring(0, 3);

            decimal respostaApi = await Api.ObterTaxaDeCambio(siglaMoedaAlvo, siglaMoedaBase);
            decimal resultado = valorDecimal / respostaApi;

            decimal resultadoMostrar = Math.Round(resultado, 2);
            MostraResultado(resultadoMostrar);
            //Txt_Valor.Text = resu;
        }
        private void MostraResultado(decimal resultado)
        {
            string[] moedaBase = Cbx_MoedaBase.Text.Split('|');
            string[] moedaAlvo = Cbx_MoedaAlvo.Text.Split('|');
            Bd_Resultado.Visibility = Visibility.Visible;
            lbl_Resultado.Content = $"{Txt_Valor.Text} {moedaBase[1]} equivalem á {resultado} {moedaAlvo[1]}";

            Cambio cambio = new Cambio();
            cambio.Valor = Txt_Valor.Text;
            cambio.Date = DateTime.Now;
            cambio.MoedaBase = moedaBase[0];
            cambio.MoedaAlvo = moedaAlvo[0];
            cambio.Resultado = resultado;
            Database.AdicionaTransacao(cambio);
            AtualizaHistorico();
            _ =AtualizaGraficoAsync();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizaHistorico();
            Bd_Resultado.Visibility = Visibility.Hidden;

            Cbx_MoedaBase.Items.Add("Moeda Base");
            foreach(var moeda in MoedasDisponiveis.moedas)
            {
                Cbx_MoedaBase.Items.Add($"{moeda.Key} | {moeda.Value}");
            }

            Cbx_MoedaBase.SelectedIndex = 0;

            Cbx_MoedaAlvo.Items.Add("Moeda Alvo");
            foreach (var moeda in MoedasDisponiveis.moedas)
            {
                Cbx_MoedaAlvo.Items.Add($"{moeda.Key} | {moeda.Value}");
            }

            Cbx_MoedaAlvo.SelectedIndex = 0;
        }

        private async Task AtualizaGraficoAsync()
        {
            string[] moedaBase = Cbx_MoedaBase.Text.Split('|');
            string[] moedaAlvo = Cbx_MoedaAlvo.Text.Split('|');
            string moeda1 = moedaBase[0].Trim();
            string moeda2 = moedaAlvo[0].Trim();
            decimal um = await ApiData.Data(moeda1, moeda2, 1);
            decimal dois = await ApiData.Data(moeda1, moeda2, 2);
            decimal tres = await ApiData.Data(moeda1, moeda2, 3);
            decimal quatro = await ApiData.Data(moeda1, moeda2, 4);
            decimal cinco = await ApiData.Data(moeda1, moeda2, 5);
            decimal seis = await ApiData.Data(moeda1, moeda2, 6);
            decimal sete = await ApiData.Data(moeda1, moeda2, 7);
            decimal oito = await ApiData.Data(moeda1, moeda2, 8);
            decimal nove = await ApiData.Data(moeda1, moeda2, 9);
            decimal dez = await ApiData.Data(moeda1, moeda2, 10);
            decimal onze = await ApiData.Data(moeda1, moeda2, 11);
            decimal doze = await ApiData.Data(moeda1, moeda2, 12);

            var model = new PlotModel { Title = "Últimos 12 Meses" };
            // Configurar o eixo X (meses)
            var dateAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Mês",
                MajorGridlineStyle = LineStyle.Solid
            };

            // Configurar o eixo Y (valor da moeda)
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Valor",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };

            model.Axes.Add(dateAxis);
            model.Axes.Add(yAxis);

            // Simular dados dos últimos 12 meses
            var valoresMensais = new[]
            {
            new DataPoint(0, (double)um),
            new DataPoint(1, (double)dois),
            new DataPoint(2, (double)tres),
            new DataPoint(3, (double)quatro),
            new DataPoint(4, (double)cinco),
            new DataPoint(5, (double)seis),
            new DataPoint(6, (double)sete),
            new DataPoint(7, (double)oito),
            new DataPoint(8, (double)nove),
            new DataPoint(9, (double)dez),
            new DataPoint(10, (double)onze),
            new DataPoint(11, (double)doze)
        };

            var series = new LineSeries
            {
                Title = "Valor da Moeda (Exemplo)",
                ItemsSource = valoresMensais,
                DataFieldX = "X",
                DataFieldY = "Y"
            };

            model.Series.Add(series);

            // Definir o modelo do gráfico no PlotView
            Grafico.Model = model;
        }

        private void Txt_Valor_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Txt_Valor.Text == "Valor")
            {
                Txt_Valor.Text = "";
                Txt_Valor.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01757a"));
            }
        }

        private void Btn_Inverter_Click(object sender, RoutedEventArgs e)
        {
            int itemMoedaAlvo = Cbx_MoedaAlvo.SelectedIndex;
            int itemMoedaBase = Cbx_MoedaBase.SelectedIndex;

            Cbx_MoedaAlvo.SelectedIndex = itemMoedaBase;
            Cbx_MoedaBase.SelectedIndex = itemMoedaAlvo;
        }

        private void Btn_LimparHistorico_Click(object sender, RoutedEventArgs e)
        {
            Database.LimpaHistorico();
            Historico.Items.Clear();
        }

    }
}
