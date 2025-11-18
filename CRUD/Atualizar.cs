using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CRUD
{
    class Atualizar
    {
        public static void AtualizarAluno(string conexao)
        {
            Console.Write("\nId do aluno a atualizar:");
            string idTexto = Console.ReadLine();
            int id;
            if (!int.TryParse(idTexto, out id))
            {
                Console.WriteLine("\nId inválido");
                return;
            }

            Console.Write("\nNovo nome:");
            string novoNome = Console.ReadLine();

            Console.Write("\nNova idade:");
            string idadeInput = Console.ReadLine();

            Console.Write("\nNovo curso:");
            string novoCurso = Console.ReadLine();

            string selectSql = "SELECT Nome, Idade, Curso FROM alunos WHERE Id = @id";

            try
            {
                MySqlConnection conn = Program.GetConnection(conexao);
                conn.Open();

                MySqlCommand selectCmd = new MySqlCommand(selectSql, conn);
                selectCmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = selectCmd.ExecuteReader();

                if (!reader.Read())
                {
                    Console.WriteLine("\nAluno não encontrado\n");
                    reader.Close();
                    conn.Close();
                    return;
                }

                string nomeAtual = reader.GetString("Nome");
                int idadeAtual = reader.GetInt32("Idade");
                string cursoAtual = reader.GetString("Curso");

                reader.Close();

                string nomeFinal = (string.IsNullOrWhiteSpace(novoNome)) ? nomeAtual : novoNome;
                int idadeFinal;
                if (!int.TryParse(idadeInput, out idadeFinal))
                    idadeFinal = idadeAtual;

                string cursoFinal = (string.IsNullOrWhiteSpace(novoCurso)) ? cursoAtual : novoCurso;

                string updateSql = "UPDATE alunos SET Nome = @nome, Idade = @idade, Curso = @curso WHERE Id = @id";

                /* Erro da Apostila: o comando UPDATE não tinha WHERE, atualizando todos os registros.
                   Corrigido adicionando WHERE Id=@id. */

                MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                updateCmd.Parameters.AddWithValue("@nome", nomeFinal);
                updateCmd.Parameters.AddWithValue("@idade", idadeFinal);
                updateCmd.Parameters.AddWithValue("@curso", cursoFinal);
                updateCmd.Parameters.AddWithValue("@id", id);

                int linhas = updateCmd.ExecuteNonQuery();
                Console.WriteLine(linhas + "\nregistro atualizado");

                conn.Close();
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro ao atualizar:" + mex.Message);
            }
        }
    }
}
