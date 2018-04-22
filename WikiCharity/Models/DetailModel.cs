using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class DetailModel
    {
        public string ABN { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }
        public string Tax { get; set; }
        public int StaffFull { get; set; }
        public int StaffPart { get; set; }
        public int StaffCasual { get; set; }
        public int StaffVolun { get; set; }
        public string Address { get; set; }

        public string Website { get; set; }
        public string Description { get; set; }
        public string Beneficiaries { get; set; }

        public string AccountInfo { get; set; } = "";
        public double Donations { get; set; } = 0;
        public double EmpExpense { get; set; } = 0;
        public string ReportFrom { get; set; } = "";
        public string ReportTo { get; set; } = "";
        public double GoverGrant { get; set; } = 0;
        public double GDInAus { get; set; } = 0;
        public double GDOutAus { get; set; } = 0;
        public double NetAssets { get; set; } = 0;
        public double NetSurplus { get; set; } = 0;
        public double TotalAssets { get; set; } = 0;
        public double TotalCurrentLia { get; set; } = 0;
        public double TotalNotCurrentLia { get; set; } = 0;
        public double TotalCurrentAssets { get; set; } = 0;
        public double TotalNotCurrentAssets { get; set; } = 0;
        public double Expense { get; set; } = 0;
        public double TotalGrossIncome { get; set; } = 0;
        public double TotalLia { get; set; } = 0;
        public string Year { get; set; } = "";

        public double EmpExpensePersentage { get; set; } = 0;
        public double OtherIncome { get; set; } = 0;
        public double AllOtherExpense { get; set; } = 0;
        public double DebtEquityRadio { get; set; } = 0;
        public double SavingRadio { get; set; } = 0;
        public double CurrentRadio { get; set; } = 0;
        public double DebtRadio { get; set; } = 0;
        public double EmpExpInSizeMedian { get; set; } = 0;
        public double DebtEquityInSizeMedian { get; set; } = 0;
        public double SavingRadioInSizeMedian { get; set; } = 0;
        public double CurrentRadioInSizeMedian { get; set; } = 0;
        public double DebtRadioInSizeMedian { get; set; } = 0;
    }
}