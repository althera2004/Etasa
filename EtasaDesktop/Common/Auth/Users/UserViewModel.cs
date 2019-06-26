using EtasaDesktop.Common;
using EtasaDesktop.Common.Tools;
using EtasaDesktop.Distribution.Clients;
using EtasaDesktop.Distribution.Orders.Imports;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows;
using System.Windows.Input;


namespace EtasaDesktop.Common.Auth.Users
{
    class UserViewModel : ViewModelBase
    {
        private UserDataSet.System_UserSummariesRow _selectedUser;


        public UserDataSet.System_UserSummariesRow SelectedUser
        {
            get => _selectedUser;
            set
            {
                Set(ref _selectedUser, value);
            }
        }


        public ObservableCollection<UserDataSet.System_UserSummariesRow> Users { get; private set; }

        public UserViewModel()
        {
            Users = new ObservableCollection<UserDataSet.System_UserSummariesRow>();
            Refresh();
        }


        public void Refresh()
        {

            Users.Clear();

            UserDataSet ds = new UserDataSet();
            UserDataSetTableAdapters.System_UserSummariesTableAdapter adapt = new UserDataSetTableAdapters.System_UserSummariesTableAdapter();
           
               adapt.Fill(ds.System_UserSummaries);

            foreach (UserDataSet.System_UserSummariesRow row in ds.System_UserSummaries.Rows)
            {
                Users.Add(row);
            }

        }
    }
}
