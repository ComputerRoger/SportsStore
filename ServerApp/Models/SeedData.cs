using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models
{
    public class SeedData
    {
        public static void SeedDatabase( DataContext dataContext )
        {
            //  Ensure that the database has been created and updated before the seed data is applied.
            dataContext.Database.Migrate();

            if( dataContext.Products.Count() == 0 )
            {
                //  The database has no data, so fill it.

                //  Create a series of model objects.
                //  Associate the objects with one another.
                var s1 = new Supplier
                {
                    Name = "Splash Dudes",
                    City = "San Jose",
                    State = "CA"
                }; var s2 = new Supplier
                {
                    Name = "Soccer Town",
                    City = "Chicago",
                    State = "IL"
                }; var s3 = new Supplier
                {
                    Name = "Chess Co",
                    City = "New York",
                    State = "NY"
                };

                dataContext.Products.AddRange(
                    new Product
                    {
                        Name = "Kayak",
                        Description = "A boat for one persion",
                        Category = "Watersports",
                        Price = 275,
                        Supplier = s1,
                        Ratings = new List<Rating>
                        {
                            new Rating
                            {
                                Stars = 4
                            },
                            new Rating
                            {
                                Stars = 3
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Lifejacket",
                        Description = "Protective and fashionable",
                        Category = "Watersports",
                        Price = 48.95m,
                        Supplier = s1,
                        Ratings = new List<Rating>
                        {
                            new Rating
                            {
                                Stars = 2 
                            },
                            new Rating
                            {
                                Stars = 5
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Soccer Ball",
                        Description = "FIFA approved size and weight",
                        Category = "Soccer",
                        Price = 19.50m,
                        Supplier = s2,
                        Ratings = new List<Rating>
                        {
                            new Rating
                            {
                                Stars = 1
                            },
                            new Rating
                            {
                                Stars = 3
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Corner Flags",
                        Description = "Give your pitch a professional touch",
                        Category = "Soccer",
                        Price = 24.95m,
                        Supplier = s2,
                        Ratings = new List<Rating>
                        {
                            new Rating
                            {
                                Stars = 3
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Stadium",
                        Description = "Flat-packed 35,000 seat stadium",
                        Category = "Soccer",
                        Price = 79500,
                        Supplier = s2,
                        Ratings = new List<Rating>
                        {
                            new Rating
                            {
                                Stars = 4
                            },
                            new Rating
                            {
                                Stars = 3
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Thinking Cap",
                        Description = "Improve brain efficiency by 75%",
                        Category = "Chess",
                        Price = 16,
                        Supplier = s3,
                        Ratings = new List<Rating>
                        {
                            new Rating
                            {
                                Stars = 5
                            },
                            new Rating
                            {
                                Stars = 4
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Unsteady Chair",
                        Description = "Secretly give your opponent a disadvantage",
                        Category = "Chess",
                        Price = 29.95m,
                        Supplier = s3,
                        Ratings = new List<Rating>
                        {
                            new Rating
                            {
                                Stars = 3
                            }
                        }
                    },
                    new Product
                    {
                        Name = "Human Chess Board",
                        Description = "A fun game for the family",
                        Category = "Chess",
                        Price = 75,
                        Supplier = s3
                    },
                    new Product
                    {
                        Name = "Bling-Bling King",
                        Description = "Gold-plated, diamond-studded King",
                        Category = "Chess",
                        Price = 1200,
                        Supplier = s3
                    }
                    );

                //  Save the model objects to the database.
                dataContext.SaveChanges();
            }
        }
    }
}
