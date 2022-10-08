using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public class LogicManager
    {
        public static readonly short sr_SizeOfLicenseID = 8;

        //               LicenseID, VehicleForm
        private Dictionary<string, VehicleForm> m_VehiclesInGarage = new Dictionary<string, VehicleForm>();

        public Dictionary<string, VehicleForm> VehiclesInGarage
        {
            get { return m_VehiclesInGarage; }
            set { m_VehiclesInGarage = value; }
        }

        /// <summary>
        /// The T typeparam represents enum. 
        /// The method appends all enum attributes names in i_StrBuilder.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_StrBuilder"></param>
        /// <returns></returns>
        public static string BuildStringBuilderFromEnum<T>(StringBuilder i_StrBuilder)
        {
            int index = 1;

            i_StrBuilder.Append(Environment.NewLine);

            foreach (string eValue in Enum.GetNames(typeof(T)))
            {
                i_StrBuilder.Append(index++).Append(". ");
                i_StrBuilder.Append(eValue).Append(Environment.NewLine);
            }

            return i_StrBuilder.ToString();
        }

        /// <summary>
        /// Checking if the param represents valid license ID string.
        /// </summary>
        /// <param name="i_LicenseID"></param>
        public static void ValidateID(string i_LicenseID)
        {
            if (i_LicenseID.Length != sr_SizeOfLicenseID)
            {
                throw new ValueOutOfRangeException(sr_SizeOfLicenseID, sr_SizeOfLicenseID, $"License ID is not valid! Should contain only {sr_SizeOfLicenseID} characters");
            }
            foreach (char singleChar in i_LicenseID)
            {
                if (!char.IsDigit(singleChar) && !char.IsLetter(singleChar))
                {
                    throw new FormatException("License ID is not valid! Should contain only digit and letters");
                }
            }
        }

        public void ChangeVehicleStatusByID(string i_LicenseID, eVehicleStatusInGarage i_NewVehicleStatusInGarage)
        {
            m_VehiclesInGarage[i_LicenseID].VehicleStatusInGarage = i_NewVehicleStatusInGarage;
        }

        public void InflateVehicleWheelsToMaximum(string i_LicenseID)
        {
            m_VehiclesInGarage[i_LicenseID].Vehicle.InflateVehicleWheelsToMaximum();
        }

        /// <summary>
        /// Checking if the vehicle is with fuel egnition.
        /// Checking if the type of fuel given is equivalent to the egnition fuel type.
        /// Checking if the amount of fuel given can fill the egnition (If there is not too much fuel).
        /// Setting the new amount of fuel in vehicle, Otherwise throw an exception.
        /// </summary>
        /// <param name="i_LicenseID"></param>
        /// <param name="i_FuelType"></param>
        /// <param name="i_AmountToFuel"></param>
        public void FuelVehicle(string i_LicenseID, eFuelType i_FuelType, float i_AmountToFuel)
        {
            if (m_VehiclesInGarage[i_LicenseID].Vehicle.EgnitionSystem is FuelEgnitionSystem)
            {
                eFuelType fuelType = (m_VehiclesInGarage[i_LicenseID].Vehicle.EgnitionSystem as FuelEgnitionSystem).FuelType;
                if (fuelType == i_FuelType)
                {
                    m_VehiclesInGarage[i_LicenseID].Vehicle.EgnitionSystem.FillCurrentEnergyWithAmount(i_AmountToFuel);
                }
                else
                {
                    throw new ArgumentException($"Operation failed! The fuel type entered is not matching to the vehicle fuel type: {fuelType}");
                }
            }
            else
            {
                throw new ArgumentException($"Operation failed! The vehicle with license ID:{ i_LicenseID } is not with fuel egnition!");
            }
        }

        /// <summary>
        /// Checking if the vehicle is with electric egnition.
        /// Checking if the amount of time to charge given can charge the egnition (If the given time is not too much).
        /// Setting the new battery time left in vehicle, Otherwise throw an exception.
        /// </summary>
        /// <param name="i_LicenseID"></param>
        /// <param name="i_AmountToCharge"></param>
        public void ChargeVehicle(string i_LicenseID, float i_AmountToCharge)
        {
            if (m_VehiclesInGarage[i_LicenseID].Vehicle.EgnitionSystem is ElectricEgnitionSystem)
            {
                m_VehiclesInGarage[i_LicenseID].Vehicle.EgnitionSystem.FillCurrentEnergyWithAmount(i_AmountToCharge);
            }
            else
            {
                throw new ArgumentException($"Operation failed! The vehicle with license ID:{ i_LicenseID } is not with electric egnition!");
            }
        }

        /// <summary>
        /// Initialize a StringBuilder, concatinating it will all vehicle details by sending him to abstract method BuildVehicleDetails.
        /// </summary>
        /// <param name="i_LicenseID"></param>
        /// <returns>StringBuilder represents all vehicle details</returns>
        public StringBuilder BuildVehicleDetails(string i_LicenseID)
        {
            StringBuilder vehicleDetails = new StringBuilder();

            vehicleDetails.Append(Environment.NewLine);
            vehicleDetails.AppendLine(string.Format($"Here's all details of vehicle with license ID: {i_LicenseID}"));
            m_VehiclesInGarage[i_LicenseID].BuildVehicleDetails(vehicleDetails);

            return vehicleDetails;
        }

        /// <summary>
        /// Checking if the input string is between the shown enum values.
        /// </summary>
        /// <param name="i_enum"></param>
        /// <param name="i_Input"></param>
        /// <returns></returns>
        public static int ParseInputToEnumNumber(Type i_Enum, string i_Input)
        {
            int userChoice;

            if (!i_Enum.IsEnum)
            {
                //For developers only
                throw new FormatException("Checks only input for enum");
            }

            if (!int.TryParse(i_Input, out userChoice))
            {
                throw new FormatException("Not valid input! Should be only a number from the list");
            }

            BetweenValues(userChoice, 1, Enum.GetValues(i_Enum).Length, "Should be a number from the list above");

            return userChoice;
        }

        /// <summary>
        /// Extracting the number in string param.
        /// </summary>
        /// <param name="i_String"></param>
        /// <returns></returns>
        public static int ExtractNumberInString(string i_String)
        {
            int num = 0;

            foreach (char ch in i_String)
            {
                if (Char.IsDigit(ch))
                {
                    num *= 10;
                    num += ch - '0';
                }
            }

            return num;
        }

        /// <summary>
        /// Checking for valid parsing from string to int according to the string param.
        /// </summary>
        /// <param name="i_Input"></param>
        /// <returns></returns>
        public static int GetAValidIntNumberFromInput(string i_Input)
        {
            int userChoice;

            if (i_Input[0] == '0')
            {
                throw new FormatException("Not valid input! Number should not be start with 0");
            }

            if (!int.TryParse(i_Input, out userChoice))
            {
                throw new FormatException("Not valid input! Should contain only digits");
            }

            return userChoice;
        }

        public static float GetAValidFloatNumberFromInput(string i_Input) // IS NECESSARY???
        {
            float userChoice;

            if (!float.TryParse(i_Input, out userChoice))
            {
                throw new FormatException("Not valid input! Insert cargo capacity contains only numbers"); // ?!?!@?$!?!
            }

            return userChoice;
        }

        /// <summary>
        /// Handling yes/no questions - only strings: "true/false" or "yes/no" inputs are valid.
        /// </summary>
        /// <param name="i_Input"></param>
        /// <returns>Bolean answer for the yes/no question.</returns>
        public static bool GetAValidYesNoQuestion(string i_Input)
        {
            bool userChoice;

            i_Input = i_Input.ToLower();
            switch (i_Input)
            {
                case "yes":
                    userChoice = true;
                    break;
                case "no":
                    userChoice = false;
                    break;
                case "true":
                    userChoice = true;
                    break;
                case "false":
                    userChoice = false;
                    break;
                default:
                    throw new FormatException("Not valid input! It's a Yes-No questions");
            }

            return userChoice;
        }

        /// <summary>
        /// Checking wether i_Num is between i_Start to i_End values.
        /// </summary>
        /// <param name="i_Num"></param>
        /// <param name="i_Start"></param>
        /// <param name="i_End"></param>
        /// <param name="i_VariableType"></param>
        public static void BetweenValues(int i_Num, int i_Start, int i_End, string i_VariableType)
        {
            if (i_Num < i_Start || i_Num > i_End)
            {
                throw new ValueOutOfRangeException($"Not valid input! {i_VariableType} should be between {i_Start} to {i_End}");
            }
        }
    }
}
