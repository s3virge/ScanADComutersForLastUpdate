using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanADComutersForLastUpdate
{
    class File
    {
        public static void WriteToCSV(ArrayList list, string fileName = "ADComputersLastUpdate.csv")
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
                {
                    foreach (var item in list)
                    {
                        sw.WriteLine($"{item}");
                    }
                }

                Console.WriteLine($"Information was saved to file {fileName}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
