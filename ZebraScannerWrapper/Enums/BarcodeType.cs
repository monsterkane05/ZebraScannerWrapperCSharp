using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraScannerWrapper.Enums
{
    public enum BarcodeType
    {
        UNKNOWN = 0,
        CODE_39 = 1,
        CODABAR = 2,
        CODE_128 = 3,
        DISCRETE_2_OF_5 = 4,
        IATA = 5,
        INTERLEAVED_2_OF_5 = 6,
        CODE_93 = 7,
        UPC_A = 8,
        UPC_E0 = 9,
        EAN_8 = 10,
        EAN_13 = 11,
        CODE_11 = 12,
        CODE_49 = 13,
        MSI = 14,
        EAN_128 = 15,
        UPC_E1 = 16,
        PDF_417 = 17,
        CODE_16K = 18,
        CODE_39_FULL_ASCII = 19,
        UPC_D = 20,
        CODE_39_TRIOPTIC = 21,
        BOOKLAND = 22,
        COUPON_CODE = 23,
        NW_7 = 24,
        ISBT_128 = 25,
        MICRO_PDF = 26,
        DATAMATRIX = 27,
        QR_CODE = 28,
        MICRO_PDF_CCA = 29,
        POSTNET_US = 30,
        PLANET_CODE = 31,
        CODE_32 = 32,
        ISBT_128_CON = 33,
        JAPAN_POSTAL = 34,
        AUSTRALIAN_POSTAL = 35,
        DUTCH_POSTAL = 36,
        MAXICODE = 37,
        CANADIAN_POSTAL = 38,
        UK_POSTAL = 39,
        MICRO_QR_CODE = 44,
        AZTEC = 45,
        GS1_DATABAR = 48,
        RSS_LIMITED = 49,
        GS1_DATABAR_EXPANDED = 50,
        SCANLET = 55,
        UPC_A_PLUS_2 = 72,
        UPC_E0_PLUS_2 = 73,
        EAN_8_PLUS_2 = 74,
        EAN_13_PLUS_2 = 75,
        UPC_E1_PLUS_2 = 80,
        CCA_EAN_128 = 81,
        CCA_EAN_13 = 82,
        CCA_EAN_8 = 83,
        CCA_RSS_EXPANDED = 84,
        CCA_RSS_LIMITED = 85,
        CCA_RSS_14 = 86,
        CCA_UPC_A = 87,
        CCA_UPC_E = 88,
        CCC_EAN_128 = 89,
        TLC_39 = 90,
        CCB_EAN_128 = 97,
        CCB_EAN_13 = 98,
        CCB_EAN_8 = 99,
        CCB_RSS_EXPANDED = 100,
        CCB_RSS_LIMITED = 101,
        CCB_RSS_14 = 102,
        CCB_UPC_A = 103,
        CCB_UPC_E = 104,
        SIGNATURE_CAPTURE = 105,
        MATRIX_2_OF_5 = 113,
        CHINESE_2_OF_5 = 114,
        UPC_A_PLUS_5 = 136,
        UPC_E0_PLUS_5 = 137,
        EAN_8_PLUS_5 = 138,
        EAN_13_PLUS_5 = 139,
        UPC_E1_PLUS_5 = 144
    }

    public class BarcodeTypeConvertor()
    {
        public static BarcodeType GetBarcodeTypeFromNixdorfB(int n) 
        {
            switch (n)
            {
                case 1:
                    return BarcodeType.CODE_39;
                case 2:
                    return BarcodeType.CODABAR;
                case 3:
                    return BarcodeType.CODE_128;
                case 4:
                    return BarcodeType.DISCRETE_2_OF_5;
                case 6:
                    return BarcodeType.INTERLEAVED_2_OF_5;
                case 7:
                    return BarcodeType.CODE_93;
                case 8:
                    return BarcodeType.UPC_A;
                case 9:
                    return BarcodeType.UPC_E0;
                case 10:
                    return BarcodeType.EAN_8;
                case 14:
                    return BarcodeType.MSI;
                case 15:
                    return BarcodeType.EAN_128;
                case 17:
                    return BarcodeType.PDF_417;
                case 26:
                    return BarcodeType.MICRO_PDF;
                case 27:
                    return BarcodeType.DATAMATRIX;
                case 28:
                    return BarcodeType.QR_CODE;
                case 37:
                    return BarcodeType.MAXICODE;
                case 45:
                    return BarcodeType.AZTEC;
                case 48:
                    return BarcodeType.GS1_DATABAR;
                case 49:
                    return BarcodeType.RSS_LIMITED;
                case 50:
                    return BarcodeType.GS1_DATABAR_EXPANDED;
                default:
                    return BarcodeType.UNKNOWN;
            }
        }
    }
}
