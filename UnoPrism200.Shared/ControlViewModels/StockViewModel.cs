using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnoPrism200.Bases;
using UnoPrism200.Helper;
using UnoPrism200.Helpers;
using UnoPrism200.Infrastructure.Interfaces;
using UnoPrism200.Infrastructure.Models;
using Windows.Storage;

namespace UnoPrism200.ControlViewModels
{
    /// <summary>
    /// Stock ViewModel
    /// </summary>
    public class StockViewModel : DialogViewModelBase
    {
        private readonly IDalSync _dalSync;

        private IAdvancedCollectionView _stocks;
        /// <summary>
        /// Stocks
        /// </summary>
        public IAdvancedCollectionView Stocks
        {
            get { return _stocks; }
            set { SetProperty(ref _stocks ,value); }
        }

        public StockViewModel()
        {
            if (DesignTimeHelpers.IsRunningInApplicationRuntimeMode)
            {
                return;
            }

            var list = new List<StockExchange>
                {
                    new StockExchange { Code = "AACG", Name = "Ata Creativity Global", Close = 1.06, Volume = "12,200" }
                };
            Stocks = new AdvancedCollectionView(list);
        }

        public StockViewModel(IDalSync dalSync)
        {
            _dalSync = dalSync;
            Init();
        }

        private async void Init()
        {
            Title = "Add stock";

            string jsonText = string.Empty;
            //var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //var file = await folder.GetFileAsync("/Assets/NASDAQStockExchage.json");
            //var jsonText = File.ReadAllText($"{path}/Assets/NASDAQStockExchage.json");
            using (var stream = await StreamHelperEx.GetEmbeddedFileStreamAsync(GetType(), "NASDAQStockExchage.json"))
            {
                byte[] bytes = new byte[stream.Length];
                var result = await stream.ReadAsync(bytes, 0, bytes.Length);
                jsonText = Encoding.UTF8.GetString(bytes);
            }
            var allStocks = await Json.ToObjectAsync<IList<StockExchange>>(jsonText);
            Stocks = new AdvancedCollectionView(allStocks as IList);
        }
    }
}
