using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterKIOSK
{
    public static class ByteHelper
    {
        // Encoding / Code Pages
        public static readonly byte[] Encoding1251 = new byte[] { 0x1B, 0x74, 0x11 };
        public static readonly byte[] LineBreak = new byte[] { 0x0A };

        // Bold
        // public static readonly byte[] BoldOn = new byte[] { 0x1B, 0x21, 0x38 };//kladomat
        public static readonly byte[] BoldOn = new byte[] { 0x1B, 0x45, 0x01 };
        public static readonly byte[] BoldOff = new byte[] { 0x1B, 0x45, 0x00 };

        // Underline
        public static readonly byte[] UnderlineOn = new byte[] { 0x1B, 0x2D, 0x01 };
        public static readonly byte[] UnderlineOff = new byte[] { 0x1B, 0x2D, 0x00 };

        // Font sizes
        public static readonly byte[] FontTypeA = new byte[] { 0x1B, 0x4D, 0x00 };
        public static readonly byte[] FontTypeB = new byte[] { 0x1B, 0x4D, 0x01 }; //typeB is smaller !

        public static readonly byte[] SmallFont = new byte[] { 0x1D, 0x21, 0x00 };
        public static readonly byte[] MediumFont = new byte[] { 0x1D, 0x21, 0x11 };
        public static readonly byte[] LargeFont = new byte[] { 0x1D, 0x21, 0x22 };

        // Alignment
        public static readonly byte[] AlignLeft = new byte[] { 0x1B, 0x61, 0x00 };
        public static readonly byte[] AlignCenter = new byte[] { 0x1B, 0x61, 0x01 };
        public static readonly byte[] AlignRight = new byte[] { 0x1B, 0x61, 0x02 };

        public static readonly byte[] CutPaper = new byte[] { 0x1D, 0x56, 0x41, 0x00 };


        public static readonly byte[] ResetPrinter = new byte[] { 0x1B, 0x40 };

        //public static readonly byte[] InverseOn = new byte[] { 0x1B, 0x21, 0x20 };
        //public static readonly byte[] InverseOff = new byte[] { 0x1B, 0x21, 0x00 };

        // Inverse printing commands for GS B (used by Citizen CT-S4000)
        // Inverse printing commands for GS B (used by Citizen CT-S4000)
        public static readonly byte[] InverseOn = new byte[] { 0x1D, 0x42, 0x01 }; // Inverse on (white text on black)
        public static readonly byte[] InverseOff = new byte[] { 0x1D, 0x42, 0x00 }; // Inverse off (black text on white)

        public static readonly byte[] StatusCommand = new byte[] { 0x10, 0x04, 0x01 }; //printer status
        public static readonly byte[] StatusCommandOffline = new byte[] { 0x10, 0x04, 0x02 };
        public static readonly byte[] StatusCommandError = new byte[] { 0x10, 0x04, 0x04 };
        public static readonly byte[] StatusCommandPaperSensor = new byte[] { 0x10, 0x04, 0x05 };


        //Line Spacing
        public static readonly byte[] LineSpacingDefault = new byte[] { 0x1B, 0x33, 24 };

        public static byte[] LineSpacing24 = new byte[] { 0x1B, 0x33, 24 }; // 24 dots
        public static byte[] LineSpacing30 = new byte[] { 0x1B, 0x33, 30 }; // 30 dots
        public static byte[] LineSpacing45 = new byte[] { 0x1B, 0x33, 45 }; // 45 dots
        public static byte[] LineSpacing60 = new byte[] { 0x1B, 0x33, 60 }; // 60 dots
        //public static readonly byte[] LineSpacingDefault = new byte[] { 0x1B, 0x32 }; //larger


        //Add Lines last byte is used to set how many new lines will be added
        public static byte[] Add1Line = new byte[] { 0x1B, 0x64, 0x00 }; //Add 1 one line 
        public static byte[] Add5Lines = new byte[] { 0x1B, 0x64, 0x05 }; //Add 5 lines 
        public static byte[] Add10Lines = new byte[] { 0x1B, 0x64, 0x0A }; //Add 10 lines 

        //Italic
        public static byte[] ItalicOn = new byte[] { 0x1B, 0x25, 0x47 };//Italic On
        public static byte[] ItalicOff = new byte[] { 0x1B, 0x25, 0x48 };//Italic On

        //Font Size
        public static byte[] FontSizeDefault = new byte[] { 0x1D, 0x21, 0x00 };//Default Size1x1 for printer EP802-TM
        public static byte[] FontSize1x1 = new byte[] { 0x1D, 0x21, 0x00 };//Default Size1x1 for printer EP802-TM
        public static byte[] FontSize1x2 = new byte[] { 0x1D, 0x21, 0x10 };//Size1x2
        public static byte[] FontSize1x3 = new byte[] { 0x1D, 0x21, 0x20 };//Size1x3
        public static byte[] FontSize1x4 = new byte[] { 0x1D, 0x21, 0x30 };//Size1x4
        public static byte[] FontSize2x1 = new byte[] { 0x1D, 0x21, 0x01 };//Size2x1
        public static byte[] FontSize2x2 = new byte[] { 0x1D, 0x21, 0x11 };//Size2x2
        public static byte[] FontSize2x3 = new byte[] { 0x1D, 0x21, 0x21 };//Size2x3
        public static byte[] FontSize2x4 = new byte[] { 0x1D, 0x21, 0x31 };//Size2x4
        public static byte[] FontSize3x1 = new byte[] { 0x1D, 0x21, 0x02 };//Size3x1
        public static byte[] FontSize3x2 = new byte[] { 0x1D, 0x21, 0x12 };//Size3x2
        public static byte[] FontSize3x3 = new byte[] { 0x1D, 0x21, 0x22 };//Size3x3
        public static byte[] FontSize3x4 = new byte[] { 0x1D, 0x21, 0x32 };//Size3x4
        public static byte[] FontSize4x1 = new byte[] { 0x1D, 0x21, 0x03 };//Size4x1
        public static byte[] FontSize4x2 = new byte[] { 0x1D, 0x21, 0x13 };//Size4x2
        public static byte[] FontSize4x3 = new byte[] { 0x1D, 0x21, 0x23 };//Size4x3
        public static byte[] FontSize4x4 = new byte[] { 0x1D, 0x21, 0x33 };//Size4x4

        //Character Space 
        public static byte[] CharachterDefault = new byte[] { 0x1B, 0x2B, 0x4A, 0 };//CharacterSize0
        public static byte[] CharachterSize5 = new byte[] { 0x1B, 0x2B, 0x4A, 5 };//CharacterSize5
        public static byte[] CharachterSize7 = new byte[] { 0x1B, 0x2B, 0x4A, 7 };//CharacterSize7
        public static byte[] CharachterSize8 = new byte[] { 0x1B, 0x2B, 0x4A, 8 };//CharacterSize8
        public static byte[] CharachterSize9 = new byte[] { 0x1B, 0x2B, 0x4A, 9 };//CharacterSize9


        //Left Margin
        public static byte[] LeftMargin0 = new byte[] { 0x1D, 0x4C, 0x00, 0x00 };//Left Margin 0
        public static byte[] LeftMargin5 = new byte[] { 0x1D, 0x4C, 0x05, 0x00 };//Left Margin 5
        public static byte[] LeftMargin10 = new byte[] { 0x1D, 0x4C, 0x0A, 0x00 };//Left Margin 10
        public static byte[] LeftMargin15 = new byte[] { 0x1D, 0x4C, 0x0F, 0x00 };//Left Margin 15



        //BarCode
        public static byte[] BarCodeHeight = new byte[] { 0x1D, 0x68, 100 };//dots 40 
        public static byte[] BarCodeWidth1 = new byte[] { 0x1D, 0x77, 1 };// 1
        public static byte[] BarCodeWidth2 = new byte[] { 0x1D, 0x77, 2 };// 2
        public static byte[] BarCodeWidth3 = new byte[] { 0x1D, 0x77, 3 };// 3
        public static byte[] BarCodeWidth4 = new byte[] { 0x1D, 0x77, 4 };// 4

        public static byte[] BarCodeNoPrinting = new byte[] { 0x1D, 0x48, 0x00};//No Printing  
        public static byte[] BarCodePrintingPositionAbove = new byte[] { 0x1D, 0x48, 0x01 };//Above Printing
        public static byte[] BarCodePrintingPositionBelow = new byte[] { 0x1D, 0x48, 0x02 };//Below Printing
        public static byte[] BarCodePrintingPositionAboveAndBelow = new byte[] { 0x1D, 0x48, 0x03 };//Above and Below Printing

        public static byte[] BarCodeHRICharachtersSize12x24 = new byte[] { 0x1D, 0x66, 0x00};//Font 12x24
        public static byte[] BarCodeHRICharachtersSize8x16 = new byte[] { 0x1D, 0x66, 0x01};//Font 8x16

        public static byte[] BarCodeLeftAlignment = new byte[] { 0x1D, 0x50,};//Left
        public static byte[] BarCodeCenterAlignment = new byte[] { 0x1D, 0x50, 1 };//Center
        public static byte[] BarCodeRightAlignment = new byte[] { 0x1D, 0x50, 2 };//Right

        public static byte[] BarCodePrint = new byte[] { 0x1B, 0x62 };//Print Barcode

        //Special Charachters 
        public static byte Blank = 0x20;// empty space
        public static byte LeftBracket = 0x28;// (
        public static byte RightBracket = 0x29;// )
        public static byte ExclamationMark = 0x21;// !
        public static byte QuotationMark = 0x22;// "
        public static byte HashSymbol = 0x23;// #
        public static byte DollarSign = 0x24;// $
        public static byte Percent = 0x25;// %
        public static byte OperatorAND = 0x26;// &
        public static byte SingleQuotationMark = 0x27;// '
        public static byte MultiplySymbol = 0x2A;// *
        public static byte PlusSign = 0x2B;// +
        public static byte Comma = 0x2C;// ,
        public static byte MinusSign = 0x2D;// -
        public static byte Period = 0x2E;// .
        public static byte Slash = 0x2F;// /
        public static byte Colon = 0x3A; // :
        public static byte SemiColon = 0x3B;// ;
        public static byte LessThan = 0x3C;// <
        public static byte EqualSign = 0x3D;// =
        public static byte GreaterThan = 0x3E;// >
        public static byte QuestionMark = 0x3F;// ?
        public static byte AtSign = 0x40;// @
        public static byte LeftSquareBracket = 0x5B;// [
        public static byte RightSquareBracket = 0x5D;// ]s
        public static byte BackSlash = 0x5C;// \
        public static byte Caret = 0x5E; // ^
        public static byte Underscore = 0x5F;// _
        public static byte Backtick = 0x60;// `
        public static byte LeftCurlyBracket = 0x7B;// {
        public static byte RightCurlyBracket = 0x7D;// }
        public static byte VerticalBar = 0x7C;// |
        public static byte Tilde = 0x7E;// ~


        //For Checking W80 
        public static readonly byte[] CheckFullCut = new byte[] { 0x1B, 0x69 };//our works the same
        public static readonly byte[] CheckPartialCut = new byte[] { 0x1B, 0x6D };//our works the same

        public static readonly byte[] CheckBoldOn = new byte[] { 0x1B, 0x47 };//Works to turn on
        public static readonly byte[] CheckBoldOff = new byte[] { 0x1B, 0x48 };//Works to turn off

        public static readonly byte[] CheckSetCharSize = new byte[] { 0x1D, 0x21 }; //works the same 

        public static  readonly byte[] BarcodeHeightSelection = new byte[] { 0x1D, 0x77};//these are the same
        public static  readonly byte[] BarcodeWidthSelection = new byte[] { 0x1D, 0x77};//these are the same 

        public static readonly byte[] CheckFineLeadFeed = new byte[] { 0x1B, 0x4A, 0x01 };
        public static readonly byte[] CheckFormFeed = new byte[] { 0xFF};

        public static readonly byte[] CheckRollPrintTest = new byte[] { 0x1D, 0x28, 0x41, 0x02, 0x00, 0x00, 0x02 };//Self Test to be printed out
    }

}
