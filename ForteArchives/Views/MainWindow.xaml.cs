using AppServices;
using System.Windows;

namespace ForteArchives.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double wdCoef = 0.0;

        private double IScreenWidth { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GridView_sidechanged(object sender, SizeChangedEventArgs e)
        {
            IScreenWidth = e.NewSize.Width + 10;
            wdCoef = e.NewSize.Width * 0.08;

            double dxGvHdrSize = e.NewSize.Width * .029;
            double dxGvRwHeight = e.NewSize.Width * .025;
            double dxGvFontSz = e.NewSize.Width * .012;
            double dCmbHeight = e.NewSize.Width * .02;
           
            RTGridView.FontSize = dxGvFontSz;
            RTGridView.ColumnHeaderHeight = dxGvHdrSize;
            RTGridView.RowHeight = dxGvRwHeight;
            RTGridView.UpdateLayout();
        }

        private void OnAutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            RTGridView.Columns[0].Visibility = Visibility.Collapsed;

            if (e.PropertyName.Equals("Moisture"))
            {
                e.Column.Header = ClassCommon.MoistureUnitLst[ClassCommon.MoistureType];
            }
            if (e.PropertyName.Equals("Weight"))
            {
                e.Column.Header = ClassCommon.WeightTypeLst[ClassCommon.WeightUnit];
            }

            if (e.PropertyName.Equals("Deviation"))
                e.Column.Header = "%CV";

            if (e.PropertyName.Equals("Finish"))
                e.Column.Header = "Viscosity";

            if (e.PropertyName.Equals("FC_LotIdentString"))
                e.Column.Header = "CusLotNumber";

            if (e.PropertyName.Equals("CalibrationName"))
                e.Column.Header = "Calibration";

            if (ClassCommon.WLOptions)
            {
                if (e.PropertyName.Equals("SpareSngFld3"))
                    e.Column.Header = "%CV";
            }
            if ((e.PropertyType == typeof(System.Single)) || (e.PropertyType == typeof(System.Double)))
            {
                e.Column.ClipboardContentBinding.StringFormat = "{0:0.##}";
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef;
                //e.Column.Width = 110;
            }
            else if (e.PropertyType == typeof(System.DateTime))
            {
                e.Column.ClipboardContentBinding.StringFormat = "MM-dd-yyyy HH:mm";
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef * 1.2;
            }
            else
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef;

            RTGridView.SelectedIndex = 0;
            RTGridView.Focus();
        }

    
    }
}
