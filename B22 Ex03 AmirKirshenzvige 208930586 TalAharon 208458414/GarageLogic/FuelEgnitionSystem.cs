using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public enum eFuelType { Soler = 1, Octan95, Octan96, Octan98 }
    public class FuelEgnitionSystem : EgnitionSystem
    {
        private eFuelType m_FuelType;
        private float m_CurrentFuelLiters;
        private float m_MaxFuelLitersCapacity;

        public FuelEgnitionSystem()
        {
            m_FuelType = eFuelType.Octan95;
            m_CurrentFuelLiters = 0;
            m_MaxFuelLitersCapacity = 0;
        }

        public FuelEgnitionSystem(eFuelType i_FuelType, float i_CurrentFuelLiters, float i_MaxFuelLitersCapacity)
        {
            m_FuelType = i_FuelType;
            m_CurrentFuelLiters = i_CurrentFuelLiters;
            m_MaxFuelLitersCapacity = i_MaxFuelLitersCapacity;
        }

        public eFuelType FuelType
        {
            get { return m_FuelType; }
            set { m_FuelType = value; }
        }
        public float CurrentFuelLiters
        {
            get { return m_CurrentFuelLiters; }
            set { m_CurrentFuelLiters = value; }
        }
        public float MaxFuelLitersCapacity
        {
            get { return MaxFuelLitersCapacity; }
            set { MaxFuelLitersCapacity = value; }
        }

        public override void SetEneregyPercentage()
        {
            base.EnergyPercentageLeft = (m_CurrentFuelLiters / m_MaxFuelLitersCapacity) * 100;
        }

        public override string GetCurrentEgnitionSystemMessage()
        {
            return "Fuel Egnition";
        }

        public override void FillCurrentEnergyWithAmount(float i_AmountOfEnergy)
        {
            if (m_MaxFuelLitersCapacity == m_CurrentFuelLiters)
            {
                throw new ValueOutOfRangeException("Operation failed! The fuel tank is full!");
            }
            if (((m_CurrentFuelLiters + i_AmountOfEnergy) > m_MaxFuelLitersCapacity) || (i_AmountOfEnergy <= 0))
            {
                throw new ValueOutOfRangeException($"Operation failed! Invalid amount to fuel! Should be between: {1} to {m_MaxFuelLitersCapacity - m_CurrentFuelLiters}");
            }
            else
            {
                m_CurrentFuelLiters += i_AmountOfEnergy;
            }
        }

        public override void EgnitionDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd)
        {
            StringBuilder fuelTypesStringQuestion = new StringBuilder("Please enter vehicle's fuel type from the list below:");

            i_FieldsNeedToAdd.Add("FuelType", string.Format(LogicManager.BuildStringBuilderFromEnum<eFuelType>(fuelTypesStringQuestion)));
            i_FieldsNeedToAdd.Add("MaxFuelLitersCapacity", string.Format("Please enter the maximum capacity of your vehicle's fuel tank (in liters)"));
            i_FieldsNeedToAdd.Add("CurrentFuelLiters", string.Format("Please enter how much liters left in your vehicle's fuel tank"));
        }

        public override bool IsLocalDataMemberToSet(string i_DataMemberKey)
        {
            return (i_DataMemberKey == "FuelType" || i_DataMemberKey == "MaxFuelLitersCapacity" || i_DataMemberKey == "CurrentFuelLiters");
        }

        public override void SetDataMemberIfValid(KeyValuePair<string, string> i_FieldsNeedToSet)
        {
            switch (i_FieldsNeedToSet.Key)
            {
                case "FuelType":
                    m_FuelType = (eFuelType)LogicManager.ParseInputToEnumNumber(typeof(eFuelType), i_FieldsNeedToSet.Value);
                    break;

                case "CurrentFuelLiters":
                    float currentFuel = 0;

                    currentFuel = LogicManager.GetAValidFloatNumberFromInput(i_FieldsNeedToSet.Value);
                    if (currentFuel > m_MaxFuelLitersCapacity)
                    {
                        throw new ValueOutOfRangeException(0, m_MaxFuelLitersCapacity, "Invalid input! can't contain higher than full capacity");
                    }

                    m_CurrentFuelLiters = currentFuel;
                    break;

                case "MaxFuelLitersCapacity":
                    m_MaxFuelLitersCapacity = LogicManager.GetAValidFloatNumberFromInput(i_FieldsNeedToSet.Value);
                    break;
            }
        }

        public override void BuildEgnitionDetails(StringBuilder i_VehicleDetails)
        {
            base.BuildBaseEgnitionDetails(i_VehicleDetails);

            i_VehicleDetails.AppendLine($"Fuel type: {m_FuelType}");
            i_VehicleDetails.AppendLine($"Max Fuel liters tank capacity: {m_MaxFuelLitersCapacity}");
            i_VehicleDetails.AppendLine($"Amount of current fuel liters: {m_CurrentFuelLiters}");
        }
    }
}