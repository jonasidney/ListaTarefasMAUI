namespace Ex07.ViewModels;

using Ex07.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Text.Json;
using Microsoft.Maui.Storage;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Tarefa> Tarefas { get; set; }
    public ObservableCollection<Tarefa> TarefasFiltradas {get; set;}

    private string _filtro;
    public string Filtro
    {
        get => _filtro;
        set
        {
            _filtro = value;
            OnPropertyChanged();
            ExecutarFiltrarTarefa();
        }
    }
    private string _novaTarefa = string.Empty;
    public string NovaTarefa
    {
        get => _novaTarefa;
        set
        {
            _novaTarefa = value;
            OnPropertyChanged();
        }
    }

    private string _prioridadeSelecionada = "M";
    public string PrioridadeSelecionada
    {
        get => _prioridadeSelecionada;
        set
        {
            _prioridadeSelecionada = value;
            OnPropertyChanged();
        }
    }

    public ICommand AdicionarTarefaCommand { get; }
    public ICommand FiltrarTarefaCommand { get; }
    public ICommand RemoverTarefaCommand { get; }

    public MainViewModel()
    {
        Tarefas = new ObservableCollection<Tarefa>();
        TarefasFiltradas = new ObservableCollection<Tarefa>();

        CarregarTarefas();

        AdicionarTarefaCommand = new Command(ExecutarAdicionarTarefa);
        
        FiltrarTarefaCommand = new Command(ExecutarFiltrarTarefa);

        RemoverTarefaCommand = new Command<Tarefa>(ExecutarRemoverTarefa);
    }
    
    private void ExecutarAdicionarTarefa()
    {    
        if (!string.IsNullOrWhiteSpace(NovaTarefa))
        {
            var nova = new Tarefa
            { 
                Nome = NovaTarefa,
                Prioridade = PrioridadeSelecionada,
                DataCriacao = DateTime.Now
            };
            // 1. Assina o evento de mudança de propriedade da tarefa para salvar as mudanças automaticamente
            nova.PropertyChanged += (s, e) => SalvarTarefas();

            Tarefas.Add(nova);
            
            NovaTarefa = string.Empty;
            PrioridadeSelecionada = "M";

            ExecutarFiltrarTarefa();
            SalvarTarefas();
        }
        
    }

    private void ExecutarFiltrarTarefa()
    {
        if (string.IsNullOrWhiteSpace(Filtro))
        {
            ExecutarAtualizarListaTarefa(Tarefas);
        }
        else
        {
            var filtradas = Tarefas.Where(t => t.Nome.ToLower().Contains(Filtro.ToLower())).ToList();
            ExecutarAtualizarListaTarefa(filtradas);
        }
    }

    private void ExecutarAtualizarListaTarefa(IEnumerable<Tarefa> tarefas)
    {
        TarefasFiltradas.Clear();

        foreach (var t in tarefas) TarefasFiltradas.Add(t);
    }

    private void ExecutarRemoverTarefa(Tarefa tarefaParaRemover)
    {
        if (tarefaParaRemover != null)
        {
            Tarefas.Remove(tarefaParaRemover);
            
            SalvarTarefas();

            ExecutarFiltrarTarefa();
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

        try
        {
            var tarefasSalvas = JsonSerializer.Deserialize<ObservableCollection<Tarefa>>(json);

            if (tarefasSalvas != null)
            {
                Tarefas.Clear();
                foreach (var tarefa in tarefasSalvas)
                {
                    // 1. Assina o evento de mudança de propriedade da tarefa para salvar as mudanças automaticamente
                    tarefa.PropertyChanged += (s, e) => SalvarTarefas();
                    Tarefas.Add(tarefa);
                }
                
                ExecutarAtualizarListaTarefa(Tarefas);
            }

        }
        catch (JsonException)
        {
            // Se o JSON antigo era de strings e o novo é de objetos, pode dar erro na primeira vez.
            // Se isso acontecer, limpamos tudo para começar do zero com o novo formato.
            Preferences.Default.Remove("lista_tarefas");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string prop = null) 
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    
}