using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GarageLogic;

namespace ConsoleUI
{
    public class UIManager
    {
        public enum eOperations { InsertVehicle = 1, ShowAllVehicles, ChangeVehicleStatus, Inflate, FuelVehicle, ChargeVehicle, ShowSingleVehicle }

        private LogicManager m_GarageManager = new LogicManager();

        private bool m_IsProgramRunning = true;
        public void RunUI()
        {
            UserInteraction.ShowWelcomeToGarage();
            //
            while (m_IsProgramRunning)
            {
                int operationNumber;

                UserInteraction.ShowMenu();
                UserInteraction.GetOperationNumber(out operationNumber);
                try
                {
                    ExecuteOperation((eOperations)(operationNumber));
                }
                catch (Exception ex)
                {
                    UserInteraction.ShowMessage(ex.Message);
                }
            }
        }

        private void ExecuteOperation(eOperations i_Operation)
        {
            string licenseID;

            if (i_Operation == eOperations.InsertVehicle)
            {
                UserInteraction.GetValidLicenseID(out licenseID);
                AddingNewCarToGarage(licenseID);
                UserInteraction.ScreenBreak();
            }
            else
            {
                if (m_GarageManager.VehiclesInGarage.Count == 0)
                {
                    throw new Exception("Operation failed! There is no vehicles in the garage!");
                }
                else
                {
                    switch (i_Operation)
                    {
                        case eOperations.ShowAllVehicles:
                            ShowAllVehiclesInGarage();
                            break;
                        case eOperations.ChangeVehicleStatus:
                            SetNewStatusToVehicleProcess();
                            break;
                        case eOperations.Inflate:
                            licenseID = GetLicenseIDInGarage();
                            m_GarageManager.InflateVehicleWheelsToMaximum(licenseID);
                            UserInteraction.ShowInflationToMaximumSucceed(licenseID);
                            break;
                        case eOperations.FuelVehicle:
                            FuelVehicle();
                            break;
                        case eOperations.ChargeVehicle:
                            ChargeVehicle();
                            break;
                        case eOperations.ShowSingleVehicle:
                            licenseID = GetLicenseIDInGarage();
                            ShowSingleVehicle(licenseID);
                            break;
                    }

                    UserInteraction.ScreenBreak();
                }
            }
        }

        private void FuelVehicle()
        {
            string licenseID;
            eFuelType fuelType;
            float amountToFuel;

            licenseID = GetLicenseIDInGarage();
            UserInteraction.GetValidFuelType(out fuelType);
            UserInteraction.ShowToGetAmoutOfFuel();
            UserInteraction.GetFloatAmountToFillEnergySource(out amountToFuel);

            try
            {
                m_GarageManager.FuelVehicle(licenseID, fuelType, amountToFuel);
                UserInteraction.ShowFuelVehicleSucceed(licenseID, amountToFuel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargeVehicle()
        {
            string licenseID;
            float timeToCharge;

            licenseID = GetLicenseIDInGarage();
            UserInteraction.ShowToGetAmoutOfChargingTime();
            UserInteraction.GetFloatAmountToFillEnergySource(out timeToCharge);

            try
            {
                m_GarageManager.ChargeVehicle(licenseID, timeToCharge);
                UserInteraction.ShowChargeVehicleSucceed(licenseID, timeToCharge);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowAllVehiclesInGarage()
        {
            Nullable<eVehicleStatusInGarage> filter = UserInteraction.GetVehicleStatusFilter();
            foreach (KeyValuePair<string, VehicleForm> vehicleDetails in m_GarageManager.VehiclesInGarage)
            {
                if (filter == null || vehicleDetails.Value.VehicleStatusInGarage == filter)
                {
                    ShowSingleVehicle(vehicleDetails.Key);
                }
            }
        }

        /// <summary>
        /// Adding new car to garage process.
        /// Request a new license ID.
        /// Making new Vehicle Form and adding all data members which need to be set.
        /// Moving through all data members and set them with corresponded values.
        /// </summary>
        /// <param name="i_LicenseID"></param>
        private void AddingNewCarToGarage(string i_LicenseID)
        {
            if (m_GarageManager.VehiclesInGarage.ContainsKey(i_LicenseID))
            {
                // This vehicle ID already exist -> Changing the vehicle status to be in repair.
                m_GarageManager.ChangeVehicleStatusByID(i_LicenseID, eVehicleStatusInGarage.InRepair);
                UserInteraction.ShowVehicleStatusWasChanged(i_LicenseID, m_GarageManager.VehiclesInGarage[i_LicenseID].VehicleStatusInGarage, eVehicleStatusInGarage.InRepair);
            }
            else
            {
                // Getting first details to fill.
                VehicleForm newVehicleForm = new VehicleForm();

                // Moving on all VehicleForm details to add and initialize data members.
                ShowAndSetDetailsToFillInVehicleForm(newVehicleForm);

                // Moving on all base vehicle details to add and initialize data members.
                newVehicleForm.FieldsNeedToAdd.Clear();
                newVehicleForm.Vehicle.BaseDetailsToFill(newVehicleForm.FieldsNeedToAdd);
                ShowAndSetDetailsToFillInVehicleForm(newVehicleForm);

                // Moving on all extended vehicle details to add and initialize data members.
                newVehicleForm.FieldsNeedToAdd.Clear();
                newVehicleForm.Vehicle.ExtendedDetailsToFill(newVehicleForm.FieldsNeedToAdd);
                ShowAndSetDetailsToFillInVehicleForm(newVehicleForm);
                newVehicleForm.FieldsNeedToAdd.Clear();

                // Adding the new vehicle to garage after all data members was sets.
                m_GarageManager.VehiclesInGarage.Add(i_LicenseID, newVehicleForm);
            }
        }

        /// <summary>
        /// Moving through all current fields (Data members) which need to be added (In i_VehicleForm dictionary).
        /// Requesting for a value to set and trying to set the current field with detail.
        /// </summary>
        /// <param name="i_VehicleForm"></param>
        private void ShowAndSetDetailsToFillInVehicleForm(VehicleForm i_VehicleForm)
        {
            foreach (KeyValuePair<string, string> detailPair in i_VehicleForm.FieldsNeedToAdd)
            {
                KeyValuePair<string, string> detailToAdd;

                UserInteraction.ShowMessage(detailPair.Value);
                detailToAdd = new KeyValuePair<string, string>(detailPair.Key, Console.ReadLine());
                SetDetail(i_VehicleForm, detailToAdd);
            }
        }

        /// <summary>
        /// Sending to LogicManager for setting the new data member. 
        /// If is not valid -> asking again for valid input.
        /// </summary>
        /// <param name="i_VehicleForm"></param>
        /// <param name="i_DetailToAdd"></param>
        private void SetDetail(VehicleForm i_VehicleForm, KeyValuePair<string, string> i_DetailToAdd)
        {
            try
            {
                i_VehicleForm.SetDataMemberIfValid(i_DetailToAdd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                i_DetailToAdd = new KeyValuePair<string, string>(i_DetailToAdd.Key, Console.ReadLine());
                SetDetail(i_VehicleForm, i_DetailToAdd);
            }
        }

        /// <summary>
        /// Getting valid license ID in garage.
        /// Request for new vehicle status in garage and changing it to the new status.
        /// </summary>
        private void SetNewStatusToVehicleProcess()
        {
            string licenseID;
            eVehicleStatusInGarage newVehicleStatusInGarage;

            licenseID = GetLicenseIDInGarage();
            newVehicleStatusInGarage = m_GarageManager.VehiclesInGarage[licenseID].VehicleStatusInGarage;
            UserInteraction.GetNewVehicleStatus(ref newVehicleStatusInGarage);
            UserInteraction.ShowVehicleStatusWasChanged(licenseID, m_GarageManager.VehiclesInGarage[licenseID].VehicleStatusInGarage, newVehicleStatusInGarage);
            m_GarageManager.ChangeVehicleStatusByID(licenseID, newVehicleStatusInGarage);
        }

        /// <summary>
        /// Calling to method for building all details to vehicle with license ID as mention in the param, and Show it to the user.
        /// </summary>
        /// <param name="i_LicenseID"></param>
        private void ShowSingleVehicle(string i_LicenseID)
        {
            UserInteraction.ShowMessage(m_GarageManager.BuildVehicleDetails(i_LicenseID).ToString());
        }

        /// <summary>
        /// Getting validate string represnts license ID and checking if the license ID is in the garage.
        /// </summary>
        /// <returns>The valid license ID shown in garage.</returns>
        private string GetLicenseIDInGarage()
        {
            string licenseID;

            UserInteraction.GetValidLicenseID(out licenseID);
            while (!m_GarageManager.VehiclesInGarage.ContainsKey(licenseID))
            {
                UserInteraction.ShowLicenseIDWasNotFound();
                UserInteraction.GetValidLicenseID(out licenseID);
            }

            return licenseID;
        }
    }
}
