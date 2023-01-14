using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Reflection.Metadata.BlobBuilder;

namespace WPF_XpathBrowser
{
    public partial class MainWindow : Window
    {
        private ConnectionViewModel model = new ConnectionViewModel();
        private ChromeDriver dr;
        private String url = "https://www.correosdemexico.gob.mx/SSLServicios/SeguimientoEnvio/Seguimiento.aspx";
        private ChromeDriverService cds = ChromeDriverService.CreateDefaultService();
        private ChromeOptions ops = new ChromeOptions();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = model;
            cds.HideCommandPromptWindow = true; 
            ops.AddArgument("headless");
            dr = new ChromeDriver(cds, ops);
            dr.Navigate().GoToUrl(url);
            lstrows.Visibility = Visibility.Hidden;
        }
        private void clear_form_Click(object sender, RoutedEventArgs e)
        {
            SearchTermTextBox.Text = string.Empty;
            cboyears.SelectedItem = DateTime.Now.Year;
            if (model.Rows != null) model.Rows.Clear();
            lstrows.Visibility = Visibility.Hidden;
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {   
            if (model.Rows != null) model.Rows.Clear();
            model.Rows = new ObservableCollection<Registro>();
            var txt = SearchTermTextBox.Text ?? "";
            if (!string.IsNullOrEmpty(txt))
            {
                var search = dr.FindElement(By.Id("Guia"));
                search.SendKeys(txt);
                var cbo = dr.FindElement(By.Id("Periodo"));
                cbo.SendKeys(cboyears.SelectedValue.ToString());
                var btn = dr.FindElement(By.Id("Busqueda"));
                btn.Click();

                bool get_headers = true;
                var rows = dr.FindElements(By.XPath("/html/body/form/main/div[1]/div[5]/div[1]/table/tbody/tr"));
                foreach (var row in rows)
                {
                    if (get_headers)
                    {
                        get_headers = false;
                        continue;
                    }
                    var linea = row.Text.Trim();
                    var cells = row.FindElements(By.TagName($"td"));
                    model.Rows.Add(new Registro
                    {
                        fecha = cells[0].Text,
                        hora = cells[1].Text,
                        origen = cells[2].Text,
                        evento = cells[3].Text,
                    });
                }
                if (model.Rows.Count > 0)
                    lstrows.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Favor de ingresar tu numero de guia");
            }
        }
    }
}
