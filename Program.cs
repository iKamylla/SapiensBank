using System;
using static System.Console;

// Classe Program que contém o main
public class Program
{
    public static void Main(string[] args)
    {
        var banco = new Banco();
        var ux = new UX("Sapiens Bank", banco);
        ux.Executar();

        CriarTitulo("Obrigado por usar o Sistema!");
        WriteLine(" O programa será encerrado.");
    }

    // Método auxiliar para Program (para uma saída limpa)
    private static void CriarTitulo(string titulo)
    {
        Clear();
        ForegroundColor = ConsoleColor.White;
        WriteLine("-------------------------------------------------");
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(" " + titulo);
        ForegroundColor = ConsoleColor.White;
        WriteLine("-------------------------------------------------");
    }
}
