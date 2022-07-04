using System;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Globalization;


namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var productServiceCart = ProductService.Current2;
            var productServiceInventory = ProductService.Current;
            Console.WriteLine("Welcome to Assignment 1 - eCommerce Console Application");
            bool cont = true;
            bool contnest = true;
            bool contnest2 = true;
            while (cont)
            {
                Console.WriteLine("Are you a customer (1) or admin (2)?");
                var choiceCustomerOrAdmin = (Console.ReadLine() ?? "0");

                if (choiceCustomerOrAdmin == "1")
                {
                    while (contnest)
                    {
                    menu:;
                        var actionCustomer = CustomerMenu();

                        if (actionCustomer == ActionTypeCustomer.AddCart)
                        {
                            Console.WriteLine("You chose to add product(s) to your cart");
                            Console.WriteLine("Here is a list of product(s) in inventory for reference");
                        sort:;
                            // Call ListNavigator on Inventory and show sorting options
                            var myListNavigator = productServiceInventory.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by unit price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceInventory.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedUnitListNavigator = productServiceInventory.SortedUnitListNavigator;
                                ListItems(mySortedUnitListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }
                            Console.WriteLine("Which item do you want to add to your cart? (insert id)");
                            var stringId = int.Parse(Console.ReadLine() ?? "0");
                            var productOfInterest = productServiceInventory.Products.FirstOrDefault(t => t.Id == stringId);

                            if (productOfInterest == null)
                            {
                                Console.WriteLine("Could not find the product in inventory!");
                                goto end;
                            }
                            else
                            {
                                // Check if product is already in your cart
                                for(int i = 0; i < productServiceCart.Products.Count; i++)
                                {
                                    // Check if product of interest matches any of the existing items in cart
                                    if(productServiceCart.Products[i].Name == productOfInterest.Name)
                                    {
                                        // If the product found is a product by quantity update amount
                                        if (productOfInterest is ProductByQuantity)
                                        {
                                            var poi = productOfInterest as ProductByQuantity;
                                            var poi2 = productServiceCart.Products[i] as ProductByQuantity;
                                            Console.WriteLine("How many more " + productOfInterest.Name + "s would you like to add to cart?");
                                            var stringQuantity = int.Parse(Console.ReadLine() ?? "0");

                                            if (stringQuantity > poi?.Quantity)
                                            {
                                                Console.WriteLine("Quantity exceeds amount in inventory, adding all quantities available to cart.");
                                                poi2.Quantity += poi.Quantity;
                                                poi.Quantity = 0;
                                            }
                                            else
                                            {
                                                poi2.Quantity += stringQuantity;
                                                poi.Quantity = poi.Quantity - stringQuantity;
                                            }
                                            Console.WriteLine("Product(s) added to cart!");
                                            goto end;
                                        }
                                        // If the product found is a product by weight update amount
                                        else if (productOfInterest is ProductByWeight)
                                        {
                                            var poi = productOfInterest as ProductByWeight;
                                            var poi2 = productServiceCart.Products[i] as ProductByWeight;
                                            Console.WriteLine("How much more weight of " + productOfInterest.Name + " would you like to add to cart?");
                                            var stringQuantity = int.Parse(Console.ReadLine() ?? "0");

                                            if (stringQuantity > poi?.Weight)
                                            {
                                                Console.WriteLine("Quantity exceeds amount in inventory, adding all quantities available to cart.");
                                                poi2.Weight += poi.Weight;
                                                poi.Weight = 0;
                                            }
                                            else
                                            {
                                                poi2.Weight += stringQuantity;
                                                poi.Weight = poi.Weight - stringQuantity;
                                            }
                                            Console.WriteLine("Product(s) added to cart!");
                                            goto end;
                                        }
                                    }
                                }

                                // If there is not existing item in cart already...
                                if(productOfInterest is ProductByQuantity)
                                {
                                    var poi = productOfInterest as ProductByQuantity;
                                    var newProduct = new ProductByQuantity();
                                    newProduct.Name = productOfInterest.Name;
                                    newProduct.Description = productOfInterest.Description;
                                    newProduct.Price = productOfInterest.Price;
                                    newProduct.Bogo = productOfInterest.Bogo;

                                    Console.WriteLine("How many would you like to have in cart?");
                                    var stringQuantity2 = int.Parse(Console.ReadLine() ?? "0");

                                    if (stringQuantity2 > poi.Quantity)
                                    {
                                        Console.WriteLine("Quantity exceeds amount in inventory, adding all quantities available to cart.");
                                        newProduct.Quantity = poi.Quantity;
                                        poi.Quantity = 0;
                                    }
                                    else
                                    {
                                        newProduct.Quantity = stringQuantity2;
                                        poi.Quantity = poi.Quantity - stringQuantity2;
                                    }
                                    productServiceCart.Create(newProduct);
                                    Console.WriteLine("Product(s) added to cart!");
                                }
                                else if(productOfInterest is ProductByWeight)
                                {
                                    var poi = productOfInterest as ProductByWeight;
                                    var newProduct = new ProductByWeight();
                                    newProduct.Name = productOfInterest.Name;
                                    newProduct.Description = productOfInterest.Description;
                                    newProduct.Price = productOfInterest.Price;
                                    newProduct.Bogo = productOfInterest.Bogo;

                                    Console.WriteLine("How much weight would you like to add to cart?");
                                    var stringQuantity2 = decimal.Parse(Console.ReadLine() ?? "0");

                                    if (stringQuantity2 > poi.Weight)
                                    {
                                        Console.WriteLine("Quantity exceeds amount in inventory, adding all quantities available to cart.");
                                        newProduct.Weight = poi.Weight;
                                        poi.Weight = 0;
                                    }
                                    else
                                    {
                                        newProduct.Weight = stringQuantity2;
                                        poi.Weight = poi.Weight - stringQuantity2;
                                    }
                                    productServiceCart.Create(newProduct);
                                    Console.WriteLine("Product(s) added to cart!");
                                }
                            }
                        end:;
                        }
                        else if (actionCustomer == ActionTypeCustomer.ListInventory)
                        {
                            if (productServiceInventory.Products.Count == 0)
                            {
                                Console.WriteLine("Empty Inventory!");
                                goto end;
                            }
                            Console.WriteLine("You chose to list all items in inventory");
                        sort:;
                            // Call inventory's list navigator and give sort options
                            var myListNavigator = productServiceInventory.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by unit price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceInventory.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedUnitListNavigator = productServiceInventory.SortedUnitListNavigator;
                                ListItems(mySortedUnitListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }
                        end:;
                        }
                        else if(actionCustomer == ActionTypeCustomer.ListCart)
                        {
                            if (productServiceCart.Products.Count == 0)
                            {
                                Console.WriteLine("Empty Cart!");
                                goto end;
                            }
                            Console.WriteLine("You chose to list all items in cart");
                        sort:;
                            // Call cart's list navigator and give sort options
                            var myListNavigator = productServiceCart.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by total price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceCart.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedTotalListNavigator = productServiceCart.SortedTotalListNavigator;
                                ListItems(mySortedTotalListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }
                        end:;
                        }
                        else if(actionCustomer == ActionTypeCustomer.UpdateCart)
                        {
                            Console.WriteLine("You chose to update a product in your cart");

                            if (productServiceCart.Products.Count == 0)
                            {
                                Console.WriteLine("Empty Cart!");
                                goto end;
                            }
                            Console.WriteLine("You chose to list all items in cart");
                        sort:;
                            // Call cart's list navigator and give sort options
                            var myListNavigator = productServiceCart.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by total price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceCart.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedTotalListNavigator = productServiceCart.SortedTotalListNavigator;
                                ListItems(mySortedTotalListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }

                            Console.WriteLine("Which product would you like to update? (insert id)");
                            var choice = int.Parse(Console.ReadLine() ?? "0");
                            var productOfInterest = productServiceCart.Products.FirstOrDefault(t => t.Id == choice);
                            var productOfInterestInventory = productServiceInventory.Products.FirstOrDefault(t => t.Name == productOfInterest.Name);
                            // If product by id isn't found in cart/inventory yield an error
                            if (productOfInterest == null)
                            {
                                Console.WriteLine("Invalid product in cart!");
                                goto end;
                            }
                            else if(productOfInterestInventory == null)
                            {
                                Console.WriteLine("Invalid product in inventory!");
                                goto end;
                            }
                            else
                            {
                                // Check if found product is by quantity and update information
                                if (productOfInterest is ProductByQuantity)
                                {
                                    var poiCart = productOfInterest as ProductByQuantity;
                                    var poiInventory = productOfInterestInventory as ProductByQuantity;
                                    Console.WriteLine("How many would you like to have in cart?");
                                    var stringQuantity = int.Parse(Console.ReadLine() ?? "0");
                                    var total = poiCart.Quantity + poiInventory.Quantity;

                                    // If the updated amount is greater than the quantity available in both inventory and cart...
                                    if (stringQuantity > total)
                                    {
                                        Console.WriteLine("Quantity exceeds amount in inventory, adding all quantities available to cart.");
                                        poiCart.Quantity = total;
                                        poiInventory.Quantity = 0;
                                    }
                                    else
                                    {
                                        poiCart.Quantity = stringQuantity;
                                        poiInventory.Quantity = total - stringQuantity;
                                    }
                                    productServiceInventory.Update(productOfInterestInventory);
                                    productServiceCart.Update(productOfInterest);
                                    Console.WriteLine("Updated quantity in cart!");
                                }
                                // Check if found product is by weight and update information
                                else if (productOfInterest is ProductByWeight)
                                {
                                    var poiCart = productOfInterest as ProductByWeight;
                                    var poiInventory = productOfInterestInventory as ProductByWeight;
                                    Console.WriteLine("How much weight would you like to have in cart?");
                                    var stringQuantity = int.Parse(Console.ReadLine() ?? "0");
                                    var total = poiCart.Weight + poiInventory.Weight;

                                    // If the updated amount is greater than the quantity available in both inventory and cart...
                                    if (stringQuantity > total)
                                    {
                                        Console.WriteLine("Quantity exceeds amount in inventory, adding all quantities available to cart.");
                                        poiCart.Weight = total;
                                        poiInventory.Weight = 0;
                                    }
                                    else
                                    {
                                        poiCart.Weight = stringQuantity;
                                        poiInventory.Weight = total - stringQuantity;
                                    }
                                    productServiceInventory.Update(productOfInterestInventory);
                                    productServiceCart.Update(productOfInterest);
                                    Console.WriteLine("Updated quantity in cart!");
                                }
                            }
                        end:;
                        }
                        else if(actionCustomer == ActionTypeCustomer.RemoveCart)
                        {
                            Console.WriteLine("You chose to remove a product from your cart");

                            if (productServiceCart.Products.Count == 0)
                            {
                                Console.WriteLine("Empty Cart!");
                                goto end;
                            }
                            Console.WriteLine("Here is a list of products in your cart for reference");
                        sort:;
                            // Call cart's list navigator and give sort options
                            var myListNavigator = productServiceCart.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by total price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceCart.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedTotalListNavigator = productServiceCart.SortedTotalListNavigator;
                                ListItems(mySortedTotalListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }

                            Console.WriteLine("Which product would you like to remove?");
                            var id = int.Parse(Console.ReadLine() ?? "0");
                            var productOfInterest = productServiceCart.Products.FirstOrDefault(t => t.Id == id);

                            // Re-adds the quantity back to inventory
                            for (int i = 0; i < productServiceInventory.Products.Count; i++)
                            {
                                if(productServiceInventory.Products[i].Name == productOfInterest?.Name)
                                {
                                    if (productOfInterest is ProductByQuantity)
                                    {
                                        var poi = productOfInterest as ProductByQuantity;
                                        var poi2 = productServiceInventory.Products[i] as ProductByQuantity;
                                        poi2.Quantity += poi.Quantity;
                                    }
                                }
                            }
                            productServiceCart.Delete(id);
                            Console.WriteLine("Product(s) removed from your cart!");
                        end:;
                        }
                        else if (actionCustomer == ActionTypeCustomer.SearchCart)
                        {
                            if (productServiceInventory.Products.Count == 0)
                            {
                                Console.WriteLine("Empty cart! Nothing to search for");
                                goto done;
                            }
                            Console.WriteLine("You chose to search the cart");
                            Console.WriteLine("Search by name or description");
                            var inputString = Console.ReadLine();
                            inputString = inputString.ToLower();
                            // Call search method that finds products in cart given inputString query
                            var myListNavigator = productServiceCart.Search(inputString);
                            if (myListNavigator.Empty)
                            {
                                Console.WriteLine("No results!");
                                goto done;
                            }
                        sort:;
                            // Give sort options for results found
                            Console.WriteLine("(1) Sort by name | (2) Sort by total price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceCart.SortedNameListNavigator;
                                mySortedNameListNavigator = productServiceCart.Search(inputString);
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedTotalListNavigator = productServiceCart.SortedTotalListNavigator;
                                mySortedTotalListNavigator = productServiceCart.Search(inputString);
                                ListItems(mySortedTotalListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }
                        done:;
                        }
                        else if (actionCustomer == ActionTypeCustomer.SearchInventory)
                        {
                            if (productServiceInventory.Products.Count == 0)
                            {
                                Console.WriteLine("Empty inventory! Nothing to search for");
                                goto done;
                            }
                            Console.WriteLine("You chose to search the inventory");
                            Console.WriteLine("Search by name or description");
                            var inputString = Console.ReadLine();
                            inputString = inputString.ToLower();
                            // Call search method that finds products in cart given inputString query
                            var myListNavigator = productServiceInventory.Search(inputString);
                            if (myListNavigator.Empty)
                            {
                                Console.WriteLine("No results!");
                                goto done;
                            }
                        sort:;
                            // Give sort options for results found
                            Console.WriteLine("(1) Sort by name | (2) Sort by unit price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceInventory.SortedNameListNavigator;
                                mySortedNameListNavigator = productServiceInventory.Search(inputString);
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedUnitListNavigator = productServiceInventory.SortedUnitListNavigator;
                                mySortedUnitListNavigator = productServiceInventory.Search(inputString);
                                ListItems(mySortedUnitListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }
                        done:;
                        }
                        else if(actionCustomer == ActionTypeCustomer.Save)
                        {
                            Console.WriteLine("You chose to save products");
                            productServiceCart.Save("SaveData1.json");
                            productServiceInventory.Save("SaveData2.json");
                        }
                        else if(actionCustomer == ActionTypeCustomer.Load)
                        {
                            Console.WriteLine("You chose to load products");
                            productServiceCart.Load("SaveData1.json");
                            productServiceInventory.Load("SaveData2.json");

                        }
                        else if (actionCustomer == ActionTypeCustomer.CheckOut)
                        {
                            if(productServiceCart.Products.Count == 0)
                            {
                                Console.WriteLine("No products in cart, returning back to menu!");
                                goto menu;
                            }

                            Console.WriteLine("You chose to check out");

                            decimal subtotal = 0;
                            decimal bogosale = 0;
                            foreach (var product in productServiceCart.Products)
                            {
                                // Calculate subtotals depending on if the product in the list is by quantity/weight
                                if(product is ProductByQuantity) { 
                                    var poi = product as ProductByQuantity;
                                    subtotal += poi.TotalPrice;
                                    // If product is a bogo item, calculate how many there are and respective bogo discount
                                    if (poi.Bogo)
                                    {
                                        var couples = poi.Quantity / 2;
                                        bogosale += poi.Price * couples;
                                    }
                                }
                                else if (product is ProductByWeight)
                                {
                                    var poi = product as ProductByWeight;
                                    subtotal += poi.TotalPrice;
                                    if (poi.Bogo)
                                    {
                                        // If bogo, bogo sale price is half of the weight
                                        bogosale += (poi.Weight*poi.Price) / 2;
                                    }
                                }

                            }
                            Console.WriteLine($"Subtotal is: {subtotal}");
                            Console.WriteLine($"You saved: ${bogosale} from Bogo sales!");
                            decimal taxedamount = Decimal.Multiply(subtotal, (decimal)0.07);
                            Console.WriteLine($"Taxed amount is: {taxedamount}");
                            Console.WriteLine($"Total is: {subtotal + taxedamount - bogosale}");
                            cont = false;
                            contnest = false;

                            Console.WriteLine();
                            Payment();
                        }
                        else if (actionCustomer == ActionTypeCustomer.LogOut)
                        {
                            Console.WriteLine("Logging Out!");
                            contnest = false;
                        }
                    }
                    contnest = true;
                }else if(choiceCustomerOrAdmin == "2")
                {
                    while (contnest2)
                    {
                        var actionAdmin = AdminMenu();

                        if(actionAdmin == ActionTypeAdmin.AddInventory)
                        {
                            Console.WriteLine("You chose to add product(s) to inventory");
                            Console.WriteLine("Is this product by quantity (1) or weight (2)?");
                            var quantityOrWeight = int.Parse(Console.ReadLine() ?? "1");
                            if(quantityOrWeight == 1)
                            {
                                var newProduct = new ProductByQuantity();
                                AddProductAdminQuantity(newProduct);
                                productServiceInventory.Create(newProduct);
                                Console.WriteLine("Product(s) added to inventory!");
                            }
                            else if(quantityOrWeight == 2)
                            {
                                var newProduct = new ProductByWeight();
                                AddProductAdminWeight(newProduct);
                                productServiceInventory.Create(newProduct);
                                Console.WriteLine("Product(s) added to inventory!");
                            }
                            else
                            {
                                Console.WriteLine("Unable to add product!");
                            }
                        }else if(actionAdmin == ActionTypeAdmin.ListInventory)
                        {
                            if (productServiceInventory.Products.Count == 0)
                            {
                                Console.WriteLine("Empty Inventory!");
                                goto end;
                            }
                            Console.WriteLine("You chose to list all items in inventory");
                        sort:;
                            // Call inventory's list navigator and give sort options for results found
                            var myListNavigator = productServiceInventory.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by unit price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceInventory.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedUnitListNavigator = productServiceInventory.SortedUnitListNavigator;
                                ListItems(mySortedUnitListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }
                        end:;
                        }
                        else if(actionAdmin == ActionTypeAdmin.UpdateInventory)
                        {
                            Console.WriteLine("You chose to update a product in inventory");
                            if (productServiceInventory.Products.Count == 0)
                            {
                                Console.WriteLine("Empty Inventory!");
                                goto end;
                            }
                            Console.WriteLine("Here is a list of products in inventory for reference");
                        sort:;
                            // Call inventory's list navigator and give sort options for results found
                            var myListNavigator = productServiceInventory.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by unit price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceInventory.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedUnitListNavigator = productServiceInventory.SortedUnitListNavigator;
                                ListItems(mySortedUnitListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }

                            Console.WriteLine("Which product would you like to update? (insert id)");
                            var choice = int.Parse(Console.ReadLine() ?? "0");

                            var productOfInterest = productServiceInventory.Products.FirstOrDefault(t => t.Id == choice);
                            // If product couldn't be found...
                            if (productOfInterest == null)
                            {
                                Console.WriteLine("Invalid product in inventory!");
                                goto end;
                            }
                            // If found, check if product is by quantity or weight...
                            else
                            {
                                if (productOfInterest is ProductByQuantity)
                                {
                                    var poi = productOfInterest as ProductByQuantity;
                                    AddProductAdminQuantity(poi);
                                    productServiceInventory.Update(productOfInterest);
                                    Console.WriteLine("Product updated in inventory!");
                                }else if(productOfInterest is ProductByWeight)
                                {
                                    var poi = productOfInterest as ProductByWeight; 
                                    AddProductAdminWeight(poi);
                                    productServiceInventory.Update(productOfInterest);
                                    Console.WriteLine("Product updated in inventory!");
                                }
                            }
                        end:;
                        }
                        else if(actionAdmin == ActionTypeAdmin.RemoveInventory)
                        {
                            Console.WriteLine("You chose to remove a product from inventory");
                            if (productServiceInventory.Products.Count == 0)
                            {
                                Console.WriteLine("Empty Inventory!");
                                goto end;
                            }
                            Console.WriteLine("Here's the list of products in inventory for reference");
                        sort:;
                            // Call inventory's list navigator and give sort options for results found
                            var myListNavigator = productServiceInventory.ListNavigator;
                            Console.WriteLine("(1) Sort by name | (2) Sort by unit price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceInventory.SortedNameListNavigator;
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedUnitListNavigator = productServiceInventory.SortedUnitListNavigator;
                                ListItems(mySortedUnitListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }

                            Console.WriteLine("Which product would you like to remove?");
                            var id = int.Parse(Console.ReadLine() ?? "0");
                            var productOfInterest = productServiceInventory.Products.FirstOrDefault(t => t.Id == id);
                            // If product isn't found...
                            if(productOfInterest == null)
                            {
                                Console.WriteLine("Invalid inventory product!");
                            }
                            // If product is found...
                            else
                            {
                                productServiceInventory.Delete(id);
                                Console.WriteLine("Product removed from inventory!");
                            }
                        end:;
                        }
                        else if(actionAdmin == ActionTypeAdmin.SearchInventory)
                        {
                            if (productServiceInventory.Products.Count == 0)
                            {
                                Console.WriteLine("Empty inventory! Nothing to search for");
                                goto done;
                            }
                            Console.WriteLine("You chose to search the inventory");
                            Console.WriteLine("Search by name or description");
                            var inputString = Console.ReadLine();
                            inputString = inputString.ToLower();
                            var myListNavigator = productServiceInventory.Search(inputString);
                            // If there is no results list...
                            if (myListNavigator.Empty)
                            {
                                Console.WriteLine("No results!");
                                goto done;
                            }
                        sort:;
                            // Give sort options for results found
                            Console.WriteLine("(1) Sort by name | (2) Sort by unit price | (3) No sorting");
                            var sortChoice = int.Parse(Console.ReadLine() ?? "3");
                            if (sortChoice == 1)
                            {
                                var mySortedNameListNavigator = productServiceInventory.SortedNameListNavigator;
                                mySortedNameListNavigator = productServiceInventory.Search(inputString);
                                ListItems(mySortedNameListNavigator);
                            }
                            else if (sortChoice == 2)
                            {
                                var mySortedUnitListNavigator = productServiceInventory.SortedUnitListNavigator;
                                mySortedUnitListNavigator = productServiceInventory.Search(inputString);
                                ListItems(mySortedUnitListNavigator);
                            }
                            else if (sortChoice == 3)
                            {
                                ListItems(myListNavigator);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice!");
                                goto sort;
                            }
                        done:;
                        }
                        else if (actionAdmin == ActionTypeAdmin.LogOut)
                        {
                            Console.WriteLine("Logging Out!");
                            contnest2 = false;
                        }
                    }
                    contnest2 = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, try again!");
                    continue;
                }
            }
            Console.WriteLine("Thank you for using the eCommerce Console Application!");
        }
        public static ActionTypeCustomer CustomerMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Hello Customer!");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add items to your cart.");
            Console.WriteLine("2. List all items in the inventory.");
            Console.WriteLine("3. List all items in your cart.");
            Console.WriteLine("4. Update quantity of an item in your cart.");
            Console.WriteLine("5. Remove items in your cart.");
            Console.WriteLine("6. Search for an item in your cart.");
            Console.WriteLine("7. Search for an item in the inventory.");
            Console.WriteLine("8. Save products");
            Console.WriteLine("9. Load products");
            Console.WriteLine("10. Check out.");
            Console.WriteLine("11. Log Out.");
            Console.WriteLine();

            while (true)
            {
                var input = (Console.ReadLine() ?? "0");
                switch (input)
                {
                    case "1":
                        return ActionTypeCustomer.AddCart;
                    case "2":
                        return ActionTypeCustomer.ListInventory;
                    case "3":
                        return ActionTypeCustomer.ListCart;
                    case "4":
                        return ActionTypeCustomer.UpdateCart;
                    case "5":
                        return ActionTypeCustomer.RemoveCart;
                    case "6":
                        return ActionTypeCustomer.SearchCart;
                    case "7":
                        return ActionTypeCustomer.SearchInventory;
                    case "8":
                        return ActionTypeCustomer.Save;
                    case "9":
                        return ActionTypeCustomer.Load;
                    case "10":
                        return ActionTypeCustomer.CheckOut;
                    case "11":
                        return ActionTypeCustomer.LogOut;
                    default:
                        Console.WriteLine("Invalid input, try again!");
                        continue;

                }
            }
        }

        public static ActionTypeAdmin AdminMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Hello Admin!");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add items to the inventory.");
            Console.WriteLine("2. List all items in the inventory.");
            Console.WriteLine("3. Update items in the inventory.");
            Console.WriteLine("4. Remove items in the inventory.");
            Console.WriteLine("5. Search for an item in the inventory.");
            Console.WriteLine("6. Log out.");
            Console.WriteLine();


            while (true)
            {
                var input = (Console.ReadLine() ?? "0");
                switch (input)
                {
                    case "1":
                        return ActionTypeAdmin.AddInventory;
                    case "2":
                        return ActionTypeAdmin.ListInventory;
                    case "3":
                        return ActionTypeAdmin.UpdateInventory;
                    case "4":
                        return ActionTypeAdmin.RemoveInventory;
                    case "5":
                        return ActionTypeAdmin.SearchInventory;
                    case "6":
                        return ActionTypeAdmin.LogOut;
                    default:
                        Console.WriteLine("Invalid input, try again!");
                        continue;

                }
            }
        }
        public static void AddProductAdminQuantity(ProductByQuantity? product)
        {
            if(product == null)
            {
                return;
            }

            Console.WriteLine("What is the name of the product?");
            product.Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the description for the product?");
            product.Description = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the unit price of the product?");
            product.Price = decimal.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("How many units would you like to have of the product?");
            product.Quantity = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Is this product bogo?");
            product.Bogo = bool.Parse(Console.ReadLine() ?? "FALSE"); 

        }

        public static void AddProductAdminWeight(ProductByWeight? product)
        {
            if (product == null)
            {
                return;
            }

            Console.WriteLine("What is the name of the product?");
            product.Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the description for the product?");
            product.Description = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the unit price of the product?");
            product.Price = decimal.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("What is the weight of the product?");
            product.Weight = decimal.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Is this product bogo?");
            product.Bogo = bool.Parse(Console.ReadLine() ?? "FALSE");

        }

        public static void Payment()
        {
            Console.WriteLine("Please enter your payment information below to complete transaction!");
            Console.WriteLine("What is the 16 digit credit card number?");
            var creditCardNumber = Console.ReadLine() ?? string.Empty;
            while (!(creditCardNumber.All(char.IsDigit) && creditCardNumber.Length == 16))
            {
                Console.WriteLine("Invalid format, please try again!");
                creditCardNumber = Console.ReadLine() ?? string.Empty;
            }

            Console.WriteLine("What is the expiration date? (MM/YY)");
            var expirationDate = Console.ReadLine() ?? string.Empty;
            string format = "MM/yy";
            DateTime parsedDate;
            while (!DateTime.TryParseExact(expirationDate, format, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate))
            {
                Console.WriteLine("Invalid format, please try again!");
                expirationDate = Console.ReadLine() ?? string.Empty;

            }
            Console.WriteLine("What is the 3 digit CVV code?");
            var cvvCode = Console.ReadLine() ?? string.Empty;
            while (!(cvvCode.All(char.IsDigit) && cvvCode.Length == 3))
            {
                Console.WriteLine("Invalid format, please try again!");
                cvvCode = Console.ReadLine() ?? string.Empty;
            }

            Console.WriteLine("What is the name on the card?");
            var name = Console.ReadLine() ?? string.Empty;
            while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Invalid format, please try again!");
                name = Console.ReadLine() ?? string.Empty;
            }

            Console.WriteLine("What is the billing/shipping ZIP Code?");
            var zipCode = Console.ReadLine() ?? string.Empty;
            while (!(zipCode.All(char.IsDigit) && zipCode.Length == 5))
            {
                Console.WriteLine("Invalid format, please try again!");
                zipCode = Console.ReadLine() ?? string.Empty;
            }

            Console.WriteLine("What is the shipping address?");
            var shippingAddress = Console.ReadLine() ?? string.Empty;
            while (string.IsNullOrEmpty(shippingAddress))
            {
                Console.WriteLine("Invalid format, please try again!");
                shippingAddress = Console.ReadLine() ?? string.Empty;
            }

            Console.WriteLine("Thank you for the info!");
        }

        public static void ListItems(ListNavigator<Product> myListNavigator)
        {
            foreach (var thing in myListNavigator.GetCurrentPage())
            {
                Console.WriteLine($"{thing.Key}. {thing.Value}");
            }
            // If the list has a previous/next page show those options...
            if (myListNavigator.HasPreviousPage && myListNavigator.HasNextPage)
            {
                Console.WriteLine("(1) Go to previous page | (2) Go to next page | (3) Exit");
                var pageChoice = int.Parse(Console.ReadLine() ?? "1");
                if (pageChoice == 1)
                {
                    myListNavigator.GoBackward();
                    ListItems(myListNavigator);
                }
                else if (pageChoice == 2)
                {
                    myListNavigator.GoForward();
                    ListItems(myListNavigator);
                }
                else if(pageChoice == 3)
                {
                    return;
                }
            }
            // If there is only a previous page show only previous option...
            else if (myListNavigator.HasPreviousPage && !myListNavigator.HasNextPage)
            {
                Console.WriteLine("(1) Go to previous page | (2) Exit");
                var pageChoice = int.Parse(Console.ReadLine() ?? "1");
                if (pageChoice == 1)
                {
                    myListNavigator.GoBackward();
                    ListItems(myListNavigator);
                }
                else if(pageChoice == 2)
                {
                    return;
                }
            }
            // If there is only a next page show only next page...
            else if (!myListNavigator.HasPreviousPage && myListNavigator.HasNextPage)
            {
                Console.WriteLine("(1) Go to next page | (2) Exit");
                var pageChoice = int.Parse(Console.ReadLine() ?? "1");
                if (pageChoice == 1)
                {
                    myListNavigator.GoForward();
                    ListItems(myListNavigator);
                }
                else if(pageChoice == 2)
                {
                    return;
                }
            }
            // If there is no previous/next page give no options besides exit
            else
            {
                Console.WriteLine("(1) Exit");
                var pageChoice = int.Parse(Console.ReadLine() ?? "1");
                if (pageChoice == 1)
                {
                    return;
                }
            }
        }
    }
    public enum ActionTypeCustomer
    {
        AddCart, ListInventory, ListCart, UpdateCart, RemoveCart, SearchCart, SearchInventory, Save, Load, CheckOut, LogOut
    } 

    public enum ActionTypeAdmin
    {
        AddInventory, ListInventory, UpdateInventory, RemoveInventory, SearchInventory, LogOut
    }
}