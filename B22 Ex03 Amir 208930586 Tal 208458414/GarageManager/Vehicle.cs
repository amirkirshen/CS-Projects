using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public enum eVehicleStatusInGarage { InRepair, Repaired, Payed }
    public abstract class Vehicle
    {
        private string m_ModelName;
        private string m_LicenseID;
        private EgnitionSystem m_EgnitionSystem; // Fuel/Electric system. Contains the number of energy percentage left.
        private List<Wheel> m_Wheels;
        private string m_Owner;
        private short m_OwnerPhoneNumber;
        private eVehicleStatusInGarage m_VehicleStatus = eVehicleStatusInGarage.InRepair;

        public Vehicle(string i_ModelName, string i_LicenseID, EgnitionSystem i_EgnitionSystem, short i_NumberOfWheels, string i_Owner,
                       short i_OwnerPhoneNumber, eVehicleStatusInGarage i_VehicleStatus)
        {
            m_ModelName = i_ModelName;
            m_LicenseID = i_LicenseID;
            m_EgnitionSystem = i_EgnitionSystem;
            m_Wheels = new List<Wheel>(i_NumberOfWheels);
            m_Owner = i_Owner;
            m_OwnerPhoneNumber = i_OwnerPhoneNumber;
            m_VehicleStatus = i_VehicleStatus;

            //Add details...
        }

        public string ModelName
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }

        public string LicenseID
        {
            get { return m_LicenseID; }
            set { m_LicenseID = value; }
        }

        public EgnitionSystem EgnitionSystem
        {
            get { return m_EgnitionSystem; }
            set { m_EgnitionSystem = value; }
        }

        public List<Wheel> Wheels
        {
            get { return m_Wheels; }
            set { m_Wheels = value; }
        }

        public string Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        public short OwnerPhoneNumber
        {
            get { return m_OwnerPhoneNumber; }
            set { m_OwnerPhoneNumber = value; }
        }

        public eVehicleStatusInGarage VehicleStatus
        {
            get { return m_VehicleStatus; }
            set { m_VehicleStatus = value; }
        }

        public virtual Dictionary<string, string> DetailsToFill()
        {
            Dictionary<string, string> fieldsNeedToAdd = new Dictionary<string, string>();

            fieldsNeedToAdd.Add("ModelName", string.Format("Please enter your vehicle's model"));
            fieldsNeedToAdd.Add("LicenseID", string.Format("Please enter your vehicle's LicenseID"));
            fieldsNeedToAdd.Add("NumberOfWheels", string.Format("Please enter the number of wheels in your vehicle"));
            fieldsNeedToAdd.Add("Owner", string.Format("Please enter the vehicle's owner name"));
            fieldsNeedToAdd.Add("OwnerPhoneNumber", string.Format("Please enter the vehicle's owner phone number"));
            //fieldsToAdd.Add("VehicleStatus", string.Format("Please enter the vehicle's owner phone number"));
            m_EgnitionSystem.EgnitionDetailsToFill(fieldsNeedToAdd);
            //fieldsNeedToAdd.Add("EgnitionSystem", m_EgnitionSystem.GetCurrentEgnitionSystemMessage());

            return fieldsNeedToAdd;
        }
    }
}
