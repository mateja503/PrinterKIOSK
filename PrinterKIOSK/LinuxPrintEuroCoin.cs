using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PrinterKIOSK
{
    public class LinuxPrintEuroCoin
    {
        int lcid = CultureInfo.CurrentCulture.LCID;//used for language

        [DllImport("libmsprintsdk.so", EntryPoint = "SetInit", CharSet = CharSet.Ansi)]
        public static extern int SetInit();

        [DllImport("libmsprintsdk.so", EntryPoint = "PrintSelfcheck", CharSet = CharSet.Ansi)]
        public static extern int PrintSelfcheck();

        [DllImport("libmsprintsdk.so", EntryPoint = "SetClean", CharSet = CharSet.Ansi)]
        public static extern int SetClean();


        [DllImport("libmsprintsdk.so", EntryPoint = "SetDevname", CharSet = CharSet.Ansi)]
        public static extern int SetDevname(int a,string strPort,int b);


        int m_iInit = -1;
        int m_iStatus = -1;

        int m_lcLanguage = 0;

        public void Initialize()
        {
            SetDevname(3,"",0);//SetAutoUsb

            m_iInit = SetInit();

            if (m_iInit == 0)
            {
                Console.WriteLine("SUCESS");
            }
            else
            {
                Console.WriteLine("FAILURE");
            }

        }

        public void PrintSelfcheckDemo()
        {
            if (m_iInit == 0)
            {
                PrintSelfcheck();
                Console.WriteLine("Self_Test_SUCCESS");
                SetClean();
            }

        }

    }
}
