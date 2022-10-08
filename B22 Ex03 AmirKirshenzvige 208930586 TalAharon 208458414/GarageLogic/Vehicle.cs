using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public abstract class Vehicle
    {
        private string m_ModelName = null;
        private EgnitionSystem m_EgnitionSystem = null; // Fuel/Electric system. Contains the number of energy percentage left.
        private Wheel[] m_Wheels = null;

        public Vehicle(Dictionary<string, string> i_FieldsNeedToAdd) { }

        public string ModelName
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }

        public EgnitionSystem EgnitionSystem
        {
            get { return m_EgnitionSystem; }
            set { m_EgnitionSystem = value; }
        }

        public Wheel[] Wheels
        {
            get { return m_Wheels; }
            set { m_Wheels = value; }
        }

        /// <summary>
        /// Initialize all data members which need to be filled with data in dictionary: <TypeOfDataMember, MessageToUser>.
        /// </summary>
        /// <param name="i_FieldsNeedToAdd"></param>
        public void BaseDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd)
        {
            i_FieldsNeedToAdd.Add("ModelName", string.Format("Please enter your vehicle's model"));
            i_FieldsNeedToAdd.Add("NumberOfWheels", string.Format("Please enter the number of wheels in your vehicle"));
        }

        /// <summary>
        /// Moving through all wheels in vehicle and adding their data members to fill to dictionary.
        /// </summary>
        /// <param name="i_FieldsNeedToAdd"></param>
        public void WheelsDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd)
        {
            int i = 1;
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.DetailsToFill(i_FieldsNeedToAdd, i++);
            }
        }

        public void InflateVehicleWheelsToMaximum()
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.InflateToMaximum();
            }
        }

        /// <summary>
        /// Getting KeyValuePair<string, string>, Checking if the new data member to set (key) is a local data member or not.
        /// If it is local data member -> Sending it to SetLocalDataMembers method.
        /// Otherwise -> Sending if to be setted in EgnitionSystem class or setted in it's current vehicle type class.
        /// </summary>
        /// <param name="i_FieldsNeedToSet"></param>
        public void SetDataMemberIfValid(KeyValuePair<string, string> i_FieldsNeedToSet)
        {
            //Check which data member to set:
            if (IsLocalDataMemberToSet(i_FieldsNeedToSet.Key))
            {
                SetLocalDataMembers(i_FieldsNeedToSet);
            }
            else if (m_EgnitionSystem.IsLocalDataMemberToSet(i_FieldsNeedToSet.Key))
            {
                m_EgnitionSystem.SetDataMemberIfValid(i_FieldsNeedToSet);
            }
            else if (Wheel.IsLocalDataMemberToSet(i_FieldsNeedToSet.Key))
            {
                int wheelNum = LogicManager.ExtractNumberInString(i_FieldsNeedToSet.Key);
                m_Wheels[wheelNum - 1].SetDataMemberIfValid(i_FieldsNeedToSet);
            }
            else
            {
                this.SetExtendedDataMemberIfValid(i_FieldsNeedToSet);
            }
        }

        private bool IsLocalDataMemberToSet(string i_DataMemberKey)
        {
            return (i_DataMemberKey == "ModelName" || i_DataMemberKey == "NumberOfWheels");
        }

        private void SetLocalDataMembers(KeyValuePair<string, string> i_FieldsNeedToSet)
        {
            switch (i_FieldsNeedToSet.Key)
            {
                case "ModelName":
                    SetModelName(i_FieldsNeedToSet.Value);
                    break;
                default:
                    SetWheels(i_FieldsNeedToSet.Value);
                    break;

            }
        }

        private void SetWheels(string i_Input)
        {
            int numOfWheels = int.Parse(i_Input);

            m_Wheels = new Wheel[numOfWheels];
            for (int i = 0; i < numOfWheels; i++)
            {
                m_Wheels[i] = new Wheel();
            }
        }

        public void SetEgnitionSystem(string i_Input, Dictionary<string, string> i_FieldsNeedToAdd)
        {
            switch (i_Input.ToLower())
            {
                case "electric":
                    m_EgnitionSystem = new ElectricEgnitionSystem();
                    break;
                case "fuel":
                    m_EgnitionSystem = new FuelEgnitionSystem();
                    break;
                default:
                    throw new FormatException("Not valid input! Egnition system does not exist");
            }
        }

        private void SetModelName(string i_Input)
        {
            // Any string would be fine
            m_ModelName = i_Input;
        }

        /// <summary>
        /// Concatinating all data members with their values to the StringBuilder.
        /// Sending to BuildExtendedVehicleDetails method for continue concatinating corresponded to the vehicle's type.
        /// </summary>
        /// <param name="i_VehicleDetails"></param>
        public void BuildVehicleDetails(StringBuilder i_VehicleDetails)
        {
            i_VehicleDetails.AppendLine($"Model name: {m_ModelName}");
            i_VehicleDetails.AppendLine($"-----------");
            i_VehicleDetails.AppendLine($"{m_EgnitionSystem.GetCurrentEgnitionSystemMessage()} details:");
            m_EgnitionSystem.BuildEgnitionDetails(i_VehicleDetails);
            i_VehicleDetails.AppendLine($"-----------");
            i_VehicleDetails.AppendLine($"Wheels details:");
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.BuildWheelDetails(i_VehicleDetails);
            }

            i_VehicleDetails.AppendLine($"-----------");
            BuildExtendedVehicleDetails(i_VehicleDetails);
        }

        /// <summary>
        /// Concatinating the vehicle's data members which need to be filled corresponded to his type.
        /// </summary>
        /// <param name="i_FieldsNeedToAdd"></param>
        public abstract void ExtendedDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd);

        /// <summary>
        /// Getting KeyValuePair<string, string>, Setting the data member (KeyValuePair.key) with his value (KeyValuePair.value) if it is valid.
        /// Otherwise returns an exception.
        /// </summary>
        /// <param name="i_FieldsNeedToSet"></param>
        public abstract void SetExtendedDataMemberIfValid(KeyValuePair<string, string> i_FieldsNeedToSet);

        /// <summary>
        /// Concatinating the vehicle's data members details corresponded to his type.
        /// </summary>
        /// <param name="i_VehicleDetails"></param>
        public abstract void BuildExtendedVehicleDetails(StringBuilder i_VehicleDetails);
    }
}
