using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScanADComutersForLastUpdate {
    class Main { 
        ArrayList _allCompInfo = new ArrayList();
        public static object lockObj = new object();

        public void Run() {
            Console.WriteLine("Retrieving list of hosts");
            ArrayList computers = ActiveDirectory.GetListOfComputers();
            List<Task> tasks = new List<Task>();

            Console.Clear();

            int c = 0;
            foreach (string computer in computers) {
                tasks.Add(Task.Factory.StartNew(
                    () => {
                        Do(computer);
                    }));
                //if (c >= 9) break;
                c++;
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine();
            string file = "ADComputersLastUpdate.csv";
            File.WriteToCSV(_allCompInfo, file);
            Console.WriteLine($"Press enter key for exit");
            Console.ReadLine();
        }

        public void Do(string comp) {
            string cn = ActiveDirectory.GetCanonicalName(comp);
            cn = cn.Replace($"/{comp}", "");

            if (Check.IsMachineOnline(comp) == false) {
                _allCompInfo.Add($"{comp};;; unreachable; {cn}");
                //lock (lockObj) {
                //     Console.ForegroundColor = ConsoleColor.Red;
                     Console.WriteLine($"{comp,-20} unreachable");
                //     Console.ForegroundColor = ConsoleColor.White;
                //}
               
                return;
            }

            //_allCompInfo.Add(PS.GetLastUpdate(comp));
            if (PS.GetLastUpdate(comp).Equals("Failed")) {
                _allCompInfo.Add($"{comp};;; failed; {cn}");
                //lock (lockObj) {
                //    Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{comp,-20} Failed");
                //    Console.ForegroundColor = ConsoleColor.White;
                //}
               
                return;
            }

            //lock (lockObj) {
            string update = PS.GetLastUpdate(comp);
            if (string.IsNullOrEmpty(update)) {
                update = ";";
            }
            Console.WriteLine($"{comp,-20} {update}");
            _allCompInfo.Add($"{comp};{update.Replace(" ", ";")};; {cn}");
            //}
        }
    }
}
