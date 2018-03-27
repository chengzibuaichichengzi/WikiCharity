using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WikiCharity.Models;

namespace WikiCharity.Controllers
{
    public class HomeController : Controller
    {
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
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FilterModel model)
        {
            if (model.beneficial == null && model.isDGR == null && model.size == null && model.state == null)
            {
                return RedirectToAction("Result");
            }
            else
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
                return RedirectToAction("FilterResult");
            }
        }

        public ActionResult FilterResult()
        {
            var model = Session["FilterModel"] as FilterModel;
            List<CharityModel> finalResult = new List<CharityModel>();
            finalResult = GetAllCharity();
            
            if (model.beneficial != null)
            {
                finalResult = IntersectCharity(SearchByBene(model), finalResult);
            }
            if (model.size != null)
            {
                finalResult = IntersectCharity(SearchBySize(model), finalResult);
            }
            if (model.isDGR != null)
            {
                finalResult = IntersectCharity(SearchByTax(model), finalResult);
            }
            if (model.state != null)
            {
                finalResult = IntersectCharity(SearchByState(model), finalResult);
            }
            ViewBag.Count = finalResult.Count();
            return View(finalResult);
        }

        private List<CharityModel> IntersectCharity(List<CharityModel> list1, List<CharityModel> list2)
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
        }

        private List<CharityModel> SearchByBene(FilterModel model)
        {
            List<CharityModel> charities = new List<CharityModel>();
            List<CharityModel> result = new List<CharityModel>();
            charities = GetAllCharity();
            foreach (var charity in charities)
            {
                foreach (var t in charity.tags)
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
                if (charity.DGR == true)
                {
                    result.Add(charity);
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

        public ActionResult About()
        {

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private IEnumerable<string> GetAllBenes()
        {
            List<string> list = new List<string>
            {
                "Animals", "Culture", "Education", "Health", "GovernLow", "Environment", "HumanRights", "GeneralPublic", "MutualRespect",
                "Religion", "SocialPublicWelfare", "PublicSecurity", "Community", "Aboriginal", "AgedPeople", "Children", "CommunitiesOverseas",
                "EthnicGroups", "GayLesbianBisexual", "GeneralCommunities", "Men", "MigrantsRefugee", "ReleaseOffenders", "ChronicIllness",
                "Disabilities", "Homelessness", "Unemployment", "Veterans", "CrimeVictims", "DisasterVictims", "Women", "Youth",
            };
            list.Sort();
            return list;
        }

        private IEnumerable<string> GetAllDGRs()
        {
            return new List<string>
            {
                "Yes",
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

        public ActionResult Result()
        {
            List<CharityModel> charities = new List<CharityModel>();
            charities = GetAllCharity();
            return View(charities);
        }


        private List<CharityModel> GetAllCharity()
        {
            List<CharityModel> charities = new List<CharityModel>();
            string filePath = Server.MapPath("~/Uploads/PartData.csv");
            string csvData = System.IO.File.ReadAllText(filePath);
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    CharityModel newCharity = new CharityModel();
                    newCharity.ABN = row.Split(',')[0];
                    newCharity.CharityName = row.Split(',')[1];
                    newCharity.AddressLine1 = row.Split(',')[2];
                    newCharity.AddressLine2 = row.Split(',')[3];
                    newCharity.TownCity = row.Split(',')[4];
                    newCharity.State = row.Split(',')[5];
                    newCharity.Postcode = row.Split(',')[6];
                    newCharity.Website = row.Split(',')[7];
                    newCharity.RegisDate = row.Split(',')[8];
                    newCharity.Size = row.Split(',')[9];
                    if (row.Split(',')[10] != "NA")
                    {
                        newCharity.Animals = true;
                        newCharity.tags.Add("Animals");
                    }

                    if (row.Split(',')[11] != "NA")
                    {
                        newCharity.Culture = true;
                        newCharity.tags.Add("Culture");
                    }
                    if (row.Split(',')[12] != "NA")
                    {
                        newCharity.Education = true;
                        newCharity.tags.Add("Education");
                    }
                    if (row.Split(',')[13] != "NA")
                    {
                        newCharity.Health = true;
                        newCharity.tags.Add("Health");
                    }
                    if (row.Split(',')[14] != "NA")
                    {
                        newCharity.GovernLow = true;
                        newCharity.tags.Add("GovernLow");
                    }
                    if (row.Split(',')[15] != "NA")
                    {
                        newCharity.Environment = true;
                        newCharity.tags.Add("Environment");
                    }
                    if (row.Split(',')[16] != "NA")
                    {
                        newCharity.HumanRights = true;
                        newCharity.tags.Add("HumanRights");
                    }
                    if (row.Split(',')[17] != "NA")
                    {
                        newCharity.GeneralPublic = true;
                        newCharity.tags.Add("GeneralPublic");
                    }
                    if (row.Split(',')[18] != "NA")
                    {
                        newCharity.MutualRespect = true;
                        newCharity.tags.Add("MutualRespect");
                    }
                    if (row.Split(',')[19] != "NA")
                    {
                        newCharity.Religion = true;
                        newCharity.tags.Add("Religion");
                    }
                    if (row.Split(',')[20] != "NA")
                    {
                        newCharity.SocialPublicWelfare = true;
                        newCharity.tags.Add("SocialPublicWelfare");
                    }
                    if (row.Split(',')[21] != "NA")
                    {
                        newCharity.PublicSecurity = true;
                        newCharity.tags.Add("PublicSecurity");
                    }
                    if (row.Split(',')[22] != "NA")
                    {
                        newCharity.Community = true;
                        newCharity.tags.Add("Community");
                    }
                    if (row.Split(',')[23] != "NA")
                    {
                        newCharity.Aboriginal = true;
                        newCharity.tags.Add("Aboriginal");
                    }
                    if (row.Split(',')[24] != "NA")
                    {
                        newCharity.AgedPeople = true;
                        newCharity.tags.Add("AgedPeople");
                    }
                    if (row.Split(',')[25] != "NA")
                    {
                        newCharity.Children = true;
                        newCharity.tags.Add("Children");
                    }
                    if (row.Split(',')[26] != "NA")
                    {
                        newCharity.CommunitiesOverseas = true;
                        newCharity.tags.Add("CommunitiesOverseas");
                    }
                    if (row.Split(',')[27] != "NA")
                    {
                        newCharity.EthnicGroups = true;
                        newCharity.tags.Add("EthnicGroups");
                    }
                    if (row.Split(',')[28] != "NA")
                    {
                        newCharity.GayLesbianBisexual = true;
                        newCharity.tags.Add("GayLesbianBisexual");
                    }
                    if (row.Split(',')[29] != "NA")
                    {
                        newCharity.GeneralCommunities = true;
                        newCharity.tags.Add("GeneralCommunities");
                    }
                    if (row.Split(',')[30] != "NA")
                    {
                        newCharity.Men = true;
                        newCharity.tags.Add("Men");
                    }
                    if (row.Split(',')[31] != "NA")
                    {
                        newCharity.MigrantsRefugee = true;
                        newCharity.tags.Add("MigrantsRefugee");
                    }
                    if (row.Split(',')[32] != "NA")
                    {
                        newCharity.ReleaseOffenders = true;
                        newCharity.tags.Add("ReleaseOffenders");
                    }
                    if (row.Split(',')[33] != "NA")
                    {
                        newCharity.ChronicIllness = true;
                        newCharity.tags.Add("ChronicIllness");
                    }
                    if (row.Split(',')[34] != "NA")
                    {
                        newCharity.Disabilities = true;
                        newCharity.tags.Add("Disabilities");
                    }
                    if (row.Split(',')[35] != "NA")
                    {
                        newCharity.Homelessness = true;
                        newCharity.tags.Add("Homelessness");
                    }
                    if (row.Split(',')[36] != "NA")
                    {
                        newCharity.Unemployment = true;
                        newCharity.tags.Add("Unemployment");
                    }
                    if (row.Split(',')[37] != "NA")
                    {
                        newCharity.Veterans = true;
                        newCharity.tags.Add("Veterans");
                    }
                    if (row.Split(',')[38] != "NA")
                    {
                        newCharity.CrimeVictims = true;
                        newCharity.tags.Add("CrimeVictims");
                    }
                    if (row.Split(',')[39] != "NA")
                    {
                        newCharity.DisasterVictims = true;
                        newCharity.tags.Add("DisasterVictims");
                    }
                    if (row.Split(',')[40] != "NA")
                    {
                        newCharity.Women = true;
                        newCharity.tags.Add("Women");
                    }
                    if (row.Split(',')[41] != "NA")
                    {
                        newCharity.Youth = true;
                        newCharity.tags.Add("Youth");
                    }
                    if (row.Split(',')[42] != "NA")
                    {
                        
                        newCharity.ABNStatus = true;
                        
                    }
                    if (!row.Split(',')[43].Contains("NA"))
                    {
                        
                        newCharity.DGR = true;
                        
                    }
                    charities.Add(newCharity);
                }
            }
            return charities;
        }
    }
}