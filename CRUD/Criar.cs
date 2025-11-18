using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CRUD
{
    class Criar
    {
        public static void CadastrarAluno(string conexao)
        {
            Console.Write("\nNome:");
            string nome = Console.ReadLine();

            Console.Write("\nIdade:");
            string idadeTexto = Console.ReadLine();
            int idade;
            if (!int.TryParse(idadeTexto, out idade))
            {
                Console.WriteLine("\nIdade inválida\n");
                return;
            }

            Console.Write("\nCurso:");
            string curso = Console.ReadLine();

            string sqlInsert = "INSERT INTO alunos (Nome, Idade, Curso) VALUES (@nome, @idade, @curso)";

            try
            {
                MySqlConnection conn = Program.GetConnection(conexao);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sqlInsert, conn);
                cmd.Parameters.AddWithValue("@nome", nome);   // Erro da Apostila: @nome e @idade estavam invertidos
                cmd.Parameters.AddWithValue("@idade", idade); // Corrigido conforme o tipo correto
                cmd.Parameters.AddWithValue("@curso", curso);

                int linhas = cmd.ExecuteNonQuery();
                Console.WriteLine(linhas + "\nregistro(s) inserido(s)\n");

                conn.Close();
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro ao inserir:" + mex.Message + "\n");
            }
        }
    }
}
