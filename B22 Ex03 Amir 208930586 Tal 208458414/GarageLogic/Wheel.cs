using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public class Wheel
    {
        private string m_ManufacturerName;
        private float m_CurrentAirPressure;
        private float m_MaximumAirPressure;

        public Wheel() { }

        public string ManufacturerName
        {
            get { return m_ManufacturerName; }
            set { m_ManufacturerName = value; }
        }
        public float CurrentAirPressure
        {
            get { return m_CurrentAirPressure; }
            set { m_CurrentAirPressure = value; }
        }

        public float MaximumAirPressure
        {
            get { return m_MaximumAirPressure; }
            set { m_MaximumAirPressure = value; }
        }

        public void InflateToMaximum()
        {
            m_CurrentAirPressure = m_MaximumAirPressure;
        }

        public void DetailsToFill(Dictionary<string, string> i_FieldsNeedToAdd, int i_NumOfWheel)
        {
            // Diffrent keys by i_NumOfWheel:
            i_FieldsNeedToAdd.Add($"ManufacturerName{i_NumOfWheel}", string.Format($"Please enter the  #{i_NumOfWheel} wheel's manufacturer name"));
            i_FieldsNeedToAdd.Add($"MaximumAirPressure{i_NumOfWheel}", string.Format($"What is the maximum air pressure in #{i_NumOfWheel} wheel?"));
            i_FieldsNeedToAdd.Add($"CurrentAirPressure{i_NumOfWheel}", string.Format($"What is the current air pressure in #{i_NumOfWheel} wheel?"));
        }

        public void SetDataMemberIfValid(KeyValuePair<string, string> i_DataMemberToSet)
        {
            if ((i_DataMemberToSet.Key).Contains("ManufacturerName"))
            {
                SetManufacturerName(i_DataMemberToSet.Value);
            }
            else if ((i_DataMemberToSet.Key).Contains("MaximumAirPressure"))
            {
                SetMaximumAirPressure(i_DataMemberToSet.Value);
            }
            else
            {
                SetCurrentAirPressure(i_DataMemberToSet.Value);
            }
        }

        private void SetMaximumAirPressure(string value)
        {
            m_MaximumAirPressure = float.Parse(value);
        }

        private void SetCurrentAirPressure(string value)
        {
            float currAirPressure = float.Parse(value);

            if (currAirPressure > m_MaximumAirPressure)
            {
                throw new ValueOutOfRangeException(0, m_MaximumAirPressure, "Air pressure can't be higher then max air pressure value");
            }

            m_CurrentAirPressure = currAirPressure;
        }

        private void SetManufacturerName(string value)
        {
            m_ManufacturerName = value;
        }

        public void BuildWheelDetails(StringBuilder i_VehicleDetails)
        {
            i_VehicleDetails.AppendLine($"Manufacturer name: {m_ManufacturerName}");
            i_VehicleDetails.AppendLine($"Maximum air pressure: {m_MaximumAirPressure}");
            i_VehicleDetails.AppendLine($"Current air pressure: {m_CurrentAirPressure}");
        }

        public static bool IsLocalDataMemberToSet(string i_DataMemberKey)
        {
            return (i_DataMemberKey.Contains("ManufacturerName") || i_DataMemberKey.Contains("MaximumAirPressure") || i_DataMemberKey.Contains("CurrentAirPressure"));
        }
    }
}
