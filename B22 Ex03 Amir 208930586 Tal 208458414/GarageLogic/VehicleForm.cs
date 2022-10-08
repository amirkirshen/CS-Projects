using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public enum eVehicleStatusInGarage { InRepair = 1, Repaired, Payed }

    public class VehicleForm
    {
        private string m_Owner = null;
        private string m_OwnerPhoneNumber = null;
        private eVehicleStatusInGarage m_VehicleStatusInGarage = eVehicleStatusInGarage.InRepair;
        private Vehicle m_Vehicle = null;
        // Initialize all data members which need to be filled with data in dictionary: <TypeOfDataMember, MessageToUser>.
        private Dictionary<string, string> m_FieldsNeedToAdd = new Dictionary<string, string>();

        public VehicleForm()
        {
            SetStartingFormDetails();
        }

        public string Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        public string OwnerPhoneNumber
        {
            get { return m_OwnerPhoneNumber; }
            set { m_OwnerPhoneNumber = value; }
        }

        public eVehicleStatusInGarage VehicleStatusInGarage
        {
            get { return m_VehicleStatusInGarage; }
            set { m_VehicleStatusInGarage = value; }
        }

        public Dictionary<string, string> FieldsNeedToAdd
        {
            get { return m_FieldsNeedToAdd; }
            set { m_FieldsNeedToAdd = value; }
        }

        public Vehicle Vehicle
        {
            get { return m_Vehicle; }
            set { m_Vehicle = value; }
        }

        /// <summary>
        /// Adding first data members to be filled to m_FieldsNeedToAdd dictionary.
        /// </summary>
        public void SetStartingFormDetails()
        {
            StringBuilder VehicleStatusInGarageStringQuestion = new StringBuilder("Please enter the vehicle status in garage from the list below:");

            m_FieldsNeedToAdd.Add("Owner", string.Format("Please enter vehicle's owner name"));
            m_FieldsNeedToAdd.Add("OwnerPhoneNumber", string.Format("Please enter vehicle's owner phone number"));
            m_FieldsNeedToAdd.Add("VehicleStatusInGarage", string.Format(LogicManager.BuildStringBuilderFromEnum<eVehicleStatusInGarage>(VehicleStatusInGarageStringQuestion)));
            m_FieldsNeedToAdd.Add("VehicleType", string.Format("Which type is your vehicle?"));
            m_FieldsNeedToAdd.Add("EgnitionSystem", string.Format("Please enter the egnition system type (fuel/electric)")); // AMIR - should cange to enum!?
        }

        /// <summary>
        /// Returns exception if detail to add is not valid.
        /// Check to which data member is the key relevent.
        /// </summary>
        /// <param name="i_DetailToAdd"></param>
        public void SetDataMemberIfValid(KeyValuePair<string, string> i_DetailToAdd)
        {
            switch (i_DetailToAdd.Key)
            {
                // Check the data member value and set if it is ok.
                case "Owner":
                    SetOwner(i_DetailToAdd.Value);
                    break;
                case "OwnerPhoneNumber":
                    SetOwnerPhoneNumber(i_DetailToAdd.Value);
                    break;
                case "VehicleStatusInGarage":
                    m_VehicleStatusInGarage = (eVehicleStatusInGarage)(LogicManager.ParseInputToEnumNumber(typeof(eVehicleStatusInGarage), i_DetailToAdd.Value));
                    break;
                case "VehicleType":
                    SetVehicleByType(i_DetailToAdd.Value);
                    break;
                case "EgnitionSystem":
                    m_Vehicle.SetEgnitionSystem(i_DetailToAdd.Value, m_FieldsNeedToAdd);
                    break;
                default:
                    m_Vehicle.SetDataMemberIfValid(i_DetailToAdd);
                    break;
            }
        }

        /// <summary>
        /// Setting the vehicle by his type.
        /// Throwing exception if does not match to any of the vehicle types in garage.
        /// </summary>
        /// <param name="i_VehicleType"></param>
        private void SetVehicleByType(string i_VehicleType)
        {
            switch (i_VehicleType.ToLower())
            {
                case "car":
                    m_Vehicle = new Car(m_FieldsNeedToAdd);
                    break;
                case "motorcycle":
                    m_Vehicle = new Motorcycle(m_FieldsNeedToAdd);
                    break;
                case "truck":
                    m_Vehicle = new Truck(m_FieldsNeedToAdd);
                    break;
                default:
                    throw new FormatException("Invalid input! Should contain legal vehicle type name");
            }
        }

        private void SetOwnerPhoneNumber(string i_PhoneNumberString)
        {
            long notMatterData;

            if (i_PhoneNumberString.Length != 10 || !long.TryParse(i_PhoneNumberString, out notMatterData)) // Check for other location???
            {
                throw new FormatException("Invalid input! Phone number should be a 10 digits number");
            }

            m_OwnerPhoneNumber = i_PhoneNumberString;
        }

        private void SetOwner(string i_OwnerName)
        {
            if (!i_OwnerName.All(char.IsLetter))
            {
                throw new FormatException("Invalid input! Owner name should contain only letters");
            }
            m_Owner = i_OwnerName;
        }

        /// <summary>
        /// Concatinate vehicle form's data members.
        /// Sending to method in Vehicle for continue concatinate vehicle details.
        /// </summary>
        /// <param name="i_VehicleDetails"></param>
        public void BuildVehicleDetails(StringBuilder i_VehicleDetails)
        {
            i_VehicleDetails.AppendLine("#############################################");
            i_VehicleDetails.AppendLine($"Owner: {m_Owner}");
            i_VehicleDetails.AppendLine($"Owner phone number: {m_OwnerPhoneNumber}");
            i_VehicleDetails.AppendLine($"Vehicle status in garage: {m_VehicleStatusInGarage.ToString()}");

            // Continue build vehicle's data:
            m_Vehicle.BuildVehicleDetails(i_VehicleDetails);
        }
    }
}
