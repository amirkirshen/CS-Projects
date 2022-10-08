using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public class Truck : Vehicle
    {
        private bool m_IfHasRefrigerator;
        private float m_CargoCapacity;

        public Truck(Dictionary<string, string> i_FieldsNeedToAdd) : base(i_FieldsNeedToAdd) { }

        public float CargoCapacity
        {
            get { return m_CargoCapacity; }
            set { m_CargoCapacity = value; }
        }

        public bool IfHasRefrigerator
        {
            get { return m_IfHasRefrigerator; }
            set { m_IfHasRefrigerator = value; }
        }

        public override void ExtendedDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd)
        {
            base.EgnitionSystem.EgnitionDetailsToFill(i_FieldsNeedToAdd);
            WheelsDetailsToFill(i_FieldsNeedToAdd);
            i_FieldsNeedToAdd.Add("IfHasRefrigerator", string.Format("Does the truck has refrigerator? (yes/no)")); // yes/no question
            i_FieldsNeedToAdd.Add("CargoCapacity", string.Format("Please enter the truck's cargo capacity (in kg)"));
        }

        public override void SetExtendedDataMemberIfValid(KeyValuePair<string, string> i_DataMemberToSet)
        {
            switch (i_DataMemberToSet.Key)
            {
                case "IfHasRefrigerator":
                    SetIfHasRefrigerator(i_DataMemberToSet.Value);
                    break;
                default:
                    SetCargoCapacity(i_DataMemberToSet.Value);
                    break;
            }
        }

        private void SetIfHasRefrigerator(string i_IfHasRefrigerator)
        {
            // Set engine capacity to input if input is valid
            m_IfHasRefrigerator = LogicManager.GetAValidYesNoQuestion(i_IfHasRefrigerator);
        }

        private void SetCargoCapacity(string i_CargoCapacity)
        {
            m_CargoCapacity = LogicManager.GetAValidFloatNumberFromInput(i_CargoCapacity);
        }

        public override void BuildExtendedVehicleDetails(StringBuilder i_VehicleDetails)
        {
            i_VehicleDetails.AppendLine($"Contains refrigerator: {m_IfHasRefrigerator}");
            i_VehicleDetails.AppendLine($"Cargo capacity (Kg): {m_CargoCapacity}");
        }

    }
}
