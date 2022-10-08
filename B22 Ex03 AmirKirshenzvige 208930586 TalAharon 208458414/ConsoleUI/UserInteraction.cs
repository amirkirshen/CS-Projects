using GarageLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleUI
{
    /// <summary>
    /// Handling all inputs and outputs in current UI.
    /// </summary>
    public class UserInteraction
    {
        public static readonly List<string> sr_MenuOptions = new List<string>{ "1. Add a new car to garage.",
                                                               "2. Show all current vehicles in garage.",
                                                               "3. Change vehicle status in garage.",
                                                               "4. Inflate vehicle's wheels.",
                                                               "5. Fuel a vehicle.",
                                                               "6. Charge an electric vehicle.",
                                                               "7. Show all single vehicle's data.", };
        public static void ShowMenu() // UserInteraction
        {
            StringBuilder menuStr = new StringBuilder(Environment.NewLine);

            menuStr.Append("Please enter an operation to do:").Append(Environment.NewLine);
            foreach (string msg in sr_MenuOptions)
            {
                menuStr.Append(msg).Append(Environment.NewLine);
            }

            Console.WriteLine(menuStr.ToString());
        }

        public static void ShowMessage(string i_Message)
        {
            Console.WriteLine(i_Message);
        }

        /// <summary>
        /// Create a break and clean the screen.
        /// Continue after user insert a key.
        /// </summary>
        public static void ScreenBreak()
        {
            Console.WriteLine("In order to return to the menu, Press any key.");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ShowWelcomeToGarage()
        {
            StringBuilder welcomeMsg = new StringBuilder("#*#*#*#* WELCOME TO THE GARAGE #*#*#*#*");

            welcomeMsg.Append(Environment.NewLine).Append(Environment.NewLine); ;
            Console.WriteLine(welcomeMsg.ToString());
        }

        /// <summary>
        /// Getting reference to current vehicle status and changing to new valid status.
        /// </summary>
        /// <param name="io_CurrentVehicleStatusInGarage"></param>
        public static void GetNewVehicleStatus(ref eVehicleStatusInGarage io_CurrentVehicleStatusInGarage)
        {
            eVehicleStatusInGarage vehicleStatusInGarage = io_CurrentVehicleStatusInGarage;
            string vehicleStatusStr;

            try
            {
                StringBuilder getStatus = new StringBuilder("Please enter the new vehicle's status in garage from the list:");
                Console.WriteLine(LogicManager.BuildStringBuilderFromEnum<eVehicleStatusInGarage>(getStatus));
                vehicleStatusStr = Console.ReadLine();
                vehicleStatusInGarage = (eVehicleStatusInGarage)GarageLogic.LogicManager.ParseInputToEnumNumber(typeof(eVehicleStatusInGarage), vehicleStatusStr);
                if (io_CurrentVehicleStatusInGarage == vehicleStatusInGarage)
                {
                    throw new ArgumentException("This status is already the current vehicle's status");
                }
                else
                {
                    io_CurrentVehicleStatusInGarage = vehicleStatusInGarage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetNewVehicleStatus(ref io_CurrentVehicleStatusInGarage);
            }
        }

        public static void ShowInflationToMaximumSucceed(string i_LicenseID)
        {
            Console.WriteLine($"All wheels of vehicle with license ID: {i_LicenseID} have been inflated to their maximum air pressure!");
        }

        public static void ShowToGetAmoutOfFuel()
        {
            Console.WriteLine("Please type the amount of liters to fuel:");
        }

        /// <summary>
        /// Getting a valid license ID from user, Returns it in an output param.
        /// </summary>
        /// <param name="o_LicenseID"></param>
        public static void GetValidLicenseID(out string o_LicenseID)
        {
            try
            {
                Console.WriteLine("Please enter the vehicle's license ID");
                o_LicenseID = Console.ReadLine();
                GarageLogic.LogicManager.ValidateID(o_LicenseID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetValidLicenseID(out o_LicenseID);
            }
        }

        public static void ShowToGetAmoutOfChargingTime()
        {
            Console.WriteLine("Please type the amount of time to charge:");
        }

        public static void ShowFuelVehicleSucceed(string i_LicenseID, float i_AmountWasFuel)
        {
            Console.WriteLine($"Vehicle with license ID: {i_LicenseID} has been fueled with {i_AmountWasFuel} liters.");
        }

        public static void ShowChargeVehicleSucceed(string licenseID, float timeToCharge)
        {
            Console.WriteLine($"Vehicle with license ID: {licenseID} has been charged {timeToCharge} hours.");
        }

        /// <summary>
        /// Getting valid float number from user inorder to fill the energy source with the float amount.
        /// </summary>
        /// <param name="o_Amount"></param>
        public static void GetFloatAmountToFillEnergySource(out float o_Amount)
        {
            try
            {
                o_Amount = float.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetFloatAmountToFillEnergySource(out o_Amount);
            }
        }

        /// <summary>
        /// Showing all fuel type to the user. 
        /// Getting valid fuel type from the enum list.
        /// </summary>
        /// <param name="o_FuelType"></param>
        public static void GetValidFuelType(out eFuelType o_FuelType)
        {
            string fuelTypeStr;
            StringBuilder fuelTypesStringQuestion = new StringBuilder("Please enter the vehicle's fuel type from the list below:");

            Console.WriteLine(string.Format(LogicManager.BuildStringBuilderFromEnum<eFuelType>(fuelTypesStringQuestion)));
            fuelTypeStr = Console.ReadLine();
            try
            {
                o_FuelType = (eFuelType)LogicManager.ParseInputToEnumNumber(typeof(eFuelType), fuelTypeStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetValidFuelType(out o_FuelType);
            }
        }

        /// <summary>
        /// Getting valid operation number (Between 1 to the number of possible operations).
        /// </summary>
        /// <param name="o_OperationNumber"></param>
        public static void GetOperationNumber(out int o_OperationNumber)
        {
            string operationStr;

            operationStr = Console.ReadLine();

            try
            {
                o_OperationNumber = int.Parse(operationStr);
                LogicManager.BetweenValues(o_OperationNumber, 1, Enum.GetValues(typeof(UIManager.eOperations)).Length, "Operation number");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetOperationNumber(out o_OperationNumber);
            }
        }

        /// <summary>
        /// Asking the user for filter vehicles by their status in garage (yes/no question).
        /// Asking the user the status to be filtered by (If chose to operate the filter action).
        /// </summary>
        /// <returns>Nullable<eVehicleStatusInGarage> represents the status to be filtered by.</returns>
        public static Nullable<eVehicleStatusInGarage> GetVehicleStatusFilter()
        {
            String input;
            StringBuilder filterType = new StringBuilder();
            bool usingFilter = false;
            Nullable<eVehicleStatusInGarage> filter = null;

            Console.WriteLine("Whould you like to filter vehicles by status? (yes/no)");
            input = Console.ReadLine();
            try
            {
                usingFilter = GarageLogic.LogicManager.GetAValidYesNoQuestion(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GetVehicleStatusFilter();
            }

            if (usingFilter)
            {
                Console.WriteLine("By what status would you like to filter?");
                Console.WriteLine(GarageLogic.LogicManager.BuildStringBuilderFromEnum<eVehicleStatusInGarage>(filterType));
                filterType.Clear();
                filterType.Append(Console.ReadLine());
                filter = (eVehicleStatusInGarage)GarageLogic.LogicManager.ParseInputToEnumNumber(typeof(eVehicleStatusInGarage), filterType.ToString());
            }

            return filter;
        }

        public static void ShowVehicleStatusWasChanged(string i_LicenseID, eVehicleStatusInGarage i_CurrVehicleStatusInGarage, eVehicleStatusInGarage i_NewVehicleStatusInGarage)
        {
            Console.WriteLine($"Vehicle with license ID: {i_LicenseID} has been changed his status in garage from: {i_CurrVehicleStatusInGarage} to {i_NewVehicleStatusInGarage}");
        }

        public static void ShowItIsNoFuelVehicle()
        {
            Console.WriteLine("Operation cannot be done! This is not a vehicle with fuel egnition!");
        }

        public static void ShowLicenseIDWasNotFound()
        {
            Console.WriteLine("Vehicle's license ID was not found in the garage! Try again");
        }
    }
}
