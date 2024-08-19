using AppServices;
using Prism.Events;
using Setup.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Setup.Views
{
    /// <summary>
    /// Interaction logic for SetUpView.xaml
    /// </summary>
    public partial class SetUpView : UserControl
    {

        private Prism.Events.IEventAggregator _eventAggregator;

 
        public SetUpView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this._eventAggregator = eventAggregator;
            this.DataContext = new SetUpViewModel(_eventAggregator);

        }
    }
}
