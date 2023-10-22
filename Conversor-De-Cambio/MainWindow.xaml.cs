using Conversor_De_Cambio.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Conversor_De_Cambio
{
    public partial class MainWindow : Window
    {
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
            if (Txt_Valor.Text == "")
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

            decimal respostaApi = await Api.ObterTaxaDeCambio(siglaMoedaBase, siglaMoedaAlvo);
            decimal resultado = valorDecimal / respostaApi;

            decimal resultadoMostrar = Math.Round(resultado, 2);
            MostraResultado(resultadoMostrar);
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
