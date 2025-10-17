
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PrinterKIOSK
{
    public class WindowsPrintEuroCoin
    {
        [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]//used for language
        public static extern int GetSystemDefaultLCID();

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetInit", CharSet = CharSet.Ansi)]
        public static extern int SetInit();

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetUsbportauto", CharSet = CharSet.Ansi)]
        public static extern int SetUsbportauto();

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "PrintSelfcheck", CharSet = CharSet.Ansi)]
        public static extern int PrintSelfcheck();

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetClean", CharSet = CharSet.Ansi)]
        public static extern int SetClean();

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "PrintString", CharSet = CharSet.Ansi)]
        public static extern int PrintString(string strData,int iImme);

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "PrintCutpaper", CharSet = CharSet.Ansi)]
        public static extern int PrintCutpaper(int iMode);

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetAlignment", CharSet = CharSet.Ansi)]
        public static extern int SetAlignment(int iAlignment);

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetLinespace", CharSet = CharSet.Ansi)]
        public static extern int SetLinespace(int iLinespace);

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetLinespace", CharSet = CharSet.Ansi)]
        public static extern int SetSizetext(int iHeight, int iWidth);

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetLinespace", CharSet = CharSet.Ansi)]
        public static extern int SetSizechar(int iHeight, int iWidth, int iUnderline, int iAsciitype);

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetLinespace", CharSet = CharSet.Ansi)]
        public static extern int SetBold(int iBold);

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetLinespace", CharSet = CharSet.Ansi)]
        public static extern int PrintFeedDot(int Lnumber);


        [DllImport("Msprintsdk.x64.dll", EntryPoint = "GetStatus", CharSet = CharSet.Ansi)]
        public static extern int GetStatus();
        
        [DllImport("Msprintsdk.x64.dll", EntryPoint = "GetStatusspecial", CharSet = CharSet.Ansi)]
        public static extern int GetStatusspecial();

        [DllImport("Msprintsdk.x64.dll", EntryPoint = "PrintQrcode", CharSet = CharSet.Ansi)]
        public static extern int PrintQrcode(string strData,int iLmargin,int iMside,int iRound);


        [DllImport("Msprintsdk.x64.dll", EntryPoint = "PrintRemainQR", CharSet = CharSet.Ansi)]
        public static extern int PrintRemainQR();


        [DllImport("Msprintsdk.x64.dll", EntryPoint = "SetCodepage", CharSet = CharSet.Ansi)]
        public static extern int SetCodepage(int country, int CPnumber);// 2 17; for cyrilic


        int m_iInit = -1;
        int m_iStatus = -1;

        int m_lcLanguage = 0;


        public void CheckPrinterStatusAndPrinterModelReturn() 
        {
            //serial port and bandWidth may be differente
            SerialPort serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            serialPort.Open();

            // Send DLE EOT 4 command to check paper roll
            byte[] checkPaper = new byte[] { 0x10, 0x04, 0x04 };
            serialPort.Write(checkPaper, 0, checkPaper.Length);

            // Read 1 byte response
            int response = serialPort.ReadByte();
            Console.WriteLine("Status byte: 0x" + response.ToString("X2"));

            byte[] getPrinterModel = new byte[] { 0x1D, 0x49, 0x05 };
            serialPort.Write(getPrinterModel, 0, getPrinterModel.Length);

            // Read 2 bytes response
            int highByte = serialPort.ReadByte();
            int lowByte = serialPort.ReadByte();

            int modelValue = (highByte << 8) | lowByte;

            //Console.WriteLine($"Printer Model Value (hex): 0x{modelValue:X4}");
            Console.WriteLine($"Printer Model Value (decimal): {modelValue}");

            // Decode bits
            if ((response & 0x0C) == 0x0C)
                Console.WriteLine("Paper near end");
            else if ((response & 0x60) == 0x60)
                Console.WriteLine("Paper end detected");
            else
                Console.WriteLine("Paper present");

            serialPort.Close();
        }



        public void Initialize()
        {
             SetUsbportauto();

            m_iInit = SetInit();

            if (m_iInit == 0)
            {
                Console.WriteLine("SUCESS");
                Console.WriteLine($"Printer status  {GetStatus()}");
                Console.WriteLine($"Printer GetStatusspecial  {GetStatusspecial()}");
            }
            else 
            {
                Console.WriteLine("FAILURE");
            }

        }

        public void PrintTestNewCommands() 
        {
            int vendorId = 0x0519;
            int productId = 0x2013;




        }

        public void PrintTheTestUsingByteHelpe() 
        {

            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {

                if (printerName.Contains("W80", StringComparison.OrdinalIgnoreCase))
                {
                    List<byte> commandBytes = new List<byte>();
                    commandBytes.AddRange(ByteHelper.ResetPrinter); // ESC @ reset to default standard mode 

                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Alignments------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line

                    //#region Alignments

                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Left\n"));

                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Center\n"));


                    //commandBytes.AddRange(ByteHelper.AlignRight); //Right Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Right\n"));

                    //#endregion
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment

                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Date & Time------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line

                    //#region Date and time
                    //var dateTime = DateTime.Now;
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes($"{dateTime}\n"));
                    //#endregion


                    commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("----------Underline Text, Bold, Italic----------"));
                    commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    commandBytes.AddRange(ByteHelper.LineBreak); // single line feed
                                                                 //add 1 line

                    #region Underline Text, Bold, Italic

                    commandBytes.AddRange(ByteHelper.UnderlineOn);//Underline on
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Text showing underline working\n"));
                    commandBytes.AddRange(ByteHelper.UnderlineOff);//Underline off

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode  

                    commandBytes.AddRange(ByteHelper.CheckBoldOn);//Bold on
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Text showing bold text working\n"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x00 });//Bold off does not work why is that i don't know
                    commandBytes.AddRange(ByteHelper.ResetPrinter);//reset default

                    commandBytes.AddRange(ByteHelper.ItalicOn);//Italic on 
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Text showing italic text working\n"));
                    commandBytes.AddRange(ByteHelper.ItalicOff);//Italic off


                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    commandBytes.AddRange(ByteHelper.CheckBoldOn);// Bold On
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Boldmine"));
                    commandBytes.AddRange(ByteHelper.CheckBoldOff); // Bold Off
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!\n"));

                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    commandBytes.AddRange(ByteHelper.FontSize1x2);// 1x2
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("mine"));
                    commandBytes.AddRange(ByteHelper.FontSizeDefault); // reset back to default
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!"));

                    commandBytes.AddRange(ByteHelper.AlignCenter);
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    commandBytes.AddRange(ByteHelper.UnderlineOn);//Underline on
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("mine"));
                    commandBytes.AddRange(ByteHelper.UnderlineOff);//Underline off
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!"));
                    commandBytes.AddRange(ByteHelper.LineBreak);

                    commandBytes.AddRange(ByteHelper.AlignCenter);
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    commandBytes.AddRange(ByteHelper.CheckBoldOn);//Underline on
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("mine"));
                    commandBytes.AddRange(ByteHelper.CheckBoldOff);//Underline off
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!"));

                    commandBytes.AddRange(ByteHelper.AlignLeft);
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    commandBytes.AddRange(ByteHelper.ItalicOn);//Italic on
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("mine"));
                    commandBytes.AddRange(ByteHelper.ItalicOff);//Italic off
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!\n"));

                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Added Line feed\n"));
                    commandBytes.AddRange(ByteHelper.CheckFineLeadFeed);

                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Added Form feed\n"));
                    commandBytes.AddRange(ByteHelper.CheckFormFeed);





                    //commandBytes.AddRange(ByteHelper.CheckRollPrintTest);



                    //commandBytes.AddRange(ByteHelper.CheckSetCharSize);

                    #endregion


                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Line Space------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line

                    #region Set Line Space

                    //commandBytes.AddRange(ByteHelper.LineSpacing24);//Line space 24
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 24\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 24\n"));

                    //commandBytes.AddRange(ByteHelper.LineSpacing24);//Line space 30
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 30\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 30\n"));

                    //commandBytes.AddRange(ByteHelper.LineSpacing45);//Line space 45
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 45\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 45\n"));

                    //commandBytes.AddRange(ByteHelper.LineSpacing60);//Line space 60
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 60\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 60\n"));

                    #endregion

                    //commandBytes.AddRange(ByteHelper.LineSpacing30);//Line space 30


                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Character Size------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line

                    #region Set Character Size

                    //Widtg - Height

                    //commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x11})//Size 1x1
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x1\n"));//this is  the default normal size
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size this is the normal size

                    //commandBytes.AddRange(ByteHelper.FontSize1x2);//Size 1x2
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x2\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize1x3);//Size 1x3
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x3\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize1x4);//Size 1x4
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x4\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize2x1);//Size 2x1
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x1\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize2x2);//Size 2x2
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x2\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize2x3);//Size 2x3
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x3\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize2x4);//Size 2x4
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x4\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize3x1);//Size 3x1
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x1\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize3x2);//Size 3x2
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x2\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize3x3);//Size 3x3
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x3\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize3x4);//Size 3x4
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x4\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize4x1);//Size 4x1
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x1\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize4x2);//Size 4x2
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x2\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize4x3);//Size 4x3
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x3\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1

                    //commandBytes.AddRange(ByteHelper.FontSize4x4);//Size 4x4
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x4\n"));
                    //commandBytes.AddRange(ByteHelper.FontSizeDefault);//Back to normal size 1x1


                    #endregion


                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Character Spacing------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line

                    #region Set Character Spacing

                    //commandBytes.AddRange(ByteHelper.CharachterSize5);// added 5 for dots 
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Character Space 5\n"));
                    //commandBytes.AddRange(ByteHelper.CharachterDefault);//reset

                    //commandBytes.AddRange(ByteHelper.CharachterSize7);
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Character Space 7\n"));
                    //commandBytes.AddRange(ByteHelper.CharachterDefault);////reset

                    //commandBytes.AddRange(ByteHelper.CharachterSize8);
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Character Space 8\n"));
                    //commandBytes.AddRange(ByteHelper.CharachterDefault);//reset

                    //commandBytes.AddRange(ByteHelper.CharachterSize9);
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Character Space 9\n"));
                    //commandBytes.AddRange(ByteHelper.CharachterDefault);//reset

                    #endregion
                    //commandBytes.AddRange(ByteHelper.ResetPrinter); // ESC @ reset to default standard mode  


                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Left Margin------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line


                    #region Left Margin
                    //commandBytes.AddRange(ByteHelper.LeftMargin0);//set margin 0
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 0\n"));

                    //commandBytes.AddRange(ByteHelper.LeftMargin5);
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 5\n"));

                    //commandBytes.AddRange(ByteHelper.LeftMargin10);
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 10\n"));

                    //commandBytes.AddRange(ByteHelper.LeftMargin15);
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 15\n"));

                    #endregion

                    //commandBytes.AddRange(ByteHelper.LeftMargin0);//set margin 0

                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Inverse------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line

                    #region Inverse

                    //commandBytes.AddRange(ByteHelper.InverseOn);//inverse on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Inverse White model test!!!\n"));
                    //commandBytes.AddRange(ByteHelper.InverseOff);//inverse off 

                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Inverse "));
                    //commandBytes.AddRange(ByteHelper.InverseOn);//inverse on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("White"));
                    //commandBytes.AddRange(ByteHelper.InverseOff);//inverse off 
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("model test!!!\n"));

                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Inverse "));
                    //commandBytes.AddRange(ByteHelper.InverseOn);//inverse on
                    //commandBytes.AddRange(ByteHelper.BoldOn);//Bold on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("White Bold"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("model test!!!\n"));
                    //commandBytes.AddRange(ByteHelper.ResetPrinter);


                    #endregion

                    //commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("------------Special Character------------\n"));
                    //commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment
                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line

                    #region SpecialCharacters

                    //commandBytes.AddRange(new byte[] { ByteHelper.LeftBracket, ByteHelper.Blank, ByteHelper.RightBracket, ByteHelper.Blank,
                    //    ByteHelper.ExclamationMark, ByteHelper.Blank, ByteHelper.QuotationMark, ByteHelper.Blank, ByteHelper.HashSymbol, ByteHelper.Blank,
                    //    ByteHelper.DollarSign, ByteHelper.Blank, ByteHelper.Percent, 
                    //    ByteHelper.Blank, ByteHelper.OperatorAND, ByteHelper.Blank, ByteHelper.SingleQuotationMark });// ( ) ! " # $ % & '

                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line 
                    //commandBytes.AddRange(new byte[] { ByteHelper.MultiplySymbol, ByteHelper.Blank, ByteHelper.PlusSign,
                    //    ByteHelper.Blank, ByteHelper.Comma, ByteHelper.Blank, ByteHelper.MinusSign,
                    //    ByteHelper.Blank, ByteHelper.Period, ByteHelper.Blank, ByteHelper.Slash });// * + , - . /

                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line 
                    //commandBytes.AddRange(new byte[] { ByteHelper.Colon, ByteHelper.Blank, ByteHelper.SemiColon,
                    //    ByteHelper.Blank, ByteHelper.LessThan, ByteHelper.Blank, ByteHelper.EqualSign,
                    //    ByteHelper.Blank, ByteHelper.GreaterThan, ByteHelper.Blank, ByteHelper.QuestionMark, ByteHelper.AtSign });// : ; < = > ? @

                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line
                    //commandBytes.AddRange(new byte[] { ByteHelper.LeftSquareBracket, ByteHelper.Blank, ByteHelper.RightSquareBracket, 
                    //    ByteHelper.Blank, ByteHelper.BackSlash, ByteHelper.Blank, ByteHelper.Caret,
                    //    ByteHelper.Blank, ByteHelper.Underscore, ByteHelper.Blank, ByteHelper.Backtick });// [ ] \ ^ _ `

                    //commandBytes.AddRange(ByteHelper.Add1Line);//add 1 line
                    //commandBytes.AddRange(new byte[] { ByteHelper.LeftCurlyBracket, ByteHelper.Blank, ByteHelper.RightCurlyBracket, ByteHelper.Blank, 
                    //    ByteHelper.VerticalBar, ByteHelper.Blank, ByteHelper.Tilde });// { } | ~ 

                    #endregion

                    commandBytes.AddRange(ByteHelper.AlignCenter);//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("------------BarCode------------\n"));
                    commandBytes.AddRange(ByteHelper.AlignLeft);//Left Alignment

                    #region BarCode

                    commandBytes.AddRange(ByteHelper.BarCodePrintingPositionAbove); // HRI Position: Above Barcode, human readable text za labelu
                                                                            //some printers do not support this
                                                                            //0x03 = Both above and below
                    string type = "";
                    byte barcodeType = type.ToUpper() switch
                    {
                        "UPC-A" => 0x00,
                        "UPC-E" => 0x01,
                        "EAN13" => 0x02,
                        "EAN8" => 0x03,
                        "CODE39" => 0x04,
                        "ITF" => 0x05,
                        "CODABAR" => 0x06,
                        "CODE93" => 0x48,
                        "CODE128" => 0x49,
                        _ => 0x49 // Default to CODE128 if unknown
                    };

                    
                    commandBytes.AddRange(ByteHelper.BarCodeCenterAlignment); // ESC a (Alignment)

                    // Set barcode height and width
                    commandBytes.AddRange(ByteHelper.BarCodeHeight); // GS h 100 (Barcode height)
                    commandBytes.AddRange(ByteHelper.BarCodeWidth3);   // GS w 3 (Barcode width)

                    string data = "*TEST8052*";

                    commandBytes.AddRange(new byte[] { 0x1D, 0x6B, barcodeType }); // GS k (Select CODE128 format)
                    commandBytes.Add((byte)(data.Length + 2)); // Data length + start character
                    commandBytes.Add(0x7B); // Start character for CODE128 subset B
                    commandBytes.Add(0x42); // Subset B (for alphanumeric data)
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(data)); // Barcode data
                                                                          //command.Add(0x0A); // Line feed after barcode

                    #endregion

                  
                    commandBytes.AddRange(ByteHelper.Add5Lines);//add 10 lines before cutting 

                    commandBytes.AddRange(ByteHelper.CheckFullCut);

                    bool ok = RawPrinterHelper.SendBytesToPrinter(printerName, commandBytes.ToArray());

                    Console.WriteLine(ok ? "Printed successfully!" : "Failed to print.");

                }

            }

        }

        public void PrintTheTest() 
        {
            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {

                if (printerName.Contains("W80", StringComparison.OrdinalIgnoreCase))
                {
                    List<byte> bytesToPrint = new List<byte>();

                    // Reset printer and set defaults
                    bytesToPrint.AddRange(ByteHelper.ResetPrinter);
                    bytesToPrint.AddRange(ByteHelper.LineSpacing24);
                    bytesToPrint.AddRange(ByteHelper.FontTypeA);
                    bytesToPrint.AddRange(ByteHelper.AlignLeft);

                    // "Hello " normal
                    bytesToPrint.AddRange(Encoding.UTF8.GetBytes("Hello "));

                    // "mine" bold
                    bytesToPrint.AddRange(ByteHelper.BoldOn);
                    bytesToPrint.AddRange(Encoding.UTF8.GetBytes("mine"));
                    bytesToPrint.AddRange(ByteHelper.BoldOff); // Bold OFF after the word

                    //bytesToPrint.AddRange(ByteHelper.ResetPrinter);
                    //bytesToPrint.AddRange(ByteHelper.LineSpacing24);
                    //bytesToPrint.AddRange(ByteHelper.FontTypeA);
                    //bytesToPrint.AddRange(ByteHelper.AlignLeft);
                    // " world" normal
                    bytesToPrint.AddRange(Encoding.UTF8.GetBytes(" world"));

                    // Line break
                    bytesToPrint.AddRange(ByteHelper.LineBreak);


                    //bool ok = RawPrinterHelper.SendBytesToPrinter(printerName, bytesToPrint.ToArray());

                    List<byte> commandBytes = new List<byte>();
                    //commandBytes.AddRange(bytesToPrint);
                    commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Alignments----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Alignments

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Left\n"));

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Center\n"));


                    //commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x02 }); //Right Alignemnt
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Right\n"));

                    #endregion
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Date & Time----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Date and time
                    var dateTime = DateTime.Now;
                    commandBytes.AddRange(Encoding.ASCII.GetBytes($"{dateTime}\n"));
                    #endregion

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Underline Text, Bold, Italic----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Underline Text, Bold, Italic

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x2D, 0x01});//Underline on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Text showing underline working\n"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x2D, 0x00 });//Underline off

                    ////commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode  

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x01 });//Bold on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Text showing bold text working\n"));
                    ////commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x00 });//Bold off does not work why is that i don't know
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 });//reset default

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x25, 0x47 });//Italic on 
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Text showing italic text working\n"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x25, 0x48 });//Italic off

                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    //commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x10 });// 1x2
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("mine"));
                    //commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 }); // reset back to default
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!\n"));

                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x2D, 0x01 });//Underline on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("mine"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x2D, 0x00 });//Underline off
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!\n"));

                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello "));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x25, 0x47 });//Italic on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("mine"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x25, 0x48 });//Italic off
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes(" World!!\n"));

                    #endregion

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode  

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Line Space----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Set Line Space

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x33, 24 });//Line space 30
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 24\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 24\n"));

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x33, 30 });//Line space 30
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 30\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 30\n"));

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x33, 45 });//Line space 45
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 45\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 45\n"));

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x33, 60 });//Line space 60
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 60\n"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Line space 60\n"));

                    #endregion

                    commandBytes.AddRange(new byte[] { 0x1B, 0x33, 30 });//Line space 30

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Character Size----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Set Character Size

                    //Widtg - Height

                    //commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x11})//Size 1x1
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x1\n"));//this is  the default normal size
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size this is the normal size

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x10 });//Size 1x2
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x2\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x20 });//Size 1x3
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x3\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x30 });//Size 1x4
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 1x4\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x01 });//Size 2x1
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x1\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x11 });//Size 2x2
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x2\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x21 });//Size 2x3
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x3\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x31 });//Size 2x4
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 2x4\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x02 });//Size 3x1
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x1\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x12 });//Size 3x2
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x2\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x22 });//Size 3x3
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x3\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x32 });//Size 3x4
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 3x4\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x03 });//Size 4x1
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x1\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x13 });//Size 4x2
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x2\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x32 });//Size 4x3
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x3\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1

                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x33 });//Size 4x4
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Size 4x4\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 });//Back to normal size 1x1


                    #endregion

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Character Spacing----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Set Character Spacing

                    commandBytes.AddRange(new byte[] { 0x1B, 0x2B, 0x4A, 5 });// added 5 for dots 
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Character Space 5\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x2B, 0x4A, 0 });//reset

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    commandBytes.AddRange(new byte[] { 0x1B, 0x2B, 0x4A, 7 });
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Character Space 10\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x2B, 0x4A, 0 });////reset

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    commandBytes.AddRange(new byte[] { 0x1B, 0x2B, 0x4A, 8 });
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Character Space 20\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x2B, 0x4A, 0 });//reset

                    #endregion
                    commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode  


                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Left Margin----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line


                    #region Left Margin
                    //commandBytes.AddRange(new byte[] { 0x1D, 0x4C, 0x00, 0x00 });//set margin 0
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 0\n"));
                    //commandBytes.AddRange(new byte[] { });

                    //commandBytes.AddRange(new byte[] { 0x1D, 0x4C, 0x05, 0x00 });
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 5\n"));
                    //commandBytes.AddRange(new byte[] { });

                    //commandBytes.AddRange(new byte[] { 0x1D, 0x4C, 0x0A, 0x00 });
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 10\n"));
                    //commandBytes.AddRange(new byte[] { });

                    //commandBytes.AddRange(new byte[] { 0x1D, 0x4C, 0x0F, 0x00 });
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Set Left Margin 15\n"));
                    //commandBytes.AddRange(new byte[] { });

                    #endregion

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 
                    commandBytes.AddRange(new byte[] { 0x1D, 0x4C, 0x00, 0x00 });//set margin 0

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Inverse----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Inverse

                    //commandBytes.AddRange(new byte[] { 0x1D, 0x42, 0x01});//inverse on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Inverse White model test!!!\n"));
                    //commandBytes.AddRange(new byte[] { 0x1D, 0x42, 0x00});//inverse off 

                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Inverse "));
                    //commandBytes.AddRange(new byte[] { 0x1D, 0x42, 0x01 });//inverse on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("White"));
                    //commandBytes.AddRange(new byte[] { 0x1D, 0x42, 0x00 });//inverse off 
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("model test!!!\n"));

                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Inverse "));
                    //commandBytes.AddRange(new byte[] { 0x1D, 0x42, 0x01 });//inverse on
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x01 });//Bold on
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("White Bold"));
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("model test!!!\n"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 });


                    #endregion

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Special Character----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region SpecialCharacters


                    ////commandBytes.Add(0x20); //blank
                    //commandBytes.AddRange(new byte[] { 0x28, 0x20, 0x29, 0x20, 0x21, 0x20, 0x22, 0x20, 0x23, 0x20, 0x24, 0x20, 0x25, 0x20, 0x26, 0x20, 0x27, 0x20, 0x9C, 0x20, 0x9D });// ( ) ! " # $ % & ' £ ¥
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line 
                    //commandBytes.AddRange(new byte[] { 0x2A, 0x20, 0x2B, 0x20, 0x2C, 0x20, 0x2D, 0x20, 0x2E, 0x20, 0x2F });// * + , - . /
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line 
                    //commandBytes.AddRange(new byte[] { 30, 0x20, 31, 0x20, 32, 0x20, 33, 0x20, 34, 0x20, 35, 0x20, 36, 0x20, 37, 0x20, 38, 0x20, 39 });//0 1 2 3 4 5 6 7 8 9
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line 
                    //commandBytes.AddRange(new byte[] { 0x3A, 0x20, 0x3B, 0x20, 0x3C, 0x20, 0x3D, 0x20, 0x3E, 0x20, 0x3F, 0x20, 0x40 });// : ; < = > ? @
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x5B, 0x20, 0x5D, 0x20, 0x5C, 0x20, 0x5E, 0x20, 0x5F, 0x20, 0x60 });// [ ] \ ^ _ `
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x7B, 0x20, 0x7D, 0x20, 0x7C, 0x20, 0x7E });// { } | ~ 
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x80, 0x20, 0x87, 0x20, 0x9B });//Ç ç ¢
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x8E, 0x20, 0x8F, 0x20, 0x92, 0x20, 0x83, 0x20, 0x84, 0x20, 0x85, 0x20, 0x86, 0x20, 0xA0, 0x20, 0xA6, 0x20, 0x91 });//Ä Å Æ â ä à! å á ª æ
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x90, 0x20, 0x82, 0x20, 0x88, 0x20, 0x89, 0x20, 0x8A });//É é ê ë è 
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x8B, 0x20, 0x8C, 0x20, 0x8D, 0x20, 0xA1, 0x20, 0xAD });//ï î ì í ¡
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x99, 0x20, 0x93, 0x20, 0x94, 0x20, 0x95, 0x20, 0xA2, 0x20, 0xA7 });//Ö ô ö ò ó º
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0x99, 0x20, 0x81, 0x20, 0x96, 0x20, 0x97, 0x20, 0x98, 0x20, 0xA3 });//Ü ü û ù ÿ ú
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xA4, 0x20, 0xA5 });//ñ Ñ
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xA8, 0x20, 0xA9, 0x20, 0xAA, 0x20, 0xAB, 0x20, 0xAC, 0x20, 0xAE, 0x20, 0xAF, 0x20 });//¿ ⌐ ¬ ½ ¼ «
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xB0, 0x20, 0xB1, 0x20, 0xB2, 0x20, 0xB3, 0x20, 0xB4, 0x20, 0xB5, 0x20, 0xB5, 0x20, 0xB6, 0x20, 0xB7 });//░ ▒ ▓ │ ┤ ╡ ╢ ╖
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xB8, 0x20, 0xB9, 0x20, 0xBA, 0x20, 0xBB, 0x20, 0xBC, 0x20, 0xBD, 0x20, 0xBE, 0x20, 0xBF });//╕ ╣ ║ ╗ ╝ ╜ ╛ ┐
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xC0, 0x20, 0xC1, 0x20, 0xC2, 0x20, 0xC3, 0x20, 0xC4, 0x20, 0xC5, 0x20, 0xC6, 0x20, 0xC7, });//└ ┴ ┬ ├ ─ ┼ ╞ ╟ 
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xC8, 0x20, 0xC9, 0x20, 0xCA, 0x20, 0xCB, 0x20, 0xCC, 0x20, 0xCD, 0x20, 0xCE, 0x20, 0xCF });//╚ ╔ ╩ ╦ ╠ ═ ╬ ╧
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xD0, 0x20, 0xD1, 0x20, 0xD2, 0x20, 0xD3, 0x20, 0xD4, 0x20, 0xD5, 0x20, 0xD6, 0x20, 0xD7 });//╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ 
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xD8, 0x20, 0xD9, 0x20, 0xDA, 0x20, 0xDB, 0x20, 0xDC, 0x20, 0xDD, 0x20, 0xDE, 0x20, 0xDF });//╪ ┘┌ █ ▄ ▌▐ ▀
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xE0, 0x20, 0xE1, 0x20, 0xE2, 0x20, 0xE3, 0x20, 0xE4, 0x20, 0xE5, 0x20, 0xE6, 0x20, 0xE7 });//α β г П Σ σ μ γ
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xE8, 0x20, 0xE9, 0x20, 0xEA, 0x20, 0xEB, 0x20, 0xEC, 0x20, 0xED, 0x20, 0xEE, 0x20, 0xEF });//Φ θ Ω δ ∞ φ Є ∩
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xF0, 0x20, 0xF1, 0x20, 0xF2, 0x20, 0xF3, 0x20, 0xF4, 0x20, 0xF5, 0x20, 0xF6, 0x20, 0xF6 });//≡ ± ≥ ≤ ⌠ ⌡ ÷ ≈
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line
                    //commandBytes.AddRange(new byte[] { 0xF8, 0x20, 0xF9, 0x20, 0xFA, 0x20, 0xFB, 0x20, 0xFC, 0x20, 0xFD, 0x20, 0xFE });//° • · √ ⁿ ² ▪ 
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #endregion

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("--------Charachter code page----------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    #region Charachter code page
                    commandBytes.AddRange(new byte[] { 0x1B, 0x40 });//reset 
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding big5 = Encoding.GetEncoding("GB2312");

                    // Enable Chinese mode
                    commandBytes.AddRange(new byte[] { 0x1C, 0x26 });

                    // Set double-size font (optional)
                    commandBytes.AddRange(new byte[] { 0x1C, 0x57, 0x03 }); // double width + height

                    string text = "测试中文打印";
                    // Encode Chinese text in GB2312 (or try Big5 depending on printer)
                    byte[] chineseData = big5.GetBytes(text);
                    commandBytes.AddRange(chineseData);

                    // New line
                    commandBytes.AddRange(new byte[] { 0x0A });

                    // Disable Chinese mode
                    commandBytes.AddRange(new byte[] { 0x1C, 0x2E });

                    ///////////////above used chinese character testing and it works i changed the page encoding
                    ///

                    #endregion

                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 });//Center Alignemnt
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("----------------------------------------\n"));
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 });//Left Alignment
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    //commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x00 });//add 1 line

                    commandBytes.AddRange(Encoding.ASCII.GetBytes("This line is just for testing !!!\n"));

                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x0A });//add 10 lines before cutting 

                    commandBytes.AddRange(new byte[] { 0x1D, 0x56, 0x00 });// full cut (completely separates the ticket)

                    bool ok = RawPrinterHelper.SendBytesToPrinter(printerName, commandBytes.ToArray());

                    Console.WriteLine(ok ? "Printed successfully!" : "Failed to print.");

                }

            } 
        }


        public void PrintHelloWorldUsingBytes()
        {
            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine($"{printerName}");

                if (printerName.Contains("W80", StringComparison.OrdinalIgnoreCase))
                {
                    List<byte> commandBytes = new List<byte>();

                    // Reset printer
                    commandBytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ reset to default standard mode 

                    // --- "Hello World" (left aligned, normal size) ---
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 }); // Align left
                    commandBytes.AddRange(new byte[] { 0x1B, 0x32 });       // Default line spacing
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello World\n"));


                    //commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x01 });
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes("Hello"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x00 });
                    //commandBytes.AddRange(Encoding.ASCII.GetBytes(" World"));
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x01 });
                    //commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x00 });

                    commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x01 });   // bold ON
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("HelloBold"));  // word in bold
                    commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x00 });   // bold OFF
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(" World")); // rest of line normal
                    //commandBytes.Add(0x0A);
                    commandBytes.AddRange(new byte[] { 0x1B, 0x45, 0x00 });// bold OFF

                    commandBytes.AddRange(new byte[] { 0x1D, 0x42, 0x01 });//Set inverse
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("This is the inverse text line....\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x42, 0x00 });//reset inverse


                    // --- "A015" (center, double size) ---
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x01 }); // Align center
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x11 }); // Double width + double height
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("A015\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 }); // Reset size

                    // --- Waiting info (left aligned, double height) ---
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x00 }); // Align left
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x01 }); // Double height
                    commandBytes.AddRange(Encoding.ASCII.GetBytes(
                        "There are 1 people waiting in front of you, pay attention to the service window of the call number information.\n"));
                    commandBytes.AddRange(new byte[] { 0x1D, 0x21, 0x00 }); // Reset size

                    // --- Thank you ---
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("Thank you for your cooperation!\n"));

                    // --- QR Code ---
                    string qrData = "www.google.com";
                    byte[] qrBytes = Encoding.ASCII.GetBytes(qrData);
                    int qrLength = qrBytes.Length + 3;

                    // Store QR data
                    commandBytes.AddRange(new byte[] { 0x1D, 0x28, 0x6B,
                (byte)(qrLength % 256), (byte)(qrLength / 256),
                0x31, 0x50, 0x30 });
                    commandBytes.AddRange(qrBytes);

                    // Print QR code
                    commandBytes.AddRange(new byte[] { 0x1D, 0x28, 0x6B, 3, 0, 0x31, 0x51, 0x30 });

                    // --- Date/Time (right aligned) ---
                    commandBytes.AddRange(new byte[] { 0x1B, 0x61, 0x02 }); // Align right
                    commandBytes.AddRange(Encoding.ASCII.GetBytes("2019-03-16 09:30\n"));

                    // --- Feed & Cut ---
                    commandBytes.AddRange(new byte[] { 0x1B, 0x64, 0x05 }); // Feed 5 lines
                    commandBytes.AddRange(new byte[] { 0x1D, 0x56, 0x41, 0x00 }); // Full cut

                    // Send to printer
                    bool ok = RawPrinterHelper.SendBytesToPrinter(printerName, commandBytes.ToArray());

                    Console.WriteLine(ok ? "Printed successfully!" : "Failed to print.");
                }
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

        public void PrintListOfStrings() 
        {
            if (m_iInit == 0)
            {
                SetClean();
                //This is for HelloWord string 
                SetAlignment(0);
                SetLinespace(100);
                PrintFeedDot(250);
                PrintString("Hello World", 0);
                //end for the hellow world string 

                var ok = SetCodepage(2, 17);
                Console.WriteLine($"Set CodePage: {ok}");
                PrintString("Матеја Николиќ", 0);
                SetCodepage(1, 0);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(3, 17);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(4, 17);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(5, 17);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(6, 17);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(7, 17);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(8, 17);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(9, 17);
                PrintString("Матеја Николиќ", 0);
                SetCodepage(10, 17);
                PrintString("Матеја Николиќ", 0);

                SetCodepage(0, 0);

                //start styling 
                PrintFeedDot(250);
                SetAlignment(1);
                SetSizetext(2, 2);
                SetSizechar(2, 2, 0, 1);
                SetBold(1);
                PrintString("A015", 0);
                SetBold(1);
                //end styling 


                //start styling 
                SetAlignment(0);
                SetBold(0);
                SetSizetext(2, 1);
                PrintFeedDot(50);
                PrintString("  There are 1 people waiting in front of you, pay attention to the service window of the call number information.", 0);
                //end styling

                //start styling
                PrintFeedDot(30);
                PrintString("  Thank you for your cooperation!", 0);
                //end styling


                //start styling
                PrintFeedDot(30);
                SetLinespace(100);
                SetAlignment(2);
                PrintQrcode("www.google.com", 5, 6, 0);
                //end styling

                //start styling
                PrintFeedDot(30);
                SetLinespace(100);
                SetAlignment(2);
                PrintString("2019-03-16 09:30 ", 0);
                PrintFeedDot(250);
                //end styling

               


                //or
                /*
				PrintQrcode("www.xxx.com",5,6,1);
				SetLeftmargin(240);
				PrintString("www.xxx.com",1); 
				PrintRemainQR(); 
				*/
                PrintFeedDot(150);
                SetAlignment(0);
                SetLinespace(100);

                PrintCutpaper(0);
                SetClean();
            }
            
        }

      
    }
}
