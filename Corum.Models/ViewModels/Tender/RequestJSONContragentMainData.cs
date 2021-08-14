using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Corum.Models.ViewModels.Tender
{
    public class RequestJSONContragentMainData
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataContrAgent Data { get; set; }
    }
    public class LegalAddress
    {
        [JsonProperty("countryId")]
        public int? CountryId { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("regionId")]
        public long RegionId { get; set; }

        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }

    public class PhysicalAddress
    {
        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("regionId")]
        public long RegionId { get; set; }

        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }

    public class CompanyTaxation
    {
        [JsonProperty("taxSystemId")]
        public int TaxSystemId { get; set; }

        [JsonProperty("taxSystemName")]
        public string TaxSystemName { get; set; }

        [JsonProperty("codeVAT")]
        public string CodeVAT { get; set; }

        [JsonProperty("codeIPN")]
        public string CodeIPN { get; set; }
    }

    public class BankAccount
    {
        [JsonProperty("bankName")]
        public string BankName { get; set; }

        [JsonProperty("bankMFO")]
        public string BankMFO { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
    }

    public class SupplierCEO
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("eMail")]
        public string EMail { get; set; }
    }

    public class SupplierAccountant
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("eMail")]
        public string EMail { get; set; }
    }

    public class SupplierUser
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("suppUserId")]
        public int SuppUserId { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("eMail")]
        public string EMail { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("isContactPerson")]
        public int IsContactPerson { get; set; }

        [JsonProperty("isAdmin")]
        public int IsAdmin { get; set; }

        [JsonProperty("isBlocked")]
        public int IsBlocked { get; set; }

        [JsonProperty("isDeleted")]
        public int IsDeleted { get; set; }
    }

    public class DataContrAgent
    {
        [JsonProperty("supplierId")]
        public int SupplierId { get; set; }

        [JsonProperty("supplierEdrpou")]
        public string SupplierEdrpou { get; set; }

        [JsonProperty("ownershipTypeId")]
        public int OwnershipTypeId { get; set; }

        [JsonProperty("ownershipTypeName")]
        public string OwnershipTypeName { get; set; }

        [JsonProperty("supplierName")]
        public string SupplierName { get; set; }

        [JsonProperty("registrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("legalAddress")]
        public LegalAddress LegalAddress { get; set; }

        [JsonProperty("physicalAddress")]
        public PhysicalAddress PhysicalAddress { get; set; }

        [JsonProperty("companyTaxation")]
        public CompanyTaxation CompanyTaxation { get; set; }

        [JsonProperty("bankAccounts")]
        public List<BankAccount> BankAccounts { get; set; }

        [JsonProperty("supplierCEO")]
        public SupplierCEO SupplierCEO { get; set; }

        [JsonProperty("supplierAccountant")]
        public SupplierAccountant SupplierAccountant { get; set; }

        [JsonProperty("supplierUsers")]
        public List<SupplierUser> SupplierUsers { get; set; }

        [JsonProperty("regDocsConsent")]
        public int RegDocsConsent { get; set; }

        [JsonProperty("stateId")]
        public int StateId { get; set; }

        [JsonProperty("stateName")]
        public string StateName { get; set; }

        [JsonProperty("securityStateId")]
        public int SecurityStateId { get; set; }

        [JsonProperty("securityStateName")]
        public string SecurityStateName { get; set; }

        [JsonProperty("complianceStateId")]
        public int ComplianceStateId { get; set; }

        [JsonProperty("complianceStateName")]
        public string ComplianceStateName { get; set; }

        [JsonProperty("companyType")]
        public int CompanyType { get; set; }


        [JsonProperty("webSiteUrl")]
        public string WebSiteUrl { get; set; }

        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty("contactPhone")]
        public string ContactPhone { get; set; }

    }
}
