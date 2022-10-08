using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public class ElectricEgnitionSystem : EgnitionSystem
    {
        private float m_BatteryTimeLeft;
        private float m_MaxBatteryTime;

        public ElectricEgnitionSystem()
        {
            m_BatteryTimeLeft = 0;
            m_MaxBatteryTime = 0;
        }

        public ElectricEgnitionSystem(float i_BatteryTimeLeft, float i_BatteryTime)
        {
            m_BatteryTimeLeft = i_BatteryTimeLeft;
            m_MaxBatteryTime = i_BatteryTime;
        }

        public float BatteryTimeLeft
        {
            get { return m_BatteryTimeLeft; }
            set { m_BatteryTimeLeft = value; }
        }
        public float BatteryTime
        {
            get { return BatteryTime; }
            set { BatteryTime = value; }
        }

        public override void SetEneregyPercentage()
        {
            base.EnergyPercentageLeft = (m_BatteryTimeLeft / m_MaxBatteryTime) * 100;
        }

        public override string GetCurrentEgnitionSystemMessage()
        {
            return "Electric Egnition";
        }

        public override void FillCurrentEnergyWithAmount(float i_AmountOfEnergy)
        {
            if (m_BatteryTimeLeft == m_MaxBatteryTime)
            {
                throw new ValueOutOfRangeException("Operation failed! The battery is full charged!");
            }
            if (((m_BatteryTimeLeft + i_AmountOfEnergy) > m_MaxBatteryTime) || (i_AmountOfEnergy <= 0))
            {
                throw new ValueOutOfRangeException($"Operation failed! Invalid amount of time to charge! Should be between: {1} to {m_MaxBatteryTime - m_BatteryTimeLeft}");
            }
            else
            {
                m_BatteryTimeLeft += i_AmountOfEnergy;
            }
        }


        public override void EgnitionDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd)
        {
            i_FieldsNeedToAdd.Add("MaxBatteryTime", string.Format("Please enter the max battery life of your vehicle (in hours)"));
            i_FieldsNeedToAdd.Add("BatteryTimeLeft", string.Format("Please enter how much battery time left in your vehicle (in hours)"));
        }

        public override bool IsLocalDataMemberToSet(string i_DataMemberKey)
        {
            return (i_DataMemberKey == "MaxBatteryTime" || i_DataMemberKey == "BatteryTimeLeft");
        }

        public override void SetDataMemberIfValid(KeyValuePair<string, string> i_FieldsNeedToSet)
        {
            switch (i_FieldsNeedToSet.Key)
            {
                case "BatteryTimeLeft":
                    float currentFuel = 0;

                    currentFuel = LogicManager.GetAValidFloatNumberFromInput(i_FieldsNeedToSet.Value);
                    if (currentFuel > m_MaxBatteryTime)
                    {
                        throw new ValueOutOfRangeException(0, m_MaxBatteryTime, "Invalid input! can't contain higher than full capacity");
                    }

                    m_BatteryTimeLeft = currentFuel;
                    break;

                case "MaxBatteryTime":
                    m_MaxBatteryTime = LogicManager.GetAValidFloatNumberFromInput(i_FieldsNeedToSet.Value);
                    break;
            }
        }

        public override void BuildEgnitionDetails(StringBuilder i_VehicleDetails)
        {
            base.BuildBaseEgnitionDetails(i_VehicleDetails);

            i_VehicleDetails.AppendLine($"Max battery time: {m_MaxBatteryTime}");
            i_VehicleDetails.AppendLine($"Battery time Left: {m_BatteryTimeLeft}");
        }

    }
}
