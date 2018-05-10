using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WikiCharity.Models;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data.Entity;

namespace WikiCharity.Controllers
{
    public class HomeController : Controller
    {
        //Server side DB
        //private static CharityV2ServerEntities db = new CharityV2ServerEntities();
        //private static List<Charity> allCharities = db.Charities.ToList();

        //Local DB
        private static CharityV2Entities db = new CharityV2Entities();
        private static List<Charity> allCharities = db.Charities.ToList<Charity>();

        private static List<Charity> myList = new List<Charity>();



        public ActionResult Index()
        {
            //create lists to store all string
            var benes = GetAllBenes();
            var DGRs = GetAllDGRs();
            var sizes = GetAllSizes();
            var states = GetAllStates();
            var actis = GetAllActis();
            //create a filter model to store all select lists
            var model = new FilterModel();
            model.beneficials = GetSelectListItems(benes);
            model.isDGRs = GetSelectListItems(DGRs);
            model.sizes = GetSelectListItems(sizes);
            model.states = GetSelectListItems(states);
            model.actis = GetSelectListItems(actis);
            //multiple selection list
            MultiSelectList beneList = new MultiSelectList(model.beneficials, "Value", "Text");
            //MultiSelectList stateList = new MultiSelectList(model.states, "Value", "Text");
            MultiSelectList sizeList = new MultiSelectList(model.sizes, "Value", "Text");
            MultiSelectList actiList = new MultiSelectList(model.actis, "Value", "Text");
            //stote in Viewbag
            ViewBag.multiSelectBenes = beneList;
            //ViewBag.multiSelectStates = stateList;
            ViewBag.multiSelectSizes = sizeList;
            ViewBag.multiSelectActis = actiList;

            Session["MyList"] = myList;

            return View(model);
        }

        

        [HttpPost]
        public ActionResult Index(FilterModel model)
        {
            //create lists to store all string
            var benes = GetAllBenes();
            var DGRs = GetAllDGRs();
            var sizes = GetAllSizes();
            var states = GetAllStates();
            var actis = GetAllActis();
            
            
            //use custom method GetSelectListItem to pass a list and then get select list items
            model.beneficials = GetSelectListItems(benes);
            model.sizes = GetSelectListItems(sizes);
            model.isDGRs = GetSelectListItems(DGRs);
            model.states = GetSelectListItems(states);
            model.actis = GetSelectListItems(actis);
            //store the filter model in the session
            Session["FilterModel"] = model;

            //go to filter result page
            return FilterResult(model);
            
        }

        public ActionResult FilterResult(FilterModel model)
        {
            //create lists to store all string
            var benes = GetAllBenes();
            var DGRs = GetAllDGRs();
            var sizes = GetAllSizes();
            var states = GetAllStates();
            var actis = GetAllActis();


            //use custom method GetSelectListItem to pass a list and then get select list items
            model.beneficials = GetSelectListItems(benes);
            model.sizes = GetSelectListItems(sizes);
            model.isDGRs = GetSelectListItems(DGRs);
            model.states = GetSelectListItems(states);
            model.actis = GetSelectListItems(actis);
            //store the filter model in the session
            Session["FilterModel"] = model;
            //multiple selection list
            MultiSelectList beneList = new MultiSelectList(model.beneficials, "Value", "Text");
            //MultiSelectList stateList = new MultiSelectList(model.states, "Value", "Text");
            MultiSelectList sizeList = new MultiSelectList(model.sizes, "Value", "Text");
            MultiSelectList actiList = new MultiSelectList(model.actis, "Value", "Text");
            //stote in Viewbag
            ViewBag.multiSelectBenes = beneList;
            //ViewBag.multiSelectStates = stateList;
            ViewBag.multiSelectSizes = sizeList;
            ViewBag.multiSelectActis = actiList;
            //get inout model through session
            //var model = Session["FilterModel"] as FilterModel;
            List<Charity> finalResult = new List<Charity>();
            if (model != null)
            {
                if (model.state != null)
                {
                    //store state in view bag
                    ViewBag.State = model.state;
                }
                else
                {
                    ViewBag.State = "All States";
                }
                if (model.actiString != null)
                {
                    //store main activities in view bag
                    ViewBag.Acti = string.Join(", ", model.actiString.ToArray());
                }
                else
                {
                    ViewBag.State = "All Activities";
                }
                if (model.beneString != null)
                {
                    //convert list of beneficiaries to a list and store it in Viewbag
                    ViewBag.Bene = string.Join(", ", model.beneString.ToArray());
                }
                else
                {
                    ViewBag.Bene = "All Beneficiaries";
                }

                if (model.isDGR != null)
                {
                    ViewBag.Tax = model.isDGR;
                }
                else
                {
                    ViewBag.Tax = "Any Tax";
                }

                if (model.sizeString != null)
                {
                    ViewBag.Size = string.Join(", ", model.sizeString.ToArray());
                }
                else
                {
                    ViewBag.Size = "Any Size";
                }
                if (model.name != null)
                {
                    ViewBag.Name = model.name;
                }
                else
                {
                    ViewBag.Name = "Any Charity Name";
                }

                //search for final result based on filter and name
                //finalResult = getFinalList();
                finalResult = getFinalList(model);
            }
            else
            {
                finalResult = allCharities;
                ViewBag.State = "All States";
                ViewBag.State = "All Activities";
                ViewBag.Bene = "All Beneficiaries";
                ViewBag.Tax = "Any Tax";
                ViewBag.Size = "Any Size";
                ViewBag.Name = "Any Charity Name";
            }

            return View(model);
        }

        //get search result based on 4 filters and search by name
        public List<Charity> getFinalList(FilterModel model)
        {
            //get current filter model
            //var model = Session["FilterModel"] as FilterModel;
            //get all charities first
            List<Charity> finalResult = new List<Charity>();
            finalResult = allCharities;

            //create a new list to store multi selected result from beneficiaries
            List<Charity> beneResult = new List<Charity>();
            //List<Charity> stateResult = new List<Charity>();
            List<Charity> sizeResult = new List<Charity>();
            List<Charity> actiResult = new List<Charity>();
            //create a new list to store selected benes
            List<string> benes = model.beneString;
           // List<string> states = model.stateString;
            List<string> sizes = model.sizeString;
            List<string> actis = model.actiString;

            if (actis != null)
            {
                //do a search for each selected beneficiary
                foreach (var i in actis)
                {
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.MainActivity.Contains(i)).ToList<Charity>();
                    //add every result to a final list
                    actiResult.AddRange(tempResult);
                }
                finalResult = actiResult;
            }

            if (benes != null)
            {
                //do a search for each selected beneficiary
                foreach (var i in benes)
                {
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.Beneficiaries.Contains(i)).ToList<Charity>();
                    //add every result to a final list
                    beneResult.AddRange(tempResult);
                }
                finalResult = beneResult;
            }
            //do size filtering
            if (sizes != null)
            {
                //finalResult = finalResult.Where(x => x.Size.Contains(model.size)).ToList<Charity>();
                foreach (var i in sizes)
                {
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.Size.Contains(i)).ToList<Charity>();
                    //add every result to a final list
                    sizeResult.AddRange(tempResult);
                }
                finalResult = sizeResult;
            }
            //do tax filtering
            if (!string.IsNullOrEmpty(model.isDGR))
            {
                if (model.isDGR == "Yes")
                {
                    finalResult = finalResult.Where(x => x.Tax == true).ToList<Charity>();
                }
                else
                {
                    finalResult = finalResult.Where(x => x.Tax == false).ToList<Charity>();
                }
            }
            //do state filtering
            if (!string.IsNullOrEmpty(model.state))
            {
                finalResult = finalResult.Where(x => x.State.Contains(model.state)).ToList<Charity>();
            }
            ViewBag.Count = finalResult.Count();
            //do name searching
            if (!string.IsNullOrEmpty(model.name))
            {
                finalResult = finalResult.Where(x => x.Name.ToLower().Contains(model.name.ToLower())).ToList<Charity>();
            }
            return finalResult;
        }

        //return json data for server-side loading of datatable
        [HttpPost]
        public ActionResult AjaxGetJsonData()
        {
            var model = Session["FilterModel"] as FilterModel;
            //TODO:Check why some rows get twice in datatable(eg. Wami Kata Old)
            List<Charity> finalResult = new List<Charity>();
            if (model != null)
            {
                //get search result for datatable
                finalResult = getFinalList(model);
            }
            else
            {
                finalResult = allCharities;
            }

            //server side parameters
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            int totalRows = finalResult.Count;

            //do filtering
            if (!string.IsNullOrEmpty(searchValue))
            {
                //name, beneficiaries, tax, size and state filtering
                finalResult = finalResult.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) ||
                x.Beneficiaries.ToLower().Contains(searchValue.ToLower()) ||
                x.Size.ToLower().Contains(searchValue.ToLower()) ||
                x.State.ToLower().Contains(searchValue.ToLower()) ||
                x.MainActivity.ToLower().Contains(searchValue.ToLower())).ToList<Charity>();
            }
            int totalRowsAfter = finalResult.Count;

            //do sorting
            finalResult = finalResult.OrderBy(sortColumnName + " " + sortDirection).ToList<Charity>();

            //do paging
            finalResult = finalResult.Skip(start).Take(length).ToList<Charity>();

            if (model.beneString != null)
            {
                var selectedBene = string.Join(", ", model.beneString.ToArray());
                foreach (var i in finalResult)
                {
                    var benes = new List<string>();
                    foreach (var x in model.beneString)
                    {
                        if (i.Beneficiaries.Contains(x))
                        {
                            benes.Add(x);
                        }
                    }
                    i.selectedBenes = string.Join(", ", benes.ToArray());
                }
            }
            else
            {
                foreach (var i in finalResult)
                {
                    i.selectedBenes = i.Beneficiaries;
                }
            }
            
            //return all the result in json format
            return Json(new { data = finalResult, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfter }, JsonRequestBehavior.AllowGet);
        }

       
        //return the count of searching result based on current filtering
        //the button on home page will be changed by this number
        [HttpPost]
        public ActionResult CountResult(string state, string bene, string size, string tax, string name, string acti)
        {
            var model = new FilterModel();
            model.state = state;
            model.beneficial = bene;
            model.size = size;
            model.isDGR = tax;
            model.name = name;
            model.acti = acti;
            List<Charity> finalResult = new List<Charity>();
            List<Charity> beneResult = new List<Charity>();
            List<Charity> sizeResult = new List<Charity>();
            //List<Charity> stateResult = new List<Charity>();
            List<Charity> actiResult = new List<Charity>();
            //a list to store selected beneficiaries
            List<string> benes = model.beneficial.Split(',').ToList();
            //List<string> states = model.state.Split(',').ToList();
            List<string> sizes = model.size.Split(',').ToList();
            List<string> actis = model.acti.Split(',').ToList();
            //remove the last white space
            benes.RemoveAt(benes.Count - 1);
            //states.RemoveAt(states.Count - 1);
            sizes.RemoveAt(sizes.Count - 1);
            actis.RemoveAt(actis.Count - 1);
            //in the first, finalresult is all charities
            finalResult = allCharities;
            if (!string.IsNullOrEmpty(model.acti))
            {
                //for every selected beneficiary, get a search result, and them combine them
                foreach (var i in actis)
                {
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.MainActivity.Contains(i)).ToList<Charity>();
                    actiResult.AddRange(tempResult);
                }
                finalResult = actiResult;
            }
            if (!string.IsNullOrEmpty(model.beneficial))
            {
                //for every selected beneficiary, get a search result, and them combine them
                foreach (var i in benes)
                {
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.Beneficiaries.Contains(i)).ToList<Charity>();
                    beneResult.AddRange(tempResult);
                }
                finalResult = beneResult;
            }
            //do size filtering
            if (!string.IsNullOrEmpty(model.size))
            {
                
                foreach (var i in sizes)
                {
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.Size.Contains(i)).ToList<Charity>();
                    sizeResult.AddRange(tempResult);
                }
                finalResult = sizeResult;
            }
            
            //do tax filtering
            if (!string.IsNullOrEmpty(model.isDGR) )
            {
                if (model.isDGR == "Yes")
                {
                    finalResult = finalResult.Where(x => x.Tax == true).ToList<Charity>();
                }
                else
                {
                    finalResult = finalResult.Where(x => x.Tax == false).ToList<Charity>();
                }
            }

            //do state filtering
            if (!string.IsNullOrEmpty(model.state))
            {
                finalResult = finalResult.Where(x => x.State.Contains(model.state)).ToList<Charity>();
            }
            //do name filtering
            if (!string.IsNullOrEmpty(model.name))
            {
                finalResult = finalResult.Where(x => x.Name.ToLower().Contains(model.name.ToLower())).ToList<Charity>();
            }
            //count the result number
            model.countNum = finalResult.Count().ToString();
            //return to home button
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult DonorAdvice()
        {

            ViewBag.Message = "Your application DonorAdvice page.";

            return View();
        }

        //Detail page for each charity
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //search charity by id
            Charity charity = db.Charities.Find(id);
            if (charity == null)
            {
                return HttpNotFound();
            }
            var ABN = charity.ABN;
            ViewBag.Message = "Your application description page.";
            ViewBag.PartDes = charity.Description.Substring(0, Math.Min(charity.Description.Length, 200)) + "...";
            //search charities in the financial table through ABN, cause may have 3 rows for each charity 
            List<FinancialNew> finList = db.FinancialNews.Where(i => i.ABN == ABN).ToList();

            //combine address, state, towncity and postcode
            var address = "";
            if (!charity.AddressLine2.Contains("NA"))
            {
                address = charity.AddressLine1 + ", " + charity.AddressLine2 + ", " + charity.TownCity + ", "
                    + charity.State + "," + charity.Postcode;
            }
            else
            {
                address = charity.AddressLine1 + ", " + charity.TownCity + ", "
                    + charity.State + "," + charity.Postcode;
            }


            List<DetailModel> finalModelList = new List<DetailModel>();
            //do a 3 times loop to generate 3 models for each charity
            var a = 2014;
            var b = a.ToString();
            //3 times loop for 2014, 2015 and 2016
            for (int i=0;i<3;i++)
            {
                //get all attrbutes
                DetailModel detailModel = new DetailModel();
                FinancialNew fin = new FinancialNew();
                detailModel.ABN = charity.ABN;
                detailModel.Size = charity.Size;
                if (charity.Tax == true)
                {
                    detailModel.Tax = "Yes";
                }
                else
                {
                    detailModel.Tax = "No";
                }
                detailModel.StaffFull = charity.StaffFull.Value;
                detailModel.StaffPart = charity.StaffPart.Value;
                detailModel.StaffCasual = charity.StaffCasual.Value;
                detailModel.StaffVolun = charity.StaffVolun.Value;
                detailModel.Name = charity.Name;
                detailModel.Address = address;
                detailModel.Website = charity.Website;
                detailModel.Description = charity.Description;
                detailModel.Beneficiaries = charity.Beneficiaries;
                List<FinancialNew> flist = finList.Where(f => f.FYear == b).ToList();
                //if charity has the financial data for current year (b yaer)
                if (flist.Count != 0)
                {
                    fin = finList.Where(f => f.FYear == b).ToList()[0];
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
                //if charity dosen't have the financial data for current year (b yaer)
                else
                {
                    detailModel.Year = b;
                }

                finalModelList.Add(detailModel);
                a++;
                b = a.ToString();
            }              
                
            return View(finalModelList);
        }

        //return json data for line chart on detail page
        [HttpPost]
        public ActionResult GetChartData(string Id)
        {
            
            int id = Convert.ToInt32(Id);
            List<LineModel> data = new List<LineModel>();
            Charity charity = db.Charities.Find(id);
            //Here MyDatabaseEntities  is our dbContext
            var ABN = charity.ABN;
            List<FinancialNew> finList = db.FinancialNews.Where(i => i.ABN == ABN).ToList();
            LineModel line2014 = new LineModel();
            line2014.year = "2014";
            List<FinancialNew> flist = finList.Where(f => f.FYear == "2014").ToList();
            //if this charity has 2014 financial data
            if (flist.Count != 0)
            {
                line2014.NetSurplus = finList.Where(f => f.FYear == "2014").ToList()[0].NetSurplus.Value;
            }
            LineModel line2015 = new LineModel();
            line2015.year = "2015";
            flist = finList.Where(f => f.FYear == "2015").ToList();
            //if this charity has 2015 financial data
            if (flist.Count != 0)
            {
                line2015.NetSurplus = finList.Where(f => f.FYear == "2015").ToList()[0].NetSurplus.Value;
            }
            LineModel line2016 = new LineModel();
            line2016.year = "2016";
            flist = finList.Where(f => f.FYear == "2016").ToList();
            //if this charity has 2016 financial data
            if (flist.Count != 0)
            {
                line2016.NetSurplus = finList.Where(f => f.FYear == "2016").ToList()[0].NetSurplus.Value;
            }
            data.Add(line2014);
            data.Add(line2015);
            data.Add(line2016);


            var chartData = new object[data.Count + 1];
            chartData[0] = new object[]{
                "Year",
                "Net Surplus Deficit"
            };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.year, i.NetSurplus};
            }
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        //return json data for bar chart on detail page
        public ActionResult GetBarData(string Id)
        {
            int id = Convert.ToInt32(Id);
            List<BarModel> data = new List<BarModel>();
            Charity charity = db.Charities.Find(id);
            //Here MyDatabaseEntities is our dbContext
            var ABN = charity.ABN;
            List<FinancialNew> finList = db.FinancialNews.Where(i => i.ABN == ABN).ToList();
            BarModel bar2014 = new BarModel();
            bar2014.year = "2014";
            List<FinancialNew> flist = finList.Where(f => f.FYear == "2014").ToList();
            //if this charity has 2014 financial data
            if (flist.Count != 0)
            {
                bar2014.TotalGrossIncome = finList.Where(f => f.FYear == "2014").ToList()[0].TotalGrossIncome.Value;
                bar2014.Expense = finList.Where(f => f.FYear == "2014").ToList()[0].Expense.Value;
            }
            BarModel bar2015 = new BarModel();
            bar2015.year = "2015";
            //if this charity has 2015 financial data
            flist = finList.Where(f => f.FYear == "2015").ToList();
            if (flist.Count != 0)
            {
                bar2015.TotalGrossIncome = finList.Where(f => f.FYear == "2015").ToList()[0].TotalGrossIncome.Value;
                bar2015.Expense = finList.Where(f => f.FYear == "2015").ToList()[0].Expense.Value;
            }
            BarModel bar2016 = new BarModel();
            bar2016.year = "2016";
            //if this charity has 2016 financial data
            flist = finList.Where(f => f.FYear == "2016").ToList();
            if (flist.Count != 0)
            {
                bar2016.TotalGrossIncome = finList.Where(f => f.FYear == "2016").ToList()[0].TotalGrossIncome.Value;
                bar2016.Expense = finList.Where(f => f.FYear == "2016").ToList()[0].Expense.Value;
            }
            
            data.Add(bar2016);
            data.Add(bar2015);
            data.Add(bar2014);

            var chartData = new object[data.Count + 1];
            chartData[0] = new object[]{
                "Year",
                "Revenue",
                "Expense"
            };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.year, i.TotalGrossIncome, i.Expense };
            }
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBar2Data(string Id)
        {
            int id = Convert.ToInt32(Id);
            List<Bar2Model> data = new List<Bar2Model>();
            Charity charity = db.Charities.Find(id);
            //Here MyDatabaseEntities is our dbContext
            var ABN = charity.ABN;
            List<FinancialNew> finList = db.FinancialNews.Where(i => i.ABN == ABN).ToList();
            Bar2Model bar2014 = new Bar2Model();
            bar2014.year = "2014";
            List<FinancialNew> flist = finList.Where(f => f.FYear == "2014").ToList();
            //if this charity has 2014 financial data
            if (flist.Count != 0)
            {
                bar2014.TotalAssets = finList.Where(f => f.FYear == "2014").ToList()[0].TotalAssets.Value;
                bar2014.TotalLia = finList.Where(f => f.FYear == "2014").ToList()[0].TotalLia.Value;
            }
            Bar2Model bar2015 = new Bar2Model();
            bar2015.year = "2015";
            //if this charity has 2015 financial data
            flist = finList.Where(f => f.FYear == "2015").ToList();
            if (flist.Count != 0)
            {
                bar2015.TotalAssets = finList.Where(f => f.FYear == "2015").ToList()[0].TotalAssets.Value;
                bar2015.TotalLia = finList.Where(f => f.FYear == "2015").ToList()[0].TotalLia.Value;
            }
            Bar2Model bar2016 = new Bar2Model();
            bar2016.year = "2016";
            //if this charity has 2016 financial data
            flist = finList.Where(f => f.FYear == "2016").ToList();
            if (flist.Count != 0)
            {
                bar2016.TotalAssets = finList.Where(f => f.FYear == "2016").ToList()[0].TotalAssets.Value;
                bar2016.TotalLia = finList.Where(f => f.FYear == "2016").ToList()[0].TotalLia.Value;
            }
            data.Add(bar2014);
            data.Add(bar2015);
            data.Add(bar2016);
            
            
            var chartData = new object[data.Count + 1];
            chartData[0] = new object[]{
                "Year",
                "Total Assets",
                "Total Liabilities"
            };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.year, i.TotalAssets, i.TotalLia };
            }
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                //custom email body and message
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                //send to our team email
                message.To.Add(new MailAddress("qinggari@gmail.com"));
                
                message.Subject = "Form from WikiCharity";
                message.Body = string.Format(body, model.name, model.email, model.message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult AddToList(int? Id)
        {
            Charity charity = db.Charities.Find(Id);
            myList = Session["MyList"] as List<Charity>;
            SelectedModel model = new SelectedModel();
            if (myList != null)
            {
                //using index to store matching critiaria
                int index = myList.FindIndex(item => item.Id == Id);
                //already exist in my list
                if (index >= 0)
                {
                    //remove from list
                    myList = myList.Where(item => item.Id != Id).ToList();
                    if (ModelState.IsValid)
                    {
                        charity.isSelected = "N";
                        db.Entry(charity).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                    model.isSelected = false;
                    Session["MyList"] = myList;
                }
                //does not exist in current list
                else
                {
                    if (ModelState.IsValid)
                    {
                        charity.isSelected = "Y";
                        db.Entry(charity).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                    myList.Add(charity);
                    model.isSelected = true;
                    Session["MyList"] = myList;
                }
            }
            else
            {
                myList = new List<Charity>();
                myList.Add(charity);
                model.isSelected = true;
                Session["MyList"] = myList;
            }
            
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyList()
        {
            myList = Session["MyList"] as List<Charity>;
            return View(myList);
        }

        public ActionResult Sent()
        {
            return View();
        }

        public ActionResult FAQs()
        {
            ViewBag.Message = "Your FAQs page.";

            return View();
        }

        private IEnumerable<string> GetAllActis()
        {
            List<string> list = new List<string>
            {
                "Aged Care Activities", "Animal Protection", "Civic and advocacy activities", "Culture and arts", "Economic/ social and community development",
                "Emergency Relief", "Employment and training", "Environmental activities", "Grant-making activities", "Higher education", "Hospital services and rehabilitation activities",
                "Housing activities", "Income support and maintenance", "International activities", "Law and legal services", "Mental health and crisis intervention",
                "Other Education", "Other health service delivery", "Other recreation and social club activity", "Other Philanthropic", "Others", "Primary and secondary education",
                "Religious activities", "Research", "Social services", "Sports",
            };
            list.Sort();
            return list;

        }
        

        //get all beneficiary names which will be used in the filter select list
        private IEnumerable<string> GetAllBenes()
        {
            List<string> list = new List<string>
            {
                /*"Animal protection", "Aged care activities", "Civic and advocacy activities", "Culture and arts", "Economic social and community development",
                "Emergency relief", "Employment and training", "Environmental activities", "Grant-making activities", "Higher education",
                "Hospital services and rehabilitation activities", "Housing activities", "Income support and maintenance", "International activities",
                "Law and legal services", "Mental health and crisis intervention", "Primary and secondary education", "Religious activities", "Research",
                "Social Services", "Sports", "Other education", "Other health service delivery", "Other recreation and social club activity",
                "Other philanthropic intermediaries and voluntarism promotion", "Other activity",*/
                "General community in Australia", "Females", "Males", "Early childhood – under 6", "Children – 6 to under 15", "Youth – 15 to under 25",
                "Adults -25 to under 65", "Adults – 65 and over", "Aboriginal and Torres Strait Islander people (ATSI)", "Gay, lesbian, bisexual, transgender or intersex persons (GLBTI)",
                "Migrants, refugees or asylum seekers", "Other charities", "Other beneficiaries not listed", "Overseas communities or charities",
                "People in rural/regional/remote communities", "Families", "Financially disadvantaged people", "People at risk of homelessness",
                "People with chronic or terminal illness", "People with disabilities", "Pre or Post Release Offenders and Families", "Unemployed persons",
                "Veterans or their families", "Victims of crime", "Victims of disasters", "Culturally and Linguistically Diverse",
            };
            list.Sort();
            return list;
        }

        //get tax deductable filter select list
        private IEnumerable<string> GetAllDGRs()
        {
            return new List<string>
            {
                "Yes", "No",
            };
        }

        //get state filter select list
        private IEnumerable<string> GetAllStates()
        {
            List<string> list = new List<string>
            {
                "NSW", "QLD", "VIC", "ACT", "WA", "SA", "TAS", "NT",
            };
            list.Sort();
            return list;
        }

        //get charity size filter select list
        private IEnumerable<string> GetAllSizes()
        {
            return new List<string>
            {
                "Large", "Medium", "Small",
            };
        }

        //convert list of string to list of select list items
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }
            return selectList;
        }

    }
}