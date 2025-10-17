// See https://aka.ms/new-console-template for more 

using PrinterKIOSK;
using System.Text;


//This is just for testing 

if (OperatingSystem.IsWindows())
{
    WindowsPrintEuroCoin printer = new WindowsPrintEuroCoin();


    printer.CheckPrinterStatusAndPrinterModelReturn();
    //printer.PrintTestNewCommands();
    printer.PrintTheTestUsingByteHelpe();
    //printer.CheckPrinterStatusAndPrinterModelReturn();
    //printer.Initialize();
    //printer.PrintTheTest();
    //printer.PrintHelloWorldUsingBytes();
    //printer.PrintListOfStrings();
}
else 
{
    LinuxPrintEuroCoin printer = new LinuxPrintEuroCoin();

    printer.Initialize();
    printer.PrintSelfcheckDemo();
}
