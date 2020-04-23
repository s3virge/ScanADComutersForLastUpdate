using System;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;

    class Check
    {
        /// <summary>
        /// using WMI for check;
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        //public static bool IsMacheneAvailable(string hostName)
        //{
        //    bool retVal = false;
        //    ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", hostName));
        //    ManagementClass os = new ManagementClass(scope, new ManagementPath("Win32_OperatingSystem"), null);

        //    try
        //    {
        //        ManagementObjectCollection instances = os.GetInstances();
        //        retVal = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        retVal = false;
        //        Console.WriteLine(ex.Message);
        //    }

        //    return retVal;
        //}

        /// <summary>
        /// using Ping for check with data sending;
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>        
        public static bool IsMachineUp(string hostName)
        {
            bool retVal = false;
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;
                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;

                PingReply reply = pingSender.Send(hostName, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                retVal = false;
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        /// <summary>
        /// using Ping for check
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static bool IsMachineOnline(string hostToPing)
        {
            bool retVal = false;
            Ping ping = new Ping();
            PingReply pingReply = null;

            //Debug.WriteLine($"Host to ping - {hostToPing}");

            try {
                pingReply = ping.Send(hostToPing);
                if (pingReply.Status == IPStatus.Success) {
                    retVal = true;
                }
            }
            catch{
                //throw new Exception($"While checking the availability of the computer {hostToPing} error occurred");
                //Console.WriteLine($"While checking the availability of the computer {hostToPing} error occurred");
            }

            return retVal;
        }
    }
