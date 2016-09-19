using Sabio.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using Sabio.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sabio.Web.Models;
using Sabio.Web.Domain;
using Sabio.Web.Enums;

namespace Sabio.Web.Services
{

    public class UserProfileServices : BaseService
    {

        public static void EmailConfirmed(string userId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.AspNetUsers_UpdateEmailConfirmed"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@userId", userId);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }


        public static User GetFullUserById(string id)
        {
            User p = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.UserProfile_AvatarMediaById_v2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);


               }, map: delegate (IDataReader reader, short set)
               {

                   int startingIndex = 0; //startingOrdinal

                   if (set == 0)
                   {
                       p = new User();
                       p.ProfileName = reader.GetSafeString(startingIndex++);
                       p.ProfileEmail = reader.GetSafeString(startingIndex++);
                       p.ProfilePhone = reader.GetSafeString(startingIndex++);
                       p.ProfileMobile = reader.GetSafeString(startingIndex++);
                       p.ProfileWebsite = reader.GetSafeString(startingIndex++);
                       p.ProfileAddress = reader.GetSafeString(startingIndex++);
                       p.ProfileCompany = reader.GetSafeString(startingIndex++);
                       p.UserId = reader.GetSafeString(startingIndex++);
                       p.MediaId = reader.GetSafeInt32(startingIndex++);
                       p.ThemeMediaId = reader.GetSafeInt32(startingIndex++);
                       p.ProfileLastName = reader.GetSafeString(startingIndex++);
                       p.ProfileViewCount = reader.GetSafeInt32(startingIndex++);
                       p.ConnectionsCount = reader.GetSafeInt32(startingIndex++);
                       p.GroupsCount = reader.GetSafeInt32(startingIndex++);
                       p.Tagline = reader.GetSafeString(startingIndex++);
                       p.CardTemplate = reader.GetSafeString(startingIndex++);
                       p.Id = reader.GetSafeString(startingIndex++);
                       p.Email = reader.GetSafeString(startingIndex++);
                       p.EmailConfirmed = reader.GetSafeBool(startingIndex++);
                       p.SecurityStamp = reader.GetSafeString(startingIndex++);
                       p.PhoneNumber = reader.GetSafeString(startingIndex++);
                       p.PhoneNumberConfirmed = reader.GetSafeBool(startingIndex++);
                       p.TwoFactorEnabled = reader.GetSafeBool(startingIndex++);
                       p.LockoutEndDateUtc = reader.GetSafeDateTime(startingIndex++);
                       p.LockoutEnabled = reader.GetSafeBool(startingIndex++);
                       p.AccessFailedCount = reader.GetSafeInt32(startingIndex++);
                       p.UserName = reader.GetSafeString(startingIndex++);

                       Medias m = new Medias();
                       m.MediasTableId = reader.GetSafeInt32(startingIndex++);
                       m.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                       m.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                       m.MediaType = reader.GetSafeEnum<MediaType>(startingIndex++);
                       m.ContentType = reader.GetSafeString(startingIndex++);
                       m.UserId = reader.GetSafeString(startingIndex++);
                       m.FilePath = reader.GetSafeString(startingIndex++);
                       if (m.MediasTableId > 0)
                           p.Avatar = m;

                       Medias t = new Medias();
                       t.MediasTableId = reader.GetSafeInt32(startingIndex++);
                       t.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                       t.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                       t.MediaType = reader.GetSafeEnum<MediaType>(startingIndex++);
                       t.ContentType = reader.GetSafeString(startingIndex++);
                       t.UserId = reader.GetSafeString(startingIndex++);
                       t.FilePath = reader.GetSafeString(startingIndex++);
                       if (t.MediasTableId > 0)
                           p.Theme = t;

                       Location l = new Location();
                       l.Id = reader.GetSafeInt32(startingIndex++);
                       l.Latitude = reader.GetSafeDecimal(startingIndex++);
                       l.Longitude = reader.GetSafeDecimal(startingIndex++);
                       l.Address = reader.GetSafeString(startingIndex++);
                       l.City = reader.GetSafeString(startingIndex++);
                       l.State = reader.GetSafeString(startingIndex++);
                       l.ZipCode = reader.GetSafeString(startingIndex++);
                       if (l.Id > 0)
                           p.Location = l;


                       p.Tags = new List<Tags>();
                       p.ExternalAccounts = new List<UserExternalAccounts>();
                   }


                   else if (set == 1)
                   {
                       Tags myTags = new Tags();
                       myTags.Id = reader.GetSafeInt32(startingIndex++);
                       myTags.CreatedDate = reader.GetDateTime(startingIndex++);
                       myTags.ModifiedDate = reader.GetDateTime(startingIndex++);
                       myTags.TagName = reader.GetSafeString(startingIndex++);
                       myTags.TagSlug = reader.GetSafeString(startingIndex++);
                       myTags.ParentTagId = reader.GetSafeInt32(startingIndex++);
                       p.Tags.Add(myTags);
                   }


                   else if (set == 2)
                   {
                       UserExternalAccounts myAccounts = new UserExternalAccounts();
                       myAccounts.source = reader.GetSafeString(startingIndex++);
                       myAccounts.url = reader.GetSafeString(startingIndex++);
                   
                       p.ExternalAccounts.Add(myAccounts);
                   }
               }

               );


            return p;
        }

        public static int Post(UsersProfilesRequest model)
        {
            int OutputId = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserProfile_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@profileName", model.profileName);
                   paramCollection.AddWithValue("@profileEmail", model.profileEmail);
                   paramCollection.AddWithValue("@profilePhone", model.profilePhone);
                   paramCollection.AddWithValue("@profileMobile", model.profileMobile);
                   paramCollection.AddWithValue("@profileWebsite", model.profileWebsite);
                   paramCollection.AddWithValue("@profileAddress", model.profileAddress);
                   paramCollection.AddWithValue("@profileCompany", model.profileCompany);
                   paramCollection.AddWithValue("@userId", model.userId);
                   paramCollection.AddWithValue("@Tagline", model.Tagline);

                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out OutputId);
               }
               );

            //creating SystemEvents post for recent activity feed
            SystemEventsRequest newModel = new SystemEventsRequest();

            newModel.ActorUserId = model.userId;
            newModel.EventType = SystemEventTypes.NewRegister;

            SystemEventsService.Post(newModel);

            return OutputId;
        }

        public static List<User> Get(int pageNumber, int pageSize, string queryString)
        {
            List<User> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserProfile_Select"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CurrentPage", pageNumber);
                   paramCollection.AddWithValue("@ItemsPerPage", pageSize);
                   paramCollection.AddWithValue("@Search", queryString);
               }
               , map: delegate (IDataReader reader, short set) //function from BaseService
               {
                   User p = new User();
                   int startingIndex = 0; //startingOrdinal


                   p.ProfileName = reader.GetSafeString(startingIndex++);
                   p.ProfileLastName = reader.GetSafeString(startingIndex++);
                   p.ProfileEmail = reader.GetSafeString(startingIndex++);
                   p.ProfilePhone = reader.GetSafeString(startingIndex++);
                   p.ProfileMobile = reader.GetSafeString(startingIndex++);
                   p.ProfileWebsite = reader.GetSafeString(startingIndex++);
                   p.ProfileAddress = reader.GetSafeString(startingIndex++);
                   p.ProfileCompany = reader.GetSafeString(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.Tagline = reader.GetSafeString(startingIndex++);
                   p.Id = reader.GetSafeString(startingIndex++);
                   p.Email = reader.GetSafeString(startingIndex);
                   // update from user model

                   if (list == null)
                   {
                       list = new List<User>();
                   }

                   list.Add(p);

               }
               );

            return DecorateUsers(list);

        }

        public static int GetPagedUsers()
        {

            int totalItems = 0;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserProfile_CountAll"
                           , inputParamMapper: null
                           , map: delegate (IDataReader reader, short set)
                           {

                               int startingIndex = 0;

                               totalItems = reader.GetSafeInt32(startingIndex++);

                           });

            return totalItems;


        }

        public static void Put(string id, UsersProfilesRequest model)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserProfile_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@profileName", model.profileName);
                   paramCollection.AddWithValue("@profileLastName", model.profileLastName);
                   paramCollection.AddWithValue("@profilePhone", model.profilePhone);
                   paramCollection.AddWithValue("@profileMobile", model.profileMobile);
                   paramCollection.AddWithValue("@profileWebsite", model.profileWebsite);
                   paramCollection.AddWithValue("@profileAddress", model.profileAddress);
                   paramCollection.AddWithValue("@profileCompany", model.profileCompany);
                   paramCollection.AddWithValue("@userId", id);
                   paramCollection.AddWithValue("@Tagline", model.Tagline);


                   // adding new paramerter
                   SqlParameter s = new SqlParameter("@TagsId", SqlDbType.Structured);
                   if (model.profileTags != null && model.profileTags.Any())
                   {
                       s.Value = new IntIdTable(model.profileTags);
                   }
                   paramCollection.Add(s);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );


            foreach (UserExternalAccounts accountInfo in model.ExternalAccounts)
            {
                PostExternalAccounts(accountInfo, id);
            }

        }

        public static void PutAdmin(Guid userId, UserAdminUpdateRequest model)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserProfile_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@profileName", model.profileName);
                   paramCollection.AddWithValue("@profileLastName", model.profileLastName);
                   paramCollection.AddWithValue("@profilePhone", model.profilePhone);
                   paramCollection.AddWithValue("@profileMobile", model.profileMobile);
                   paramCollection.AddWithValue("@profileWebsite", model.profileWebsite);
                   paramCollection.AddWithValue("@profileAddress", model.profileAddress);
                   paramCollection.AddWithValue("@profileCompany", model.profileCompany);
                   paramCollection.AddWithValue("@userId", userId);
                   paramCollection.AddWithValue("@Tagline", model.Tagline);



                   SqlParameter p = new SqlParameter("@TagsId", SqlDbType.Structured);
                   if (model.UserTags != null && model.UserTags.Any())
                   {
                       p.Value = new IntIdTable(model.UserTags);
                   }
                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );



        }

        //functions for the Admin page
        public static List<User> GetJoint()
        {
            List<User> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserProfile_Join"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set) //function from BaseService
               {
                   User u = new User();
                   int startingIndex = 0;

                   u.ProfileName = reader.GetSafeString(startingIndex++);
                   u.ProfileEmail = reader.GetString(startingIndex++);
                   u.ProfilePhone = reader.GetSafeString(startingIndex++);
                   u.ProfileMobile = reader.GetSafeString(startingIndex++);
                   u.ProfileWebsite = reader.GetSafeString(startingIndex++);
                   u.ProfileAddress = reader.GetSafeString(startingIndex++);
                   u.ProfileCompany = reader.GetSafeString(startingIndex++);
                   u.UserId = reader.GetSafeString(startingIndex++);
                   u.Tagline = reader.GetSafeString(startingIndex++);
                   u.Id = reader.GetSafeString(startingIndex++);
                   u.Email = reader.GetString(startingIndex++);
                   u.EmailConfirmed = reader.GetBoolean(startingIndex++);
                   u.PasswordHash = reader.GetString(startingIndex++);
                   u.SecurityStamp = reader.GetString(startingIndex++);
                   u.PhoneNumber = reader.GetSafeString(startingIndex++);
                   u.PhoneNumberConfirmed = reader.GetBoolean(startingIndex++);
                   u.TwoFactorEnabled = reader.GetBoolean(startingIndex++);
                   u.LockoutEndDateUtc = reader.GetSafeDateTime(startingIndex++);
                   u.LockoutEnabled = reader.GetBoolean(startingIndex++);
                   u.AccessFailedCount = reader.GetSafeInt32(startingIndex++);
                   u.UserName = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<User>();
                   }

                   list.Add(u);
               }
               );


            return list;
        }

        public static void PutAdmin(UserAdminUpdateRequest model)
        {
            //send to the aspnet table
            //AspNetUserUpdateRequest a = new AspNetUserUpdateRequest();
            //a.UserId = model.UserId;     
            //a.PhoneNumber = model.PhoneNumber;
            //a.PasswordHash = model.PasswordHash;
            //a.TwoFactorEnabled = model.TwoFactorEnabled;
            //a.LockoutEndDateUtc = model.LockoutEndDateUtc;
            //a.LockoutEnabled = model.LockoutEnabled;
            //a.AccessFailedCount = model.AccessFailedCount;
            //a.UserName = model.UserName;

            //PutToAspNetTable(a);


            //send to the myprofile table
            UserAdminUpdateRequest p = new UserAdminUpdateRequest();
            p.profileName = model.profileName;
            p.profileEmail = model.profileEmail;
            p.profilePhone = model.profilePhone;
            p.profileMobile = model.profileMobile;
            p.profileWebsite = model.profileWebsite;
            p.profileAddress = model.profileAddress;
            p.profileCompany = model.profileCompany;
            p.UserId = model.UserId;
            p.Tagline = model.Tagline;

            PutToMyProfileTable(p);
        }

        public static void PutToMyProfileTable(UserAdminUpdateRequest model)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserProfile_UpdateByUserId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@profileName", model.profileName);
                   paramCollection.AddWithValue("@profileEmail", model.profileEmail);
                   paramCollection.AddWithValue("@profilePhone", model.profilePhone);
                   paramCollection.AddWithValue("@profileMobile", model.profileMobile);
                   paramCollection.AddWithValue("@profileWebsite", model.profileWebsite);
                   paramCollection.AddWithValue("@profileAddress", model.profileAddress);
                   paramCollection.AddWithValue("@profileCompany", model.profileCompany);
                   paramCollection.AddWithValue("@Tagline", model.Tagline);
                   paramCollection.AddWithValue("@UserId", model.UserId);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }

        //public static void PutToAspNetTable(AspNetUserUpdateRequest model)
        //{

        //    DataProvider.ExecuteNonQuery(GetConnection, "dbo.AspNetUsers_Update"
        ////       , inputParamMapper: delegate (SqlParameterCollection paramCollection)
        ////       {
        ////           paramCollection.AddWithValue("@UserId", model.UserId);  
        //paramCollection.AddWithValue("@PasswordHash", model.PasswordHash);
        //           paramCollection.AddWithValue("@PhoneNumber", model.PhoneNumber);
        //           paramCollection.AddWithValue("@TwoFactorEnabled", model.TwoFactorEnabled);
        //           paramCollection.AddWithValue("@LockoutEnabled", model.LockoutEnabled);
        //           paramCollection.AddWithValue("@LockoutEndDateUtc", model.LockoutEndDateUtc);
        //           paramCollection.AddWithValue("@AccessFailedCount", model.AccessFailedCount);                               
        //           paramCollection.AddWithValue("@UserName", model.UserName);

        //       }, returnParameters: delegate (SqlParameterCollection param)
        //       {

        //       }
        //       );

        //}

        public static List<User> SearchUsers(string SearchString, int PageNumber = 1, int PageSize = 5)
        {
            List<User> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserProfile_JointSearch"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@PageNumber", PageNumber);
                   paramCollection.AddWithValue("@PageSize", PageSize);
                   paramCollection.AddWithValue("@Search", SearchString);
               }
               , map: delegate (IDataReader reader, short set) //function from BaseService
               {
                   User u = new User();
                   int startingIndex = 0;

                   u.ProfileName = reader.GetSafeString(startingIndex++);
                   u.ProfileEmail = reader.GetString(startingIndex++);
                   u.ProfilePhone = reader.GetSafeString(startingIndex++);
                   u.ProfileMobile = reader.GetSafeString(startingIndex++);
                   u.ProfileWebsite = reader.GetSafeString(startingIndex++);
                   u.ProfileAddress = reader.GetSafeString(startingIndex++);
                   u.ProfileCompany = reader.GetSafeString(startingIndex++);
                   u.UserId = reader.GetSafeString(startingIndex++);
                   u.Tagline = reader.GetSafeString(startingIndex++);
                   u.Id = reader.GetSafeString(startingIndex++);
                   u.Email = reader.GetString(startingIndex++);
                   u.EmailConfirmed = reader.GetBoolean(startingIndex++);
                   u.PasswordHash = reader.GetString(startingIndex++);
                   u.SecurityStamp = reader.GetString(startingIndex++);
                   u.PhoneNumber = reader.GetSafeString(startingIndex++);
                   u.PhoneNumberConfirmed = reader.GetBoolean(startingIndex++);
                   u.TwoFactorEnabled = reader.GetBoolean(startingIndex++);
                   u.LockoutEndDateUtc = reader.GetSafeDateTime(startingIndex++);
                   u.LockoutEnabled = reader.GetBoolean(startingIndex++);
                   u.AccessFailedCount = reader.GetSafeInt32(startingIndex++);
                   u.UserName = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<User>();
                   }

                   list.Add(u);
               }
               );

            return list;
        }

        //for user tags
        public static User GetById(Guid userId)
        {

            //TO DO --- GET RID OF THIS FUNCTION AND USE BELOW
            return GetFullUserById(userId.ToString());

        }

        public static List<User> DecorateUsers(List<User> users)
        {
            if (users == null)
                return users;

            List<string> usrId = new List<string>();

            foreach (User usr in users)
            {
                usrId.Add(usr.Id);
            }

            List<UserTag> tagList = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserProfile_TagDecorate"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    SqlParameter s = new SqlParameter("@UserIds", SqlDbType.Structured);
                    if (usrId != null && usrId.Any())
                    {
                        s.Value = new NVarcharTable(usrId);
                    }
                    paramCollection.Add(s);
                }, map: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    if (set == 0)
                    {

                        UserTag t = new UserTag();

                        t.Id = reader.GetSafeInt32(startingIndex++);
                        t.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                        t.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                        t.TagName = reader.GetSafeString(startingIndex++);
                        t.TagSlug = reader.GetSafeString(startingIndex++);
                        t.ParentTagId = reader.GetSafeInt32(startingIndex++);
                        t.UserId = reader.GetSafeString(startingIndex++);

                        if (tagList == null)
                        {
                            tagList = new List<UserTag>();
                        }
                        tagList.Add(t);
                    }
                }
                );

            foreach (User usr in users)
            {
                if (tagList != null)
                {
                    List<Tags> usrTags = new List<Tags>();
                    foreach (UserTag tag in tagList)
                    {
                        if (tag.UserId == usr.UserId)
                        {
                            usrTags.Add(tag);
                        }
                    }
                    usr.Tags = usrTags;
                }
            }

            return users;
        }



        public static void PutProfileViews(string Id)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserProfiles_UpdateViewCount"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@Id", Id);


               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }

        public static List<User> GetByTagSlug(string slug)
        {
            List<User> list = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.UsersSearchByTagSlug"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@TagSlug", slug);
               }, map: delegate (IDataReader reader, short set)
               {
                   User p = new User();
                   int startingIndex = 0; //startingOrdinal

                   p.Tags = new List<Tags>();
                   Tags myTags = new Tags();
                   myTags.Id = reader.GetSafeInt32(startingIndex++);
                   myTags.CreatedDate = reader.GetDateTime(startingIndex++);
                   myTags.ModifiedDate = reader.GetDateTime(startingIndex++);
                   myTags.TagName = reader.GetSafeString(startingIndex++);
                   myTags.TagSlug = reader.GetSafeString(startingIndex++);
                   myTags.ParentTagId = reader.GetSafeInt32(startingIndex++);
                   p.Tags.Add(myTags);


                   p.ProfileName = reader.GetSafeString(startingIndex++);
                   p.ProfileEmail = reader.GetSafeString(startingIndex++);
                   p.ProfilePhone = reader.GetSafeString(startingIndex++);
                   p.ProfileMobile = reader.GetSafeString(startingIndex++);
                   p.ProfileWebsite = reader.GetSafeString(startingIndex++);
                   p.ProfileAddress = reader.GetSafeString(startingIndex++);
                   p.ProfileCompany = reader.GetSafeString(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.MediaId = reader.GetSafeInt32(startingIndex++);
                   p.ThemeMediaId = reader.GetSafeInt32(startingIndex++);

                   Medias m = new Medias();
                   m.MediasTableId = reader.GetSafeInt32(startingIndex++);
                   m.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                   m.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                   m.MediaType = reader.GetSafeEnum<MediaType>(startingIndex++);
                   m.ContentType = reader.GetSafeString(startingIndex++);
                   m.UserId = reader.GetSafeString(startingIndex++);
                   m.FilePath = reader.GetSafeString(startingIndex++);
                   if (m.MediasTableId > 0)
                       p.Avatar = m;

                   Medias t = new Medias();
                   t.MediasTableId = reader.GetSafeInt32(startingIndex++);
                   t.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                   t.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                   t.MediaType = reader.GetSafeEnum<MediaType>(startingIndex++);
                   t.ContentType = reader.GetSafeString(startingIndex++);
                   t.UserId = reader.GetSafeString(startingIndex++);
                   t.FilePath = reader.GetSafeString(startingIndex++);
                   if (t.MediasTableId > 0)
                       p.Theme = t;




                   if (list == null)
                   {
                       list = new List<User>();
                   }

                   list.Add(p);
               }
               );


            return DecorateUsers(list);

        }

        public List<string> GetMyConnections(string UserId)
        {
            List<string> list = null;                                          //if user has no group, return null.

            DataProvider.ExecuteCmd(GetConnection, "dbo.GetUserConnections"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", UserId);
               }, map: delegate (IDataReader reader, short set)
               {
                   int startingIndex = 0;

                   if (list == null)                                        //If, user is not connected but wants to connect.
                   {
                       list = new List<string>();                              //Create a new list for this user to store his/her connections.
                   }
                   list.Add(reader.GetSafeString(startingIndex++));
               }
               );
            return list;                                                    //return list as null (user not connected) or user's list of connectons.
        }


        public static List<User> Get(UsersGetRequest model) //Gets all users with lat long and  search
        {
            string UserId = UserService.GetCurrentUserId();

            List<User> list = null;
            UserProfileServices userProfileServices = new UserProfileServices();
            List<String> Connection = userProfileServices.GetMyConnections(UserId);

            DataProvider.ExecuteCmd(GetConnection, "dbo.User_SearchEpic"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@PageSize", model.PageSize);
                   paramCollection.AddWithValue("@PageNumber", model.PageNumber);
                   paramCollection.AddWithValue("@lat", model.Lat);
                   paramCollection.AddWithValue("@lon", model.Lon);
                   paramCollection.AddWithValue("@distance", model.Distance);
                   //paramCollection.AddWithValue("@TagId", model.TagId); taken out, tags decorated later
                   paramCollection.AddWithValue("@SearchString", model.SearchString);

               }, map: delegate (IDataReader reader, short set) //executes after stored proc
               {
                   User p = new User();
                   int startingIndex = 0; //startingOrdinal


                   p.ProfileName = reader.GetSafeString(startingIndex++);
                   p.ProfileEmail = reader.GetSafeString(startingIndex++);
                   p.ProfilePhone = reader.GetSafeString(startingIndex++);
                   p.ProfileMobile = reader.GetSafeString(startingIndex++);
                   p.ProfileWebsite = reader.GetSafeString(startingIndex++);
                   p.ProfileAddress = reader.GetSafeString(startingIndex++);
                   p.ProfileCompany = reader.GetSafeString(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.MediaId = reader.GetSafeInt32(startingIndex++);
                   p.ThemeMediaId = reader.GetSafeInt32(startingIndex++);
                   p.ProfileLastName = reader.GetSafeString(startingIndex++);
                   p.ProfileViewCount = reader.GetSafeInt32(startingIndex++);
                   p.ConnectionsCount = reader.GetSafeInt32(startingIndex++);
                   p.GroupsCount = reader.GetSafeInt32(startingIndex++);
                   p.Tagline = reader.GetSafeString(startingIndex++);
                   p.CardTemplate = reader.GetSafeString(startingIndex++);

                   Medias m = new Medias();
                   m.MediasTableId = reader.GetSafeInt32(startingIndex++);
                   m.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                   m.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
                   m.MediaType = reader.GetSafeEnum<MediaType>(startingIndex++);
                   m.ContentType = reader.GetSafeString(startingIndex++);
                   m.UserId = reader.GetSafeString(startingIndex++);
                   m.FilePath = reader.GetSafeString(startingIndex++);
                   if (m.MediasTableId > 0)
                       p.Avatar = m;

                   p.IsConnected = (Connection != null && Connection.Contains(p.UserId));

                   Location t = new Location();
                   t.Id = reader.GetSafeInt32(startingIndex++);
                   t.Latitude = reader.GetSafeDecimal(startingIndex++);
                   t.Longitude = reader.GetSafeDecimal(startingIndex++);
                   t.Address = reader.GetSafeString(startingIndex++);
                   t.City = reader.GetSafeString(startingIndex++);
                   t.State = reader.GetSafeString(startingIndex++);
                   t.ZipCode = reader.GetSafeString(startingIndex++);
                   if (t.Id > 0)
                       p.Location = t;



                   if (list == null)
                   {
                       list = new List<User>();
                   }

                   list.Add(p);
               }
               );


            return list;

        }



        //for adding external user accounts 
        public static void PostExternalAccounts(UserExternalAccounts model, string userId)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserExternalLinks_Upsert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", userId);
                   paramCollection.AddWithValue("@Source", model.source);
                   paramCollection.AddWithValue("@Url", model.url);



               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   
               }
               );
        }


        public static void UpdateCard(User model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserProfiles_UpdateCardTemplate"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@UserId", model.UserId);
                   paramCollection.AddWithValue("@Card", model.CardTemplate);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );
        }

    }
}