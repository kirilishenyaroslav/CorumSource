using Corum.Models.ViewModels.Tender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class ContrAgentModel
    {
        public int SupplierId { get; set; }

        public string SupplierEdrpou { get; set; }

        public int OwnershipTypeId { get; set; }

        public string OwnershipTypeName { get; set; }

        public string SupplierName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string WebSiteUrl { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public LegalAddress LegalAddressContragent { get; set; }

        public PhysicalAddress PhysicalAddressContragent { get; set; }

        public CompanyTaxation CompanyTaxationContragent { get; set; }

        public List<BankAccount> BankAccounts { get; set; }

        public SupplierCEO SupplierCEOContragent { get; set; }

        public SupplierAccountant SupplierAccountantContragent { get; set; }

        public List<SupplierUser> SupplierUsers { get; set; }

        public int RegDocsConsent { get; set; }

        public int StateId { get; set; }

        public string StateName { get; set; }

        public int SecurityStateId { get; set; }

        public string SecurityStateName { get; set; }

        public int ComplianceStateId { get; set; }

        public string ComplianceStateName { get; set; }

        public int CompanyType { get; set; }
        public List<Criteriavalues> listCritariaValues {get; set;}
        public class LegalAddress
        {
            public int CountryId { get; set; }

            public string CountryName { get; set; }

            public long RegionId { get; set; }

            public string RegionName { get; set; }

            public string City { get; set; }

            public string Street { get; set; }

            public string PostalCode { get; set; }
        }

        public class PhysicalAddress
        {
            public int CountryId { get; set; }

            public string CountryName { get; set; }

            public long RegionId { get; set; }

            public string RegionName { get; set; }

            public string City { get; set; }

            public string Street { get; set; }

            public string PostalCode { get; set; }
        }

        public class CompanyTaxation
        {
            public int TaxSystemId { get; set; }

            public string TaxSystemName { get; set; }

            public string CodeVAT { get; set; }

            public string CodeIPN { get; set; }
        }

        public class BankAccount
        {
            public string BankName { get; set; }

            public string BankMFO { get; set; }

            public string AccountNumber { get; set; }
        }

        public class SupplierCEO
        {
            public string FullName { get; set; }

            public string Phone { get; set; }

            public string EMail { get; set; }
        }

        public class SupplierAccountant
        {
            public string FullName { get; set; }

            public string Phone { get; set; }

            public string EMail { get; set; }
        }

        public class SupplierUser
        {
            public int UserId { get; set; }

            public int SuppUserId { get; set; }

            public DateTime CreateDate { get; set; }

            public string FirstName { get; set; }

            public string MiddleName { get; set; }

            public string LastName { get; set; }

            public string EMail { get; set; }

            public string Phone { get; set; }

            public int IsContactPerson { get; set; }

            public int IsAdmin { get; set; }

            public int IsBlocked { get; set; }

            public int IsDeleted { get; set; }
        }
    }
}
