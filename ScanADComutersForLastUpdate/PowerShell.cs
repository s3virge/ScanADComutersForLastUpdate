
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;

namespace ScanADComutersForLastUpdate {
    class PS {
        public static string GetLastUpdate(string host) {
            string result = "Failed";

            using (PowerShell PowerShellInstance = PowerShell.Create()) {
                // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
                PowerShellInstance.AddScript("Invoke-Command -ComputerName " + host + " -ScriptBlock { (New-Object -com 'Microsoft.Update.AutoUpdate').Results.LastInstallationSuccessDate}");
                try {
                    Collection<PSObject> obj = PowerShellInstance.Invoke();
                    //result = $"{host,-20} {obj[0]}";
                    result = $"{obj[0]}";
                    Debug.WriteLine(result);
                }
                catch (Exception e){
                    Debug.WriteLine(e.Message);
                }
                
                return result;
            }
        }
    }
}
