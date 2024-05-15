using Lista10_v2.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lista10_v2.Data
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasData(
               new Category()
               {
                   Id = 1,
                   Name = "Nabiał",
               },
               new Category()
               {
                   Id = 2,
                   Name = "Pieczywo",
               }

               ); ; ;

            modelBuilder.Entity<Article>().HasData(
                new Article()
                {
                    Id = 1,
                    Name = "Masło",
                    Price = 10,
                    ExpirationDate = new DateTime(2024, 10, 10),
                    CategoryId = 1,
                    ImagePath = null,
                },
                new Article()
                {
                    Id = 2,
                    Name = "Chleb",
                    Price = 5,
                    ExpirationDate = new DateTime(2023, 1, 10),
                    CategoryId = 2,
                    ImagePath = null,
                }

                ); ; ;

            

        }
    }
}
