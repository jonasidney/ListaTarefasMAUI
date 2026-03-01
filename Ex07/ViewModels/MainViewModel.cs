namespace Ex07.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Text.Json;
using Microsoft.Maui.Storage;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<string> Tarefas { get; set; }

    private string _novaTarefa;
    public string NovaTarefa
    {
        get => _novaTarefa;
        set
        {
            _novaTarefa = value;
            OnPropertyChanged();
        }
    }

    public ICommand AdicionarTarefaCommand { get; }

    public ICommand RemoverTarefaCommand { get; }

    public MainViewModel()
    {
        Tarefas = new ObservableCollection<string>();

        CarregarTarefas();

        AdicionarTarefaCommand = new Command(ExecutarAdicionarTarefa);

        RemoverTarefaCommand = new Command<string>(ExecutarRemoverTarefa);
    }
    
    private void ExecutarAdicionarTarefa()
    {    
        if (!string.IsNullOrWhiteSpace(NovaTarefa))
        {
            Tarefas.Add(NovaTarefa);
            NovaTarefa = string.Empty;

            SalvarTarefas();
        }
        
    }

    private void ExecutarRemoverTarefa(string tarefaParaRemover)
    {
        if (Tarefas.Contains(tarefaParaRemover))
        {
            Tarefas.Remove(tarefaParaRemover);
            SalvarTarefas();
        }
    }

    private void SalvarTarefas()
    {
        string json = JsonSerializer.Serialize(Tarefas);
        Preferences.Default.Set("lista_tarefas", json);
    }

    private void CarregarTarefas()
    {
        string json = Preferences.Default.Get("lista_tarefas", "[]");

        var tarefasSalvas = JsonSerializer.Deserialize<ObservableCollection<string>>(json);

        if (tarefasSalvas != null)
        {
            Tarefas.Clear();
            foreach (var tarefa in tarefasSalvas)
            {
                Tarefas.Add(tarefa);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string prop = null) 
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    
}