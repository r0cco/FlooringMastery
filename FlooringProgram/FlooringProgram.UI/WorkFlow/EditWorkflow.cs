﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.BLL;
using FlooringProgram.Models;
using System.IO;

namespace FlooringProgram.UI.WorkFlow
{
    public class EditWorkflow
    {
        public void Execute()
        {
            string orderDate = GetOrderDateFromUser();
           

            {
                int orderNumber = GetOrderNumberFromUser();

                var ops = new OrderOperations();
                Response response = ops.EditOrder(orderDate, orderNumber);
                if (response.Success && response != null)
                {
                    Response ultimateEdit = DisplayOrder(response);
                    FinalDisplay(ultimateEdit);
                }
                else
                {
                    Console.WriteLine("Sorry, there was no order found matching that data...");
                    Console.WriteLine("Press enter to return to the main menu...");
                    Console.ReadLine();
                }
            }
        }



        public string GetOrderDateFromUser()
        {
            string input = "";
            do
            {
                Console.Clear();
                string orderDateString = "";
                DateTime orderDate;
                Console.Write("Enter an order date: ");
                orderDateString = Console.ReadLine();
                bool validDate = DateTime.TryParse(orderDateString, out orderDate);
                bool doesExist = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDate.ToString("MMddyyyy")));
                if (doesExist && validDate)
                {

                    //DisplayAllOrdersFromDate(orderDate);
                    //return orderDate;

                    return orderDate.ToString("MMddyyyy");

                }
                if (!validDate)
                    Console.WriteLine("That does not look like a valid date...");
                if (validDate && !doesExist)
                    Console.WriteLine("There are no matching orders...");
                Console.WriteLine("Press enter to continue, or (M)ain Menu...");
                input = Console.ReadLine().ToUpper();
                if (input.ToUpper() == "M")
                {
                    return "X";
                }
            } while (true);

        }

        public int GetOrderNumberFromUser()
        {
            do
            {
                //Console.Clear();
                string orderNumberString;
                int orderNumber;

                Console.Write("Enter an order number to edit : ");
                orderNumberString = Console.ReadLine();
                //bool doesExist = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDateString));
                if (int.TryParse(orderNumberString, out orderNumber))
                {
                    return orderNumber;
                }

                Console.WriteLine("Please enter a number to check for an order.");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public Response DisplayOrder(Response response)
        {
            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine("Order date : {0}/{1}/{2}", response.Order.OrderDate.ToString().Substring(0, 2),
                    response.Order.OrderDate.ToString().Substring(2, 2),
                    response.Order.OrderDate.ToString().Substring(4));
                Console.WriteLine("Order number {0:0000}", response.Order.OrderNumber);
                Console.WriteLine("\nCustomer name : {0}", response.Order.CustomerName);
                Console.WriteLine("Area : {0} sq ft", response.Order.Area);
                Console.WriteLine("Product type : {0}", response.Order.ProductType.ProductType);
                Console.WriteLine("Material cost per sq ft : {0:c}", response.Order.ProductType.MaterialCost);
                Console.WriteLine("Labor cost per sq ft : {0:c}", response.Order.ProductType.LaborCost);
                Console.WriteLine("Total material cost : {0:c}", response.Order.MaterialCost);
                Console.WriteLine("Total labor cost : {0:c}", response.Order.LaborCost);
                Console.WriteLine("{0} state tax ({1:p}) : {2:c}", response.Order.State, response.Order.TaxRate/100,
                    response.Order.Tax);
                Console.WriteLine("\nOrder total : {0:c}", response.Order.Total);
                Console.WriteLine();
                Console.WriteLine();

                Response orderToEdit = ConfirmUserEdit(response);
                return orderToEdit;

                //method to take this input and use it to either write on the data or not
            }
            if (!response.Success)
            {
                Console.Clear();
                Console.WriteLine("There was an error");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();

            }
            return null;
        }

        public Response ConfirmUserEdit(Response OrderInfo)
        {
            Console.Write("Is this the order you wish to edit (y/n) ? : ");
            string input = Console.ReadLine();

            if (input.ToUpper() == "Y" || input.ToUpper() == "YES")
            {
                GetNewUserName(OrderInfo);
                GetNewUserState(OrderInfo);
                GetNewUserProductType(OrderInfo);
                GetNewUserArea(OrderInfo);
                var ops = new OrderOperations();
                Response response = ops.EditedOrder(OrderInfo);
                return response;
            }
            else
            {
                Console.WriteLine("No changes will be made to this order.");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
                return OrderInfo;
            }
        }

        public Response GetNewUserName(Response orderInfo)
        {
            string newName = "";
            Console.WriteLine("Press enter if no change...");
            Console.Write("Enter new customer name ({0}) : ", orderInfo.Order.CustomerName);
            newName = Console.ReadLine();
            if (newName != "")
            {
                orderInfo.Order.CustomerName = newName;
            }
            return orderInfo;
        }

        public Response GetNewUserState(Response orderInfo)
        {
            string newState = "";
            Console.WriteLine("Press enter if no change...");
            Console.Write("Enter new state as 2-letter abbreviation ({0}) : ", orderInfo.Order.State);
            newState = Console.ReadLine().ToUpper();
            if (newState != "")
            {
                orderInfo.Order.State = newState;
            }
            return orderInfo;
        }

        public Response GetNewUserProductType(Response orderInfo)
        {
            string newProductType = "";
            Console.WriteLine("Press enter if no change...");
            Console.Write("Enter new product type ({0}) : ", orderInfo.Order.ProductType.ProductType);
            newProductType = Console.ReadLine();
            if (newProductType != "")
            {
                orderInfo.Order.ProductType.ProductType = newProductType;
            }
            return orderInfo;
        }

        public Response GetNewUserArea(Response orderInfo)
        {
            do
            {
                
            
            decimal newArea;
            string newAreaString = "";
            Console.WriteLine("Press enter if no change...");
            Console.Write("Enter new area ({0}) : ", orderInfo.Order.Area);
            newAreaString = Console.ReadLine();
            bool validArea = decimal.TryParse(newAreaString, out newArea);
            if (newAreaString != "" && validArea && newArea > 0)
            {
                orderInfo.Order.Area = newArea;
                return orderInfo;
            }
               
            } while (true);
        }

        public void FinalDisplay(Response response)
        {
            if (response.Success && response != null)
            {
                Console.Clear();
                Console.WriteLine("Order date : {0}/{1}/{2}", response.Order.OrderDate.ToString().Substring(0, 2),
                    response.Order.OrderDate.ToString().Substring(2, 2),
                    response.Order.OrderDate.ToString().Substring(4));
                Console.WriteLine("Order number {0:0000}", response.Order.OrderNumber);
                Console.WriteLine("\nCustomer name : {0}", response.Order.CustomerName);
                Console.WriteLine("Area : {0} sq ft", response.Order.Area);
                Console.WriteLine("Product type : {0}", response.Order.ProductType.ProductType);
                Console.WriteLine("Material cost per sq ft : {0:c}", response.Order.ProductType.MaterialCost);
                Console.WriteLine("Labor cost per sq ft : {0:c}", response.Order.ProductType.LaborCost);
                Console.WriteLine("Total material cost : {0:c}", response.Order.MaterialCost);
                Console.WriteLine("Total labor cost : {0:c}", response.Order.LaborCost);
                Console.WriteLine("{0} state tax ({1:p}) : {2:c}", response.Order.State, response.Order.TaxRate/100,
                    response.Order.Tax);
                Console.WriteLine("\nOrder total : {0:c}", response.Order.Total);
                Console.WriteLine();
                Console.WriteLine();
                ConfirmFinalLastEdit(response);

                //method to take this input and use it to either write on the data or not
            }
            if (!response.Success || response == null)
            {
                Console.Clear();
                Console.WriteLine("There was an error");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }

        public void ConfirmFinalLastEdit(Response response)
        {
            Console.Write("Do you wish to save these changes to the order (y/n) ? : ");
            string input = Console.ReadLine();
            if (input.ToUpper() == "Y" || input.ToUpper() == "YES")
            {
                var ops = new OrderOperations();
                ops.PassEditBLL(response);
            }
            else
            {
                Console.WriteLine("No changes will be made to this order.");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }

        public void DisplayAllOrdersFromDate(string orderDate)
        {
            OrderOperations ops = new OrderOperations();
            var response = ops.GetAllOrdersFromDate(orderDate);

            if (response.Success)
            {
                foreach (var order in response.OrderList)
                {
                    Console.WriteLine("Order number {0}", order.OrderNumber);
                    Console.WriteLine("\t{0}, Total : {1:c}\n", order.CustomerName, order.Total);
                }


            }
            if (!response.Success)
            {
                Console.Clear();
                Console.WriteLine("There was an error");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }
    }
}
