using System.Text.Json.Serialization;

public class Conta
{
    public int Numero { get; set; }
    public string Cliente { get; set; }
    public string Cpf { get; set; }
    public string Senha { get; set; }
    public decimal Saldo { get; set; } 
    public decimal Limite { get; set; }

    [JsonIgnore]
    public decimal SaldoDisponível => Saldo + Limite;

    public Conta(int numero, string cliente, string cpf, string senha, decimal limite = 0)
    {
        Numero = numero;
        Cliente = cliente;
        Cpf = cpf;
        Senha = senha;
        Limite = limite;
        // O saldo será carregado do JSON ou será 0 por padrão
    }

    // Métodos de negócios adicionados

    /// <summary>Efetua um saque na conta.</summary>
    /// <returns>Retorna <c>true</c> se o saque foi bem-sucedido.</returns>
    public bool Sacar(decimal valor)
    {
        if (valor > 0 && SaldoDisponível >= valor)
        {
            Saldo -= valor;
            return true;
        }
        return false;
    }

    /// <summary>Efetua um depósito na conta.</summary>
    public void Depositar(decimal valor)
    {
        if (valor > 0)
        {
            Saldo += valor;
        }
    }

    /// <summary>Aumenta o limite de crédito da conta.</summary>
    public void AumentarLimite(decimal novoLimite)
    {
        if (novoLimite > Limite)
        {
            Limite = novoLimite;
        }
    }

    /// <summary>Diminui o limite de crédito da conta.</summary>
    /// <returns>Retorna <c>true</c> se o limite foi diminuído com sucesso.</returns>
    public bool DiminuirLimite(decimal novoLimite)
    {
        // Verifica se a redução do limite não deixa o saldo disponível negativo
        if (novoLimite < Limite && Saldo + novoLimite >= 0)
        {
            Limite = novoLimite;
            return true;
        }
        return false;
    }
}
