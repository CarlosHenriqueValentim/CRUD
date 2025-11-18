using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CRUD
{
    class Excluir
    {
        public static void ExcluirAluno(string conexao)
        {
            Console.Write("Id do aluno a excluir:");
            string idTexto = Console.ReadLine();
            int id;

            if (!int.TryParse(idTexto, out id))
            {
                Console.WriteLine("\nId inválido\n");
                return;
            }

            string deleteSql = "DELETE FROM alunos WHERE Id = @id";

            try
            {
                MySqlConnection conn = Program.GetConnection(conexao);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(deleteSql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                int linhas = cmd.ExecuteNonQuery();
                Console.WriteLine(linhas + "\nregistro excluído\n");

                string countSql = "SELECT COUNT(*) FROM alunos";
                MySqlCommand countCmd = new MySqlCommand(countSql, conn);
                long total = (long)countCmd.ExecuteScalar();

                if (total == 0)
                {
                    string resetSql = "ALTER TABLE alunos AUTO_INCREMENT = 1";
                    MySqlCommand resetCmd = new MySqlCommand(resetSql, conn);
                    resetCmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro ao excluir:" + mex.Message + "\n");
            }
        }
    }
}
