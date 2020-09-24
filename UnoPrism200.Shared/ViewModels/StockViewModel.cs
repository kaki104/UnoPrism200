using Microsoft.Toolkit.Uwp.UI;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UnoPrism200.Infrastructure.EventArgs;
using UnoPrism200.Infrastructure.Events;
using UnoPrism200.Infrastructure.Interfaces;
using UnoPrism200.Infrastructure.Models;
using Windows.UI.Xaml.Controls;

namespace UnoPrism200.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        private readonly IDalSync _dal;
        private readonly ISampleDataGenerator _sampleDataGenerator;
        private readonly IDialogService _dialogService;
        private IList<StockPrice> _stockPrices;

        public IList<StockPrice> StockPrices
        {
            get => _stockPrices;
            set => SetProperty(ref _stockPrices, value);
        }

        public ICommand AddCommand { get; set; }

        public ICommand SelectCommand { get; set; }

        private ListViewSelectionMode _selectionMode;

        public ListViewSelectionMode SelectionMode
        {
            get => _selectionMode;
            set => SetProperty(ref _selectionMode, value);
        }


        private StockPrice _selectedStock;

        public StockPrice SelectedStock
        {
            get => _selectedStock;
            set => SetProperty(ref _selectedStock, value);
        }

        public StockViewModel()
        {
            if (DesignTimeHelpers.IsRunningInApplicationRuntimeMode)
            {
                return;
            }

            StockPrices = new List<StockPrice>
                {
                    new StockPrice { Id = 1, Symbol = "MSFT", Name = "Microsoft Corp", Price = 200.00m, Change = 10.5f }
                };
            SelectedStock = StockPrices.First();
        }

        public StockViewModel(IContainerProvider containerProvider,
            IDalSync dal,
            ISampleDataGenerator sampleDataGenerator,
            IDialogService dialogService)
            : base(containerProvider)
        {
            _dal = dal;
            _sampleDataGenerator = sampleDataGenerator;
            _dialogService = dialogService;
            Init();
        }

        private void Init()
        {
            AddCommand = new DelegateCommand(OnAdd);
            SelectCommand = new DelegateCommand(OnSelect);
            StockPrices = new ObservableCollection<StockPrice>();
            EventAggregator.GetEvent<StockChangeEvent>()
                .Subscribe(ReceivedStockChange, Prism.Events.ThreadOption.UIThread, false);
        }

        private void OnSelect()
        {
            if (SelectionMode == ListViewSelectionMode.Multiple)
            {
                SelectionMode = ListViewSelectionMode.Single;
            }
            else
            {
                SelectionMode = ListViewSelectionMode.Multiple;
            }
        }

        private void OnAdd()
        {
            _sampleDataGenerator.Stop();
            //StockPrices.Clear();

            _dialogService.ShowDialog("StockControl", null,
                result =>
                {
                    if (result.Result != ButtonResult.OK) return;
                    GetStockPrices(_dal);
                    _sampleDataGenerator.Start();
                });
        }

        private void ReceivedStockChange(StockChangeEventArgs obj)
        {
            StockPrice stock = StockPrices.FirstOrDefault(s => s.Id == obj.Id);
            if (stock == null)
            {
                return;
            }

            stock.Change = obj.Change;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ApplicationCommands.SetShellCommands(checkCommand: SelectCommand);

            GetStockPrices(_dal);
            _sampleDataGenerator.Start();
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            _sampleDataGenerator.Stop();
        }

        private void GetStockPrices(IDalSync dal)
        {
            StockPrices.Clear();
            IList<Stock> stocks = dal.GetAll<Stock>();
            IEnumerable<Valuation> prices = from v0 in dal.GetTable<Valuation>()
                                            join vv in (from v1 in dal.GetTable<Valuation>()
                                                        group v1 by v1.StockId into g
                                                        select new { StockId = g.Key, MaxTime = g.Max(i => i.Time) })
                                            on new { v0.StockId, v0.Time.Ticks } equals new { vv.StockId, vv.MaxTime.Ticks }
                                            select v0;
            IEnumerable<StockPrice> stockPrices = from s in stocks
                                                  join p in prices on s.Id equals p.StockId
                                                  select new StockPrice
                                                  {
                                                      Id = s.Id,
                                                      Symbol = s.Symbol,
                                                      BasePrice = p.Price,
                                                      Price = p.Price,
                                                      Name = s.Name
                                                  };
            foreach (StockPrice item in stockPrices)
            {
                StockPrices.Add(item);
            }
        }
    }
}
