using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class FinancialCSV
    {
        public string ABN { get; set; }
        public string AccountInfo { get; set; }
        public double Donations { get; set; }
        public double EmpExpense { get; set; }
        public string ReportFrom { get; set; }
        public string ReportTo { get; set; }
        public double GoverGrant { get; set; }
        public double GDInAus { get; set; }
        public double GDOutAus { get; set; }
        public double NetAssets { get; set; }
        public double NetSurplus { get; set; }
        public double TotalAssets { get; set; }
        public double TotalCurrentLia { get; set; }
        public double TotalNotCurrentLia { get; set; }
        public double TotalCurrentAssets { get; set; }
        public double TotalNotCurrentAssets { get; set; }
        public double Expense { get; set; }
        public double TotalGrossIncome { get; set; }
        public double TotalLia { get; set; }
        public string Year { get; set; }

        public double EmpExpensePersentage { get; set; }
        public double OtherIncome { get; set; }
        public double AllOtherExpense { get; set; }
        public double DebtEquityRadio { get; set; }
        public double SavingRadio { get; set; }
        public double CurrentRadio { get; set; }
        public double DebtRadio { get; set; }
        public double EmpExpInSizeMedian { get; set; }
        public double DebtEquityInSizeMedian { get; set; }
        public double SavingRadioInSizeMedian { get; set; }
        public double CurrentRadioInSizeMedian { get; set; }
        public double DebtRadioInSizeMedian { get; set; }
    }
}