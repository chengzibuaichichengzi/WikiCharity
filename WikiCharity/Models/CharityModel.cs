using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiCharity.Models
{
    public class CharityModel
    {
        public string ABN { get; set; }
        public string CharityName { get; set; }

        public string State { get; set; }
        public string TownCity { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public string Website { get; set; }
        public string RegisDate { get; set; }
        public string Size { get; set; }
        public bool Animals { get; set; } = false;
        public bool Culture { get; set; } = false;
        public bool Education { get; set; } = false;
        public bool Health { get; set; } = false;
        public bool GovernLow { get; set; } = false;
        public bool Environment { get; set; } = false;
        public bool HumanRights { get; set; } = false;
        public bool GeneralPublic { get; set; } = false;
        public bool MutualRespect { get; set; } = false;
        public bool Religion { get; set; } = false;
        public bool SocialPublicWelfare { get; set; } = false;
        public bool PublicSecurity { get; set; } = false;
        public bool Community { get; set; } = false;
        public bool Aboriginal { get; set; } = false;
        public bool AgedPeople { get; set; } = false;
        public bool Children { get; set; } = false;
        public bool CommunitiesOverseas { get; set; } = false;
        public bool EthnicGroups { get; set; } = false;
        public bool GayLesbianBisexual { get; set; } = false;
        public bool GeneralCommunities { get; set; } = false;
        public bool Men { get; set; } = false;
        public bool MigrantsRefugee { get; set; } = false;
        public bool ReleaseOffenders { get; set; } = false;
        public bool ChronicIllness { get; set; } = false;
        public bool Disabilities { get; set; } = false;
        public bool Homelessness { get; set; } = false;
        public bool Unemployment { get; set; } = false;
        public bool Veterans { get; set; } = false;
        public bool CrimeVictims { get; set; } = false;
        public bool DisasterVictims { get; set; } = false;
        public bool Women { get; set; } = false;
        public bool Youth { get; set; } = false;
        public bool ABNStatus { get; set; } = false;
        public bool DGR { get; set; } = false;

        public List<string> tags { get; set; } = new List<string>();
    }
}