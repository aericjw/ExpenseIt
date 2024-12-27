using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dynatrace.OneAgent.Sdk.Api;
using Dynatrace.OneAgent.Sdk.Api.Infos;
using Dynatrace.OpenKit.API;

namespace ExpenseIt
{
    /// <summary>
    /// Interaction logic for ExpenseItHome.xaml
    /// </summary>
    public partial class ExpenseItHome : Page
    {
        private ISession session;
        private IOneAgentSdk oneAgentSdk;
        public ExpenseItHome()
        {
            InitializeComponent();
            // Use existing session object and oneagentsdk initialized in App.xaml.cs
            session = (ISession)Application.Current.Properties["Session"];
            oneAgentSdk = (IOneAgentSdk)Application.Current.Properties["OneAgentSDK"];
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Register Button Click as a User Action
            IRootAction click = session.EnterAction("Click on View Expense Report");

            // Get Current Trace Context
            ITraceContextInfo traceContextInfo = oneAgentSdk.TraceContextInfo;
            string traceId = traceContextInfo.TraceId;
            string spanId = traceContextInfo.SpanId;

            // Report TraceID and SpanID as custom values on the user action
            click.ReportValue("TraceID", traceId);
            click.ReportValue("SpanID", spanId);

            // View Expense Report
            ExpenseReportPage expenseReportPage = new ExpenseReportPage(this.peopleListBox.SelectedItem);
            this.NavigationService.Navigate(expenseReportPage);

            // End User Action
            click.LeaveAction();
        }
    }
}