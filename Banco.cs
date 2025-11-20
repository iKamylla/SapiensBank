using System.Text.Json;
using System.Collections.Generic; 
using System.IO;               
using System;           

public class Banco
{
    public List<Conta> Contas { get; set; } = new List<Conta>();

    public Banco()
    {
        GetContas();
    }

    public void GetContas()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var fullPath = Path.Combine(path, "SapiensBank", "banco.json");
        
        if (File.Exists(fullPath))
        {
            var json = File.ReadAllText(fullPath);
        
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
        
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var fullPath = Path.Combine(directoryPath, "banco.json");
        var options = new JsonSerializerOptions { WriteIndented = true };
        
        var json = JsonSerializer.Serialize(Contas, options);
        File.WriteAllText(fullPath, json);
    }
}
