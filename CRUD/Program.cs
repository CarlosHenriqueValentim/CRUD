using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CRUD
{
    internal class Program
    {
        static string conexao = "server=localhost;uid=root;pwd=root;database=escola;port=3306";

        /* Erro da Apostila: a porta original era 5432 (do PostgreSQL), o que causava falha na conexão.
           Corrigi para 3306, que é a porta padrão do MySQL. */

        static void Main(string[] args)
        {
            for(;;)
            {
                try
                {
                    Console.Write("Gerenciador de Alunos - (C# + MySQL WorkBench 8.0)\n\n" +
                        "Menu\n\n" +
                        "1 - Cadastrar aluno\n" +
                        "2 - Listar todos os alunos\n" +
                        "3 - Buscar aluno por nome\n" +
                        "4 - Atualizar aluno\n" +
                        "5 - Excluir aluno\n" +
                        "6 - Exibir total de alunos\n" +
                        "Q - Sair \n\n" +
                        "Escolha uma opção:");

                    string opc = Console.ReadLine();

                    switch (opc)
                    {
                        case "1": Criar.CadastrarAluno(conexao); break;
                        case "2": Ler.ListarAlunos(conexao); break;
                        case "3": Ler.BuscarAlunoPorNome(conexao); break;
                        case "4": Atualizar.AtualizarAluno(conexao); break;
                        case "5": Excluir.ExcluirAluno(conexao); break;
                        case "6": Ler.ExibirTotalAlunos(conexao); break;
                        case "Q":
                        case "q":
                            Console.Write("\nCompilação Finalizada :)\n");
                            return;
                        default:
                            Console.WriteLine("\nOpção inválida\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nErro inesperado: " + ex.Message + "\n");
                }
            }
        }

        public static MySqlConnection GetConnection(string conexao)
        {
            return new MySqlConnection(conexao);
        }
    }
}
