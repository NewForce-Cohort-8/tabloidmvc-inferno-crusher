using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration config) : base(config) { }

        // This method returns a list of Category objects along with associated Post objects
        public List<Category> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // SQL query to select category and post information
                    cmd.CommandText = @"SELECT c.Id, c.[Name], p.CategoryId, p.Title FROM Category c 
                     LEFT JOIN Post p ON c.id = p.CategoryId ORDER BY c.Name";

                    var reader = cmd.ExecuteReader();

                    // List to store the categories
                    var categories = new List<Category>();


                    // Loop through the results
                    while (reader.Read())
                    {
                        // Create a new Category object
                        Category category =new Category()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            
                        };

                        // Check if the category is not already in the list, add it if not
                        if (!categories.Any(x => x.Id == category.Id))
                        {
                            categories.Add(category);
                        }

                        // Check if the category is already in the list
                        if (categories.Any(x => x.Id == reader.GetInt32(reader.GetOrdinal("Id"))))
                        {
                            // Find the existing category in the list
                            Category current = categories.Find(x => x.Id == reader.GetInt32(reader.GetOrdinal("Id")));

                            // Check if the 'Title' field is not null
                            if (!reader.IsDBNull(reader.GetOrdinal("Title")))
                            {
                                // Create a new Post object and add it to the 'Posts' list of the current category
                                current.Posts.Add( new Post
                                {
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                });

                            }

                        }
                        
                    }

                    reader.Close();

                    return categories;
                }
            }
        }


        //add a category
        public void AddCategory(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Category ([Name])
                    OUTPUT INSERTED.ID
                    VALUES (@name);
                ";

                    cmd.Parameters.AddWithValue("@name", category.Name);


                    int id = (int)cmd.ExecuteScalar();

                    category.Id = id;
                }
            }
        }



        //get category by Id
        public Category GetCategoryById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                         SELECT Id, [Name]
                         FROM Category
                         WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Category category = null;

                    while (reader.Read())
                    {
                        if (category == null)
                        {
                            category = new Category
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };


                        }
                    }
                    reader.Close();
                    return category;
                }
            }
        }


        //WARNING Categories have dependencies!!!!!!!!!!

        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        //code to delete record here...
                        cmd.CommandText = @"
                             DELETE
                             FROM Category
                             WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@id", categoryId);
                        cmd.ExecuteNonQuery();
                    }

                    catch (SqlException ex) when (ex.Number == 547)
                    {
                        //notify the user the record is in use here...
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }
    }

}
