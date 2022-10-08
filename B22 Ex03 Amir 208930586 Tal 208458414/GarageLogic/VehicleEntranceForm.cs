using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public enum eVehicleStatusInGarage { InRepair, Repaired, Payed }

    public class VehicleEntranceForm
    {
        private string m_Owner = null;
        private Nullable<int> m_OwnerPhoneNumber = null;
        private eVehicleStatusInGarage m_VehicleStatus = eVehicleStatusInGarage.InRepair;

        public string Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        public Nullable<int> OwnerPhoneNumber
        {
            get { return m_OwnerPhoneNumber; }
            set { m_OwnerPhoneNumber = value; }
        }

        public eVehicleStatusInGarage VehicleStatus
        {
            get { return m_VehicleStatus; }
            set { m_VehicleStatus = value; }
        }
    }
}
