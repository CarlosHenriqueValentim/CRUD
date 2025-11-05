using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConectandoBanco
{
    internal class Program
    {
        const string CONNECTION_STRING = "server=localhost;uid=root;pwd=root;database=escola;port=3306";

        static void Main(string[] args)
        {
            for(; ; )
            {
                try
                {
                    Console.Write("Gerenciador de Alunos - Console (C# + MySQL WorkBench 8.0)\n\nMenu\n\n1 - Cadastrar aluno\n2 - Listar todos os alunos\n3 - Buscar aluno por nome\n4 - Atualizar aluno\n5 - Excluir aluno\n6 - Exibir total de alunos\nQ - Sair \n\nEscolha uma opção:");
                    var opc = Console.ReadLine();

                    switch (opc)
                    {
                        case "1": CadastrarAluno(); break;
                        case "2": ListarAlunos(); break;
                        case "3": BuscarAlunoPorNome(); break;
                        case "4": AtualizarAluno(); break;
                        case "5": ExcluirAluno(); break;
                        case "6": ExibirTotalAlunos(); break;
                        case "Q": return;
                        case "q": return;
                        default: Console.WriteLine("\nOpção inválida\n"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nErro inesperado: \n" + ex.Message);
                }
            }
        }

        static MySqlConnection GetConnection()
        {
            return new MySqlConnection(CONNECTION_STRING);
        }

        static void CadastrarAluno()
        {
            Console.Write("\nNome: ");
            string nome = Console.ReadLine();
            Console.Write("Idade: ");
            if (!int.TryParse(Console.ReadLine(), out int idade))
            {
                Console.WriteLine("\nIdade inválida\n");
                return;
            }
            Console.Write("Curso: ");
            string curso = Console.ReadLine();

            string sqlInsert = "INSERT INTO alunos (Nome, Idade, Curso) VALUES (@nome, @idade, @curso)";

            try
            {
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var cmd = new MySqlCommand(sqlInsert, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@idade", idade);
                        cmd.Parameters.AddWithValue("@curso", curso);
                        int linhas = cmd.ExecuteNonQuery();
                        Console.WriteLine(linhas + " registro(s) inserido(s).");
                    }
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro ao inserir: " + mex.Message);
            }
        }

        static void ListarAlunos()
        {
            string sqlSelect = "SELECT Id, Nome, Idade, Curso FROM alunos";
            try
            {
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var cmd = new MySqlCommand(sqlSelect, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("\nNenhum aluno encontrado\n");
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
                    }
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("\nErro ao listar: " + mex.Message);
            }
        }

        static void BuscarAlunoPorNome()
        {
            Console.Write("\nDigite parte ou todo o nome:");
            string termo = Console.ReadLine();
            string sql = "SELECT Id, Nome, Idade, Curso FROM alunos WHERE Nome LIKE @termo";
            try
            {
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            bool encontrou = false;
                            while (reader.Read())
                            {
                                encontrou = true;
                                int id = reader.GetInt32("Id");
                                string nome = reader.GetString("Nome");
                                int idade = reader.GetInt32("Idade");
                                string curso = reader.GetString("Curso");
                                Console.WriteLine("Id: " + id + " | Nome: " + nome + " | Idade: " + idade + " | Curso: " + curso);
                            }
                            if (!encontrou) Console.WriteLine("Nenhum resultado.");
                        }
                    }
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("Erro na busca: " + mex.Message);
            }
        }

        static void AtualizarAluno()
        {
            Console.Write("Id do aluno a atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            Console.Write("Novo nome (enter para manter): ");
            string novoNome = Console.ReadLine();
            Console.Write("Nova idade (enter para manter): ");
            string idadeInput = Console.ReadLine();
            Console.Write("Novo curso (enter para manter): ");
            string novoCurso = Console.ReadLine();

            string selectSql = "SELECT Nome, Idade, Curso FROM alunos WHERE Id = @id";

            try
            {
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var selectCmd = new MySqlCommand(selectSql, conexao))
                    {
                        selectCmd.Parameters.AddWithValue("@id", id);
                        using (var reader = selectCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                Console.WriteLine("Aluno não encontrado.");
                                return;
                            }

                            string nomeAtual = reader.GetString("Nome");
                            int idadeAtual = reader.GetInt32("Idade");
                            string cursoAtual = reader.GetString("Curso");

                            string nomeFinal = string.IsNullOrWhiteSpace(novoNome) ? nomeAtual : novoNome;
                            int idadeFinal = int.TryParse(idadeInput, out int tmp) ? tmp : idadeAtual;
                            string cursoFinal = string.IsNullOrWhiteSpace(novoCurso) ? cursoAtual : novoCurso;

                            reader.Close();

                            string updateSql = "UPDATE alunos SET Nome = @nome, Idade = @idade, Curso = @curso WHERE Id = @id";
                            using (var updateCmd = new MySqlCommand(updateSql, conexao))
                            {
                                updateCmd.Parameters.AddWithValue("@nome", nomeFinal);
                                updateCmd.Parameters.AddWithValue("@idade", idadeFinal);
                                updateCmd.Parameters.AddWithValue("@curso", cursoFinal);
                                updateCmd.Parameters.AddWithValue("@id", id);
                                int linhas = updateCmd.ExecuteNonQuery();
                                Console.WriteLine(linhas + " registro(s) atualizado(s).");
                            }
                        }
                    }
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("Erro ao atualizar: " + mex.Message);
            }
        }

        static void ExcluirAluno()
        {
            Console.Write("Id do aluno a excluir: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nId inválido\n");
                return;
            }

            string sql = "DELETE FROM alunos WHERE Id = @id";
            try
            {
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int linhas = cmd.ExecuteNonQuery();
                        Console.WriteLine(linhas + " registro(s) excluído(s).");
                    }
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("Erro ao excluir: " + mex.Message);
            }
        }

        static void ExibirTotalAlunos()
        {
            string sql = "SELECT COUNT(*) FROM alunos";
            try
            {
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var cmd = new MySqlCommand(sql, conexao))
                    {
                        var result = cmd.ExecuteScalar();
                        Console.WriteLine("Total de alunos: " + result);
                    }
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("Erro ao contar: " + mex.Message);
            }
        }
    }
}
