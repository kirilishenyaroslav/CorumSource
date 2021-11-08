using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.Models.Tender
{
    public class RegisterFormFromContragents
    {
        public int Id { get; set; }
        public int RegisterMessageToContragentId { get; set; }
        public string carBrand { get; set; }
        public string stateNumberCar { get; set; }
        public string trailerNumber { get; set; }
        public Nullable<double> loadCapacity { get; set; }
        public Nullable<double> distance { get; set; }
        public string fullNameOfDriver { get; set; }
        public string phoneNumber { get; set; }
        public string drivingLicenseNumber { get; set; }
        public string contragentName { get; set; }
        public string note { get; set; }
        public string stateBorderCrossingPoint { get; set; }
        public string seriesPassportNumber { get; set; }
        public Nullable<bool> scannedCopyOfSignedOrder { get; set; }
        public Nullable<bool> scannedCopyOfRegistrationCertificate { get; set; }
        public Nullable<bool> scanCopyOfPassport { get; set; }
        public Nullable<bool> scannedCopyOfAdmissionToTransportation { get; set; }
        public Nullable<bool> scannedCopyOfCivilPassport { get; set; }
        public Nullable<bool> IsUpdate { get; set; }
        public System.DateTime dateCreate { get; set; }
        public System.DateTime dateUpdate { get; set; }
        public System.Guid tenderItemUuid { get; set; }
        public Nullable<bool> flag { get; set; }

        public long orderId { get; set; }
        public int tenderNumber { get; set; }
        public string emailOperacionist { get; set; }
        public string emailContragent { get; set; }
        public System.DateTime dateDownloading { get; set; }
        public System.DateTime dateUnloading { get; set; }
        public string industryName { get; set; }
        public string descriptionTender { get; set; }
        public Nullable<int> acceptedTransportUnits { get; set; }
        public Nullable<double> cost { get; set; }
        public System.Guid formUuid { get; set; }
        public int industryId { get; set; }
        public string routeShort { get; set; }
        public string nameCargo { get; set; }
        public double weightCargo { get; set; }
        public int DelayPayment { get; set; }

        public bool IsEditable { get; set; }

        public string transportDimensions { get; set; }

        public virtual RegisterMessageToContragents RegisterMessageToContragents { get; set; }

        public Nullable<int> fullMassTC { get; set; }
        public Nullable<int> massWithoutLoadTC1 { get; set; }
        public Nullable<int> fullMassTC2Trailer { get; set; }
        public Nullable<int> massWithoutLoadTC2Trailer { get; set; }
        public Nullable<bool> filesTTH_CMR { get; set; }
        public Nullable<bool> filesInvoice { get; set; }
        public Nullable<bool> filesActOfCompletion { get; set; }
    }
}
