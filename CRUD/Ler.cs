using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CRUD
{
    class Ler
    {
        public static void ListarAlunos(string conexao)
        {
            string sqlSelect = "SELECT Id, Nome, Idade, Curso FROM alunos";
            try
            {
                MySqlConnection conn = Program.GetConnection(conexao);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sqlSelect, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("\nNenhum aluno encontrado\n");
                    reader.Close();
                    conn.Close();
                    return;
                }

                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string nome = reader.GetString("Nome");
                    int idade = reader.GetInt32("Idade");
                    string curso = reader.GetString("Curso");
                    Console.WriteLine("Id: " + id + " | Nome: " + nome + " | Idade: " + idade + " | Curso: " + curso);
                }

                reader.Close();
                conn.Close();
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro ao listar: " + mex.Message + "\n");
            }
        }

        public static void BuscarAlunoPorNome(string conexao)
        {
            Console.Write("\nDigite parte ou todo o nome:");
            string termo = Console.ReadLine();

            string sql = "SELECT Id, Nome, Idade, Curso FROM alunos WHERE Nome LIKE @termo";
            try
            {
                MySqlConnection conn = Program.GetConnection(conexao);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");

                MySqlDataReader reader = cmd.ExecuteReader();
                bool encontrou = false;

                while (reader.Read())
                {
                    encontrou = true;
                    int id = reader.GetInt32("Id");
                    string nome = reader.GetString("Nome");
                    int idade = reader.GetInt32("Idade"); // Erro da Apostila: usava GetString em vez de GetInt32
                    string curso = reader.GetString("Curso");
                    Console.WriteLine("Id: " + id + " | Nome: " + nome + " | Idade: " + idade + " | Curso: " + curso);
                }

                if (!encontrou)
                    Console.WriteLine("\nNenhum resultado\n");

                reader.Close();
                conn.Close();
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro na busca:" + mex.Message + "\n");
            }
        }

        public static void ExibirTotalAlunos(string conexao)
        {
            string sql = "SELECT COUNT(*) FROM alunos";
            try
            {
                MySqlConnection conn = Program.GetConnection(conexao);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();

                Console.WriteLine("\nTotal de alunos:" + result);

                conn.Close();
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro ao contar:" + mex.Message);
            }
        }
    }
}
