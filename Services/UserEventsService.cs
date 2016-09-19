using Sabio.Web.Models.Requests;
using Sabio.Web.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sabio.Data;
using Sabio.Web.Enums;
using Sabio.Web.Services.Interfaces;

namespace Sabio.Web.Services
{
    public class UserEventsService: BaseService, IUserEventsService
    {
        public void Post(UserEventsRequest model)
        {
         
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserEvents_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   //([RsvpStatus]
                   //,[EventId]
                   //,[UserId])
                   paramCollection.AddWithValue("@RsvpStatus", model.RsvpStatus);
                   paramCollection.AddWithValue("@EventId", model.EventId);
                   paramCollection.AddWithValue("@UserId", model.UserId);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
   
               }
               );

            //Create SystemEvents post for RSVP to event for recent activity feed
            //getting all event info - passing only event creator userid to SystemEvents
            Events e = EventsService.GetById(model.EventId);

            SystemEventsRequest newModel = new SystemEventsRequest();

            newModel.ActorUserId = model.UserId;
            newModel.TargetId = model.EventId;
            newModel.EventType = SystemEventTypes.NewRSVP;
            newModel.TargetUserId = e.UserId;


            SystemEventsService.Post(newModel);


        }

        public List<UserEvents> GetByUser(string UserId)
        {
            List<UserEvents> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserEvents_Select"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", UserId);
               }, map: delegate (IDataReader reader, short set) //executes after stored proc
               {
                   UserEvents p = new UserEvents();
                   int startingIndex = 0; //startingOrdinal


                   // e.Id - int
                   //,e.UserId - string
                   //,e.EventType - int
                   //,e.isPublic - bool
                   //,e.Title - string
                   //,e.Description - string
                   //,e.Start - DateTime
                   //,e.[End] - DateTime
                   //,a.RsvpStatus - int

                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.EventType = reader.GetSafeInt32(startingIndex++);
                   p.isPublic = reader.GetSafeBool(startingIndex++);
                   p.Title = reader.GetSafeString(startingIndex++);
                   p.Description = reader.GetSafeString(startingIndex++);
                   p.Start = reader.GetSafeDateTime(startingIndex++);
                   p.End = reader.GetSafeDateTime(startingIndex++);
                   p.RsvpStatus = reader.GetSafeInt32(startingIndex++);

                   Location l = new Location();

                   l.Id = reader.GetSafeInt32(startingIndex++);
                   l.Latitude = reader.GetSafeDecimalNullable(startingIndex++);
                   l.Longitude = reader.GetSafeDecimalNullable(startingIndex++);
                   l.Address = reader.GetSafeString(startingIndex++);
                   l.City = reader.GetSafeString(startingIndex++);
                   l.State = reader.GetSafeString(startingIndex++);
                   l.ZipCode = reader.GetSafeString(startingIndex++);

                   p.location = l;

                   if (list == null)
                   {
                       list = new List<UserEvents>();
                   }
                   list.Add(p);
               }
               );
            return list;
        }

    }
}