using Microsoft.Toolkit.Uwp.UI;
using Prism.Common;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnoPrism200.Infrastructure.Interfaces;
using UnoPrism200.Infrastructure.Models;

namespace UnoPrism200.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        private readonly IDalSync _dal;

        private IList<StockPrice> _stockPrices;

        public IList<StockPrice> StockPrices
        {
            get { return _stockPrices; }
            set { SetProperty(ref _stockPrices ,value); }
        }


        public StockViewModel(IContainerProvider containerProvider,
            IDalSync dal) 
            : base(containerProvider)
        {
            _dal = dal;

            StockPrices = new ObservableCollection<StockPrice>();
            if(DesignTimeHelpers.IsRunningInEnhancedDesignerMode)
            {
                StockPrices.Add(new StockPrice { Id = 1, Symbol = "MSFT", Price = 200.00m, Change = 0 });
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetStockPrices(_dal);
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
                              select new StockPrice { Id = s.Id, Symbol = s.Symbol, Price = p.Price };
            foreach (var item in stockPrices)
            {
                StockPrices.Add(item);
            }
        }
    }
}
