using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ex07.Models;
public class Tarefa : INotifyPropertyChanged
{
    private string _nome = String.Empty;
    public string Nome
    { 
        get => _nome;
        set
        {
            _nome = value;
            OnPropertyChanged();   
        }
    }
    private bool _concluida = false;
    public bool Concluida
    { 
        get => _concluida;
        set
        {
            _concluida = value;
            OnPropertyChanged();   
        }
    }
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? prop = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

}