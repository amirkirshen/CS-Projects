using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public enum eColors { Red = 1, White, Green, Blue }
    public enum eNumOfDoors { Two = 2, Three, Four, Five }
    public class Car : Vehicle
    {
        private eColors m_Color;
        private eNumOfDoors m_NumOfDoors;

        public Car(Dictionary<string, string> i_FieldsNeedToAdd) : base(i_FieldsNeedToAdd)
        {
        }

        public eColors Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public eNumOfDoors NumOfDoors
        {
            get { return m_NumOfDoors; }
            set { m_NumOfDoors = value; }
        }

        public override void ExtendedDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd)
        {
            StringBuilder colorTypesStringQuestion = new StringBuilder("Please enter the car's color from the list below:");
            StringBuilder NumOfDoorsStringQuestion = new StringBuilder("Please enter the number of doors in your car from the list below:");

            base.EgnitionSystem.EgnitionDetailsToFill(i_FieldsNeedToAdd);
            WheelsDetailsToFill(i_FieldsNeedToAdd);
            i_FieldsNeedToAdd.Add("Color", string.Format(LogicManager.BuildStringBuilderFromEnum<eColors>(colorTypesStringQuestion)));
            i_FieldsNeedToAdd.Add("NumOfDoors", string.Format(LogicManager.BuildStringBuilderFromEnum<eNumOfDoors>(NumOfDoorsStringQuestion)));
        }

        public override void SetExtendedDataMemberIfValid(KeyValuePair<string, string> i_DataMemberToSet)
        {
            if (i_DataMemberToSet.Key == "Color")
            {
                SetColor(i_DataMemberToSet.Value);
            }
            else
            {
                SetNumOfDoors(i_DataMemberToSet.Value);
            }
        }

        private void SetColor(string i_Color)
        {
            //Set engine capacity to input if input is valid.
            int userChoice = LogicManager.ParseInputToEnumNumber(typeof(eColors), i_Color);

            m_Color = (eColors)(userChoice);
        }

        private void SetNumOfDoors(string i_NumOfDoors)
        {
            int userChoice = LogicManager.ParseInputToEnumNumber(typeof(eNumOfDoors), i_NumOfDoors);

            m_NumOfDoors = (eNumOfDoors)(userChoice + 1);
        }

        public override void BuildExtendedVehicleDetails(StringBuilder i_VehicleDetails)
        {
            i_VehicleDetails.AppendLine($"Color: {m_Color.ToString()}");
            i_VehicleDetails.AppendLine($"Num of doors: {m_NumOfDoors.ToString()}");
        }
    }
}
