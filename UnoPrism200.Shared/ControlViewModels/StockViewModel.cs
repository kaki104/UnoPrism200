using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using Uno.Extensions;
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
            var list = new List<Stock>();
            using (var stream = await StreamHelperEx.GetEmbeddedFileStreamAsync(GetType(), "NASDAQ.dat"))
            {
                char[] delimiter = new char[] { '\t' };
                using(var reader = new StreamReader(stream))
                {
                    while(reader.Peek() > 0)
                    {
                        var items = reader.ReadLine().Split(delimiter);
                        list.Add(new Stock { Symbol = items[0], Name = items[1] });
                    }
                }
            }
            var acv = new AdvancedCollectionView(list);
            acv.SortDescriptions.Add(new SortDescription("Symbol", SortDirection.Ascending));
            Stocks = acv;

            PropertyChanged += StockViewModel_PropertyChanged;
        }

        private void StockViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(InputText):
                    Debug.WriteLine(InputText);
                    if(InputText.Length > 1)
                    {
                        ((AdvancedCollectionView)Stocks).Filter = null;
                        ((AdvancedCollectionView)Stocks).Filter = 
                            x => ((Stock)x).Symbol.Contains(InputText, StringComparison.OrdinalIgnoreCase)
                                || ((Stock)x).Name.Contains(InputText, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        ((AdvancedCollectionView)Stocks).Filter = null;
                    }
                    break;
            }
        }
    }
}
