using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uno.Extensions;
using Uno.Extensions.Specialized;
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

        private IEnumerable _stocks;
        /// <summary>
        /// Stocks
        /// </summary>
        public IEnumerable Stocks
        {
            get { return _stocks; }
            set { SetProperty(ref _stocks ,value); }
        }

        private string _inputText;

        public string InputText
        {
            get { return _inputText; }
            set { SetProperty(ref _inputText ,value); }
        }

        public ICommand CloseCommand { get; set; }

        public ICommand AddWatchCommand { get; set; }

        public ICommand RemoveWatchCommand { get; set; }

        public StockViewModel()
        {
            if (DesignTimeHelpers.IsRunningInApplicationRuntimeMode)
            {
                return;
            }

            Stocks = new List<Stock>
                {
                    new Stock { Symbol = "AACG", Name = "Ata Creativity Global" }
                };
        }

        public StockViewModel(IDalSync dalSync)
        {
            _dalSync = dalSync;
            Init();
        }

        private async void Init()
        {
            Title = "Add stock";
            InputText = string.Empty;
            
            CloseCommand = new DelegateCommand(OnClose);
            AddWatchCommand = new DelegateCommand(OnAddWatch);
            RemoveWatchCommand = new DelegateCommand(OnRemoveWatch);

            await SetStocksAsync("NASDAQ.dat");

            PropertyChanged += StockViewModel_PropertyChanged;
        }

        private void OnRemoveWatch()
        {
            
        }

        private void OnAddWatch()
        {
            
        }

        private void OnClose()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        public override void OnDialogClosed()
        {
            ((AdvancedCollectionView)Stocks).Clear();
        }

        private async Task SetStocksAsync(string fileName)
        {
            var list = new List<Stock>();
            var registedStocks = _dalSync.GetAll<Stock>();

            using (var stream = await StreamHelperEx.GetEmbeddedFileStreamAsync(GetType(), fileName))
            {
                char[] delimiter = new char[] { '\t' };
                using (var reader = new StreamReader(stream))
                {
                    while (reader.Peek() > 0)
                    {
                        var items = reader.ReadLine().Split(delimiter);
                        var addItem = new Stock { Symbol = items[0], Name = items[1] };
                        if (registedStocks.Any(s => s.Symbol == addItem.Symbol))
                        {
                            addItem.IsRegisted = true;
                        }
                        list.Add(addItem);
                    }
                }
            }
            var acv = new AdvancedCollectionView(list);
            acv.SortDescriptions.Add(new SortDescription("Symbol", SortDirection.Ascending));
            Stocks = acv;
        }

        private void StockViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(InputText):
                    ((AdvancedCollectionView)Stocks).ClearObservedFilterProperties();
                    if (InputText.Length > 0)
                    {
                        ((AdvancedCollectionView)Stocks).Filter =
                            x => ((Stock)x).Symbol.Contains(InputText, StringComparison.OrdinalIgnoreCase)
                                || ((Stock)x).Name.Contains(InputText, StringComparison.OrdinalIgnoreCase);
                    }
                    break;
            }
        }
    }
}
