using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Scrape
{
    public class NetworkManagement
    {
        public static void setIP(string ip_address, string subnet_mask)
        {
            ManagementClass objMC =
              new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    ManagementBaseObject setIP;
                    ManagementBaseObject newIP =
                      objMO.GetMethodParameters("EnableStatic");

                    newIP["IPAddress"] = new string[] { ip_address };
                    newIP["SubnetMask"] = new string[] { subnet_mask };

                    setIP = objMO.InvokeMethod("EnableStatic", newIP, null);
                }
            }
        }

        public static void setGateway(string gateway)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    ManagementBaseObject setGateway;
                    ManagementBaseObject newGateway =
                      objMO.GetMethodParameters("SetGateways");

                    newGateway["DefaultIPGateway"] = new string[] { gateway };
                    newGateway["GatewayCostMetric"] = new int[] { 1 };

                    setGateway = objMO.InvokeMethod("SetGateways", newGateway, null);
                }
            }
        }

        public static void setDNS(string NIC, string DNS)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    // if you are using the System.Net.NetworkInformation.NetworkInterface
                    // you'll need to change this line to
                    // if (objMO["Caption"].ToString().Contains(NIC))
                    // and pass in the Description property instead of the name 
                    if (objMO["Caption"].Equals(NIC))
                    {
                        ManagementBaseObject newDNS =
                          objMO.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = DNS.Split(',');
                        ManagementBaseObject setDNS =
                          objMO.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                    }
                }
            }
        }
    }
}
