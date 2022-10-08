using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    enum eLicenseType { A, A1, B1, BB }

    public class Motorcycle : Vehicle
    {
        private eLicenseType m_LicenseType;
        private int m_EngineCapacityCC;

        public Motorcycle(Dictionary<string, string> i_FieldsNeedToAdd) : base(i_FieldsNeedToAdd)
        {
        }

        public override void ExtendedDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd)
        {
            StringBuilder LicenseTypeStringQuestion = new StringBuilder("Please enter the motorcycle license type from the list below:");

            base.EgnitionSystem.EgnitionDetailsToFill(i_FieldsNeedToAdd);
            WheelsDetailsToFill(i_FieldsNeedToAdd);
            i_FieldsNeedToAdd.Add("LicenseType", string.Format(LogicManager.BuildStringBuilderFromEnum<eLicenseType>(LicenseTypeStringQuestion)));
            i_FieldsNeedToAdd.Add("EngineCapacityCC", string.Format("Please enter motorcycle's engine capacity in CC"));
        }

        public override void SetExtendedDataMemberIfValid(KeyValuePair<string, string> i_DataMemberToSet)
        {
            //Checks if inputs are valid.
            //valid - initialize data members
            //not valid - throw an exception

            if (i_DataMemberToSet.Key == "EngineCapacityCC")
            {
                SetEngineCapacityCC(i_DataMemberToSet.Value);
            }
            else
            {
                SetLicenseType(i_DataMemberToSet.Value);
            }
        }

        private void SetEngineCapacityCC(string i_EngineCapacityCC)
        {
            //Set engine capacity to input if input is valid

            m_EngineCapacityCC = LogicManager.GetAValidIntNumberFromInput(i_EngineCapacityCC);
        }

        private void SetLicenseType(string i_EngineCapacityCC)
        {
            int userChoice = LogicManager.ParseInputToEnumNumber(typeof(eLicenseType), i_EngineCapacityCC);

            m_LicenseType = (eLicenseType)(userChoice);
        }

        public override void BuildExtendedVehicleDetails(StringBuilder i_VehicleDetails)
        {
            i_VehicleDetails.AppendLine($"License type: {m_LicenseType.ToString()}");
            i_VehicleDetails.AppendLine($"Engine capacity (CC): {m_EngineCapacityCC}");
        }
    }
}
