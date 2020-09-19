using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnoPrism200.Infrastructure.Events;
using UnoPrism200.Infrastructure.Interfaces;
using UnoPrism200.Infrastructure.Models;

namespace UnoPrism200.Infrastructure.Services
{
    public class SampleDataGenerator : ISampleDataGenerator
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDalSync _dal;
        private IList<Stock> _stocks;
        private bool _isWork;

        public SampleDataGenerator(IEventAggregator eventAggregator,
            IDalSync dalSync)
        {
            _eventAggregator = eventAggregator;
            _dal = dalSync;
        }

        private void InitStocks()
        {
            if (_stocks != null)
            {
                _stocks.Clear();
            }
            _stocks = _dal.GetAll<Stock>();
        }

        public void Start()
        {
            if (_isWork) return;
            InitStocks();
            _isWork = true;
            DataGeneration();
        }

        private async void DataGeneration()
        {
            var random = new Random();

            while (_isWork)
            {
                var index = random.Next(0, _stocks.Count);
                var change = random.NextDouble() * 100;
                var sign = random.Next(0, 10);
                if (sign % 3 == 0)
                {
                    change = change * -1;
                }
                _eventAggregator.GetEvent<StockChangeEvent>()
                    .Publish(new EventArgs.StockChangeEventArgs
                    {
                        Id = _stocks[index].Id,
                        Change = Convert.ToSingle(change)
                    });
                await Task.Delay(100);
            }
        }

        public void Stop()
        {
            _isWork = false;
        }
    }
}
