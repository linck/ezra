﻿using System;
using Xamarin.Forms;

namespace Ezra.Views
{
    public partial class ReportEditionPage : ContentPage
    {
        public ReportEditionPage()
        {
            InitializeComponent();
        }

        private void OnEntryFocused(Entry sender, FocusEventArgs e)
        {
            if (sender.Text == "0")
            {
                sender.Text = "";
            }
        }

        private void OnEntryUnfocused(Entry sender, FocusEventArgs e)
        {
            if (sender.Text == "")
            {
                sender.Text = "0";
            }
        }

        private void OnNumericEntryTextChanged(Entry sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(sender.Text) || int.TryParse(sender.Text, out var n))
            {
                sender.Text = e.NewTextValue;
            } else
            {
                sender.Text = e.OldTextValue;
            }
        }
    }
}
