using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Conversor_De_Cambio.Classes;

public class Database
{
    public static bool AdicionaTransacao(Cambio cambio)
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\55479\Desktop\Conversor-De-Cambio\Conversor-De-Cambio\DataBase\Database1.mdf;Integrated Security=True;Connect Timeout=30";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO [Historico] (Valor, Data, MoedaAlvo, MoedaBase, Resultado) VALUES (@Valor, @Data, @MoedaAlvo, @MoedaBase, @Resultado)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Valor", cambio.Valor);
                    command.Parameters.AddWithValue("@Data", cambio.Date);
                    command.Parameters.AddWithValue("@MoedaAlvo", cambio.MoedaAlvo);
                    command.Parameters.AddWithValue("@MoedaBase", cambio.MoedaBase);
                    command.Parameters.AddWithValue("@Resultado", cambio.Resultado);
                    command.ExecuteNonQuery();

                    // A execução do comando foi bem-sucedida.
                    return true;
                }
            }
            catch (SqlException ex)
            {
                // Aqui você pode tratar a exceção ou registrar informações sobre o erro.
                // Por exemplo, você pode usar ex.Message para obter detalhes do erro.
                MessageBox.Show("Erro ao executar a consulta SQL: " + ex.Message);
                return false;
            }
        }
    }

    public static List<Cambio> RecuperaHistorico()
    {
        List<Cambio> historicoItens = new List<Cambio>();

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\55479\Desktop\Conversor-De-Cambio\Conversor-De-Cambio\DataBase\Database1.mdf;Integrated Security=True;Connect Timeout=30";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT Valor, MoedaBase, MoedaAlvo, Resultado, Data FROM Historico";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Cambio historicoItem = new Cambio
                    {
                        Valor = reader["Valor"].ToString(),
                        MoedaBase = reader["MoedaBase"].ToString(),
                        MoedaAlvo = reader["MoedaAlvo"].ToString(),
                        Resultado = (decimal)reader["Resultado"],
                        Date = (DateTime)reader["Data"]
                    };

                    historicoItens.Add(historicoItem);
                }
            }
        }

        return historicoItens;
    }

    public static void LimpaHistorico()
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\55479\Desktop\Conversor-De-Cambio\Conversor-De-Cambio\DataBase\Database1.mdf;Integrated Security=True;Connect Timeout=30";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "DELETE FROM Historico";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}