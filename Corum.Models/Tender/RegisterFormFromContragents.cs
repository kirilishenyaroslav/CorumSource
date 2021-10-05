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
        public double loadCapacity { get; set; }
        public double distance { get; set; }
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

        public virtual RegisterMessageToContragents RegisterMessageToContragents { get; set; }
    }
}
