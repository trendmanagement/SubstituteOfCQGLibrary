using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace RealtimeSpreadMonitor
{
    public class InstrumentTransactionSummary
    {
        //HashTable
        //contains symbol and location in list
        public ConcurrentDictionary<string, ContractList> contractHashTable = new ConcurrentDictionary<string, ContractList>();

        //public Dictionary<string, ContractList> contractHashTableNotActive = new Dictionary<string, ContractList>();
    }

    public class ContractList
    {
        //contains currently selected contract and number of contracts to roll into

        public string cqgSymbol;

        public int indexOfInstrumentInInstrumentsArray;

        public OPTION_SPREAD_CONTRACT_TYPE contractType;

        //option info
        public int optionMonthInt;
        public int optionYear;
        public double strikePrice;
        //public int idUnderlyingContract;

        //future info
        public int contractMonthInt;
        public int contractYear;

        public DateTime expirationDate;
        //public double yearFraction;

        //calcYearFrac(optionStrategies[i].legInfo[legCounter].expirationDate,
        //                                                    DateTime.Now.Date);

        /*
         * public int idOption;
        public String optionName;
        public char optionMonth;
        public int optionMonthInt;
        public int optionYear;
        public double optionStrikePrice;
        public char optionCallOrPut;
        

        //future info
        public int idContract;
        public String contractName;
        public char contractMonth;
        public int contractMonthInt;
        public int contractYear;
        
        //common to both futures and options
        public DateTime expirationDate;
         * */

        public OptionSpreadExpression expression;

        public bool currentlyRollingContract;

        public int numberOfContracts;

        public int numberOfContractsNotActive;

        public int tempNumberOfContracts;

        public int tempNumberOfContractsNotActive;

        public double futurePriceUsedToCalculateStrikes;

        public bool reachedBarAfterDecisionBar_ForRoll = false;

        //public bool actionTest;

        public DateTime decisionTime;
        public DateTime transTime;
    }

    //public class DisplayedContracts
    //{
    //    public string cqgSymbol;
    //    public bool displayedInSummary;
    //}
}
