using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;
using RealtimeSpreadMonitor.Forms;
using System.Reflection;
//using XSerializer;

namespace RealtimeSpreadMonitor
{
    //http://www.asp.net/web-api/overview/web-api-clients/calling-a-web-api-from-a-net-client

    //http://www.cmegroup.com/confluence/display/EPICSANDBOX/Margin+Service+API+-+Onboarding+and+Verification
    //https://www.cmegroup.com/confluence/display/EPICSANDBOX/Margin+Service+API+-+ALL-IN-ONE+Call

    public enum RequestType
    {
        MODELTYPE,
        FCMTYPE
    }

    class MarginRequestRun
    {
        public int instrumentId;
        public RequestType requestType;
    }

    class CMEMarginCall
    {
        //static string mainCMEMarginAPIHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/";

        static string mainCMEMarginAPIHttp = "https://cmecore.cmegroup.com/MarginServiceApi/";

        //static string marginsHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/margins";
        //static string transactionsHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/transactions";
        //static string portfoliosHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/portfolios";

        static string smartClickID = "API_TMQREXO_MARGIN";
        static string pwd = "NJoyce111174$";

        

        private List<OptionSpreadExpression> optionSpreadExpressionList;
        private OptionStrategy[] optionStrategies;
        private Instrument[] instruments;
        private OptionRealtimeMonitor optionRealtimeMonitor;
        private LiveSpreadTotals portfolioSpreadTotals;

        private List<ADMPositionImportWeb> admPositionImportWeb;

        private bool fileOnly;

        //HttpClient httpClient;

        //Queue<int[,]> instrumentMargin = new Queue<int[,]>();

        string[] csvHeaders;

        private double CMECoreStrikeFormat(double strikePrice, int idInstrument)
        {
            if (idInstrument == 31 || idInstrument == 32) //ZSE or ZCE
            {
                return strikePrice * 0.01;
            }
            else
            {
                return strikePrice;
            }
        }


        public CMEMarginCall(List<OptionSpreadExpression> optionSpreadExpressionList,
            OptionStrategy[] optionStrategies, Instrument[] instruments,
            OptionRealtimeMonitor optionRealtimeMonitor,
            LiveSpreadTotals portfolioSpreadTotals, 
            List<ADMPositionImportWeb> admPositionImportWeb, bool fileOnly)
        {
            this.optionSpreadExpressionList = optionSpreadExpressionList;

            this.optionStrategies = optionStrategies;

            this.instruments = instruments;

            this.optionRealtimeMonitor = optionRealtimeMonitor;

            this.portfolioSpreadTotals = portfolioSpreadTotals;

            this.admPositionImportWeb = admPositionImportWeb;

            this.fileOnly = fileOnly;
        }

        public void generateMarginRequest()
        {
            csvHeaders = new string[13];
            csvHeaders[0] = "Firm Id";
            csvHeaders[1] = "Acct Id";
            csvHeaders[2] = "Exchange";
            csvHeaders[3] = "Ticker Symbol";
            csvHeaders[4] = "Product Name";
            csvHeaders[5] = "CC Code";
            csvHeaders[6] = "Period Code";
            csvHeaders[7] = "Put / Call";
            csvHeaders[8] = "Strike";
            csvHeaders[9] = "Underlying Period Code";
            csvHeaders[10] = "Net Positions";
            csvHeaders[11] = "Margin Type";
            csvHeaders[12] = "Client ID";



            for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
            {
                //int instrumentCnt = 2;
                //RequestType requestType = RequestType.FCMTYPE;
                
                foreach (RequestType requestType in (RequestType[])Enum.GetValues(typeof(RequestType)))
                {
                    Thread calcMarginCallThread = new Thread(new ParameterizedThreadStart(makeRequestCall));
                    calcMarginCallThread.IsBackground = true;

                    MarginRequestRun mrr = new MarginRequestRun();
                    mrr.instrumentId = instrumentCnt;
                    if (requestType == RequestType.FCMTYPE)
                    {
                        mrr.requestType = RequestType.FCMTYPE;
                    }
                    else
                    {
                        mrr.requestType = RequestType.MODELTYPE;
                    }

                    calcMarginCallThread.Start(mrr);
                }

            }

            //http://msdn.microsoft.com/en-us/library/t249c2y7%28v=vs.110%29.aspx

            //int instrumentCnt = 0;
            //while (instrumentCnt < instruments.Length)
            //{
            //    if (instrumentMargin.Count > 0)
            //    {
            //        int[,] instrumentInfo = instrumentMargin.Dequeue();

            //        Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod));

            //        t.Start("My Parameter");

            //        instrumentCnt++;
            //    }
            //}
        }
        
        public void makeRequestCall(Object obj)
        {
            MarginRequestRun mrr = (MarginRequestRun)obj;

            int instrumentCnt = mrr.instrumentId;

            RequestType requestType = mrr.requestType;

            HttpClient httpClient = new HttpClient();
                            
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.BaseAddress = new Uri(mainCMEMarginAPIHttp);

            httpClient.DefaultRequestHeaders.Add("username", smartClickID);
            httpClient.DefaultRequestHeaders.Add("password", pwd);
                
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            //portfolioSpreadTotals.initialMarginTotals = 0;
            //portfolioSpreadTotals.maintenanceMarginTotals = 0;

            //portfolioSpreadTotals.initialFCM_MarginTotals = 0;
            //portfolioSpreadTotals.maintenanceFCM_MarginTotals = 0;



            List<string[]> output = new List<string[]>();
            output.Add(csvHeaders);

            if (requestType == RequestType.MODELTYPE)
            //for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
            {
                

                for (int i = 0; i < optionSpreadExpressionList.Count; i++)
                {
                    if (optionSpreadExpressionList[i].optionExpressionType != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                        && optionSpreadExpressionList[i].spreadIdx.Count > 0
                        && optionSpreadExpressionList[i].legIdx.Count > 0
                        && optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary != 0
                        && optionSpreadExpressionList[i].instrument.idInstrument == instruments[instrumentCnt].idInstrument)
                    {
                        string[] line = new string[12];

                        line[0] = "tml1";
                        line[1] = instruments[instrumentCnt].exchangeSymbol + "_" + requestType.ToString();
                        line[2] = optionSpreadExpressionList[i].instrument.spanExchWebAPISymbol;
                        line[3] = "";
                        line[4] = "";

                        int spreadIdx = optionSpreadExpressionList[i].spreadIdx[0];
                        int legIdx = optionSpreadExpressionList[i].legIdx[0];

                        if (optionSpreadExpressionList[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            line[5] = optionSpreadExpressionList[i].instrument.spanFutureCode;

                            line[6] = optionStrategies[spreadIdx]
                            .legInfo[legIdx].contractYear.ToString()
                            +
                            optionStrategies[spreadIdx]
                            .legInfo[legIdx].contractMonthInt.ToString("00");

                            line[7] = "";
                            line[8] = "";
                            line[9] = "";
                        }
                        else
                        {
                            line[5] = optionSpreadExpressionList[i].instrument.spanOptionCode;

                            line[6] = optionStrategies[spreadIdx]
                            .legInfo[legIdx].optionYear.ToString()
                            +
                            optionStrategies[spreadIdx]
                            .legInfo[legIdx].optionMonthInt.ToString("00");

                            if (optionSpreadExpressionList[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.CALL)
                            {
                                line[7] = "CALL";
                            }
                            else
                            {
                                line[7] = "PUT";
                            }

                            line[8] = CMECoreStrikeFormat(optionSpreadExpressionList[i].strikePrice,
                                                            optionSpreadExpressionList[i].instrument.idInstrument).ToString();

                            //int findFutCnt = 0;
                            //while (findFutCnt < optionStrategies[spreadIdx]
                            //        .legInfo.Length)
                            //{
                            //    if (optionStrategies[spreadIdx]
                            //        .legInfo[findFutCnt].legContractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            //    {
                            //        break;
                            //    }

                            //    findFutCnt++;
                            //}

                            line[9] = optionStrategies[spreadIdx]
                            .legInfo[optionStrategies[spreadIdx].idxOfFutureLeg].contractYear.ToString()
                            +
                            optionStrategies[spreadIdx]
                            .legInfo[optionStrategies[spreadIdx].idxOfFutureLeg].contractMonthInt.ToString("00");
                        }





                        line[10] = optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary.ToString();
                        line[11] = "";

                        output.Add(line);
                    }
                }
            }
            else
            {
                for (int i = 0; i < admPositionImportWeb.Count; i++)
                {
                    int netPos = (int)admPositionImportWeb[i].Net
                        + admPositionImportWeb[i].transNetLong
                        - admPositionImportWeb[i].transNetShort;

                    if (netPos != 0
                        && admPositionImportWeb[i].instrument.idInstrument == instruments[instrumentCnt].idInstrument)
                    {
                        string[] line = new string[12];

                        line[0] = "tml1";
                        line[1] = instruments[instrumentCnt].exchangeSymbol + "_" + requestType.ToString();
                        line[2] = instruments[instrumentCnt].spanExchWebAPISymbol;
                        line[3] = "";
                        line[4] = "";

                       // int spreadIdx = optionSpreadExpressionList[i].spreadIdx[0];
                       // int legIdx = optionSpreadExpressionList[i].legIdx[0];

                        if (admPositionImportWeb[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            line[5] = instruments[instrumentCnt].spanFutureCode;

                            line[6] = admPositionImportWeb[i].contractInfo.contractYear.ToString()
                            +
                            admPositionImportWeb[i].contractInfo.contractMonthInt.ToString("00");

                            line[7] = "";
                            line[8] = "";
                            line[9] = "";
                        }
                        else
                        {
                            line[5] = instruments[instrumentCnt].spanOptionCode;

                            line[6] = admPositionImportWeb[i].contractInfo.optionYear.ToString()
                            +
                            admPositionImportWeb[i].contractInfo.optionMonthInt.ToString("00");

                            if (admPositionImportWeb[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.CALL)
                            {
                                line[7] = "CALL";
                            }
                            else
                            {
                                line[7] = "PUT";
                            }

                            line[8] = //admPositionImportWeb[i].strikeInDecimal.ToString();
                                        CMECoreStrikeFormat(admPositionImportWeb[i].strikeInDecimal,
                                                            instruments[instrumentCnt].idInstrument).ToString();


                            line[9] = admPositionImportWeb[i].contractInfo.contractYear.ToString()
                            +
                            admPositionImportWeb[i].contractInfo.contractMonthInt.ToString("00");
                        }





                        line[10] = netPos.ToString();
                        line[11] = "";

                        output.Add(line);
                    }
                }
            }

                //String[] line1 = { "tml2", "2", "CME", "", "", "EC", "201409", "", "", "", "1", "" };
                //output[1] = line1;

                //OZNN4 P1230

                //String[] line2 = { "tml2", "2", "CBT", "", "", "21", "201407", "PUT", "123", "201409", "1", "" };
                //output[2] = line2;

                int length = output.Count();

                StringBuilder csvTrades = new StringBuilder();

                //stringVal.Clear();
                for (int index = 0; index < length; index++)
                {
                    string lineInput;

                    if (index == 0)
                    {
                        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"";
                    }
                    else
                    {
                        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"" + ",";
                    }

                    csvTrades.Append(lineInput);

                    csvTrades.Append("\n");

                    //stringVal.Append(",");
                }

                //String currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (fileOnly)
                {
                    String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "CSV_CORE_TRADES");

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.WriteAllText(dir + "\\"
                        + instruments[instrumentCnt].exchangeSymbol + "_"+ 
                        requestType.ToString() + "_csvTrades" + ".csv", csvTrades.ToString());
                }
                else
                {
                    RunAsync(csvTrades, instrumentCnt, requestType, httpClient);
                }
            
        }

        public async void RunAsync(StringBuilder csvTrades, int instrumentCnt,
            RequestType requestType, HttpClient httpClient)        
        {
            

            //string[][] output = new string[3][];
            //output[0] = csvHeaders;
            ////String[] line = { "test","te","CBT","ZNU4","10Y TREASURY NOTE FUTURES","21","201409","","","","1","" };
            ////output[1] = line;

            //String[] line1 = { "tml2", "2", "CME", "", "", "EC", "201409", "", "", "", "1", "" };
            //output[1] = line1;

            ////OZNN4 P1230

            //String[] line2 = { "tml2", "2", "CBT", "", "", "21", "201407", "PUT", "123", "201409", "1", "" };
            //output[2] = line2;

            //int length = output.Count();

            //StringBuilder stringVal = new StringBuilder();

            ////stringVal.Clear();
            //for (int index = 0; index < length; index++)
            //{
            //    string lineInput;

            //    if (index == 0)
            //    {
            //        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"";
            //    }
            //    else
            //    {
            //        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"" + ",";
            //    }

            //    stringVal.Append(lineInput);

            //    stringVal.Append("\n");

            //    //stringVal.Append(",");
            //}

            //Console.Write(stringVal.ToString());

            //File.WriteAllText("D:/test.csv", stringVal.ToString());

            //StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8);
            //sw. = Encoding.UTF8;


            XmlWriter xw = XmlWriter.Create(sw);
            //xw.Settings.Encoding = new UTF8Encoding();
            xw.WriteStartDocument(true);

            //<core:marginReq xmlns:core="http://cmegroup.com/schema/core/1.1">

            xw.WriteStartElement("core", "marginReq", "http://cmegroup.com/schema/core/1.2");

            //xw.WriteAttributes

            //xw.WriteAttributeString("marginReq", "xmlns", "http://cmegroup.com/schema/core/1.1");

            xw.WriteStartElement("margin");

            xw.WriteStartElement("transactions");

            xw.WriteStartElement("transaction");

            xw.WriteAttributeString("type", "TRADE");
            xw.WriteAttributeString("id", "0");

            xw.WriteStartElement("payload");
            xw.WriteAttributeString("encoding", "STRING");
            xw.WriteAttributeString("format", "CSV");

            xw.WriteStartElement("string");
            xw.WriteString(csvTrades.ToString());

            xw.WriteEndElement();

            xw.WriteEndElement(); //transactions

            xw.WriteEndElement();

            xw.WriteEndElement();

            xw.WriteStartElement("amounts");

            xw.WriteAttributeString("ccy", "USD");
            xw.WriteAttributeString("conc", "0.0");
            xw.WriteAttributeString("init", "0.0");
            xw.WriteAttributeString("maint", "0.0");
            xw.WriteAttributeString("nonOptVal", "0.0");
            xw.WriteAttributeString("optVal", "0.0");


            xw.WriteEndDocument();
            xw.Close();

            //Console.WriteLine(sw.ToString());


            String test = sw.ToString();//.Substring(38);
            Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            Console.WriteLine(test);

            //using (var httpClient = new HttpClient())
            //{
                
                //httpClient.DefaultRequestHeaders.Accept.Clear();
                //httpClient.BaseAddress = new Uri(mainCMEMarginAPIHttp);

                //httpClient.DefaultRequestHeaders.Add("username", smartClickID);
                //httpClient.DefaultRequestHeaders.Add("password", pwd);
                
                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                //client.DefaultRequestHeaders.Add("sessionId", "86CA12AD287137BF59030573FE4D71B7");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "margins?complete=true");

                request.Content = new StringContent(test, Encoding.UTF8, "application/xml");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                //HttpResponseMessage response = await client.PostAsync("margins?complete=true", test);
                //HttpResponseMessage response = await client.PostAsXmlAsync("margins?complete=true", test);

                string marginId = "";
                bool continueWithMarginCollection = false;

                if (response.IsSuccessStatusCode)
                {
                    //Product product = await response.Content.ReadAsAsync>Product>();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                    //String w = response.Content.ReadAsAsync(String);
                    String x = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(x);

                    StringReader sr = new StringReader(x);

                    XmlReader xmlReader = XmlReader.Create(sr);

                    

                    //****
                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            if (xmlReader.Name == "ns2:marginRpt")
                            {
                                string status = xmlReader.GetAttribute("status");

                                if (status.CompareTo("ERROR") == 0)
                                {
                                    break;
                                }
                            }

                            if (xmlReader.Name == "margin")
                            {
                                marginId = xmlReader.GetAttribute("id");

                                if (requestType == RequestType.MODELTYPE)
                                {
                                    instruments[instrumentCnt].coreAPImarginId = marginId;
                                }
                                else
                                {
                                    instruments[instrumentCnt].coreAPI_FCM_marginId = marginId;
                                }

                                continueWithMarginCollection = true;

                                Console.WriteLine(marginId);

                                break;

                                //id = xmlReader.ReadString();
                            }
                        }
                    }

                    //*******
                }

               
            //}

            bool notReceivedMargin = true;

            while (notReceivedMargin && continueWithMarginCollection)
            {

                Thread.Sleep(2000);

                Console.WriteLine("xxx " + marginId);

                request = new HttpRequestMessage(HttpMethod.Get, "margins/" + marginId);

                //request.Content = new StringContent(test, Encoding.UTF8, "application/xml");

                response = await httpClient.SendAsync(request);

                //HttpResponseMessage response = await client.PostAsync("margins?complete=true", test);
                //HttpResponseMessage response = await client.PostAsXmlAsync("margins?complete=true", test);


                if (response.IsSuccessStatusCode)
                {
                    //Product product = await response.Content.ReadAsAsync>Product>();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                    //String w = response.Content.ReadAsAsync(String);
                    String x = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(x);

                    StringReader sr = new StringReader(x);

                    XmlReader xmlReader = XmlReader.Create(sr);

                    //****
                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            if (xmlReader.Name == "ns2:marginRpt")
                            {
                                string status = xmlReader.GetAttribute("status");

                                if (status.CompareTo("ERROR") == 0)
                                {
                                    continueWithMarginCollection = false;

                                    break;
                                }
                                else if (status.CompareTo("PROCESSING") == 0)
                                {
                                    break;
                                }
                            }

                            if (xmlReader.Name == "amounts")
                            {
                                string initialMargin = xmlReader.GetAttribute("init");

                                string maintenanceMargin = xmlReader.GetAttribute("maint");

                                portfolioSpreadTotals.initialMarginTotals = 0;
                                portfolioSpreadTotals.maintenanceMarginTotals = 0;

                                portfolioSpreadTotals.initialFCM_MarginTotals = 0;
                                portfolioSpreadTotals.maintenanceFCM_MarginTotals = 0;

                                if (requestType == RequestType.MODELTYPE)
                                {
                                    instruments[instrumentCnt].coreAPIinitialMargin = Convert.ToDouble(initialMargin);

                                    instruments[instrumentCnt].coreAPImaintenanceMargin = Convert.ToDouble(maintenanceMargin);

                                    for (int totalInstCnt = 0; totalInstCnt < instruments.Length; totalInstCnt++)
                                    {
                                        portfolioSpreadTotals.initialMarginTotals += instruments[totalInstCnt].coreAPIinitialMargin;

                                        portfolioSpreadTotals.maintenanceMarginTotals += instruments[totalInstCnt].coreAPImaintenanceMargin;
                                    }

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_MARGIN,
                                        instrumentCnt, instruments[instrumentCnt].coreAPIinitialMargin);

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_MARGIN,
                                        instrumentCnt, instruments[instrumentCnt].coreAPImaintenanceMargin);

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_MARGIN,
                                        instruments.Length, portfolioSpreadTotals.initialMarginTotals);

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_MARGIN,
                                        instruments.Length, portfolioSpreadTotals.maintenanceMarginTotals);
                                }
                                else
                                {
                                    instruments[instrumentCnt].coreAPI_FCM_initialMargin = Convert.ToDouble(initialMargin);

                                    instruments[instrumentCnt].coreAPI_FCM_maintenanceMargin = Convert.ToDouble(maintenanceMargin);

                                    //portfolioSpreadTotals.initialFCM_MarginTotals += instruments[instrumentCnt].coreAPI_FCM_initialMargin;

                                    //portfolioSpreadTotals.maintenanceFCM_MarginTotals += instruments[instrumentCnt].coreAPI_FCM_maintenanceMargin;

                                    for (int totalInstCnt = 0; totalInstCnt < instruments.Length; totalInstCnt++)
                                    {
                                        portfolioSpreadTotals.initialFCM_MarginTotals += instruments[totalInstCnt].coreAPI_FCM_initialMargin;

                                        portfolioSpreadTotals.maintenanceFCM_MarginTotals += instruments[totalInstCnt].coreAPI_FCM_maintenanceMargin;
                                    }

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_FCM_MARGIN,
                                        instrumentCnt, instruments[instrumentCnt].coreAPI_FCM_initialMargin);

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_FCM_MARGIN,
                                        instrumentCnt, instruments[instrumentCnt].coreAPI_FCM_maintenanceMargin);

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_FCM_MARGIN,
                                        instruments.Length, portfolioSpreadTotals.initialFCM_MarginTotals);

                                    optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_FCM_MARGIN,
                                        instruments.Length, portfolioSpreadTotals.maintenanceFCM_MarginTotals);
                                }


                                

                                notReceivedMargin = false;

                                break;
                            }
                        }
                    }

                    //*******
                }
            }
        }

        //public void getMarginFromPortfolio(Object obj)
        //{
        //    RunReceiveMargin(int instrumentCnt) 
        //}
        
        //public async void RunReceiveMargin(int instrumentCnt) 
        //{
            
        //}

        async Task RunAsync2()
        //
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://cmecorenr.cmegroup.com/MarginServiceApi/");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("username", "spickering");
                client.DefaultRequestHeaders.Add("password", "NJoyce111174$");
                //client.DefaultRequestHeaders.Add("sessionId", "42D52622EAF1ED384988E02088281401.instance1");
                /*
                 * Headers = {sessionId: 42D52622EAF1ED384988E02088281401.instance1
Pragma: no-cache
Cache-Control: no-store, no-cache
Date: Tue, 27 May 2014 18:10:18 GMT
Server: Apache-Coyote/1.1
Set-Cookie: JSESSIONID=42D52622EAF1ED384988E02088281401.instance1; Path=/coreapi/; H...
                 */

                string[] csvHeaders = new string[13];
                csvHeaders[0] = "Firm Id";
                csvHeaders[1] = "Acct Id";
                csvHeaders[2] = "Exchange";
                csvHeaders[3] = "Ticker Symbol";
                csvHeaders[4] = "Product Name";
                csvHeaders[5] = "CC Code";
                csvHeaders[6] = "Period Code";
                csvHeaders[7] = "Put / Call";
                csvHeaders[8] = "Strike";
                csvHeaders[9] = "Underlying Period Code";
                csvHeaders[10] = "Net Positions";
                csvHeaders[11] = "Margin Type";
                csvHeaders[12] = "Client ID";

                string[][] output = new string[2][];
                output[0] = csvHeaders;
                //String[] line = { "test","te","CBT","ZNU4","10Y TREASURY NOTE FUTURES","21","201409","","","","1","" };
                //output[1] = line;

                String[] line1 = { "test10", "2", "CME", "6EU4", "", "EC", "201409", "", "", "", "1", "" };
                output[1] = line1;

                //String[] line2 = { "tml1", "2", "CBT", "OZNN4 P1230", "", "21", "201407", "PUT", "123", "201409", "1", "" };
                //output[1] = line2;

                int length = output.GetLength(0);

                StringBuilder stringVal = new StringBuilder();

                //stringVal.Clear();
                for (int index = 0; index < length; index++)
                {
                    string lineInput;

                    if (index == 0)
                    {
                        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"";
                    }
                    else
                    {
                        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"" + ",";
                    }

                    stringVal.Append(lineInput);

                    stringVal.Append("\n");

                    //stringVal.Append(",");
                }

                Console.Write(stringVal.ToString());

                File.WriteAllText("D:/test.csv", stringVal.ToString());



                //StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8);
                
                //sw. = Encoding.UTF8;



                // XmlWriterSettings xws = new XmlWriterSettings();

                //Encoding utf8noBOM = new UTF8Encoding();
                //xws.Encoding = Encoding.UTF8;

                //sw.Encoding = utf8noBOM;

                XmlWriter xw = XmlWriter.Create(sw);
                //xw.Settings.Encoding = new UTF8Encoding();
                xw.WriteStartDocument(true);

                //<core:marginReq xmlns:core="http://cmegroup.com/schema/core/1.1">

                xw.WriteStartElement("ns2", "portfolioReq", "http://cmegroup.com/schema/core/1.2");

                //xw.WriteAttributes

                //xw.WriteAttributeString("marginReq", "xmlns", "http://cmegroup.com/schema/core/1.1");

                xw.WriteStartElement("portfolio");

                xw.WriteAttributeString("desc", "My Desc");
                xw.WriteAttributeString("name", "My Name");
                xw.WriteAttributeString("rptCcy", "USD");
                xw.WriteAttributeString("id", "9");

                xw.WriteStartElement("entities");
                xw.WriteAttributeString("clrMbrFirmId", "My Firm");
                xw.WriteAttributeString("custAcctId", "My Account");

                xw.WriteEndElement();

                //xw.WriteStartElement("transactions");

                //xw.WriteStartElement("transaction");

                //xw.WriteAttributeString("type", "TRADE");
                //xw.WriteAttributeString("id", "0");

                //xw.WriteStartElement("payload");
                //xw.WriteAttributeString("encoding", "STRING");
                //xw.WriteAttributeString("format", "CSV");

                //xw.WriteStartElement("string");
                //xw.WriteString(stringVal.ToString());

                //xw.WriteEndElement();

                //xw.WriteEndElement(); //transactions

                //xw.WriteEndElement();

                //xw.WriteEndElement();

                //xw.WriteStartElement("amounts");

                //xw.WriteAttributeString("ccy", "USD");
                //xw.WriteAttributeString("conc", "0.0");
                //xw.WriteAttributeString("init", "0.0");
                //xw.WriteAttributeString("maint", "0.0");
                //xw.WriteAttributeString("nonOptVal", "0.0");
                //xw.WriteAttributeString("optVal", "0.0");




                xw.WriteEndDocument();
                xw.Close();

                Console.WriteLine(sw.ToString());

                //sb.Replace("utf-16", "UTF-8");

                //string finalXMLstring = Encoding.UTF8.GetString();

                //string xmlX = ""

                //string myObjectAsXml = SerializationHelper.SerializeEntity<T>(targetObject);

                //XmlSerializer x = new XmlSerializer(typeof(Margin));

                //StringWriter sww = new StringWriter();
                // XmlWriter writer = XmlWriter.Create(sww);
                // //xsSubmit.Serialize(writer, subReq);

                //x.Serialize(writer, margin);

                //HttpResponseMessage response = await client.GetAsync("portfolios/test1");

                ////response.EnsureSuccessStatusCode();

                ////string x = response.Headers.Select("sessionId");

                //if (response.IsSuccessStatusCode)
                //{
                //    String x = await response.Content.ReadAsStringAsync();

                //    Console.WriteLine(x);
                //}

                String test = sw.ToString();  //.Substring(38);
                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                Console.WriteLine(test);

                HttpResponseMessage response = await client.PostAsXmlAsync("portfolios", test);

                TSErrorCatch.debugWriteOut(test);

                //HttpResponseMessage response = await client.PostAsXmlAsync("margins/complete=true", xml);

                //client.GetAsync("margins/sp");
                //HttpResponseMessage response = await client.GetAsync("portfolios");

                //Task<string> response = await client.GetStringAsync("portfolios/").Result;

                //Task<HttpResponseMessage> getStringTask = client.PostAsync("https://cmecorenr.cmegroup.com/MarginServiceApi/margins", null);

                //HttpResponseMessage response = await getStringTask;

                //HttpResponseMessage response = await client.GetAsync("/margins/sp");

                //var jsonString = await client.GetStringAsync(portfoliosHttp);

                //TSErrorCatch.debugWriteOut(jsonString);

                if (response.IsSuccessStatusCode)
                {
                    //Product product = await response.Content.ReadAsAsync>Product>();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);

                    String x = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(x);
                }
            }
        }


        async Task RunAsyncXXX()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://cmecorenr.cmegroup.com/MarginServiceApi/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                client.DefaultRequestHeaders.Add("username", "spickering");
                client.DefaultRequestHeaders.Add("password", "NJoyce111174$");

                //client.DefaultRequestHeaders.Add("Content-Type", "application/xml");

                //client.DefaultRequestHeaders.Add("sessionId", "86CA12AD287137BF59030573FE4D71B7");

                //client.DefaultRequestHeaders.Add(new MediaTypeHeaderValue())
                //client.CancelPendingRequests();
                //client.

                // New code:
                //HttpResponseMessage response = await client.PostAsXmlAsync("?username='spickering' passord='NJoyce111174$'");


                /////////////////////////////////////////////
                //Margin margin = new Margin();
                //Transaction[] transactions = new Transaction[1];
                //transactions[0] = new Transaction();
                //transactions[0].type = TransactionType.TRADE;
                //transactions[0].id = "0";

                //TransactionPayload payload = new TransactionPayload();
                //payload.format = TransactionFormat.CSV;
                //payload.encoding = "STRING";
                //StringBuilder stringVal = new StringBuilder();
                //stringVal.Append("Firm ID,Acct ID,Trade ID,Currency,Ticker,Reference Entity Name,Seniority,Restructuring,Maturity,Buy/Sell,Notional,Coupon,CC Code,CC Code Desc,Clearable");
                //stringVal.Append("test,test,,USD,CG12V2,CDX.NA.IG.12-V2,SR,XR,2014-06,BUY,2000000,0.01,CG12V2,CDXIG12V2.SR.XR.USD,Y");
                //stringVal.Append("test,test,,USD,CG12V2,CDX.NA.IG.12-V2,SR,XR,2014-06,BUY,2000000,0.01,CG12V2,CDXIG12V2.SR.XR.USD,Y");
                //stringVal.Append("test,test,,USD,CG12V2,CDX.NA.IG.12-V2,SR,XR,2014-06,BUY,2000000,0.01,CG12V2,CDXIG12V2.SR.XR.USD,Y");

                //payload.STRING = stringVal.ToString();

                //transactions[0].payload = payload;

                //MarginAmounts ma = new MarginAmounts();

                //ma.ccy = CMECurrency.AUD;
                //ma.conc = 0;
                //ma.init = 0;
                //ma.maint = 0;
                //ma.nonOptVal = 0;
                //ma.optVal = 0;

                //margin.amounts = ma;

                //margin.transactions = transactions;

                /////////////////////////////////////////////

                //XmlSerializer<Margin> serializer = new XmlSerializer<Margin>();
                //string xml = serializer.Serialize(margin);

                //System.Xml.Serialization.XmlSerializer xmlX = new XmlSerializer(margin.GetType());
                //xmlX.Serialize(Console.Out, margin);

                //string xml = "";

                //"Firm ID,Acct ID,Trade ID,Currency,Ticker,Reference Entity Name,
                //Seniority,Restructuring,Maturity,Buy/Sell,Notional,Coupon,CC Code,CC Code Desc,Clearable"

                string[] csvHeaders = new string[13];
                csvHeaders[0] = "Firm Id";
                csvHeaders[1] = "Acct Id";
                csvHeaders[2] = "Exchange";
                csvHeaders[3] = "Ticker Symbol";
                csvHeaders[4] = "Product Name";
                csvHeaders[5] = "CC Code";
                csvHeaders[6] = "Period Code";
                csvHeaders[7] = "Put / Call";
                csvHeaders[8] = "Strike";
                csvHeaders[9] = "Underlying Period Code";
                csvHeaders[10] = "Net Positions";
                csvHeaders[11] = "Margin Type";
                csvHeaders[12] = "Client ID";

                string[][] output = new string[2][];
                output[0] = csvHeaders;
                //String[] line = { "test","te","CBT","ZNU4","10Y TREASURY NOTE FUTURES","21","201409","","","","1","" };
                //output[1] = line;

                //String[] line1 = { "test10", "2", "CME", "6EU4", "", "EC", "201409", "", "", "", "1", "" };
                //output[1] = line1;

                String[] line2 = { "tml2", "2", "CBT", "OZNN4 P1230", "", "21", "201407", "PUT", "123", "201409", "1", "" };
                output[1] = line2;

                int length = output.GetLength(0);

                StringBuilder stringVal = new StringBuilder();

                //stringVal.Clear();
                for (int index = 0; index < length; index++)
                {
                    string lineInput;

                    if (index == 0)
                    {
                        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"";
                    }
                    else
                    {
                        lineInput = "\"" + string.Join("\",\"", output[index]) + "\"" + ",";
                    }

                    stringVal.Append(lineInput);

                    stringVal.Append("\n");

                    //stringVal.Append(",");
                }

                Console.Write(stringVal.ToString());

                File.WriteAllText("D:/test.csv", stringVal.ToString());



                //StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriterWithEncoding(Encoding.UTF8);
                //sw. = Encoding.UTF8;



                // XmlWriterSettings xws = new XmlWriterSettings();

                //Encoding utf8noBOM = new UTF8Encoding();
                //xws.Encoding = Encoding.UTF8;

                //sw.Encoding = utf8noBOM;

                XmlWriter xw = XmlWriter.Create(sw);
                //xw.Settings.Encoding = new UTF8Encoding();
                xw.WriteStartDocument(true);

                //<core:marginReq xmlns:core="http://cmegroup.com/schema/core/1.1">

                xw.WriteStartElement("core", "marginReq", "http://cmegroup.com/schema/core/1.2");

                //xw.WriteAttributes

                //xw.WriteAttributeString("marginReq", "xmlns", "http://cmegroup.com/schema/core/1.1");

                xw.WriteStartElement("margin");

                xw.WriteStartElement("transactions");

                xw.WriteStartElement("transaction");

                xw.WriteAttributeString("type", "TRADE");
                xw.WriteAttributeString("id", "0");

                xw.WriteStartElement("payload");
                xw.WriteAttributeString("encoding", "STRING");
                xw.WriteAttributeString("format", "CSV");

                xw.WriteStartElement("string");
                xw.WriteString(stringVal.ToString());

                xw.WriteEndElement();

                xw.WriteEndElement(); //transactions

                xw.WriteEndElement();

                xw.WriteEndElement();

                xw.WriteStartElement("amounts");

                xw.WriteAttributeString("ccy", "USD");
                xw.WriteAttributeString("conc", "0.0");
                xw.WriteAttributeString("init", "0.0");
                xw.WriteAttributeString("maint", "0.0");
                xw.WriteAttributeString("nonOptVal", "0.0");
                xw.WriteAttributeString("optVal", "0.0");




                xw.WriteEndDocument();
                xw.Close();

                Console.WriteLine(sw.ToString());

                //sb.Replace("utf-16", "UTF-8");

                //string finalXMLstring = Encoding.UTF8.GetString();

                //string xmlX = ""

                //string myObjectAsXml = SerializationHelper.SerializeEntity<T>(targetObject);

                //XmlSerializer x = new XmlSerializer(typeof(Margin));

                //StringWriter sww = new StringWriter();
                // XmlWriter writer = XmlWriter.Create(sww);
                // //xsSubmit.Serialize(writer, subReq);

                //x.Serialize(writer, margin);

                //HttpResponseMessage response = await client.GetAsync("portfolios/test1");

                ////response.EnsureSuccessStatusCode();

                ////string x = response.Headers.Select("sessionId");

                //if (response.IsSuccessStatusCode)
                //{
                //    String x = await response.Content.ReadAsStringAsync();

                //    Console.WriteLine(x);
                //}

                String test = sw.ToString();//.Substring(38);
                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                Console.WriteLine(test);


                HttpResponseMessage response = await client.PostAsXmlAsync("margins?complete=true", test);

                //TSErrorCatch.debugWriteOut(test);

                //HttpResponseMessage response = await client.PostAsXmlAsync("margins/complete=true", xml);

                //client.GetAsync("margins/sp");
                //HttpResponseMessage response = await client.GetAsync("portfolios");

                //Task<string> response = await client.GetStringAsync("portfolios/").Result;

                //Task<HttpResponseMessage> getStringTask = client.PostAsync("https://cmecorenr.cmegroup.com/MarginServiceApi/margins", null);

                //HttpResponseMessage response = await getStringTask;

                //HttpResponseMessage response = await client.GetAsync("/margins/sp");

                //var jsonString = await client.GetStringAsync(portfoliosHttp);

                //TSErrorCatch.debugWriteOut(jsonString);

                if (response.IsSuccessStatusCode)
                {
                    //Product product = await response.Content.ReadAsAsync>Product>();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                    //String w = response.Content.ReadAsAsync(String);
                    String x = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(x);
                }
            }
        }
    }
}
