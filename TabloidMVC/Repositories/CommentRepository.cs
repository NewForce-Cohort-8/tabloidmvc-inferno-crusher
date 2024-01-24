using Microsoft.Extensions.Hosting;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (Subject, Content, PostId, UserProfileId, CreateDateTime)
                        OUTPUT INSERTED.ID
                        VALUES (
                            @Subject, @Content, @PostId, @UserProfileId, @CreateDateTime)";
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Comment comment)
        {

        }
        public void Delete(int id)
        {

        }
        public List<Comment> GetAllPostComments(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime, u.Id AS UserId, u.DisplayName, u.FirstName, u.LastName, u.Email, u.CreateDateTime AS UserCreateDate, u.ImageLocation, u.UserTypeId 
                       FROM Comment c
                       LEFT JOIN UserProfile u ON c.UserProfileId = u.Id
                       WHERE c.PostId = @postId
                       ORDER BY c.CreateDateTime DESC";

                    cmd.Parameters.AddWithValue("@postId", postId);
                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            UserProfile = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("UserCreateDate")),
                                ImageLocation = null,
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId"))
                            }
                        };

                        comments.Add(comment);
                    }

                    reader.Close();

                    return comments;
                }
            }
        }
    }
}
