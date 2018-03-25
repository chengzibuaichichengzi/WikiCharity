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
            List<CharityModel> charities = new List<CharityModel>();
            string filePath = Server.MapPath("~/Uploads/FinalData4.csv");
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
                    }
                    
                    if (row.Split(',')[11] != "NA")
                    {
                        newCharity.Culture = true;
                    }
                    if (row.Split(',')[12] != "NA")
                    {
                        newCharity.Education = true;
                    }
                    if (row.Split(',')[13] != "NA")
                    {
                        newCharity.Health = true;
                    }
                    if (row.Split(',')[14] != "NA")
                    {
                        newCharity.GovernLow = true;
                    }
                    if (row.Split(',')[15] != "NA")
                    {
                        newCharity.Environment = true;
                    }
                    if (row.Split(',')[16] != "NA")
                    {
                        newCharity.HumanRights = true;
                    }
                    if (row.Split(',')[17] != "NA")
                    {
                        newCharity.GeneralPublic = true;
                    }
                    if (row.Split(',')[18] != "NA")
                    {
                        newCharity.MutualRespect = true;
                    }
                    if (row.Split(',')[19] != "NA")
                    {
                        newCharity.Religion = true;
                    }
                    if (row.Split(',')[20] != "NA")
                    {
                        newCharity.SocialPublicWelfare = true;
                    }
                    if (row.Split(',')[21] != "NA")
                    {
                        newCharity.PublicSecurity = true;
                    }
                    if (row.Split(',')[22] != "NA")
                    {
                        newCharity.Community = true;
                    }
                    if (row.Split(',')[23] != "NA")
                    {
                        newCharity.Aboriginal = true;
                    }
                    if (row.Split(',')[24] != "NA")
                    {
                        newCharity.AgedPeople = true;
                    }
                    if (row.Split(',')[25] != "NA")
                    {
                        newCharity.Children = true;
                    }
                    if (row.Split(',')[26] != "NA")
                    {
                        newCharity.CommunitiesOverseas = true;
                    }
                    if (row.Split(',')[27] != "NA")
                    {
                        newCharity.EthnicGroups = true;
                    }
                    if (row.Split(',')[28] != "NA")
                    {
                        newCharity.GayLesbianBisexual = true;
                    }
                    if (row.Split(',')[29] != "NA")
                    {
                        newCharity.GeneralCommunities = true;
                    }
                    if (row.Split(',')[30] != "NA")
                    {
                        newCharity.Men = true;
                    }
                    if (row.Split(',')[31] != "NA")
                    {
                        newCharity.MigrantsRefugee = true;
                    }
                    if (row.Split(',')[32] != "NA")
                    {
                        newCharity.ReleaseOffenders = true;
                    }
                    if (row.Split(',')[33] != "NA")
                    {
                        newCharity.ChronicIllness = true;
                    }
                    if (row.Split(',')[34] != "NA")
                    {
                        newCharity.Disabilities = true;
                    }
                    if (row.Split(',')[35] != "NA")
                    {
                        newCharity.Homelessness = true;
                    }
                    if (row.Split(',')[36] != "NA")
                    {
                        newCharity.Unemployment = true;
                    }
                    if (row.Split(',')[37] != "NA")
                    {
                        newCharity.Veterans = true;
                    }
                    if (row.Split(',')[38] != "NA")
                    {
                        newCharity.CrimeVictims = true;
                    }
                    if (row.Split(',')[39] != "NA")
                    {
                        newCharity.DisasterVictims = true;
                    }
                    if (row.Split(',')[40] != "NA")
                    {
                        newCharity.Women = true;
                    }
                    if (row.Split(',')[41] != "NA")
                    {
                        newCharity.Youth = true;
                    }
                    if (row.Split(',')[42] != "NA")
                    {
                        newCharity.ABNStatus = true;
                    }
                    if (row.Split(',')[43] != "NA")
                    {
                        newCharity.DGR = true;
                    }
                    charities.Add(newCharity);
                }
            }
            return View(charities);
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
    }
}