using AppServices;
using AppServices.Control;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.ViewModels
{
    public class SetUpViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;

        private readonly ClassSqlHandler sqlHandler;

        private string _host;
        public string Host
        {
            get { return _host; }
            set { SetProperty(ref _host, value); }
        }

        private string _instance;
        public string Instant
        {
            get { return _instance; }
            set { SetProperty(ref _instance, value); }
        }

        private List<string> _servercomboList = new List<string>();
        public List<string> ServercomboList
        {
            get { return _servercomboList; }
            set { SetProperty(ref _servercomboList, value); }
        }

        private bool _SearchDone = false;
        public bool SearchDone
        {
            get { return _SearchDone; }
            set { SetProperty(ref _SearchDone, value); }
        }

        private bool _bModify = false;
        public bool BModify
        {
            get { return _bModify; }
            set { SetProperty(ref _bModify, value); }
        }

        private bool _blocal;
        public bool BLocal
        {
            get => _blocal;
            set
            {
                SetProperty(ref _blocal, value);
                BRemote = (value == true) ? false : true;
            }
        }
        private bool _bremote;
        public bool BRemote
        {
            get => _bremote;
            set => SetProperty(ref _bremote, value);
        }

        private bool _testGood = false;
        public bool TestGood
        {
            get { return _testGood; }
            set { SetProperty(ref _testGood, value); }
        }


        private string _database;
        public string Database
        {
            get { return _database; }
            set { SetProperty(ref _database, value); }
        }

        private string _userid = "forte";
        public string Userid
        {
            get { return _userid; }
            set { SetProperty(ref _userid, value); }
        }

        private string _password = "etrof";
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public int MoistureTypeSelect { get; set; }

        private bool _mcChecked;
        public bool MCChecked
        {
            get { return _mcChecked; }
            set
            {
                if (value) MoistureTypeSelect = 0;
                SetProperty(ref _mcChecked, value);
            }
        }
        private bool _mrChecked;
        public bool MRChecked
        {
            get { return _mrChecked; }
            set
            {
                if (value) MoistureTypeSelect = 1;
                SetProperty(ref _mrChecked, value);
            }
        }
        private bool _adChecked;
        public bool ADChecked
        {
            get { return _adChecked; }
            set
            {
                if (value) MoistureTypeSelect = 2;
                SetProperty(ref _adChecked, value);
            }
        }

        private bool _bdChecked;
        public bool BDChecked
        {
            get { return _bdChecked; }
            set
            {
                if (value) MoistureTypeSelect = 3;
                SetProperty(ref _bdChecked, value);
            }
        }

        private bool _kgChecked;
        public bool KGChecked
        {
            get { return _kgChecked; }
            set { SetProperty(ref _kgChecked, value); }
        }

        private bool _lbChecked;
        public bool LBChecked
        {
            get { return _lbChecked; }
            set { SetProperty(ref _lbChecked, value); }
        }

        private string _customerName; // = ClassCommon.CustomerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                SetProperty(ref _customerName, value);
                //  if (value != null) ClassCommon.CustomerName = value;
            }
        }


        private int _selectedServerCombo;
        public int SelectedServerCombo
        {
            get => _selectedServerCombo;
            set
            {
                SetProperty(ref _selectedServerCombo, value);
                char[] separators = { '\\' };  //Host\\Instant
                string strNewHost = ServercomboList[SelectedServerCombo].ToString();
                string[] words = strNewHost.Split(separators);
                Host = words[0];
                Instant = words[1];
                BLocal = (Host == Environment.MachineName) ? true : false;
            }
        }

        private string _testConStatus = string.Empty;
        public string TestConStatus
        {
            get { return _testConStatus; }
            set { SetProperty(ref _testConStatus, value); }
        }


        public SetUpViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;


            sqlHandler = ClassSqlHandler.Instance;
            Host = sqlHandler.LocalWorkStationID;
            Instant = sqlHandler.SqlInstance;

            switch (sqlHandler.MoistureUnit)
            {
                case 0:
                    MCChecked = true;
                    break;
                case 1:
                    MRChecked = true;
                    break;
                case 2:
                    ADChecked = true;
                    break;
                case 3:
                    BDChecked = true;
                    break;
                default:
                    break;
            }

            if (sqlHandler.WeightUnit == 0)
            {
                KGChecked = true;
            }
            else
                LBChecked = true;
        }

        public DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand =>
        _searchCommand ?? (_searchCommand =
          new DelegateCommand(SearchCommandExecute).ObservesCanExecute(() => BModify).ObservesProperty(() => SearchDone));
        private void SearchCommandExecute()
        {
            SearchSqlServers();
        }

        private async void SearchSqlServers()
        {
            LoadingWindow? tempWindow = new LoadingWindow();
            tempWindow.Show();

            try
            {
                DataTable? dt = await sqlHandler.GetServers();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["InstanceName"].ToString() == "")
                            ServercomboList.Add(row["ServerName"].ToString());
                        else
                            ServercomboList.Add(row["ServerName"].ToString() + @"\" + row["InstanceName"].ToString());
                    }

                    SearchDone = true;
                    //SQLStatusMsg = "Search Done!";
                    SelectedServerCombo = 1;
                    SearchDone = true;
                }
                dt = null;
            }
            catch (Exception ex)
            {
                ClsSerilog.LogMessage(ClsSerilog.ERROR, $"ERROR in SearchSQLServer <> {ex.Message}");
            }
            tempWindow.Close();
            if (tempWindow != null) tempWindow = null;
        }


        public DelegateCommand _settingsCommand;
        public DelegateCommand SettingsCommand =>
        _settingsCommand ?? (_settingsCommand =
          new DelegateCommand(SettingsCommandExecute));
        private void SettingsCommandExecute()
        {
            BModify = true;
        }

        public DelegateCommand _testSQLCommand;
        public DelegateCommand TestSQLCommand =>
        _testSQLCommand ?? (_testSQLCommand =
          new DelegateCommand(TestSQLCommandExecute).ObservesProperty(() => TestGood).ObservesCanExecute(() => SearchDone)); //TestGood

        private void TestSQLCommandExecute()
        {
            TestGood = sqlHandler.TestSqlConnection(Host, Instant, Database, Userid, Password);
            TestConStatus = (TestGood == true) ? "Test Passed" : "Test Fail";
        }

        public DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
        _saveCommand ?? (_saveCommand =
          new DelegateCommand(SaveCommandExecute).ObservesCanExecute(() => BModify));

        private void SaveCommandExecute()
        {
            BModify = false;
            sqlHandler.WeightUnit = (KGChecked) ? 0 : 1;
            sqlHandler.MoistureUnit = MoistureTypeSelect;

            if (TestGood == true)
            {
                sqlHandler.LocalWorkStationID = Host;
                sqlHandler.SqlInstance = Instant;
            }

          //  _eventAggregator.GetEvent<CloseSetUpWindow>().Publish(true);
        }
    }

}
