using System;
using System.Linq;
using static System.Console;

public class UX
{
    private readonly Banco _banco;
    private readonly string _titulo;

    public UX(string titulo, Banco banco)
    {
        _titulo = titulo;
        _banco = banco;
    }

    public void Executar()
    {
        CriarTitulo(_titulo);
        WriteLine(" [1] Criar Conta");
        WriteLine(" [2] Listar Contas");
        WriteLine(" [3] Efetuar Saque");
        WriteLine(" [4] Efetuar Depósito");
        WriteLine(" [5] Aumentar Limite");
        WriteLine(" [6] Diminuir Limite"); 
        ForegroundColor = ConsoleColor.Red;
        WriteLine("\n [9] Sair");
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        Write(" Digite a opção desejada: ");
        var opcao = ReadLine() ?? "";
        ForegroundColor = ConsoleColor.White;

        switch (opcao)
        {
            case "1": CriarConta(); break;
            case "2": MenuListarContas(); break;
            case "3": EfetuarSaque(); break;     
            case "4": EfetuarDeposito(); break;   
            case "5": AumentarLimite(); break;   
            case "6": DiminuirLimite(); break;  
        }

        if (opcao != "9")
        {
            Executar();
        }
        _banco.SaveContas();
    }

    // Implementações de Funcionalidades

    private void CriarConta()
    {
        CriarTitulo(_titulo + " - Criar Conta");

        int numero;
        
        while (true)
        {
            Write(" Numero:  ");
            
            if (int.TryParse(ReadLine(), out numero) && numero > 0)
            {
                if (_banco.Contas.Any(c => c.Numero == numero))
                {
                    CriarRodape("ERRO: Já existe uma conta com este número. Tente novamente.");
                    continue;
                }
                break;
            }
            CriarRodape("ERRO: Número de conta inválido. Pressione ENTER para tentar novamente.");
        }

        Write(" Cliente: ");
        var cliente = ReadLine() ?? "";
        Write(" CPF:     ");
        var cpf = ReadLine() ?? "";
        Write(" Senha:   ");
        var senha = ReadLine() ?? "";
        
        decimal limite;
        
        while (true)
        {
            Write(" Limite:  ");
            
            if (decimal.TryParse(ReadLine(), out limite) && limite >= 0)
            {
                break;
            }
            CriarRodape("ERRO: Limite inválido. Pressione ENTER para tentar novamente.");
        }

        var conta = new Conta(numero, cliente, cpf, senha, limite);
        _banco.Contas.Add(conta);

        CriarRodape("Conta criada com sucesso!");
    }

    private void MenuListarContas()
    {
        CriarTitulo(_titulo + " - Listar Contas");
        if (!_banco.Contas.Any())
        {
            WriteLine(" Não há contas cadastradas.");
        }
        else
        {
            foreach (var conta in _banco.Contas)
            {
                WriteLine($" Conta: {conta.Numero} - {conta.Cliente}");
                WriteLine($" Saldo: {conta.Saldo:C} | Limite: {conta.Limite:C}");
                WriteLine($" Saldo Disponível: {conta.SaldoDisponível:C}\n");
            }
        }
        CriarRodape();
    }

    private void EfetuarSaque()
    {
        CriarTitulo(_titulo + " - Efetuar Saque");
        var conta = LocalizarContaComSenha(out string senha);

        if (conta == null) return;
        
        if (conta.Senha != senha)
        {
            CriarRodape("ERRO: Senha incorreta.");
            return;
        }

        Write(" Valor do Saque: ");
        if (decimal.TryParse(ReadLine(), out decimal valor) && valor > 0)
        {
            if (conta.Sacar(valor))
            {
                CriarRodape($"Saque de {valor:C} efetuado com sucesso. Saldo atual: {conta.Saldo:C}");
            }
            else
            {
                CriarRodape("ERRO: Saldo disponível insuficiente.");
            }
        }
        else
        {
            CriarRodape("ERRO: Valor de saque inválido.");
        }
    }

    private void EfetuarDeposito()
    {
        CriarTitulo(_titulo + " - Efetuar Depósito");
        var conta = LocalizarContaSemSenha(); 

        if (conta == null) return;

        Write(" Valor do Depósito: ");
        if (decimal.TryParse(ReadLine(), out decimal valor) && valor > 0)
        {
            conta.Depositar(valor);
            CriarRodape($"Depósito de {valor:C} efetuado com sucesso. Saldo atual: {conta.Saldo:C}");
        }
        else
        {
            CriarRodape("ERRO: Valor de depósito inválido.");
        }
    }

    private void AumentarLimite()
    {
        CriarTitulo(_titulo + " - Aumentar Limite");
        var conta = LocalizarContaComSenha(out string senha);
        
        if (conta == null) return;
        
        if (conta.Senha != senha)
        {
            CriarRodape("ERRO: Senha incorreta.");
            return;
        }
        
        Write($" Limite Atual: {conta.Limite:C}\n");
        Write(" Novo Limite: ");
        
        if (decimal.TryParse(ReadLine(), out decimal novoLimite) && novoLimite > conta.Limite) 
        {
            conta.AumentarLimite(novoLimite);
            CriarRodape($"Limite aumentado para {conta.Limite:C} com sucesso.");
        }
        else
        {
            CriarRodape("ERRO: Novo limite inválido ou não é maior que o limite atual.");
        }
    }

    private void DiminuirLimite()
    {
        CriarTitulo(_titulo + " - Diminuir Limite");
        var conta = LocalizarContaComSenha(out string senha);
        
        if (conta == null) return;

        if (conta.Senha != senha)
        {
            CriarRodape("ERRO: Senha incorreta.");
            return;
        }
        
        Write($" Limite Atual: {conta.Limite:C}\n");
        Write(" Novo Limite: ");
        
        if (decimal.TryParse(ReadLine(), out decimal novoLimite) && novoLimite < conta.Limite)
        {
            if (conta.DiminuirLimite(novoLimite))
            {
                CriarRodape($"Limite diminuído para {conta.Limite:C} com sucesso.");
            }
            else
            {
                CriarRodape("ERRO: Não é possível diminuir o limite, pois o Saldo + Novo Limite ficaria negativo.");
            }
        }
        else
        {
            CriarRodape("ERRO: Novo limite inválido ou não é menor que o limite atual.");
        }
    }

    // Métodos Auxiliares de Localização

    /// <summary>Localiza uma conta e solicita a senha.</summary>
    private Conta? LocalizarContaComSenha(out string senhaDigitada)
    {
        senhaDigitada = "";
        Write(" Número da Conta: ");
        if (!int.TryParse(ReadLine(), out int numeroConta))
        {
            CriarRodape("ERRO: Número de conta inválido.");
            return null;
        }

        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numeroConta);

        if (conta == null)
        {
            CriarRodape("ERRO: Conta não encontrada.");
            return null;
        }

        Write(" Senha: ");
        senhaDigitada = ReadLine() ?? "";
        return conta;
    }
    
    /// <summary>Localiza uma conta sem solicitar senha.</summary>
    private Conta? LocalizarContaSemSenha()
    {
        Write(" Número da Conta: ");
        if (!int.TryParse(ReadLine(), out int numeroConta))
        {
            CriarRodape("ERRO: Número de conta inválido.");
            return null;
        }

        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numeroConta);

        if (conta == null)
        {
            CriarRodape("ERRO: Conta não encontrada.");
        }
        return conta;
    }
    
    // Métodos de UI (Sem Alterações)

    private void CriarLinha()
    {
        WriteLine("-------------------------------------------------");
    }

    private void CriarTitulo(string titulo)
    {
        Clear();
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(" " + titulo);
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
    }

    private void CriarRodape(string? mensagem = null)
    {
        CriarLinha();
        ForegroundColor = ConsoleColor.Green;
        if (mensagem != null)
            WriteLine(" " + mensagem);
        Write(" ENTER para continuar");
        ForegroundColor = ConsoleColor.White;
        ReadLine();
    }
}
