using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UnoPrism200.Bases;
using UnoPrism200.Helper;
using UnoPrism200.Infrastructure.Interfaces;
using UnoPrism200.Infrastructure.Models;
using Windows.UI.Popups;

namespace UnoPrism200.ControlViewModels
{
    /// <summary>
    /// Stock ViewModel
    /// </summary>
    public class StockViewModel : DialogViewModelBase
    {
        private readonly IDalSync _dalSync;
        private readonly IUtility _utility;

        private IEnumerable _stocks;
        /// <summary>
        /// Stocks
        /// </summary>
        public IEnumerable Stocks
        {
            get => _stocks;
            set => SetProperty(ref _stocks, value);
        }

        private string _inputText;

        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
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

        public StockViewModel(IDalSync dalSync,
            IUtility utility)
            : base()
        {
            _dalSync = dalSync;
            _utility = utility;
        }

        protected override void Init()
        {
            Title = "Add stock";
            InputText = string.Empty;

            CloseCommand = new DelegateCommand(OnClose);
            AddWatchCommand = new DelegateCommand<Stock>(OnAddWatch);
            RemoveWatchCommand = new DelegateCommand<Stock>(OnRemoveWatch);

            PropertyChanged += StockViewModel_PropertyChanged;
        }

        private void OnRemoveWatch(Stock obj)
        {
            _utility.ShowConfirmSimple("Are you sure you want to delete it?",
                callback => 
                {
                    if (callback.Result != ButtonResult.OK) return;

                    _dalSync.Delete(obj);
                    SQLite.TableQuery<Valuation> vals = _dalSync.GetTable<Valuation>()
                        .Where(v => v.StockId == obj.Id);
                    foreach (Valuation item in vals)
                    {
                        _dalSync.Delete(item);
                    }
                    obj.IsRegisted = false;

                    _utility.ShowMessageSimple("Work completed");
                });
        }

        private async void OnAddWatch(Stock obj)
        {
            Random random = new Random();
            if (_dalSync.Insert(obj) != 0)
            {
                Stock newItem = _dalSync.GetTable<Stock>()
                    .First(s => s.Symbol == obj.Symbol);
                int result = _dalSync.Insert(new Valuation
                {
                    StockId = newItem.Id,
                    Price = random.Next(10, 200),
                    Time = DateTime.Now
                });
            }
            obj.IsRegisted = true;
            MessageDialog message = new MessageDialog("Work completed");
            await message.ShowAsync();
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
            List<Stock> list = new List<Stock>();
            IList<Stock> registedStocks = _dalSync.GetAll<Stock>();

            using (Stream stream = await StreamHelperEx.GetEmbeddedFileStreamAsync(GetType(), fileName))
            {
                char[] delimiter = new char[] { '\t' };
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (reader.Peek() > 0)
                    {
                        string[] items = reader.ReadLine().Split(delimiter);
                        Stock addItem = new Stock { Symbol = items[0], Name = items[1] };
                        Stock existItem = registedStocks
                            .FirstOrDefault(s => s.Symbol == addItem.Symbol);
                        if (existItem != null)
                        {
                            addItem.IsRegisted = true;
                            addItem.Id = existItem.Id;
                        }
                        list.Add(addItem);
                    }
                }
            }
            AdvancedCollectionView acv = new AdvancedCollectionView(list);
            acv.SortDescriptions.Add(new SortDescription("Symbol", SortDirection.Ascending));
            Stocks = acv;
        }

        private void StockViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
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

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetStocksAsync("NASDAQ.dat");
        }
    }
}
