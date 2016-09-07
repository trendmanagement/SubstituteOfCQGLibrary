using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor
{
    public class ADMDataCommonMethods
    {
        Array optionMonthTypesArray;

        

        public ADMDataCommonMethods()
        {
            Type optionMonthTypes = typeof(OPTION_MONTHS);
            optionMonthTypesArray = Enum.GetNames(optionMonthTypes);
        }

        public void subContractType(ADM_Input_Data admObject,
            String PSUBTY)
        {
            if (PSUBTY.Trim().Length == 0)
            {
                admObject.isFuture = true;

                admObject.callPutOrFuture = OPTION_SPREAD_CONTRACT_TYPE.FUTURE;
            }
            else
            {
                admObject.isFuture = false;

                if (PSUBTY.CompareTo("C") == 0)
                {
                    admObject.callPutOrFuture = OPTION_SPREAD_CONTRACT_TYPE.CALL;
                }
                else
                {
                    admObject.callPutOrFuture = OPTION_SPREAD_CONTRACT_TYPE.PUT;
                }
            }


        }

        public String generateOptionCQGSymbol(ADM_Input_Data admSummaryImport,
            String PSUBTY, DateTime PCTYM,
            Instrument instrument)
        {
            StringBuilder cqgSymbol = new StringBuilder();

#if DEBUG
            try
#endif
            {
                //String typeSymbol = admSummaryImport.PSUBTY;

                if (admSummaryImport.isFuture)
                {
                    cqgSymbol.Append("F");

                    cqgSymbol.Append(".");

                    cqgSymbol.Append(instrument.CQGsymbol);

                    //int year = (Convert.ToInt16(admSummaryImport.PCTYM.Substring(0, 4)) - 2000) % 10;

                    //**********
                    //String dateFormat = "yyyyMM";

                    //DateTime admSummaryYearMonth = DateTime.ParseExact(PCTYM, dateFormat, CultureInfo.InvariantCulture);
                    int year = PCTYM.Year % 100;
                    //**********

                    String monthChar = optionMonthTypesArray.GetValue(PCTYM.Month - 1).ToString();

                    cqgSymbol.Append(monthChar);

                    cqgSymbol.Append(year);
                }
                else
                {
                    cqgSymbol.Append(PSUBTY);

                    cqgSymbol.Append(".");

                    cqgSymbol.Append(instrument.CQGsymbol);

                    //int year = (Convert.ToInt16(admSummaryImport.PCTYM.Substring(0, 4)) - 2000) % 10;

                    //**********
                    //String dateFormat = "yyyyMM";

                    //DateTime admSummaryYearMonth = DateTime.ParseExact(PCTYM, dateFormat, CultureInfo.InvariantCulture);
                    int year = PCTYM.Year % 10;
                    //**********

                    String monthChar = optionMonthTypesArray.GetValue(PCTYM.Month - 1).ToString();

                    cqgSymbol.Append(monthChar);

                    cqgSymbol.Append(year);

                    cqgSymbol.Append(admSummaryImport.strike);
                }

                //TSErrorCatch.debugWriteOut(cqgSymbol.ToString());

                //cqgSymbol.Append(optionBuilderSpreadStructure[spreadCounter]
                //                    .optionBuildLegStructureArray[legCounter]
                //                    .optionBuildLegDataAtRollDateList[legRollListCounter]
                //                    .optionExpirationRollInto.optionMonth.ToString());

                //cqgSymbol.Append((optionBuilderSpreadStructure[spreadCounter]
                //                    .optionBuildLegStructureArray[legCounter]
                //                    .optionBuildLegDataAtRollDateList[legRollListCounter]
                //                    .optionExpirationRollInto.optionYear % 10));

                //cqgSymbol.Append(ConversionAndFormatting.convertToTickMovesString(
                //                        optionBuilderSpreadStructure[spreadCounter]
                //                    .optionBuildLegStructureArray[legCounter]
                //                    .optionBuildLegDataAtRollDateList[legRollListCounter].optionStrikePrice,
                //                        optionBuilderSpreadStructure[spreadCounter].instrument.optionStrikeIncrement,
                //                        optionBuilderSpreadStructure[spreadCounter].instrument.optionStrikeDisplay));
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            return cqgSymbol.ToString();
        }

        
        internal void readADMDetailedPositionImportWeb(String[] fileNames,
            List<ADMPositionImportWeb> admSummaryFieldsList, ImportFileCheck importFileCheck,
            Instrument[] instruments)//, bool[] doNotImportFile)
        {
            BrokerImportFiles brokerImportFiles = BrokerImportFiles.BACKUP_SAVED_FILE;
            FTPInputFileTypes cSVImportFileType = FTPInputFileTypes.ADM_WEB_INTERFACE_FILES;

            importFileCheck.importfile = true;

            //bool importFile = true;

#if DEBUG
            try
#endif
            {
                Type admSummaryFieldTypes = typeof(ADM_DETAIL_FIELDS);
                Array admSummaryFieldTypeArray = Enum.GetNames(admSummaryFieldTypes);

                for (int fileCounter = 0; fileCounter < fileNames.Length; fileCounter++)
                {
                    bool firstLine = true;

                    //if(importFileCheck.importingBackedUpSavedFile)
                    //{
                    //    firstLine = false;
                    //}





                    if (File.Exists(fileNames[fileCounter]))
                    {
                        DateTime lastWriteTime = File.GetLastWriteTime(fileNames[fileCounter]);

                        {
                            FileStream fileStream = new FileStream(fileNames[fileCounter],
                                FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            StreamReader streamReader = new StreamReader(fileStream);
                            
                            //String separator = "\t";

                            if (fileNames[fileCounter].ToLower().EndsWith(".csv"))
                            {
                                

                                string[] parsedFileName = fileNames[fileCounter].Split('\\');

                                if (parsedFileName.Last().Substring(0, 3).ToLower().CompareTo("pos") == 0)
                                {
                                    cSVImportFileType = FTPInputFileTypes.POSITION_FILE_WEDBUSH;

                                    brokerImportFiles = BrokerImportFiles.GMI_IMPORT_CSV_FILES;
                                }
                                else if (parsedFileName.Last().Substring(0, 3).ToLower().CompareTo("prl") == 0)
                                {
                                    cSVImportFileType = FTPInputFileTypes.TRANSACTION_FILE_WEDBUSH;

                                    brokerImportFiles = BrokerImportFiles.GMI_IMPORT_CSV_FILES;
                                }

                                else if (parsedFileName.Last().ToLower().EndsWith("adatpos.csv"))
                                {
                                    cSVImportFileType = FTPInputFileTypes.POSITION_FILE_ADM;

                                    brokerImportFiles = BrokerImportFiles.GMI_IMPORT_CSV_FILES;
                                }
                                else if (parsedFileName.Last().ToLower().EndsWith("aaprlmcsv.csv"))
                                {
                                    cSVImportFileType = FTPInputFileTypes.TRANSACTION_FILE_ADM;

                                    brokerImportFiles = BrokerImportFiles.ADM_WEB_IMPORT_FILES;
                                }
                                else
                                {
                                    importFileCheck.importfile = false;

                                    String caption = "An imported file is not a 'pos' or 'prl' file";
                                    String message = fileNames[fileCounter] + "\nis not a 'pos' or 'prl'";
                                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                                    System.Windows.Forms.DialogResult result;

                                    // Displays the MessageBox.
                                    result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                                }

                            }
                            else
                            {
                                cSVImportFileType = FTPInputFileTypes.ADM_WEB_INTERFACE_FILES;
                                brokerImportFiles = BrokerImportFiles.ADM_WEB_IMPORT_FILES;
                            }

                            while (!(streamReader.EndOfStream) && importFileCheck.importfile)
                            {
                                String line = streamReader.ReadLine();
                                if (line.Length > 0)
                                {
                                    List<String> stringList;  // = readSeparatedLine(line, '\t');


                                    if (cSVImportFileType == FTPInputFileTypes.ADM_WEB_INTERFACE_FILES
                                        || importFileCheck.importingBackedUpSavedFile)
                                    {
                                        stringList = readSeparatedLine(line, '\t');
                                    }
                                    else
                                    {
                                        stringList = readSeparatedLine(line, ',');
                                    }

                                    if (cSVImportFileType == FTPInputFileTypes.POSITION_FILE_ADM
                                        || cSVImportFileType == FTPInputFileTypes.TRANSACTION_FILE_ADM)
                                    {
                                        //there isn't a header line in these 2 file types from ADM ftp
                                        firstLine = false;
                                    }

                                    if (firstLine)
                                    {
                                        firstLine = false;

                                        if (importFileCheck.importingBackedUpSavedFile)
                                        {
                                            brokerImportFiles = BrokerImportFiles.BACKUP_SAVED_FILE;

                                            //if (stringList.Count > 0)
                                            {
                                                //brokerImportFiles = BrokerImportFiles.BACKUP_SAVED_FILE;

                                                //if (stringList[0].CompareTo(
                                                //        Enum.GetName(typeof(BrokerImportFiles), BrokerImportFiles.ADM_WEB_IMPORT_FILES)) == 0)
                                                //{
                                                    
                                                //}
                                                //else if (stringList[0].CompareTo(
                                                //        Enum.GetName(typeof(BrokerImportFiles), BrokerImportFiles.GMI_IMPORT_CSV_FILES)) == 0)
                                                //{
                                                //    brokerImportFiles = BrokerImportFiles.GMI_IMPORT_CSV_FILES;
                                                //}
                                            }

                                        }
                                        //else if (brokerImportFiles == BrokerImportFiles.ADM_FILES)
                                        else if(cSVImportFileType == FTPInputFileTypes.ADM_WEB_INTERFACE_FILES)
                                        {
                                            int stringListCounter = 0;

                                            while (stringListCounter < stringList.Count)
                                            {
                                                if (stringListCounter >= admSummaryFieldTypeArray.Length
                                                    ||
                                                    (stringListCounter < admSummaryFieldTypeArray.Length
                                                    && stringList[stringListCounter].CompareTo(
                                                    admSummaryFieldTypeArray.GetValue(stringListCounter)) != 0))
                                                {
                                                    String caption = "ADM FIELDS HAVE CHANGED, FIELDS DO NOT MATCH PREVIOUS INPUTS";
                                                    String message = fileNames[fileCounter] + "\nADM FIELDS HAVE CHANGED, THERE IS A PROBLEM \nFILE WILL NOT BE IMPORTED";
                                                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                                                    System.Windows.Forms.DialogResult result;

                                                    // Displays the MessageBox.
                                                    result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);

                                                    importFileCheck.importfile = false;

                                                    break;
                                                }

                                                stringListCounter++;
                                            }
                                        }
                                    }
                                    else if ((importFileCheck.importingBackedUpSavedFile
                                        || cSVImportFileType == FTPInputFileTypes.ADM_WEB_INTERFACE_FILES)

                                    && importFileCheck.importfile && stringList.Count > 0
                                    && stringList[(int)ADM_DETAIL_FIELDS.PEXCH].Trim().Length > 0
                                    && stringList[(int)ADM_DETAIL_FIELDS.PFC].Trim().Length > 0
                                    && stringList[(int)ADM_DETAIL_FIELDS.PCTYM].Trim().Length > 0)
                                    {
                                        ADMPositionImportWeb aDMSummaryImport = new ADMPositionImportWeb();

                                        admSummaryFieldsList.Add(aDMSummaryImport);

                                        //aDMSummaryImport.rowInSummaryFieldList = admSummaryFieldsList.Count - 1;

                                        aDMSummaryImport.RecordType = stringList[(int)ADM_DETAIL_FIELDS.RecordType];
                                        aDMSummaryImport.POFFIC = stringList[(int)ADM_DETAIL_FIELDS.POFFIC];
                                        aDMSummaryImport.PACCT = stringList[(int)ADM_DETAIL_FIELDS.PACCT];
                                        aDMSummaryImport.PCUSIP = stringList[(int)ADM_DETAIL_FIELDS.PCUSIP];
                                        aDMSummaryImport.PCUSIP2 = stringList[(int)ADM_DETAIL_FIELDS.PCUSIP2];
                                        aDMSummaryImport.Description = stringList[(int)ADM_DETAIL_FIELDS.Description];

                                        aDMSummaryImport.LongQuantity = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.LongQuantity);
                                        aDMSummaryImport.ShortQuantity = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.ShortQuantity);
                                        //aDMSummaryImport.Net = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.TradeDate);

                                        //aDMSummaryImport.netContractsEditable = aDMSummaryImport.Net;

                                        aDMSummaryImport.TradeDate = stringList[(int)ADM_DETAIL_FIELDS.TradeDate];
                                        aDMSummaryImport.TradePrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.TradePrice);

                                        aDMSummaryImport.WeightedPrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.WeightedPrice);

                                        //aDMSummaryImport.AveragePrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.AveragePrice);

                                        aDMSummaryImport.RealTimePrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.RealTimePrice);
                                        aDMSummaryImport.SettledPrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.SettledPrice);
                                        aDMSummaryImport.PrelimPrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.PrelimPrice);
                                        aDMSummaryImport.Value = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.Value);
                                        aDMSummaryImport.ClosedValue = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.ClosedValue);
                                        aDMSummaryImport.SettledValue = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.SettledValue);

                                        aDMSummaryImport.Currency = stringList[(int)ADM_DETAIL_FIELDS.Currency];
                                        aDMSummaryImport.PSUBTY = stringList[(int)ADM_DETAIL_FIELDS.PSUBTY];
                                        aDMSummaryImport.PEXCH = getIntOutOfStringList(stringList, ADM_DETAIL_FIELDS.PEXCH);
                                        aDMSummaryImport.PFC = stringList[(int)ADM_DETAIL_FIELDS.PFC];
                                        aDMSummaryImport.aDMStrike = stringList[(int)ADM_DETAIL_FIELDS.Strike];

                                        aDMSummaryImport.PCTYM = stringList[(int)ADM_DETAIL_FIELDS.PCTYM];

                                        aDMSummaryImport.PCARD = stringList[(int)ADM_DETAIL_FIELDS.PCARD];


                                        setupInstrumentAndfillStrikeInDecimal(aDMSummaryImport,
                                            instruments, brokerImportFiles);
                                    }
                                    else if (cSVImportFileType == FTPInputFileTypes.POSITION_FILE_WEDBUSH
                                        && stringList.Count > 283
                                        &&
                                        (stringList[0].CompareTo("C") != 0))
                                    {
                                        ADMPositionImportWeb aDMSummaryImport = new ADMPositionImportWeb();

                                        admSummaryFieldsList.Add(aDMSummaryImport);

                                        if (stringList[0].CompareTo("P") == 0)
                                        {
                                            aDMSummaryImport.RecordType = "Position";
                                        }
                                        else
                                        {
                                            aDMSummaryImport.RecordType = "Transaction";
                                        }


                                        aDMSummaryImport.POFFIC = stringList[2];
                                        aDMSummaryImport.PACCT = stringList[3];
                                        aDMSummaryImport.PCUSIP = stringList[5];
                                        aDMSummaryImport.PCUSIP2 = stringList[6];
                                        aDMSummaryImport.Description = stringList[77];

                                        aDMSummaryImport.LongQuantity = Convert.ToDouble(stringList[274]);
                                        aDMSummaryImport.ShortQuantity = Convert.ToDouble(stringList[275]);

                                        aDMSummaryImport.TradeDate = stringList[16];
                                        aDMSummaryImport.TradePrice = Convert.ToDouble(stringList[17]);

                                        aDMSummaryImport.WeightedPrice = 0;

                                        //aDMSummaryImport.AveragePrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.AveragePrice);

                                        aDMSummaryImport.RealTimePrice = 0;
                                        aDMSummaryImport.SettledPrice = 0;
                                        aDMSummaryImport.PrelimPrice = 0;
                                        aDMSummaryImport.Value = 0;
                                        aDMSummaryImport.ClosedValue = 0;
                                        aDMSummaryImport.SettledValue = 0;

                                        aDMSummaryImport.Currency = stringList[283];
                                        aDMSummaryImport.PSUBTY = stringList[10];
                                        aDMSummaryImport.PEXCH = Convert.ToInt32(stringList[97]);
                                        aDMSummaryImport.PFC = stringList[98];
                                        aDMSummaryImport.aDMStrike = stringList[12];

                                        aDMSummaryImport.PCTYM = stringList[7];

                                        aDMSummaryImport.PCARD = stringList[139];

                                        setupInstrumentAndfillStrikeInDecimal(aDMSummaryImport,
                                            instruments, brokerImportFiles);
                                    }
                                    else if (cSVImportFileType == FTPInputFileTypes.TRANSACTION_FILE_WEDBUSH
                                        && stringList.Count > 64
                                        &&
                                        (stringList[1].CompareTo("C") != 0))
                                    {
                                        ADMPositionImportWeb aDMSummaryImport = new ADMPositionImportWeb();

                                        admSummaryFieldsList.Add(aDMSummaryImport);

                                        if (stringList[1].CompareTo("T") == 0)
                                        {
                                            aDMSummaryImport.RecordType = "Transaction";
                                        }
                                        else
                                        {
                                            aDMSummaryImport.RecordType = "Position";
                                        }


                                        aDMSummaryImport.POFFIC = stringList[3];
                                        aDMSummaryImport.PACCT = stringList[5];
                                        aDMSummaryImport.PCUSIP = "";
                                        aDMSummaryImport.PCUSIP2 = "";
                                        aDMSummaryImport.Description = stringList[17];

                                        aDMSummaryImport.LongQuantity = Convert.ToDouble(stringList[43]);
                                        aDMSummaryImport.ShortQuantity = Convert.ToDouble(stringList[44]);

                                        aDMSummaryImport.TradeDate = stringList[40];
                                        aDMSummaryImport.TradePrice = Convert.ToDouble(stringList[49]);

                                        aDMSummaryImport.WeightedPrice = 0;

                                        aDMSummaryImport.RealTimePrice = Convert.ToDouble(stringList[27]);
                                        aDMSummaryImport.SettledPrice = 0;
                                        aDMSummaryImport.PrelimPrice = 0;
                                        aDMSummaryImport.Value = 0;
                                        aDMSummaryImport.ClosedValue = 0;
                                        aDMSummaryImport.SettledValue = 0;

                                        aDMSummaryImport.Currency = stringList[36];
                                        aDMSummaryImport.PSUBTY = stringList[23];
                                        aDMSummaryImport.PEXCH = Convert.ToInt32(stringList[14]);
                                        aDMSummaryImport.PFC = stringList[16];
                                        aDMSummaryImport.aDMStrike = stringList[24];

                                        aDMSummaryImport.PCTYM = stringList[20];

                                        aDMSummaryImport.PCARD = stringList[64];

                                        setupInstrumentAndfillStrikeInDecimal(aDMSummaryImport,
                                            instruments, brokerImportFiles);
                                    }
                                    else if (cSVImportFileType == FTPInputFileTypes.POSITION_FILE_ADM
                                        && 
                                        (stringList[0].CompareTo("C") != 0) )
                                    {
                                        ADMPositionImportWeb aDMSummaryImport = new ADMPositionImportWeb();

                                        admSummaryFieldsList.Add(aDMSummaryImport);

                                        if (stringList[0].CompareTo("P") == 0)
                                        {
                                            aDMSummaryImport.RecordType = "Position";
                                        }
                                        else
                                        {
                                            aDMSummaryImport.RecordType = "Transaction";
                                        }


                                        aDMSummaryImport.POFFIC = stringList[2];
                                        aDMSummaryImport.PACCT = stringList[3];
                                        aDMSummaryImport.PCUSIP = stringList[5];
                                        aDMSummaryImport.PCUSIP2 = "";
                                        aDMSummaryImport.Description = stringList[20];

                                        aDMSummaryImport.LongQuantity = Convert.ToDouble(stringList[71]);
                                        aDMSummaryImport.ShortQuantity = Convert.ToDouble(stringList[72]);

                                        aDMSummaryImport.TradeDate = stringList[13];


                                        aDMSummaryImport.TradePrice = Convert.ToDouble(stringList[14]);



                                        aDMSummaryImport.WeightedPrice = 0;

                                        //aDMSummaryImport.AveragePrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.AveragePrice);

                                        aDMSummaryImport.RealTimePrice = 0;
                                        aDMSummaryImport.SettledPrice = 0;
                                        aDMSummaryImport.PrelimPrice = 0;
                                        aDMSummaryImport.Value = 0;
                                        aDMSummaryImport.ClosedValue = 0;
                                        aDMSummaryImport.SettledValue = 0;

                                        aDMSummaryImport.Currency = stringList[76];
                                        aDMSummaryImport.PSUBTY = stringList[9];
                                        aDMSummaryImport.PEXCH = Convert.ToInt32(stringList[29]);
                                        aDMSummaryImport.PFC = stringList[30];
                                        aDMSummaryImport.aDMStrike = stringList[11];

                                        aDMSummaryImport.PCTYM = stringList[6];

                                        aDMSummaryImport.PCARD = stringList[53];

                                        setupInstrumentAndfillStrikeInDecimal(aDMSummaryImport,
                                            instruments, brokerImportFiles);
                                    }
                                    else if (cSVImportFileType == FTPInputFileTypes.TRANSACTION_FILE_ADM
                                        && stringList.Count >= 38
                                        && 
                                        (stringList[0].CompareTo("C") != 0) )
                                    {
                                        DateTime tradeDateTest = new DateTime(Convert.ToInt16(stringList[12]),
                                            Convert.ToInt16(stringList[10]), 
                                            Convert.ToInt16(stringList[11]));



                                        if (stringList[0].CompareTo("T") == 0
                                            && tradeDateTest.CompareTo(DateTime.Now.Date) != 0)
                                        {
                                            //skip
                                        }
                                        else
                                        {

                                            ADMPositionImportWeb aDMSummaryImport = new ADMPositionImportWeb();

                                            admSummaryFieldsList.Add(aDMSummaryImport);

                                            if (stringList[0].CompareTo("P") == 0)
                                            {
                                                aDMSummaryImport.RecordType = "Position";
                                            }
                                            else
                                            {
                                                aDMSummaryImport.RecordType = "Transaction";
                                            }


                                            aDMSummaryImport.POFFIC = stringList[2];
                                            aDMSummaryImport.PACCT = stringList[3];
                                            aDMSummaryImport.PCUSIP = stringList[5];
                                            aDMSummaryImport.PCUSIP2 = "";
                                            aDMSummaryImport.Description = stringList[25];

                                            if (stringList[14].Trim().Length > 0)
                                            {
                                                aDMSummaryImport.LongQuantity = Convert.ToDouble(stringList[14]);
                                            }

                                            if (stringList[15].Trim().Length > 0)
                                            {
                                                aDMSummaryImport.ShortQuantity = Convert.ToDouble(stringList[15]);
                                            }

                                            StringBuilder tradeDate = new StringBuilder();
                                            tradeDate.Append(stringList[12]);
                                            tradeDate.Append(stringList[10]);
                                            tradeDate.Append(stringList[11]);

                                            aDMSummaryImport.TradeDate = tradeDate.ToString();


                                            aDMSummaryImport.TradePrice = Convert.ToDouble(stringList[29]);



                                            aDMSummaryImport.WeightedPrice = 0;

                                            //aDMSummaryImport.AveragePrice = getDoubleOutOfStringList(stringList, ADM_DETAIL_FIELDS.AveragePrice);

                                            aDMSummaryImport.RealTimePrice = 0;
                                            aDMSummaryImport.SettledPrice = 0;
                                            aDMSummaryImport.PrelimPrice = 0;
                                            aDMSummaryImport.Value = 0;
                                            aDMSummaryImport.ClosedValue = 0;
                                            aDMSummaryImport.SettledValue = 0;

                                            aDMSummaryImport.Currency = stringList[32];
                                            aDMSummaryImport.PSUBTY = stringList[7];
                                            aDMSummaryImport.PEXCH = Convert.ToInt32(stringList[27]);
                                            aDMSummaryImport.PFC = stringList[26];

                                            if (stringList[8].Trim().Length > 0)
                                            {
                                                aDMSummaryImport.aDMStrike = stringList[8].Trim();
                                            }
                                            else
                                            {
                                                aDMSummaryImport.aDMStrike = "0";
                                            }

                                            aDMSummaryImport.PCTYM = stringList[6];

                                            aDMSummaryImport.PCARD = stringList[13];

                                            setupInstrumentAndfillStrikeInDecimal(aDMSummaryImport,
                                                instruments, brokerImportFiles);
                                        }
                                    }

                                }
                            }

                            streamReader.Close();
                            fileStream.Close();

                        }

                        

                    }
                }
                //                 else
                //                 {
                //                     String caption = "ADM FIELDS HAVE CHANGED, FIELDS DO NOT MATCH PREVIOUS INPUTS";
                //                     String message = fileName + "\nTHERE IS A PROBLEM \nFILE WILL NOT BE IMPORTED";
                //                     MessageBoxButtons buttons = MessageBoxButtons.OK;
                //                     System.Windows.Forms.DialogResult result;
                // 
                //                     // Displays the MessageBox.
                //                     result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                //                 }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            if (!importFileCheck.importfile)
            {
                admSummaryFieldsList.Clear();
            }

            //return importFile;
        }


        internal void setupInstrumentAndfillStrikeInDecimal(ADMPositionImportWeb aDMSummaryImport,
            Instrument[] instruments, BrokerImportFiles brokerImportFiles)
        {

            if(aDMSummaryImport.PFC.CompareTo("05") == 0)
            {
                aDMSummaryImport.PFC = "S-";
            }
            else if (aDMSummaryImport.PFC.CompareTo("02") == 0)
            {
                aDMSummaryImport.PFC = "C-";
            }

            int instrumentCounter = 0;
            while (instrumentCounter < instruments.Length)
            {
                if (aDMSummaryImport.PFC.CompareTo(instruments[instrumentCounter].admCode) == 0)
                {
                    break;
                }

                instrumentCounter++;
            }

            if (instrumentCounter >= instruments.Length)
            {
                instrumentCounter = 0;
            }

            aDMSummaryImport.instrumentArrayIdx = instrumentCounter;

            aDMSummaryImport.instrument = instruments[instrumentCounter];



            subContractType((ADM_Input_Data)aDMSummaryImport,
                aDMSummaryImport.PSUBTY);

            
            if (brokerImportFiles == BrokerImportFiles.GMI_IMPORT_CSV_FILES)
            {
                aDMSummaryImport.strikeInDecimal = Convert.ToDouble(aDMSummaryImport.aDMStrike)
                    * instruments[instrumentCounter].admFuturePriceFactor;
            }
            else if (brokerImportFiles == BrokerImportFiles.ADM_WEB_IMPORT_FILES)
            {
                aDMSummaryImport.strikeInDecimal =
                    ConversionAndFormatting.convertToTickMovesDouble(aDMSummaryImport.aDMStrike,
                        instruments[instrumentCounter].optionStrikeIncrement,
                        instruments[instrumentCounter].optionADMStrikeDisplay);
            }
            else if (brokerImportFiles == BrokerImportFiles.BACKUP_SAVED_FILE)
            {
                aDMSummaryImport.strikeInDecimal = Convert.ToDouble(aDMSummaryImport.aDMStrike);
            }


            aDMSummaryImport.strike =
                    ConversionAndFormatting.convertToTickMovesString(aDMSummaryImport.strikeInDecimal,
                        instruments[aDMSummaryImport.instrumentArrayIdx].optionStrikeIncrement,
                        instruments[aDMSummaryImport.instrumentArrayIdx].optionStrikeDisplay);
            
        }


        private double getDoubleOutOfStringList(List<String> stringList, ADM_DETAIL_FIELDS summaryField)
        {
#if DEBUG
            try
#endif
            {
                return Convert.ToDouble(stringList[(int)summaryField]);
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
            return 0;
        }

        private int getIntOutOfStringList(List<String> stringList, ADM_DETAIL_FIELDS summaryField)
        {
#if DEBUG
            try
#endif
            {
                return Convert.ToInt32(stringList[(int)summaryField]);
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
            return 0;
        }

        private double getDoubleOutOfStringList(List<String> stringList, ADM_SUMMARY_FIELDS summaryField)
        {
#if DEBUG
            try
#endif
            {
                return Convert.ToDouble(stringList[(int)summaryField]);
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
            return 0;
        }

        private int getIntOutOfStringList(List<String> stringList, ADM_SUMMARY_FIELDS summaryField)
        {
#if DEBUG
            try
#endif
            {
                return Convert.ToInt32(stringList[(int)summaryField]);
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
            return 0;
        }

        private List<String> readStringToParse(String separatedLine, char separator)
        {
            List<String> listOfStrings = new List<string>();

#if DEBUG
            try
#endif
            {
                String[] separatedArray = separatedLine.Split(separator);

                for (int arrayCnt = 0; arrayCnt < separatedArray.Length; arrayCnt++)
                {
                    String x = separatedArray[arrayCnt].Replace("\"", "");

                    listOfStrings.Add(x); //separatedArray[arrayCnt].Trim());
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            return listOfStrings;
        }

        private List<String> readSeparatedLine(String separatedLine, char separator)
        {
            List<String> listOfStrings = new List<string>();

#if DEBUG
            try
#endif
            {
                String[] separatedArray = separatedLine.Split(separator);

                for (int arrayCnt = 0; arrayCnt < separatedArray.Length - 1; arrayCnt++)
                {
                    String x = separatedArray[arrayCnt].Replace("\"", "");

                    listOfStrings.Add(x); //separatedArray[arrayCnt].Trim());
                }

                

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            return listOfStrings;
        }

        public String getNameOfADMPositionImportWebStored()
        {
            return System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                        TradingSystemConstants.FCM_DATA_FOLDER, TradingSystemConstants.FCM_STORED_DATA_FILE);
        }

        public DateTime getFileDateTimeOfADMPositionsWeb(String nameOfStoredFile)
        {
            try
            {
                if (File.Exists(nameOfStoredFile))
                {
                    return File.GetLastWriteTime(nameOfStoredFile);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return new DateTime(1900, 1, 1);
        }

        public void copyADMStoredDataFile(String[] admInFileNames)
        {
            try
            {
                //                 String fullFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                //                         TradingSystemConstants.ADM_DATA_FOLDER, TradingSystemConstants.ADM_STORED_DATA_FILE);

                String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), TradingSystemConstants.FCM_DATA_FOLDER);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                //System.IO.File.Copy(admInFileName, getNameOfADMPositionImportWebStored(), true);

                //FileOptions fo = new FileOptions();


                System.IO.StreamWriter fs = new System.IO.StreamWriter(getNameOfADMPositionImportWebStored());

                for (int fileCounter = 0; fileCounter < admInFileNames.Length; fileCounter++)
                {

                    bool firstLine = true;



                    if (File.Exists(admInFileNames[fileCounter]))
                    {
                        FileStream fileStream = new FileStream(admInFileNames[fileCounter],
                            FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        StreamReader streamReader = new StreamReader(fileStream);

                        while (!(streamReader.EndOfStream))
                        {
                            String line = streamReader.ReadLine();

                            if (firstLine)
                            {
                                if (fileCounter == 0)
                                {
                                    fs.WriteLine(line);
                                }

                                firstLine = false;

                            }
                            else
                            {
                                fs.WriteLine(line);
                            }


                        }

                        streamReader.Close();

                        fileStream.Close();
                    }
                }

                fs.Close();
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        internal void copyADMDataToFile(List<ADMPositionImportWeb> admPositionImportWeb, ImportFileCheck importFileCheck)
        {
            try
            {
                //                 String fullFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                //                         TradingSystemConstants.ADM_DATA_FOLDER, TradingSystemConstants.ADM_STORED_DATA_FILE);

                String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), TradingSystemConstants.FCM_DATA_FOLDER);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }


                System.IO.StreamWriter fs = new System.IO.StreamWriter(getNameOfADMPositionImportWebStored());

                bool firstLine = true;

                StringBuilder stringOut = new StringBuilder();

                for (int lineCounter = 0; lineCounter < admPositionImportWeb.Count; lineCounter++)
                {
                    stringOut.Clear();

                    if (firstLine)
                    {
                        stringOut.Append(Enum.GetName(typeof(BrokerImportFiles), BrokerImportFiles.BACKUP_SAVED_FILE));
                        stringOut.Append("\t");

                        fs.WriteLine(stringOut.ToString());

                        stringOut.Clear();

                        firstLine = false;
                    }




                    stringOut.Append(admPositionImportWeb[lineCounter].RecordType);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].POFFIC);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PACCT);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PCUSIP);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PCUSIP2);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].Description);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].LongQuantity);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].ShortQuantity);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].TradeDate);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].TradePrice / admPositionImportWeb[lineCounter].instrument.admFuturePriceFactor);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].WeightedPrice);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].RealTimePrice);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].SettledPrice);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PrelimPrice);

                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].Value);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].ClosedValue);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].SettledValue);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].Currency);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PSUBTY);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PEXCH);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PFC);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].strikeInDecimal);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PCTYM);
                    stringOut.Append("\t");
                    stringOut.Append(admPositionImportWeb[lineCounter].PCARD);
                    stringOut.Append("\t");


                    fs.WriteLine(stringOut.ToString());


                }

                fs.Close();
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void copyADMPositionImportWeb(ADMPositionImportWeb copyInto, ADMPositionImportWeb copyFrom)
        {
            copyInto.MODEL_OFFICE_ACCT = copyFrom.MODEL_OFFICE_ACCT;
            copyInto.acctGroup = copyFrom.acctGroup;

            copyInto.dateTime = copyFrom.dateTime;

            copyInto.instrumentArrayIdx = copyFrom.instrumentArrayIdx;

            copyInto.instrument = copyFrom.instrument;

            copyInto.cqgSymbol = copyFrom.cqgSymbol;

            copyInto.isFuture = copyFrom.isFuture;

            copyInto.callPutOrFuture = copyFrom.callPutOrFuture;

            copyInto.strikeInDecimal = copyFrom.strikeInDecimal;

            copyInto.strike = copyFrom.strike;

            copyInto.contractInfo = copyFrom.contractInfo;
            copyInto.contractData = copyFrom.contractData;



            copyInto.modelLots = copyFrom.modelLots;
            copyInto.orderLots = copyFrom.orderLots;
            copyInto.rollIntoLots = copyFrom.rollIntoLots;
            copyInto.rebalanceLots = copyFrom.rebalanceLots;
            copyInto.liveADMRowIdx = copyFrom.liveADMRowIdx;
            copyInto.netContractsEditable = copyFrom.netContractsEditable;


            copyInto.transAvgLongPrice = copyFrom.transAvgLongPrice;
            copyInto.transAvgShortPrice = copyFrom.transAvgShortPrice;

            copyInto.transNetLong = copyFrom.transNetLong;
            copyInto.transNetShort = copyFrom.transNetShort;
            copyInto.Net = copyFrom.Net;



            copyInto.RecordType = copyFrom.RecordType;

            copyInto.POFFIC = copyFrom.POFFIC;
            copyInto.PACCT = copyFrom.PACCT;

            copyInto.PCUSIP = copyFrom.PCUSIP;
            copyInto.PCUSIP2 = copyFrom.PCUSIP2;
            copyInto.Description = copyFrom.Description;
            copyInto.LongQuantity = copyFrom.LongQuantity;
            copyInto.ShortQuantity = copyFrom.ShortQuantity;



            //copyInto.netContractsEditable = copyFrom.netContractsEditable;

            copyInto.WeightedPrice = copyFrom.WeightedPrice;

            //copyInto.AveragePrice = copyFrom.AveragePrice;

            copyInto.RealTimePrice = copyFrom.RealTimePrice;
            copyInto.SettledPrice = copyFrom.SettledPrice;
            copyInto.PrelimPrice = copyFrom.PrelimPrice;
            copyInto.Value = copyFrom.Value;
            copyInto.ClosedValue = copyFrom.ClosedValue;
            copyInto.SettledValue = copyFrom.SettledValue;
            copyInto.Currency = copyFrom.Currency;
            copyInto.PSUBTY = copyFrom.PSUBTY;
            copyInto.PEXCH = copyFrom.PEXCH;
            copyInto.PFC = copyFrom.PFC;

            copyInto.aDMStrike = copyFrom.aDMStrike;

            copyInto.PCTYM = copyFrom.PCTYM;
            copyInto.PCTYM_dateTime = copyFrom.PCTYM_dateTime;

            copyInto.PCARD = copyFrom.PCARD;
        }

        public void saveADMStrategyInfo(OptionSpreadManager optionSpreadManager)
        {
            String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY);

            SaveOutputFile sof = new SaveOutputFile(dir);

            StringBuilder admFileName = new StringBuilder();

            admFileName.Append(optionSpreadManager.initializationParmsSaved.idPortfolioGroup);
            admFileName.Append("_");
            admFileName.Append(TradingSystemConstants.FCM_EXCLUDE_CONTRACT_INFO_FILE);


            sof.createConfigFile(admFileName.ToString());

            ////sof.writeTextLineFile("test");

            createADMLiveInfoLine(optionSpreadManager, sof);

            sof.closeAndSaveFile();


        }

        


        public void readADMExcludeContractInfo(OptionSpreadManager optionSpreadManager)
        {
            StringBuilder admFileName = new StringBuilder();

            admFileName.Append(optionSpreadManager.initializationParmsSaved.idPortfolioGroup);
            admFileName.Append("_");
            admFileName.Append(TradingSystemConstants.FCM_EXCLUDE_CONTRACT_INFO_FILE);

            String fileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY,
                admFileName.ToString());

            SaveOutputFile sof = new SaveOutputFile();

            sof.openReadFile(fileName);

            String line = sof.readStoredADMStrategyInfoList_1Line();

            //int spreadId = -1;

            while (line != null)
            {
                List<String> separatedLine = readSeparatedLine(line, ',');

                if (separatedLine.Count <= 2)
                {
                    optionSpreadManager.zeroPriceContractList.Add(separatedLine[0]);
                }
                else if (separatedLine.Count > 2)
                {
                    if (Convert.ToBoolean(separatedLine[1]))
                    {
                        optionSpreadManager.zeroPriceContractList.Add(separatedLine[0]);
                    }
                    else if (Convert.ToBoolean(separatedLine[2]))
                    {
                        optionSpreadManager.exceptionContractList.Add(separatedLine[0]);
                    }
                }

                //spreadId =
                //    readLineFillADMLiveStrategyInfo(optionSpreadManager.liveADMStrategyInfoList, separatedLine, spreadId);

                line = sof.readStoredADMStrategyInfoList_1Line();
            }

            sof.closeReadingStreams();
        }

        

        private void createADMLiveInfoLine(OptionSpreadManager optionSpreadManager,
            SaveOutputFile sof)
        {
            StringBuilder csvLine = new StringBuilder();

            //optionSpreadManager.admPositionImportWebListForCompare

            for (int instrumentCount = 0;
                instrumentCount < optionSpreadManager.admPositionImportWebListForCompare.Count;
                instrumentCount++)
            {

                //if (optionSpreadManager.admPositionImportWebListForCompare[instrumentCount].exclude)
                if (optionSpreadManager.zeroPriceContractList.Contains(
                        optionSpreadManager.admPositionImportWebListForCompare[instrumentCount].cqgSymbol))
                {

                    csvLine.Clear();

                    csvLine.Append(optionSpreadManager.admPositionImportWebListForCompare[instrumentCount].cqgSymbol);
                    csvLine.Append(",true,false,");

                    sof.writeTextLineFile(csvLine.ToString());
                }
                else if (optionSpreadManager.exceptionContractList.Contains(
                        optionSpreadManager.admPositionImportWebListForCompare[instrumentCount].cqgSymbol))
                {

                    csvLine.Clear();

                    csvLine.Append(optionSpreadManager.admPositionImportWebListForCompare[instrumentCount].cqgSymbol);
                    csvLine.Append(",false,true,");

                    sof.writeTextLineFile(csvLine.ToString());
                }
            }

            
        }

        
    }
}
