using Microsoft.Toolkit.Uwp.UI;
using Prism.Commands;
using Prism.Common;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        private IList<StockPrice> _stockPrices;

        public IList<StockPrice> StockPrices
        {
            get { return _stockPrices; }
            set { SetProperty(ref _stockPrices ,value); }
        }

        public ICommand AddCommand { get; set; }

        public ICommand SelectCommand { get; set; }

        private ListViewSelectionMode _selectionMode;

        public ListViewSelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set { SetProperty(ref _selectionMode ,value); }
        }


        private StockPrice _selectedStock;

        public StockPrice SelectedStock
        {
            get { return _selectedStock; }
            set { SetProperty(ref _selectedStock ,value); }
        }

        public StockViewModel()
        {
            if (DesignTimeHelpers.IsRunningInApplicationRuntimeMode) return;

            StockPrices = new List<StockPrice>
                {
                    new StockPrice { Id = 1, Symbol = "MSFT", Name = "Microsoft Corp", Price = 200.00m, Change = 10.5f }
                };
            SelectedStock = StockPrices.First();
        }

        public StockViewModel(IContainerProvider containerProvider,
            IDalSync dal,
            ISampleDataGenerator sampleDataGenerator) 
            : base(containerProvider)
        {
            _dal = dal;
            _sampleDataGenerator = sampleDataGenerator;
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
            if(SelectionMode == ListViewSelectionMode.Multiple)
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
        }

        private void ReceivedStockChange(StockChangeEventArgs obj)
        {
            var stock = StockPrices.FirstOrDefault(s => s.Id == obj.Id);
            if (stock == null) return;
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
            var stocks = dal.GetAll<Stock>();
            var prices = from v0 in dal.GetTable<Valuation>()
                         join vv in (from v1 in dal.GetTable<Valuation>()
                                     group v1 by v1.StockId into g
                                     select new { StockId = g.Key, MaxTime = g.Max(i => i.Time) })
                         on new { v0.StockId, v0.Time.Ticks } equals new { vv.StockId, vv.MaxTime.Ticks }
                         select v0;
            var stockPrices = from s in stocks
                              join p in prices on s.Id equals p.StockId
                              select new StockPrice 
                              { 
                                  Id = s.Id, Symbol = s.Symbol, Price = p.Price,
                                  Name = s.Name
                              };
            foreach (var item in stockPrices)
            {
                StockPrices.Add(item);
            }
        }
    }
}
