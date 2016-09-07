using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Concurrent;

namespace RealtimeSpreadMonitor
{
    public class SaveOutputFile
    {
        static int padBuffer = 20;
        static String dir;

        String fileName;

        FileStream fileStream;

        StreamReader streamReader;

        StreamWriter streamWriter;

        public SaveOutputFile()
        {

        }

        public SaveOutputFile(String fullOutputDirectory, bool setFullOutputDirectory)
        {
            try
            {
                dir = fullOutputDirectory;

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public SaveOutputFile(String finalFolder)
        {
            try
            {
                dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), finalFolder);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public SaveOutputFile(String groupFolder, String currentDate)
        {
            try
            {
                dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), groupFolder, currentDate);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public SaveOutputFile(String groupFolder, String currentDate, String finalFolder)
        {
            try
            {
                //dir = Directory.GetCurrentDirectory() + "\\" + groupFolder + "\\" + currentDate + "\\" + finalFolder + "\\";

                dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), groupFolder, currentDate, finalFolder);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        ~SaveOutputFile()
        {
            try
            {
                //streamWriter.Close();
                //fileStream.Close();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void createFile(String fileName)
        {
            try
            {
                this.fileName = fileName;

                fileStream = new FileStream(dir + fileName,
                    FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                streamWriter = new StreamWriter(fileStream);

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public bool createFile(String fileName, String instrument, String studyName)
        {
            try
            {
                this.fileName = fileName;
                //this.instrument = instrument;
                
                fileStream = new FileStream(dir + fileName + ".txt",
                    FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                streamWriter = new StreamWriter(fileStream);

                return true;

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return false;
        }

        public bool createCSVFile(String fileName)
        {
            try
            {
                this.fileName = fileName;
                //this.instrument = instrument;
                //this.studyName = studyName;

                //File.Exists(fullFile)

                //String fileNameFull = dir + fileName + ".csv";

                //                 if(File.Exists(fileNameFull))
                //                 {
                //                     File.Open(fileNameFull);
                //                     
                //                 }


                fileStream = new FileStream(System.IO.Path.Combine(dir, fileName),
                    FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                streamWriter = new StreamWriter(fileStream);

                return true;

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return false;
        }

        public void createConfigFile(String fileName)
        {
            try
            {
                this.fileName = fileName;
                //this.instrument = instrument;
                //this.studyOut = studyOut;

                fileStream = new FileStream(System.IO.Path.Combine(dir, fileName),
                    FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                streamWriter = new StreamWriter(fileStream);

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        

        public void writeLnToFile(DateTime dateTimeOut, double arrayValue)
        {
            try
            {
                String dateLine = dateTimeOut.ToString("MM/dd/yy", DateTimeFormatInfo.InvariantInfo);
                String timeLine = dateTimeOut.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
                String lineOfArray = dateLine.PadRight(padBuffer, ' ') + "\t"
                    + timeLine.PadRight(padBuffer, ' ')
                    + "\t" + arrayValue.ToString().PadRight(padBuffer, ' ');
                streamWriter.WriteLine(lineOfArray);

                streamWriter.Flush();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void modifyLnInFile(DateTime dateTimeOut, double arrayValue)
        {
            try
            {
                String dateLine = dateTimeOut.ToString("MM/dd/yy", DateTimeFormatInfo.InvariantInfo);
                String timeLine = dateTimeOut.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
                String lineOfArray = dateLine.PadRight(padBuffer, ' ') + "\t"
                    + timeLine.PadRight(padBuffer, ' ')
                    + "\t" + arrayValue.ToString().PadRight(padBuffer, ' ') + "\r\n";


                streamWriter.Write(lineOfArray);

                streamWriter.Flush();
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void writeLnToEnd(DateTime dateTimeOut, double arrayValue)//DateTime arrayDateTime, double arrayValues)
        {
            try
            {
                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                writeLnToFile(dateTimeOut, arrayValue);
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void ModifyFile(DateTime dateTimeOut, double arrayValue, int lineNum)
        {
            try
            {
                int offset = (padBuffer * 3 + 4) * lineNum;


                fileStream.Seek(offset, SeekOrigin.Begin);


                modifyLnInFile(dateTimeOut, arrayValue);

                fileStream.SetLength(offset);

                streamWriter.BaseStream.Seek(0, SeekOrigin.End);

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }










        public void writeConfigLineFile(String label, double value)
        {
            try
            {
                String lineOut;

                lineOut = label + "=" + value + "\r\n";

                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                streamWriter.Write(lineOut);

                streamWriter.Flush();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void writeConfigLineFile(String label, String value)
        {
            try
            {
                String lineOut;

                lineOut = label + "=" + value + "\r\n";

                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                streamWriter.Write(lineOut);

                streamWriter.Flush();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }


        public void writeTextLineFile(String text)
        {
            try
            {
                String lineOut;

                lineOut = text + "\r\n";

                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                streamWriter.Write(lineOut);

                streamWriter.Flush();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void closeAndSaveFile()
        {
            try
            {
                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void closeReadingStreams()
        {
            if (fileStream != null)
                fileStream.Close();

            if (streamReader != null)
                streamReader.Close();
        }

        public void openReadFile(String fileName)
        {
            if (File.Exists(fileName))
            {
                fileStream = new FileStream(fileName,
                        FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    
                streamReader = new StreamReader(fileStream);
            }
        }

        public String readStoredADMStrategyInfoList_1Line()
        {
            try
            {
                if (streamReader != null && !streamReader.EndOfStream)
                {
                        return streamReader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return null;
        }

        public String[] readInitializeConfigFile(
            String initializationFile, int configFileTypeIn)
        {
            String[] initializationConfigs = null;

            try
            {
                //String fullFile = Directory.GetCurrentDirectory()
                //    + "\\" + TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY + "\\" + initializationFile;

                String fullFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                    TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY,
                    initializationFile);

                Array configNames;

                if (configFileTypeIn == (int)REALTIME_CONFIG_FILE_INPUT_TYPE.INITIALIZATION_FILE)
                {
                    Type configTypes = typeof(INITIALIZATION_CONFIG_VARS);
                    configNames = Enum.GetNames(configTypes);
                }
                else
                {
                    Type configTypes = typeof(REALTIME_CONFIGURATION);
                    configNames = Enum.GetNames(configTypes);
                }

                initializationConfigs = new String[configNames.Length];

                //bool foundPort = false;

                if (File.Exists(fullFile))
                {

                    FileStream fileStream = new FileStream(fullFile,
                        FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader streamReader = new StreamReader(fileStream);

                    while (!streamReader.EndOfStream)
                    {
                        String line = streamReader.ReadLine();
                        if (line.Length > 0)
                        {
                            int locationOfEqual = line.IndexOf("=");
                            String token = line.Substring(0, locationOfEqual);
                            String value = line.Substring(locationOfEqual + 1);

                            for (int i = 0; i < configNames.Length; i++)
                            {
                                if (token.CompareTo(configNames.GetValue(i).ToString()) == 0)
                                {
                                    initializationConfigs[i]
                                        = value;
                                }
                            }
                        }
                    }

                    fileStream.Close();
                    streamReader.Close();
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return initializationConfigs;
        }

        

        

        public Dictionary<string,string> readInstrumentAccount(string cfgAcctFileName)
        {
            Dictionary<string, string> instrumentAcctHashSet = new Dictionary<string, string>();

            try
            {
                //String fullFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                //    TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY,
                //    TradingSystemConstants.INSTRUMENT_ACCT_CSV);

                String fullFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                    TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY,
                    cfgAcctFileName);

                if (File.Exists(fullFile))
                {

                    FileStream fileStream = new FileStream(fullFile,
                        FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader streamReader = new StreamReader(fileStream);

                    while (!streamReader.EndOfStream)
                    {
                        String line = streamReader.ReadLine();
                        if (line.Length > 0)
                        {
                            string[] instrumentAcctArray = line.Split(',');

                            instrumentAcctHashSet.Add(instrumentAcctArray[0], instrumentAcctArray[1]);

                            //int locationOfEqual = line.IndexOf("=");
                            //String token = line.Substring(0, locationOfEqual);
                            //String value = line.Substring(locationOfEqual + 1);

                            //if (token.CompareTo("DAYLIGHTSAVINGS") == 0)
                            //{
                            //    return Convert.ToBoolean(value);
                            //}
                            //Debug.WriteLine("$$$"+ token +"$$$");
                            //Debug.WriteLine("$$$"+ value +"$$$");
                        }
                    }

                    fileStream.Close();
                    streamReader.Close();

                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return instrumentAcctHashSet;
        }


        public void readInstrumentSpecificFIXField(string fullFile,
            ConcurrentDictionary<string, InstrumentSpecificFIXFields> instrumentSpecificFIXFieldHashSet)
        {
            //format of exchange specific fields
            //FCM, TT Gateway, TT Exchange, TT symbol, 
            //TAG 47 RULE80A, TAG 204 CustomerOrFirm, TAG 18205 (TT ACCT), 
            //TAG 440 ClearingAccount, TAG 16102 FFT2
            
            try
            {

                if (File.Exists(fullFile))
                {

                    FileStream fileStream = new FileStream(fullFile,
                        FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader streamReader = new StreamReader(fileStream);

                    while (!streamReader.EndOfStream)
                    {
                        String lineWithTokenAndValue = streamReader.ReadLine();

                        int locationOfEqual = lineWithTokenAndValue.IndexOf("=");
                        String token = lineWithTokenAndValue.Substring(0, locationOfEqual);
                        String line = lineWithTokenAndValue.Substring(locationOfEqual + 1);

                        if (token.CompareTo(TradingSystemConstants.INSTRUMENT_SPECIFIC_FIX_FIELDS) == 0
                            && line.Length > 0)
                        {
                            string[] instrumentAcctArray = line.Split(',');

                            if (instrumentAcctArray.Length >= 8)
                            {

                                StringBuilder key = new StringBuilder();
                                key.Append(instrumentAcctArray[0]);
                                key.Append(instrumentAcctArray[1]);
                                key.Append(instrumentAcctArray[2]);
                                key.Append(instrumentAcctArray[3]);

                                InstrumentSpecificFIXFields instSpecFIX = new InstrumentSpecificFIXFields();
                                
                                instSpecFIX.FCM = instrumentAcctArray[0];
                                instSpecFIX.TTGateway = instrumentAcctArray[1];
                                instSpecFIX.TTExchange = instrumentAcctArray[2];
                                instSpecFIX.TTSymbol = instrumentAcctArray[3];

                                instSpecFIX.TAG_47_Rule80A = Convert.ToChar(instrumentAcctArray[4]);
                                instSpecFIX.TAG_204_CustomerOrFirm = Convert.ToInt16(instrumentAcctArray[5]);
                                instSpecFIX.TAG_18205_TTAccountType = instrumentAcctArray[6];
                                instSpecFIX.TAG_440_ClearingAccount = instrumentAcctArray[7];
                                //instSpecFIX.TAG_16102_FFT2 = instrumentAcctArray[8];

                                instrumentSpecificFIXFieldHashSet.TryAdd(key.ToString(), instSpecFIX);
                            }
                        }
                    }

                    fileStream.Close();
                    streamReader.Close();

                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }



        public List<OptionStrategy> readSupplementContracts(string fullFile, OptionArrayTypes optionArrayTypes,
            int portfolioId,
            QueryFutureIdContractDelegate queryFutureIdContractDelegate, object queryObject)
        {
            List<OptionStrategy> optionStrategies = new List<OptionStrategy>();

            try
            {

                if (File.Exists(fullFile))
                {

                    FileStream fileStream = new FileStream(fullFile,
                        FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader streamReader = new StreamReader(fileStream);

                    int optionStrategyCount = 1;

                    while (!streamReader.EndOfStream)
                    {
                        String line = streamReader.ReadLine();

                        if (line.Length > 0)
                        {
                            string[] supplementContractStrategy = line.Split(',');

                            if (supplementContractStrategy.Length >= 3)
                            {
                                string idcontract = queryFutureIdContractDelegate(queryObject,
                                    Convert.ToInt32(supplementContractStrategy[0].Trim()),
                                    Convert.ToInt32(supplementContractStrategy[1].Trim()),
                                    Convert.ToInt32(supplementContractStrategy[2].Trim()));

                                OptionStrategy optionStrategy = new OptionStrategy();

                                optionStrategy.optionStrategyParameters = new OptionStrategyParameter[
                                    optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0)];

                                for (int i = 0; i < optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0); i++)
                                {
                                    optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();
                                }

                                optionStrategies.Add(optionStrategy);

                                optionStrategy.supplementContract = true;

                                optionStrategy.idPortfoliogroup = portfolioId;

                                optionStrategy.idInstrument = Convert.ToInt32(supplementContractStrategy[0]);

                                optionStrategy.idStrategy = optionStrategyCount;

                                optionStrategyCount++;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].strategyStateFieldType 
                                    = TBL_STRATEGY_STATE_FIELDS.numberOfLegs;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue = 1;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.spreadStructure;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure].stateValueStringNotParsed = "2";

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.activeContractIndexes;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes].stateValueStringNotParsed = idcontract;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_ROLL_PARAMETER_SCRIPT;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes].stateValueStringNotParsed = "";

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.cfgContracts].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.cfgContracts;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.cfgContracts].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.cfgContracts].stateValueStringNotParsed = "";

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.currentPosition;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueStringNotParsed = "";

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryRule].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.entryRule;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryRule].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_ENTRY_SCRIPT;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryRule].stateValueStringNotParsed = "";

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryLots].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.entryLots;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryLots].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryLots].stateValueStringNotParsed = "";

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitRule].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.exitRule;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitRule].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_EXIT_SCRIPT;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitRule].stateValueStringNotParsed = "";

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.exitLots;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots].stateValueStringNotParsed = "";


                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.marginRequirement].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.marginRequirement;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.marginRequirement].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.marginRequirement].stateValue = 0;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rRisk;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue = 0;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk_R].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rRisk_R;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk_R].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk_R].stateValue = 0;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.oneR].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.oneR;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.oneR].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.oneR].stateValue = 0;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rStatus;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].stateValue = 0;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus_R].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rStatus_R;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus_R].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus_R].stateValue = 0;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idRiskType].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.idRiskType;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idRiskType].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idRiskType].stateValue = 0;

                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idSignalType].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.idSignalType;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idSignalType].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                                optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idSignalType].stateValue = 0;


                                optionStrategy.lockedIn_R = 0;
                            }
                        }
                    }

                    fileStream.Close();
                    streamReader.Close();

                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return optionStrategies;
        }
    }
}
