using System.Text.Json;
using System.Collections.Generic; // Garante que a lista funcione
using System.IO;                  // Necessário para File e Path
using System;                     // Necessário para Environment

public class Banco
{
    public List<Conta> Contas { get; set; } = new List<Conta>();

    public Banco()
    {
        GetContas();
    }

    public void GetContas()
    {
        // Define o caminho completo para o arquivo JSON
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var fullPath = Path.Combine(path, "SapiensBank", "banco.json");
        
        if (File.Exists(fullPath))
        {
            var json = File.ReadAllText(fullPath);
            // O Deserialize precisa encontrar as definições de Conta e suas propriedades
            var contas = JsonSerializer.Deserialize<List<Conta>>(json); 
            if (contas != null)
            {
                Contas = contas;
            }
        }
    } 

    public void SaveContas()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var directoryPath = Path.Combine(path, "SapiensBank");
        
        // Cria o diretório se ele não existir
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var fullPath = Path.Combine(directoryPath, "banco.json");
        var options = new JsonSerializerOptions { WriteIndented = true };
        
        // Serializa a lista de contas para JSON
        var json = JsonSerializer.Serialize(Contas, options);
        File.WriteAllText(fullPath, json);
    }
}
