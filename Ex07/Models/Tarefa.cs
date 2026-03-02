using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ex07.Models;
public class Tarefa : INotifyPropertyChanged
{
    public DateTime DataCriacao { get; set; } = DateTime.Now;
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
    
    private string _prioridade = "M";
    public string Prioridade
    { 
        get => _prioridade;
        set
        {
            _prioridade = value;
            OnPropertyChanged();   
        }
    }

    private DateTime? _dataConclusao;
    public DateTime? DataConclusao
    { 
        get => _dataConclusao;
        set
        {
            _dataConclusao = value;
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
            // Lógica: Se True, registra agora. Se False, limpa o registro.
            DataConclusao = value ? DateTime.Now : null;
            OnPropertyChanged();
            // Avisamos que a DataConclusao mudou para o Label atualizar também
            OnPropertyChanged(nameof(DataConclusao));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? prop = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

}