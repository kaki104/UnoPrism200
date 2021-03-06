﻿using Prism.Mvvm;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.Models
{
    /// <summary>
    /// Stock sqlite entity
    /// </summary>
    public class Stock : BindableBase
    {
        private bool isRegisted;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        [Ignore]
        public bool IsRegisted 
        { 
            get => isRegisted; 
            set => SetProperty(ref isRegisted ,value); 
        }
    }
}
