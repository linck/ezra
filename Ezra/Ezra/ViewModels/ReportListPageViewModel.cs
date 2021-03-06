﻿using Acr.UserDialogs;
using Ezra.Data;
using Ezra.Models;
using Ezra.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ezra.ViewModels
{
    public class ReportListPageViewModel : BaseViewModel
    {
        public ObservableCollection<ReportItem> ReportItems { get; set; }
        public ReportItemDatabase ReportItemDatabase { get; set; }

        private DateTime dateControl = DateTime.Now;
        public DateTime DateControl
        {
            get { return dateControl; }
            set
            {
                SetProperty(ref dateControl, value);
                FormatTitle();
                Load();
            }
        }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddCommand { get; set; }


        public ReportListPageViewModel(INavigationService navigationService, 
            IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            FormatTitle();
            ReportItems = new ObservableCollection<ReportItem>();
            ReportItemDatabase = new ReportItemDatabase();
            EditCommand = new DelegateCommand<object>(EditCommandExecute);
            DeleteCommand = new DelegateCommand<object>(DeleteCommandExecute);
            AddCommand = new DelegateCommand(AddCommandExecute);
        }

        private async void DeleteCommandExecute(object id)
        {
            var answer = await DialogService.DisplayAlertAsync($"Remover Relatório", "Gostaria realmente remover?", "Sim", "Não");
            if(answer)
            {
                ReportItemDatabase.Delete((int) id);
                Load();
            }
        }

        private void EditCommandExecute(object reportItem)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(EzraConstants.REPORT_ITEM_NAV_PARAM, (ReportItem) reportItem);
            navigationParams.Add(EzraConstants.IS_EDITING_NAV_PARAM, true);
            NavigationService.NavigateAsync(EzraConstants.REPORT_EDITION_PAGE, navigationParams);
        }

        private void AddCommandExecute()
        {
            NavigationService.NavigateAsync(EzraConstants.REPORT_EDITION_PAGE);
        }

        public async void Load()
        {
            var dialogs = UserDialogs.Instance;
            dialogs.ShowLoading();
            await Task.Delay(500);
            ReportItems.Clear();
            List<ReportItem> items = ReportItemDatabase.ListReportsOrdered(DateControl.Month, DateControl.Year);
            foreach (var item in items)
            {
                ReportItems.Add(item);
            }
            dialogs.HideLoading();
        }

        private void FormatTitle()
        {
            var formatedMonthTitle = String.Format("{0:MMM yyyy}", DateControl);
            Title = formatedMonthTitle.Substring(0, 1).ToUpper() + formatedMonthTitle.Substring(1);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey(EzraConstants.DATE_CONTROL_NAV_PARAM))
            {
                DateControl = (DateTime)parameters[EzraConstants.DATE_CONTROL_NAV_PARAM];
            }
            FormatTitle();
            Load();
        }
    }

    public class SourceMock
    {
        public SourceMock(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
}
