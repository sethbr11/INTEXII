using System;
using System.Linq;
using INTEXII.Models;

namespace INTEXII.Data
{
    public static class DbSeeder
    {
        public static void Seed(IntexW24datasetContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Products.Any())
            {
                var products = new[]
                {
                    new Product
                    {
                        ProductId = 1,
                        Name = "Harry Potter Hogwarts Express",
                        Year = 2023,
                        NumParts = 801,
                        Price = 120,
                        ImgLink = "/images/HarryPotterLego-removebg.png",
                        PrimaryColor = "Red",
                        SecondaryColor = "Black",
                        Description = "Step aboard the legendary Hogwarts Express train and experience the magic!",
                        Category = "Harry Potter",
                        PublicCategory = "Harry Potter",
                        PopularityRank = 5.0,
                        Recommendation1 = 2,
                        Recommendation2 = 3,
                        Recommendation3 = 4,
                        Recommendation4 = 5,
                        Recommendation5 = 6
                    },
                    new Product
                    {
                        ProductId = 2,
                        Name = "Ferrari Daytona SP3",
                        Year = 2022,
                        NumParts = 3778,
                        Price = 399,
                        ImgLink = "/images/Ferrari.jpg",
                        PrimaryColor = "Red",
                        SecondaryColor = "Black",
                        Description = "A challenging build that makes a stunning display model for any supercar fan.",
                        Category = "Technic",
                        PublicCategory = "Technic",
                        PopularityRank = 4.9,
                        Recommendation1 = 1,
                        Recommendation2 = 3,
                        Recommendation3 = 5,
                        Recommendation4 = 6,
                        Recommendation5 = 7
                    },
                    new Product
                    {
                        ProductId = 3,
                        Name = "Minecraft The Ruined Portal",
                        Year = 2021,
                        NumParts = 316,
                        Price = 29,
                        ImgLink = "/images/Minecraft-removebg-preview.png",
                        PrimaryColor = "Purple",
                        SecondaryColor = "Grey",
                        Description = "Take a Minecraft player's passion for the game to the next level.",
                        Category = "Minecraft",
                        PublicCategory = "Minecraft",
                        PopularityRank = 4.7,
                        Recommendation1 = 1,
                        Recommendation2 = 2,
                        Recommendation3 = 4,
                        Recommendation4 = 5,
                        Recommendation5 = 8
                    },
                    new Product
                    {
                        ProductId = 4,
                        Name = "Star Wars Ahsoka Tano",
                        Year = 2023,
                        NumParts = 605,
                        Price = 45,
                        ImgLink = "/images/Ahsoka-removebg-preview.png",
                        PrimaryColor = "Grey",
                        SecondaryColor = "Blue",
                        Description = "Build and display the iconic Star Wars hero with dual lightsabers.",
                        Category = "Star Wars",
                        PublicCategory = "Star Wars",
                        PopularityRank = 4.8,
                        Recommendation1 = 1,
                        Recommendation2 = 3,
                        Recommendation3 = 5,
                        Recommendation4 = 6,
                        Recommendation5 = 7
                    },
                    new Product
                    {
                        ProductId = 5,
                        Name = "Classic Batman Batmobile",
                        Year = 2021,
                        NumParts = 345,
                        Price = 60,
                        ImgLink = "/images/Batman.jpg",
                        PrimaryColor = "Black",
                        SecondaryColor = "Red",
                        Description = "Fight crime in Gotham City with this iconic Batmobile build.",
                        Category = "DC",
                        PublicCategory = "DC",
                        PopularityRank = 4.6,
                        Recommendation1 = 1,
                        Recommendation2 = 2,
                        Recommendation3 = 4,
                        Recommendation4 = 7,
                        Recommendation5 = 8
                    },
                    new Product
                    {
                        ProductId = 6,
                        Name = "Creator Cute Pug",
                        Year = 2022,
                        NumParts = 150,
                        Price = 15,
                        ImgLink = "/images/product-1.png",
                        PrimaryColor = "Tan",
                        SecondaryColor = "Black",
                        Description = "A delightful and cute pug creator kit that's easy to build.",
                        Category = "Creator",
                        PublicCategory = "Creator",
                        PopularityRank = 4.5,
                        Recommendation1 = 1,
                        Recommendation2 = 2,
                        Recommendation3 = 7,
                        Recommendation4 = 8,
                        Recommendation5 = 3
                    },
                    new Product
                    {
                        ProductId = 7,
                        Name = "Space Shuttle Discovery",
                        Year = 2021,
                        NumParts = 2354,
                        Price = 199,
                        ImgLink = "/images/product-2.png",
                        PrimaryColor = "White",
                        SecondaryColor = "Black",
                        Description = "Celebrate the wonders of space exploration with this detailed NASA space shuttle.",
                        Category = "Creator Expert",
                        PublicCategory = "Creator Expert",
                        PopularityRank = 4.9,
                        Recommendation1 = 2,
                        Recommendation2 = 4,
                        Recommendation3 = 6,
                        Recommendation4 = 8,
                        Recommendation5 = 1
                    },
                    new Product
                    {
                        ProductId = 8,
                        Name = "Botanical Flower Bouquet",
                        Year = 2021,
                        NumParts = 756,
                        Price = 59,
                        ImgLink = "/images/product-3.png",
                        PrimaryColor = "Green",
                        SecondaryColor = "Pink",
                        Description = "A beautiful and unique display model that never needs watering.",
                        Category = "Botanical",
                        PublicCategory = "Botanical",
                        PopularityRank = 4.8,
                        Recommendation1 = 3,
                        Recommendation2 = 5,
                        Recommendation3 = 6,
                        Recommendation4 = 7,
                        Recommendation5 = 1
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            if (!context.Customers.Any())
            {
                var customer = new Customer
                {
                    CustomerId = 1,
                    FirstName = "Demo",
                    LastName = "User",
                    BirthDate = "1995-01-01",
                    CountryOfResidence = "USA",
                    Gender = "M",
                    Age = 31,
                    Username = "demo"
                };

                context.Customers.Add(customer);
                context.SaveChanges();
            }

            if (!context.Recommendations.Any())
            {
                var recommendation = new Recommendation
                {
                    CustomerId = 1,
                    RecProdId1 = 1,
                    RecProdId2 = 2,
                    RecProdId3 = 3,
                    RecProdId4 = 4
                };

                context.Recommendations.Add(recommendation);
                context.SaveChanges();
            }
        }
    }
}
