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

namespace WikiCharity.Controllers
{
    public class HomeController : Controller
    {
        //Server side DB
        //private static CharityTableDBEntities5 db = new CharityTableDBEntities5();
        //private static List<Charity> allCharities = db.Charities.ToList<Charity>();
        //Local DB
        private static WIKICHARITYEntities db = new WIKICHARITYEntities();
        private static List<Charity> allCharities = db.Charities.ToList<Charity>();

        public ActionResult Index()
        {
            var benes = GetAllBenes();
            var DGRs = GetAllDGRs();
            var sizes = GetAllSizes();
            var states = GetAllStates();
            var model = new FilterModel();
            model.beneficials = GetSelectListItems(benes);
            model.isDGRs = GetSelectListItems(DGRs);
            model.sizes = GetSelectListItems(sizes);
            model.states = GetSelectListItems(states);
            MultiSelectList beneList = new MultiSelectList(model.beneficials, "Value", "Text");
            ViewBag.multiSelectBenes = beneList;
            model.selectedBenes = beneList;
            allCharities = db.Charities.ToList<Charity>();

            


            GetAllFinancial();
            return View(model);
        }

        /*public MultiSelectList GetBeneSelect(string[] selectedValue)
        {
            var benes = GetAllBenes();
            var n = 1;
            List<Bene> list = new List<Bene>();
            foreach (var i in benes){
                list.Add(new Bene() { ID = n, Name = i });
            }
            return new MultiSelectList(list, "ID", "Name", selectedValue);
        }A*/

        [HttpPost]
        public ActionResult Index(FilterModel model, FormCollection form)
        {
                var benes = GetAllBenes();
                var sizes = GetAllSizes();
                var DGRs = GetAllDGRs();
                var states = GetAllStates();
                model.beneficials = GetSelectListItems(benes);
                model.sizes = GetSelectListItems(sizes);
                model.isDGRs = GetSelectListItems(DGRs);
                model.states = GetSelectListItems(states);
                Session["FilterModel"] = model;
            //model.beneficial = form["multiBenes"].;
            //string selectedBenes = form["multiBenes"];
            //ViewBag.multiSelectBenes = 


                return RedirectToAction("FilterResult");
            
        }

        public ActionResult FilterResult()
        {
            var model = Session["FilterModel"] as FilterModel;
            ViewBag.State = model.state;
            if (model.beneString != null)
            {
                ViewBag.Bene = string.Join(", ", model.beneString.ToArray());
            }
            else
            {
                ViewBag.Bene = "All Beneficiaries";
            }
            
            ViewBag.Tax = model.isDGR;
            ViewBag.Size = model.size;
            List<Charity> finalResult = new List<Charity>();
            finalResult = getFinalList();
            return View(finalResult);
        }

        public List<Charity> getFinalList()
        {
            //get current filter model
            var model = Session["FilterModel"] as FilterModel;
            //get all charities first
            List<Charity> finalResult = new List<Charity>();
            finalResult = allCharities;

            //create a new list to store multi selected result from beneficiaries
            List<Charity> beneResult = new List<Charity>();
            //create a new list to store selected benes
            List<string> names = model.beneString;


            if (names != null)
            {
                //do a search for each selected beneficiary
                foreach (var i in names)
                {
                    
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.Beneficiaries.Contains(i)).ToList<Charity>();
                    //add every result to a final list
                    beneResult.AddRange(tempResult);
                }
                finalResult = beneResult;
            }
            if (!string.IsNullOrEmpty(model.size))
            {
                finalResult = finalResult.Where(x => x.Size.Contains(model.size)).ToList<Charity>();
            }
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
            if (!string.IsNullOrEmpty(model.state))
            {
                finalResult = finalResult.Where(x => x.State.Contains(model.state)).ToList<Charity>();
            }
            ViewBag.Count = finalResult.Count();
            if (!string.IsNullOrEmpty(model.name))
            {
                finalResult = finalResult.Where(x => x.Name.ToLower().Contains(model.name.ToLower())).ToList<Charity>();
            }
            return finalResult;
        }

        [HttpPost]
        public ActionResult AjaxGetJsonData()
        {
            var model = Session["FilterModel"] as FilterModel;
            List<Charity> finalResult = new List<Charity>();
            finalResult = getFinalList();
            //List<TableResultModel> list = new List<TableResultModel>();
            
            /*foreach (var i in finalResult)
            {
                TableResultModel result = new TableResultModel();
                result.Name = i.CharityName;
                result.Beneficiaries = String.Join(", ", i.beneficiaries.ToArray());
                if (i.DGR == true)
                {
                    result.Tax = "Yes";
                }
                else
                {
                    result.Tax = "No";
                }
                result.Size = i.Size;
                result.State = i.State;
                list.Add(result);
            }*/

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
                finalResult = finalResult.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) ||
                x.Beneficiaries.ToLower().Contains(searchValue.ToLower()) ||
                x.Size.ToLower().Contains(searchValue.ToLower()) ||
                x.State.ToLower().Contains(searchValue.ToLower())).ToList<Charity>();
            }
            int totalRowsAfter = finalResult.Count;

            //do sorting
            finalResult = finalResult.OrderBy(sortColumnName + " " + sortDirection).ToList<Charity>();

            //do paging
            finalResult = finalResult.Skip(start).Take(length).ToList<Charity>();
 
            return Json(new { data = finalResult, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfter }, JsonRequestBehavior.AllowGet);
        }

       

        [HttpPost]
        public ActionResult CountResult(string state, string bene, string size, string tax)
        {
            var model = new FilterModel();
            model.state = state;
            model.beneficial = bene;
            model.size = size;
            model.isDGR = tax;
            List<Charity> finalResult = new List<Charity>();
            List<Charity> beneResult = new List<Charity>();
            List<string> names = model.beneficial.Split(',').ToList();
            names.RemoveAt(names.Count - 1);
            finalResult = allCharities;
            if (!string.IsNullOrEmpty(model.beneficial))
            {
                foreach (var i in names)
                {
                    //finalResult = IntersectCharity(SearchByBene(model), finalResult);
                    List<Charity> tempResult = new List<Charity>();
                    tempResult = finalResult.Where(x => x.Beneficiaries.Contains(i)).ToList<Charity>();
                    beneResult.AddRange(tempResult);
                }
                finalResult = beneResult;
            }
            if (!string.IsNullOrEmpty(model.size))
            {
                //finalResult = IntersectCharity(SearchBySize(model), finalResult);
                finalResult = finalResult.Where(x => x.Size.Contains(model.size)).ToList<Charity>();
            }
            if (!string.IsNullOrEmpty(model.isDGR) )
            {
                if (model.isDGR == "Yes")
                {
                    finalResult = finalResult.Where(x => x.Tax == true).ToList<Charity>();
                }
                //finalResult = IntersectCharity(SearchByTax(model), finalResult);
                else
                {
                    finalResult = finalResult.Where(x => x.Tax == false).ToList<Charity>();
                }
            }
            if (!string.IsNullOrEmpty(model.state))
            {
                //finalResult = IntersectCharity(SearchByState(model), finalResult);
                finalResult = finalResult.Where(x => x.State.Contains(model.state)).ToList<Charity>();
            }
            model.countNum = finalResult.Count().ToString();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /*private List<CharityModel> IntersectCharity(List<CharityModel> list1, List<CharityModel> list2)
        {
            List<CharityModel> finalResult = new List<CharityModel>();
            foreach (var c1 in list1)
            {
                foreach (var c2 in list2)
                {
                    if (c1.ABN == c2.ABN)
                    {
                        finalResult.Add(c1);
                        break;
                    }
                }
            }
            return finalResult;
        }*/

        /*private List<CharityModel> SearchByBene(FilterModel model)
        {
            List<CharityModel> charities = new List<CharityModel>();
            List<CharityModel> result = new List<CharityModel>();
            charities = GetAllCharity();
            foreach (var charity in charities)
            {
                foreach (var t in charity.beneficiaries)
                {
                    if (t == model.beneficial)
                    {
                        result.Add(charity);
                        break;
                    }
                }
            }
            return result;
        }

        private List<CharityModel> SearchByTax(FilterModel model)
        {
            List<CharityModel> charities = new List<CharityModel>();
            List<CharityModel> result = new List<CharityModel>();
            charities = GetAllCharity();
            foreach (var charity in charities)
            {
                if (model.isDGR == "Yes")
                {
                    if (charity.DGR == true)
                    {
                        result.Add(charity);
                    }
                }
                if (model.isDGR == "No")
                {
                    if (charity.DGR == false)
                    {
                        result.Add(charity);
                    }
                }
                
            }
            return result;
        }

        private List<CharityModel> SearchBySize(FilterModel model)
        {
            List<CharityModel> charities = new List<CharityModel>();
            List<CharityModel> result = new List<CharityModel>();
            charities = GetAllCharity();
            foreach (var charity in charities)
            {
                if (charity.Size == model.size)
                {
                    result.Add(charity);
                }
            }
            return result;
        }

        private List<CharityModel> SearchByState(FilterModel model)
        {
            List<CharityModel> charities = new List<CharityModel>();
            List<CharityModel> result = new List<CharityModel>();
            charities = GetAllCharity();
            foreach (var charity in charities)
            {
                if (charity.State == model.state)
                {
                    result.Add(charity);
                }
            }
            return result;
        }

        private List<CharityModel> SearchByName(FilterModel model)
        {
            List<CharityModel> charities = new List<CharityModel>();
            List<CharityModel> result = new List<CharityModel>();
            charities = GetAllCharity();
            foreach (var charity in charities)
            {
                if (charity.CharityName.ToUpper().Contains(model.name.ToUpper()))
                {
                    result.Add(charity);
                }
            }
            return result;
        }
        */

        public ActionResult About()
        {

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Charity charity = db.Charities.Find(id);
            if (charity == null)
            {
                return HttpNotFound();
            }
            var ABN = charity.ABN;
            ViewBag.Message = "Your application description page.";
            /*List<FinancialNew> finList = db.FinancialNews.Where(i => i.ABN == ABN).ToList();
            FinancialNew fin2014 = finList.Where(i => i.FYear == "2014").ToList()[0];
            FinancialNew fin2015 = finList.Where(i => i.FYear == "2015").ToList()[0];
            FinancialNew fin2016 = finList.Where(i => i.FYear == "2016").ToList()[0];
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

            DetailModel detailModel2014 = new DetailModel();
            detailModel2014.Name = charity.Name;
            detailModel2014.Address = address;
            detailModel2014.Website = charity.Website;
            detailModel2014.Description = charity.Description;
            detailModel2014.Beneficiaries = charity.Beneficiaries;
            detailModel2014.Name = charity.Name;

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
        public double DebtRadioInSizeMedian { get; set; }*/
                
                
            return View(charity);
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
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("qinggari@gmail.com"));
                
                message.Subject = "Form from WikiCharity";
                message.Body = string.Format(body, model.name, model.email, model.message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    
                    await smtp.SendMailAsync(message);
                    return View(model);
                }
            }
            return View(model);
        }

      
        public ActionResult FAQs()
        {
            ViewBag.Message = "Your FAQs page.";

            return View();
        }


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

        private IEnumerable<string> GetAllDGRs()
        {
            return new List<string>
            {
                "Yes", "No",
            };
        }

        private IEnumerable<string> GetAllStates()
        {
            List<string> list = new List<string>
            {
                "NSW", "QLD", "VIC", "ACT", "WA", "SA", "TAS", "NT",
            };
            list.Sort();
            return list;
        }

        private IEnumerable<string> GetAllSizes()
        {
            return new List<string>
            {
                "Large", "Medium", "Small",
            };
        }

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

        private List<Charity> GetAllCharity()
        {
            List<Charity> charities = new List<Charity>();
            charities = db.Charities.ToList<Charity>();
            return charities;
        }

        private List<FinancialCSV> GetAllFinancial()
        {
            List<FinancialCSV> charities = new List<FinancialCSV>();
            string filePath = Server.MapPath("~/Uploads/FinancialData3.csv");
            string csvData = System.IO.File.ReadAllText(filePath);
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    FinancialCSV newCharity = new FinancialCSV();
                    newCharity.ABN = row.Split(',')[0];
                    newCharity.AccountInfo = row.Split(',')[1];
                    newCharity.Donations = Convert.ToDouble(row.Split(',')[2]);
                    newCharity.EmpExpense = Convert.ToDouble(row.Split(',')[3]);
                    newCharity.ReportFrom = row.Split(',')[4];
                    newCharity.ReportTo = row.Split(',')[5];
                    newCharity.GoverGrant = Convert.ToDouble(row.Split(',')[6]);
                    newCharity.GDInAus = Convert.ToDouble(row.Split(',')[7]);
                    newCharity.GDOutAus = Convert.ToDouble(row.Split(',')[8]);
                    newCharity.NetAssets = Convert.ToDouble(row.Split(',')[9]);
                    newCharity.NetSurplus = Convert.ToDouble(row.Split(',')[10]);
                    newCharity.TotalAssets = Convert.ToDouble(row.Split(',')[11]);
                    newCharity.TotalCurrentAssets = Convert.ToDouble(row.Split(',')[12]);
                    newCharity.TotalCurrentLia = Convert.ToDouble(row.Split(',')[13]);
                    newCharity.Expense = Convert.ToDouble(row.Split(',')[14]);
                    newCharity.TotalGrossIncome = Convert.ToDouble(row.Split(',')[15]);
                    newCharity.TotalLia = Convert.ToDouble(row.Split(',')[16]);
                    newCharity.TotalNotCurrentAssets = Convert.ToDouble(row.Split(',')[17]);
                    newCharity.TotalNotCurrentLia = Convert.ToDouble(row.Split(',')[18]);
                    newCharity.Year = row.Split(',')[19];
                    newCharity.EmpExpensePersentage = Convert.ToDouble(row.Split(',')[20]);
                    newCharity.OtherIncome = Convert.ToDouble(row.Split(',')[21]);
                    newCharity.AllOtherExpense = Convert.ToDouble(row.Split(',')[22]);
                    newCharity.DebtEquityRadio = Convert.ToDouble(row.Split(',')[23]);
                    newCharity.SavingRadio = Convert.ToDouble(row.Split(',')[24]);
                    newCharity.CurrentRadio = Convert.ToDouble(row.Split(',')[25]);
                    newCharity.DebtRadio = Convert.ToDouble(row.Split(',')[26]);
                    newCharity.EmpExpInSizeMedian = Convert.ToDouble(row.Split(',')[27]);
                    newCharity.DebtEquityInSizeMedian = Convert.ToDouble(row.Split(',')[28]);
                    newCharity.SavingRadioInSizeMedian = Convert.ToDouble(row.Split(',')[29]);
                    newCharity.CurrentRadioInSizeMedian = Convert.ToDouble(row.Split(',')[30]);
                    newCharity.DebtRadioInSizeMedian = Convert.ToDouble(row.Split(',')[31]);
                    charities.Add(newCharity);
                }
            }
            return charities;
        }


                    /*private List<CharityModel> GetAllCharity()
                    {
                        List<CharityModel> charities = new List<CharityModel>();
                        string filePath = Server.MapPath("~/Uploads/newData1.csv");
                        string filePath2 = Server.MapPath("~/Uploads/desData.csv");
                        string csvData = System.IO.File.ReadAllText(filePath);
                        string desData = System.IO.File.ReadAllText(filePath2);
                        foreach (string row in csvData.Split('\n'))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                CharityModel newCharity = new CharityModel();
                                newCharity.ABN = row.Split(',')[0];
                                newCharity.CharityName = row.Split(',')[1];
                                newCharity.OtherName = row.Split(',')[2];
                                newCharity.AddressLine1 = row.Split(',')[3];
                                newCharity.AddressLine2 = row.Split(',')[4];
                                newCharity.TownCity = row.Split(',')[5];
                                newCharity.State = row.Split(',')[6];
                                newCharity.Postcode = row.Split(',')[7];
                                newCharity.Size = row.Split(',')[8];
                                if (row.Split(',')[9] == "Y")
                                {
                                    newCharity.BRC = true;
                                }
                                else
                                {
                                    newCharity.BRC = false;
                                }
                                if (row.Split(',')[10] == "Y")
                                {
                                    newCharity.ConductedActivity = true;
                                }
                                else
                                {
                                    newCharity.ConductedActivity = false;
                                }
                                newCharity.MainActivity = row.Split(',')[11];
                                if (row.Split(',')[12] != "N")
                                {
                                    newCharity.activities.Add("Animal protection");
                                }
                                if (row.Split(',')[13] != "N")
                                {
                                    newCharity.activities.Add("Aged care activities");
                                }
                                if (row.Split(',')[14] != "N")
                                {
                                    newCharity.activities.Add("Civic and advocacy activities");
                                }
                                if (row.Split(',')[15] != "N")
                                {
                                    newCharity.activities.Add("Culture and arts");
                                }
                                if (row.Split(',')[16] != "N")
                                {
                                    newCharity.activities.Add("Economic social and community development");
                                }
                                if (row.Split(',')[17] != "N")
                                {
                                    newCharity.activities.Add("Emergency relief");
                                }
                                if (row.Split(',')[18] != "N")
                                {
                                    newCharity.activities.Add("Employment and training");
                                }
                                if (row.Split(',')[19] != "N")
                                {
                                    newCharity.activities.Add("Environmental activities");
                                }
                                if (row.Split(',')[20] != "N")
                                {
                                    newCharity.activities.Add("Grant-making activities");
                                }
                                if (row.Split(',')[21] != "N")
                                {
                                    newCharity.activities.Add("Higher education");
                                }
                                if (row.Split(',')[22] != "N")
                                {
                                    newCharity.activities.Add("Hospital services and rehabilitation activities");
                                }
                                if (row.Split(',')[23] != "N")
                                {
                                    newCharity.activities.Add("Housing activities");
                                }
                                if (row.Split(',')[24] != "N")
                                {
                                    newCharity.activities.Add("Income support and maintenance");
                                }
                                if (row.Split(',')[25] != "N")
                                {
                                    newCharity.activities.Add("International activities");
                                }
                                if (row.Split(',')[26] != "N")
                                {
                                    newCharity.activities.Add("Law and legal services");
                                }
                                if (row.Split(',')[27] != "N")
                                {
                                    newCharity.activities.Add("Mental health and crisis intervention");
                                }
                                if (row.Split(',')[28] != "N")
                                {
                                    newCharity.activities.Add("Primary and secondary education");
                                }
                                if (row.Split(',')[29] != "N")
                                {
                                    newCharity.activities.Add("Religious activities");
                                }
                                if (row.Split(',')[30] != "N")
                                {
                                    newCharity.activities.Add("Research");
                                }
                                if (row.Split(',')[31] != "N")
                                {
                                    newCharity.activities.Add("Social Services");
                                }
                                if (row.Split(',')[32] != "N")
                                {
                                    newCharity.activities.Add("Sports");
                                }
                                if (row.Split(',')[33] != "N")
                                {
                                    newCharity.activities.Add("Other education");
                                }
                                if (row.Split(',')[34] != "N")
                                {
                                    newCharity.activities.Add("Other health service delivery");
                                }
                                if (row.Split(',')[35] != "N")
                                {
                                    newCharity.activities.Add("Other recreation and social club activity");
                                }
                                if (row.Split(',')[36] != "N")
                                {
                                    newCharity.activities.Add("Other philanthropic intermediaries and voluntarism promotion");
                                }
                                if (row.Split(',')[37] != "N")
                                {
                                    newCharity.activities.Add("Other activity");
                                }
                                if (row.Split(',')[38] != "N")
                                {
                                    newCharity.operateStates.Add("ACT");
                                }
                                if (row.Split(',')[39] != "N")
                                {
                                    newCharity.operateStates.Add("NSW");
                                }
                                if (row.Split(',')[40] != "N")
                                {
                                    newCharity.operateStates.Add("NT");
                                }
                                if (row.Split(',')[41] != "N")
                                {
                                    newCharity.operateStates.Add("QLD");
                                }
                                if (row.Split(',')[42] != "N")
                                {
                                    newCharity.operateStates.Add("SA");
                                }
                                if (row.Split(',')[43] != "N")
                                {
                                    newCharity.operateStates.Add("TAS");
                                }
                                if (row.Split(',')[44] != "N")
                                {
                                    newCharity.operateStates.Add("VIC");
                                }
                                if (row.Split(',')[45] != "N")
                                {
                                    newCharity.operateStates.Add("WA");
                                }
                                if (row.Split(',')[46] != "N")
                                {
                                    newCharity.operateStates.Add("Overseas");
                                }
                                if (row.Split(',')[47] != "N")
                                {
                                    newCharity.beneficiaries.Add("General community in Australia");
                                }
                                if (row.Split(',')[48] != "N")
                                {
                                    newCharity.beneficiaries.Add("Females");
                                }
                                if (row.Split(',')[49] != "N")
                                {
                                    newCharity.beneficiaries.Add("Males");
                                }
                                if (row.Split(',')[50] != "N")
                                {
                                    newCharity.beneficiaries.Add("Early childhood – under 6");
                                }
                                if (row.Split(',')[51] != "N")
                                {
                                    newCharity.beneficiaries.Add("Children – 6 to under 15");
                                }
                                if (row.Split(',')[52] != "N")
                                {
                                    newCharity.beneficiaries.Add("Youth – 15 to under 25");
                                }
                                if (row.Split(',')[53] != "N")
                                {
                                    newCharity.beneficiaries.Add("Adults -25 to under 65");
                                }
                                if (row.Split(',')[54] != "N")
                                {
                                    newCharity.beneficiaries.Add("Adults – 65 and over");
                                }
                                if (row.Split(',')[55] != "N")
                                {
                                    newCharity.beneficiaries.Add("Aboriginal and Torres Strait Islander people (ATSI)");
                                }
                                if (row.Split(',')[56] != "N")
                                {
                                    newCharity.beneficiaries.Add("Gay, lesbian, bisexual, transgender or intersex persons (GLBTI)");
                                }
                                if (row.Split(',')[57] != "N")
                                {
                                    newCharity.beneficiaries.Add("Migrants, refugees or asylum seekers");
                                }
                                if (row.Split(',')[58] != "N")
                                {
                                    newCharity.beneficiaries.Add("Other charities");
                                }
                                if (row.Split(',')[59] != "N")
                                {
                                    newCharity.beneficiaries.Add("Other beneficiaries not listed");
                                }
                                if (row.Split(',')[60] != "N")
                                {
                                    newCharity.beneficiaries.Add("Overseas communities or charities");
                                }
                                if (row.Split(',')[61] != "N")
                                {
                                    newCharity.beneficiaries.Add("People in rural/regional/remote communities");
                                }
                                if (row.Split(',')[62] != "N")
                                {
                                    newCharity.beneficiaries.Add("Families");
                                }
                                if (row.Split(',')[63] != "N")
                                {
                                    newCharity.beneficiaries.Add("Financially disadvantaged people");
                                }
                                if (row.Split(',')[64] != "N")
                                {
                                    newCharity.beneficiaries.Add("People at risk of homelessness");
                                }
                                if (row.Split(',')[65] != "N")
                                {
                                    newCharity.beneficiaries.Add("People with chronic or terminal illness");
                                }
                                if (row.Split(',')[66] != "N")
                                {
                                    newCharity.beneficiaries.Add("People with disabilities");
                                }
                                if (row.Split(',')[67] != "N")
                                {
                                    newCharity.beneficiaries.Add("Pre or Post Release Offenders and Families");
                                }
                                if (row.Split(',')[68] != "N")
                                {
                                    newCharity.beneficiaries.Add("Unemployed persons");
                                }
                                if (row.Split(',')[69] != "N")
                                {
                                    newCharity.beneficiaries.Add("Veterans or their families");
                                }
                                if (row.Split(',')[70] != "N")
                                {
                                    newCharity.beneficiaries.Add("Victims of crime");
                                }
                                if (row.Split(',')[71] != "N")
                                {
                                    newCharity.beneficiaries.Add("Victims of disasters");
                                }
                                if (row.Split(',')[72] != "N")
                                {
                                    newCharity.beneficiaries.Add("Culturally and Linguistically Diverse");
                                }
                                if (!row.Split(',')[73].Contains("NA"))
                                {
                                    newCharity.StaffFull = Int32.Parse(row.Split(',')[73]);
                                }
                                else
                                {
                                    newCharity.StaffFull = 0;
                                }
                                if (!row.Split(',')[74].Contains("NA"))
                                {
                                    newCharity.StaffPart = Int32.Parse(row.Split(',')[74]);
                                }
                                else
                                {
                                    newCharity.StaffPart = 0;
                                }
                                if (!row.Split(',')[75].Contains("NA"))
                                {
                                    newCharity.StaffCasual = Int32.Parse(row.Split(',')[75]);
                                }
                                else
                                {
                                    newCharity.StaffCasual = 0;
                                }
                                if (!row.Split(',')[76].Contains("NA"))
                                {
                                    newCharity.StaffVolun = Int32.Parse(row.Split(',')[76]);
                                }
                                else
                                {
                                    newCharity.StaffVolun = 0;
                                }
                                if (!row.Split(',')[77].Contains("NA"))
                                {

                                    newCharity.DGR = true;


                                }
                                newCharity.Website = row.Split(',')[78];
                                charities.Add(newCharity);
                            }
                        }
                        int a = 0;
                        foreach (string row in desData.Split('\n'))
                        {
                            charities[a].description = row;
                            a++;
                        }
                            return charities;
                    }*/




                }
}