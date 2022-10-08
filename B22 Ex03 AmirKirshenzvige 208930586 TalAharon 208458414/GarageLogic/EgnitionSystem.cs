using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    enum eEnergyPercentageBounds { NoEnergy = 0, FullEnergy = 100 }
    public abstract class EgnitionSystem
    {
        private float m_EnergyPercentageLeft = (float)eEnergyPercentageBounds.FullEnergy; // Needs to be changed for what the user insert?!!?!

        public EgnitionSystem()
        {
        }

        public float EnergyPercentageLeft
        {
            get { return m_EnergyPercentageLeft; }
            set { m_EnergyPercentageLeft = value; }
        }

        /// <summary>
        /// Setting the energy percentage left corresponded to the specific calculation in each egnition type.
        /// </summary>
        /// <returns></returns>
        public abstract void SetEneregyPercentage();

        /// <summary>
        /// Returns the egnition type in string.
        /// </summary>
        /// <returns></returns>
        public abstract string GetCurrentEgnitionSystemMessage();

        /// <summary>
        /// Concatinating the egnition's data members which need to be filled corresponded to his type.
        /// </summary>
        /// <param name="i_FieldsNeedToAdd"></param>
        public abstract void EgnitionDetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd);

        /// <summary>
        /// Concatinating the egnition's data members details corresponded to his type.
        /// </summary>
        /// <param name="i_VehicleDetails"></param>
        public abstract void BuildEgnitionDetails(StringBuilder i_VehicleDetails);

        public void BuildBaseEgnitionDetails(StringBuilder i_VehicleDetails)
        {
            SetEneregyPercentage();
            i_VehicleDetails.AppendLine($"Energy percentage left: {m_EnergyPercentageLeft}");
        }

        /// <summary>
        /// Getting KeyValuePair<string, string>, Checking if the new data member to set (KeyValuePair.key) and it's value (KeyValuePair.value) are valid.
        /// If the new detail to add is valid -> Set them in the corresponded data member.
        /// </summary>
        /// <param name="i_FieldsNeedToSet"></param>
        public abstract void SetDataMemberIfValid(KeyValuePair<string, string> i_FieldsNeedToSet);

        /// <summary>
        /// Checking if the new data member to set (key) is a local data member according to the egnition type or not.
        /// </summary>
        /// <param name="i_FieldsNeedToSet"></param>
        public abstract bool IsLocalDataMemberToSet(string i_DataMemberKey);

        /// <summary>
        /// Checking if the amount of energy to fill is valid.
        /// If it is valid -> Changing the current egnition energy amount.
        /// </summary>
        /// <param name="i_FieldsNeedToSet"></param>
        public abstract void FillCurrentEnergyWithAmount(float i_AmountOfEnergy);
    }
}
