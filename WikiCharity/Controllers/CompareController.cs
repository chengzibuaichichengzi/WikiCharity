using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiCharity.Models;

namespace WikiCharity.Controllers
{
    public class CompareController : Controller
    {
        //Server side DB
        private static CharityV2ServerEntities db = new CharityV2ServerEntities();
        private static List<Charity> allCharities = db.Charities.ToList();

        //Local DB
        //private static CharityV2Entities db = new CharityV2Entities();
        //private static List<Charity> allCharities = db.Charities.ToList<Charity>();

        //my favorite list (maximum 10)
        private static List<Charity> myList = new List<Charity>();
        //my compare list (2 or 3)
        private static List<Charity> compareList = new List<Charity>();

        // GET: Compare
        public ActionResult FavList()
        {
            myList = Session["MyList"] as List<Charity>;
            compareList = new List<Charity>();
            Session["CompareList"] = compareList;
            
            return View(myList);
        }

        public ActionResult compare()
        {
            compareList = Session["CompareList"] as List<Charity>;
            if (compareList != null)
            {
                ViewBag.id1 = compareList[0].Id;
                ViewBag.id2 = compareList[1].Id;
            }
            else
            {
                ViewBag.id1 = 20000;
                ViewBag.id2 = 20000;
            }
            List<DetailModel> finalModelList = new List<DetailModel>();
            for (int i = 0; i < 2; i++)
            {
                var ABN = "";
                ABN = compareList[i].ABN;
                List<FinancialNew> finList = new List<FinancialNew>();
                finList = db.FinancialNews.Where(x => x.ABN == ABN).ToList();
                finList = finList.Where(x => x.FYear == "2016").ToList();
                DetailModel detailModel = new DetailModel();
                detailModel.ABN = compareList[i].ABN;
                detailModel.Size = compareList[i].Size;
                if (compareList[i].Tax == true)
                {
                    detailModel.Tax = "Yes";
                }
                else
                {
                    detailModel.Tax = "No";
                }
                detailModel.StaffFull = compareList[i].StaffFull.Value;
                detailModel.StaffPart = compareList[i].StaffPart.Value;
                detailModel.StaffCasual = compareList[i].StaffCasual.Value;
                detailModel.StaffVolun = compareList[i].StaffVolun.Value;
                detailModel.Name = compareList[i].Name;
                //detailModel.Address = address;
                detailModel.Website = compareList[i].Website;
                detailModel.Description = compareList[i].Description;
                detailModel.Beneficiaries = compareList[i].Beneficiaries;
                FinancialNew fin = new FinancialNew();
                if (finList.Count != 0)
                {
                    fin = finList[0];
                    detailModel.AccountInfo = fin.AccountInfo;
                    detailModel.Donations = fin.Donations.Value;
                    detailModel.EmpExpense = fin.EmpExpense.Value;
                    detailModel.ReportFrom = fin.ReportFrom;
                    detailModel.ReportTo = fin.ReportTo;
                    detailModel.GoverGrant = fin.GoverGrant.Value;
                    detailModel.GDInAus = fin.GDInAus.Value;
                    detailModel.GDOutAus = fin.GDOutAus.Value;
                    detailModel.NetAssets = fin.NetAssets.Value;
                    detailModel.NetSurplus = fin.NetSurplus.Value;
                    detailModel.TotalAssets = fin.TotalAssets.Value;
                    detailModel.TotalCurrentLia = fin.TotalCurrentLia.Value;
                    detailModel.TotalNotCurrentLia = fin.TotalNotCurrentLia.Value;
                    detailModel.TotalCurrentAssets = fin.TotalCurrentAssets.Value;
                    detailModel.TotalNotCurrentAssets = fin.TotalNotCurrentAssets.Value;
                    detailModel.Expense = fin.Expense.Value;
                    detailModel.TotalGrossIncome = fin.TotalGrossIncome.Value;
                    detailModel.TotalLia = fin.TotalLia.Value;
                    detailModel.Year = fin.FYear;
                    detailModel.EmpExpensePersentage = fin.EmpExpensePersentage.Value;
                    detailModel.OtherIncome = fin.OtherIncome.Value;
                    detailModel.AllOtherExpense = fin.AllOtherExpense.Value;
                    detailModel.DebtEquityRadio = fin.DebtEquityRadio.Value;
                    detailModel.SavingRadio = fin.SavingRadio.Value;
                    detailModel.CurrentRadio = fin.CurrentRadio.Value;
                    detailModel.DebtRadio = fin.DebtRadio.Value;
                    detailModel.EmpExpInSizeMedian = fin.EmpExpInSizeMedian.Value;
                    detailModel.DebtEquityInSizeMedian = fin.DebtEquityInSizeMedian.Value;
                    detailModel.SavingRadioInSizeMedian = fin.SavingRadioInSizeMedian.Value;
                    detailModel.CurrentRadioInSizeMedian = fin.CurrentRadioInSizeMedian.Value;
                    detailModel.DebtRadioInSizeMedian = fin.DebtRadioInSizeMedian.Value;
                }
                else
                {
                    detailModel.Year = "2016";
                }
                finalModelList.Add(detailModel);
            }
            
            return View(finalModelList);
        }

        public ActionResult compares()
        {
            compareList = Session["CompareList"] as List<Charity>;
            if (compareList != null)
            {
                ViewBag.id1 = compareList[0].Id;
                ViewBag.id2 = compareList[1].Id;
                ViewBag.id3 = compareList[2].Id;
            }
            else
            {
                ViewBag.id1 = 20000;
                ViewBag.id2 = 20000;
                ViewBag.id2 = 20000;
            }
            List<DetailModel> finalModelList = new List<DetailModel>();
            for (int i = 0; i < 3; i++)
            {
                var ABN = "";
                ABN = compareList[i].ABN;
                List<FinancialNew> finList = new List<FinancialNew>();
                finList = db.FinancialNews.Where(x => x.ABN == ABN).ToList();
                finList = finList.Where(x => x.FYear == "2016").ToList();
                DetailModel detailModel = new DetailModel();
                detailModel.ABN = compareList[i].ABN;
                detailModel.Size = compareList[i].Size;
                if (compareList[i].Tax == true)
                {
                    detailModel.Tax = "Yes";
                }
                else
                {
                    detailModel.Tax = "No";
                }
                detailModel.StaffFull = compareList[i].StaffFull.Value;
                detailModel.StaffPart = compareList[i].StaffPart.Value;
                detailModel.StaffCasual = compareList[i].StaffCasual.Value;
                detailModel.StaffVolun = compareList[i].StaffVolun.Value;
                detailModel.Name = compareList[i].Name;
                //detailModel.Address = address;
                detailModel.Website = compareList[i].Website;
                detailModel.Description = compareList[i].Description;
                detailModel.Beneficiaries = compareList[i].Beneficiaries;
                FinancialNew fin = new FinancialNew();
                if (finList.Count != 0)
                {
                    fin = finList[0];
                    detailModel.AccountInfo = fin.AccountInfo;
                    detailModel.Donations = fin.Donations.Value;
                    detailModel.EmpExpense = fin.EmpExpense.Value;
                    detailModel.ReportFrom = fin.ReportFrom;
                    detailModel.ReportTo = fin.ReportTo;
                    detailModel.GoverGrant = fin.GoverGrant.Value;
                    detailModel.GDInAus = fin.GDInAus.Value;
                    detailModel.GDOutAus = fin.GDOutAus.Value;
                    detailModel.NetAssets = fin.NetAssets.Value;
                    detailModel.NetSurplus = fin.NetSurplus.Value;
                    detailModel.TotalAssets = fin.TotalAssets.Value;
                    detailModel.TotalCurrentLia = fin.TotalCurrentLia.Value;
                    detailModel.TotalNotCurrentLia = fin.TotalNotCurrentLia.Value;
                    detailModel.TotalCurrentAssets = fin.TotalCurrentAssets.Value;
                    detailModel.TotalNotCurrentAssets = fin.TotalNotCurrentAssets.Value;
                    detailModel.Expense = fin.Expense.Value;
                    detailModel.TotalGrossIncome = fin.TotalGrossIncome.Value;
                    detailModel.TotalLia = fin.TotalLia.Value;
                    detailModel.Year = fin.FYear;
                    detailModel.EmpExpensePersentage = fin.EmpExpensePersentage.Value;
                    detailModel.OtherIncome = fin.OtherIncome.Value;
                    detailModel.AllOtherExpense = fin.AllOtherExpense.Value;
                    detailModel.DebtEquityRadio = fin.DebtEquityRadio.Value;
                    detailModel.SavingRadio = fin.SavingRadio.Value;
                    detailModel.CurrentRadio = fin.CurrentRadio.Value;
                    detailModel.DebtRadio = fin.DebtRadio.Value;
                    detailModel.EmpExpInSizeMedian = fin.EmpExpInSizeMedian.Value;
                    detailModel.DebtEquityInSizeMedian = fin.DebtEquityInSizeMedian.Value;
                    detailModel.SavingRadioInSizeMedian = fin.SavingRadioInSizeMedian.Value;
                    detailModel.CurrentRadioInSizeMedian = fin.CurrentRadioInSizeMedian.Value;
                    detailModel.DebtRadioInSizeMedian = fin.DebtRadioInSizeMedian.Value;
                }
                else
                {
                    detailModel.Year = "2016";
                }
                finalModelList.Add(detailModel);
            }
            return View(finalModelList);
        }

        public ActionResult AddToCompare(int? Id)
        {
            Charity charity = db.Charities.Find(Id);
            compareList = Session["CompareList"] as List<Charity>;
            SelectedModel model = new SelectedModel();
            //session doesn't expire
            if (compareList != null)
            {
                //using index to store matching critiaria
                int index = compareList.FindIndex(item => item.Id == Id);
                //already exist in compare list
                if (index >= 0)
                {
                    //remove from list
                    compareList = compareList.Where(item => item.Id != Id).ToList();
                    model.isSelected = false;
                    model.compareNum = compareList.Count();
                    Session["CompareList"] = compareList;
                }
                //does not exist in current list
                else
                {
                    compareList.Add(charity);
                    model.isSelected = true;
                    model.compareNum = compareList.Count();
                    Session["CompareList"] = compareList;
                }
            }
            else
            {
                compareList = new List<Charity>();
                compareList.Add(charity);
                model.isSelected = true;
                model.compareNum = compareList.Count();
                Session["CompareList"] = compareList;
            }
            ViewBag.comparedNumber = model.compareNum;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBarData()
        {
            compareList = Session["CompareList"] as List<Charity>;
            List<CompareModel> data = new List<CompareModel>();
            foreach (var x in compareList)
            {
                int id = x.Id;
                Charity charity = db.Charities.Find(id);
                var ABN = charity.ABN;
                List<FinancialNew> finList = db.FinancialNews.Where(i => i.ABN == ABN).ToList();
                finList = finList.Where(i => i.FYear == "2016").ToList();
                CompareModel bar1 = new CompareModel();
                bar1.name = x.Name;
                if (finList.Count != 0)
                {
                    bar1.TotalGrossIncome = finList[0].TotalGrossIncome.Value;
                    bar1.Expense = finList[0].Expense.Value;
                }
                data.Add(bar1);
            } 
                      
            var chartData = new object[data.Count + 1];
            chartData[0] = new object[]{
                "Name",
                "Revenue",
                "Expense"
            };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.name, i.TotalGrossIncome, i.Expense };
            }
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
    }
}