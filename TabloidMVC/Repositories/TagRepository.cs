using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }
        public List<Tag> GetAllPublishedTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT t.id, t.Name FROM Tag t
                       order by name";


                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        tags.Add(tag);
                    }

                    reader.Close();

                    return tags;
                }
            }
        }
        public Tag GetUserTagById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT  id, Name FROM Tag
                        WHERE id = @id ";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    Tag tag = null;

                    if (reader.Read())
                    {
                        tag = NewTagFromReader(reader);
                    }

                    reader.Close();

                    return tag;
                }
            }
        }

        private Tag NewTagFromReader(SqlDataReader reader)
        {
            return new Tag()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
            };
        }
        public void DeleteTag(int Id)
            {
    using (SqlConnection conn = Connection)
    {
        conn.Open();

        using (SqlCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                            DELETE FROM Tag
                            WHERE Id = @id
                        ";

            cmd.Parameters.AddWithValue("@id", Id);

            cmd.ExecuteNonQuery();
        }
    }
}
public void Add(Tag tag)
{
    using (var conn = Connection)
    {
        conn.Open();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText =@"
                        INSERT INTO Tag ([Name])
                    OUTPUT INSERTED.ID
                    VALUES (@name);";
            cmd.Parameters.AddWithValue("@name", tag.Name);
            
            tag.Id = (int)cmd.ExecuteScalar();
                  }
    }
}
    }
}



//        private Post NewPostFromReader(SqlDataReader reader)
//        {
//            return new Post()
//            {
//                Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                Title = reader.GetString(reader.GetOrdinal("Title")),
//                Content = reader.GetString(reader.GetOrdinal("Content")),
//                ImageLocation = DbUtils.GetNullableString(reader, "HeaderImage"),
//                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
//                PublishDateTime = DbUtils.GetNullableDateTime(reader, "PublishDateTime"),
//                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
//                Category = new Category()
//                {
//                    Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
//                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
//                },
//                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
//                UserProfile = new UserProfile()
//                {
//                    Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
//                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
//                    Email = reader.GetString(reader.GetOrdinal("Email")),
//                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
//                    ImageLocation = DbUtils.GetNullableString(reader, "AvatarImage"),
//                    UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
//                    UserType = new UserType()
//                    {
//                        Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
//                        Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
//                    }
//                }
//            };
//        }

//    }
//}
