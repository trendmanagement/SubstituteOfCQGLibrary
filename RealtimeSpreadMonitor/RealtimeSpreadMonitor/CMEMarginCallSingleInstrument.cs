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

    

    //class MarginRequestRunSingleInstrument
    //{
    //    //public int instrumentId;
    //    public RequestType requestType;
    //}

    class CMEMarginCallSingleInstrument
    {
        //static string mainCMEMarginAPIHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/";

        static string mainCMEMarginAPIHttp = "https://cmecore.cmegroup.com/MarginServiceApi/";

        //static string marginsHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/margins";
        //static string transactionsHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/transactions";
        //static string portfoliosHttp = "https://cmecorenr.cmegroup.com/MarginServiceApi/portfolios";

        static string smartClickID = "API_TMQREXO_MARGIN";
        static string pwd = "NJoyce111174$";

        List<OptionChartMargin> contractList;

        //private List<OptionSpreadExpression> optionSpreadExpressionList;
        //private OptionStrategy[] optionStrategies;
        private Instrument instrument;

        //private Instrument outputInstrument;

        //private OptionRealtimeMonitor optionRealtimeMonitor;
        //private LiveSpreadTotals portfolioSpreadTotals;

        //private List<ADMPositionImportWeb> admPositionImportWeb;

        private OptionPLChart optionPlChart;

        private OptionPLChartUserForm optionPLChartUserForm;

        private bool fileOnly;

        //HttpClient httpClient;

        //Queue<int[,]> instrumentMargin = new Queue<int[,]>();

        string[] csvHeaders;

        public CMEMarginCallSingleInstrument(
            List<OptionChartMargin> contractList,
            
            Instrument instrument, 

            OptionPLChart optionPlChart,

            OptionPLChartUserForm optionPLChartUserForm,

            bool fileOnly)
        {
            //this.optionSpreadExpressionList = optionSpreadExpressionList;

            //this.optionStrategies = optionStrategies;

            this.contractList = contractList;

            this.instrument = instrument;

            //this.outputInstrument = outputInstrument;

            //this.optionRealtimeMonitor = optionRealtimeMonitor;

            //this.portfolioSpreadTotals = portfolioSpreadTotals;

            //this.admPositionImportWeb = admPositionImportWeb;

            this.optionPlChart = optionPlChart;

            this.optionPLChartUserForm = optionPLChartUserForm;

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



            //for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
            {
                //int instrumentCnt = 2;
                //RequestType requestType = RequestType.FCMTYPE;
                
                //foreach (RequestType requestType in (RequestType[])Enum.GetValues(typeof(RequestType)))
                {
                    Thread calcMarginCallThread = new Thread(new ParameterizedThreadStart(makeRequestCall));
                    calcMarginCallThread.IsBackground = true;

                    //MarginRequestRunSingleInstrument mrr = new MarginRequestRunSingleInstrument();
                    
                    
                    //mrr.instrumentId = instrumentCnt;
                    //if (requestType == RequestType.FCMTYPE)
                    //{
                    //    mrr.requestType = RequestType.FCMTYPE;
                    //}
                    //else
                    //{
                    //    mrr.requestType = RequestType.MODELTYPE;
                    //}

                    calcMarginCallThread.Start();
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
            //MarginRequestRun mrr = (MarginRequestRun)obj;

            //int instrumentCnt = mrr.instrumentId;

            //RequestType requestType = mrr.requestType;

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

            //if (requestType == RequestType.MODELTYPE)
            //for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
            {
                

                for (int i = 0; i < contractList.Count; i++)
                {
                    
                    {
                        string[] line = new string[12];

                        line[0] = "tml1";
                        line[1] = instrument.exchangeSymbol + "_ORDER_TEST";
                        line[2] = instrument.spanExchWebAPISymbol;
                        line[3] = "";
                        line[4] = "";

                        //int spreadIdx = optionSpreadExpressionList[i].spreadIdx[0];
                        //int legIdx = optionSpreadExpressionList[i].legIdx[0];

                        if (contractList[i].contractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            line[5] = instrument.spanFutureCode;

                            line[6] = contractList[i].contractYear.ToString()
                            +
                            contractList[i].contractMonthInt.ToString("00");

                            line[7] = "";
                            line[8] = "";
                            line[9] = "";
                        }
                        else
                        {
                            line[5] = instrument.spanOptionCode;

                            line[6] = contractList[i].optionYear.ToString()
                            +
                            contractList[i].optionMonthInt.ToString("00");

                            if (contractList[i].contractType == OPTION_SPREAD_CONTRACT_TYPE.CALL)
                            {
                                line[7] = "CALL";
                            }
                            else
                            {
                                line[7] = "PUT";
                            }

                            line[8] = contractList[i].strikePrice.ToString();

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

                            line[9] = contractList[i].contractYear.ToString()
                            +
                            contractList[i].contractMonthInt.ToString("00");
                        }





                        line[10] = contractList[i].numberOfContracts.ToString();
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
                        + instrument.exchangeSymbol + "_ORDER_TEST_csvTrades" + 
                        ".csv", csvTrades.ToString());
                }
                else
                {
                    RunAsync(csvTrades, httpClient);
                }
            
        }

        public async void RunAsync(StringBuilder csvTrades,
            HttpClient httpClient)        
        {
            

            
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

                                //outputInstrument.coreAPImarginId = marginId;

                                //if (requestType == RequestType.MODELTYPE)
                                //{
                                //    instrument.coreAPImarginId = marginId;
                                //}
                                //else
                                //{
                                //    instrument.coreAPI_FCM_marginId = marginId;
                                //}

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

                                //portfolioSpreadTotals.initialMarginTotals = 0;
                                //portfolioSpreadTotals.maintenanceMarginTotals = 0;

                                //portfolioSpreadTotals.initialFCM_MarginTotals = 0;
                                //portfolioSpreadTotals.maintenanceFCM_MarginTotals = 0;

                                //if (requestType == RequestType.MODELTYPE)
                                {
                                    double coreAPIinitialMargin = Convert.ToDouble(initialMargin);

                                    double coreAPImaintenanceMargin = Convert.ToDouble(maintenanceMargin);


                                    if (optionPlChart != null)
                                    {
                                        optionPlChart.updateInitialMargin(coreAPIinitialMargin);

                                        optionPlChart.updateMaintenanceMargin(coreAPImaintenanceMargin);
                                    }
                                    else
                                    {
                                        optionPLChartUserForm.updateInitialMargin(coreAPIinitialMargin);

                                        optionPLChartUserForm.updateMaintenanceMargin(coreAPImaintenanceMargin);
                                    }

                                    //Console.WriteLine(coreAPIinitialMargin);

                                    //for (int totalInstCnt = 0; totalInstCnt < instruments.Length; totalInstCnt++)
                                    //{
                                   //     portfolioSpreadTotals.initialMarginTotals += instruments[totalInstCnt].coreAPIinitialMargin;

                                    //    portfolioSpreadTotals.maintenanceMarginTotals += instruments[totalInstCnt].coreAPImaintenanceMargin;
                                    //}

                                    //optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_MARGIN,
                                    //    instrumentCnt, instruments[instrumentCnt].coreAPIinitialMargin);

                                    //optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_MARGIN,
                                    //    instrumentCnt, instruments[instrumentCnt].coreAPImaintenanceMargin);

                                    //optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_MARGIN,
                                    //    instruments.Length, portfolioSpreadTotals.initialMarginTotals);

                                    //optionRealtimeMonitor.fillInstrumentSummary((int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_MARGIN,
                                    //    instruments.Length, portfolioSpreadTotals.maintenanceMarginTotals);
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

    }
}
