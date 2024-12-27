using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Dynatrace.OpenKit.API;
using Dynatrace.OpenKit;
using Dynatrace.OneAgent.Sdk.Api;

namespace ExpenseIt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IOpenKit openKit;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // CUSTOM TRACING INITIALIZATION

            // Create an instance of the OneAgentSDK
            IOneAgentSdk oneAgentSdk = OneAgentSdkFactory.CreateInstance();

            // Store the OneAgentSDK object as an application property to be used application-wide
            this.Properties["OneAgentSDK"] = oneAgentSdk;

            // REAL USER MONITORING INITIALIZATION

            string applicationID = "5e833d73-c5e2-4a2c-95dc-4448939f9d85";                  // Your application's ID
            long deviceID = 42L;                                                            // Replace with a unique value per device/installation
            string endpointURL = "https://bf08595vkm.bf-sprint.dynatracelabs.com/mbeacon";  // Dynatrace endpoint URL

            // Create an instance of OpenKit to be used within the class for initialization and shutdown procedures
            openKit = new DynatraceOpenKitBuilder(endpointURL, applicationID, deviceID)
                .WithApplicationVersion("1.0.0.0")
                .WithOperatingSystem("Windows Server 2019")
                .WithManufacturer("Microsoft")
                .WithModelId("20191226")
                .Build();

            // Wait up to 10 seconds for OpenKit to complete initialization
            int timeoutInMilliseconds = 10 * 1000;
            bool success = openKit.WaitForInitCompletion(timeoutInMilliseconds);

            // Create User Session Object and store as an application property to be used application-wide
            ISession session = openKit.CreateSession();
            this.Properties["Session"] = session;

            // Identify user
            session.IdentifyUser("aeric.walls@dynatrace.com");
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // End the current session and openkit instance
            ISession session = (ISession) this.Properties["Session"];
            session.End();
            openKit.Shutdown();
        }
    }
}
