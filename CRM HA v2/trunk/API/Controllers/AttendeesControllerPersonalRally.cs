using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileServices.Models;
using AC.Domain;
using AC.DomainServices;
using AC.DataAccess;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.IO;
using System.Web;
using System.Text;

namespace MobileServices.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AttendeesController : ApiController
    {
        //public HttpResponseMessage Options()
        //{
        //    var response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.OK;
        //    return response;
        //}

        // GET api/leaderboards
        public IEnumerable<SpeakerModel> GetSpeakers(int id)
        {
            return GetSpeakers(id, DateTime.MinValue.ToString("yyyy-MM-ddTHHmmss.fff", System.Globalization.CultureInfo.CurrentCulture));
        }

        public IEnumerable<SpeakerModel> GetSpeakers(int id, string stimeStamp)
        {
            try
            {
                RequestHttpModule request = new RequestHttpModule();
                request.context_BeginRequest(null, null);
                DateTime timeStamp = DateTime.ParseExact(stimeStamp, "yyyy-MM-ddTHHmmss.fff", System.Globalization.CultureInfo.CurrentCulture);
                Event acEvent = EventServices.FindById(id);

                List<SpeakerModel> speakerModelList = null;
                speakerModelList = GetAttendeeModelList(acEvent, timeStamp);

                request.context_EndRequest(null, null);
                return speakerModelList;
            }
            catch (Exception ex)
            {
                //List<SpeakerModel> exhibitorModelList = new List<SpeakerModel>();

                //return exhibitorModelList;
                throw ex;
            }
        }

        public static List<SpeakerModel> GetAttendeeModelList(Event acEvent, DateTime timeStamp)
        {
            //siempre traigo la última lista de attendees...
            IList<Speaker> speakersList = acEvent.SpeakersList.ToList();//(c => c.ModificationDate > timeStamp);
            List<SpeakerModel> speakerModelList = new List<SpeakerModel>();
            foreach (Speaker speaker in speakersList)
            {
                List<long> activityList = new List<long>();
                List<ActivitySpeakerModel> childActivitieSpeakerList = new List<ActivitySpeakerModel>();
                foreach (Activity activity in acEvent.ActivitiesList)
                {
                    IList<ActivitySpeaker> IsIn = activity.ActivitySpeakersList.Where(actSpk => actSpk.IdSpeaker == speaker.Id).ToList<ActivitySpeaker>();
                    if (IsIn.Count() > 0)
                    {
                        activityList.Add(activity.Id);
                        ActivitySpeaker ActivitySpeakerTemp = IsIn[0];
                        childActivitieSpeakerList.Add(new ActivitySpeakerModel(ActivitySpeakerTemp.IdSpeaker, ActivitySpeakerTemp.IdActivity, ActivitySpeakerTemp.SpeakerRole, ActivitySpeakerTemp.SpeakerOrder));
                    }
                }

                bool getAll = (DateTime.MinValue.ToString("yyyy-MM-ddTHHmmss.fff", System.Globalization.CultureInfo.CurrentCulture) == timeStamp.ToString("yyyy-MM-ddTHHmmss.fff", System.Globalization.CultureInfo.CurrentCulture));
                Int64? speakerPicture;
                if (getAll)
                    speakerPicture = (speaker.Picture == null) ? 0 : (Int64)speaker.Picture.Id;
                else
                    speakerPicture = (speaker.Picture == null) ? 0 : -speaker.Picture.Id;

                if (acEvent.Id == 10135 && speakerPicture == 0 && !getAll)
                {
                    speakerPicture = null;
                }

                speakerModelList.Add(
                    new SpeakerModel
                    {
                        id = speaker.Id,
                        deleted = (speaker.Deleted ? 1 : 0),
                        name = speaker.Name,
                        lastName = speaker.Name[0]+".",
                        title = speaker.Title,
                        about = speaker.About,
                        twitter = speaker.Twitter,
                        facebook = speaker.Facebook,
                        linkedin = speaker.Linkedin,
                        email = speaker.Email,
                        showInList = speaker.ShowInList,
                        website = speaker.WebSite,
                        pictureId = speakerPicture,
                        modificationDateTime = speaker.ModificationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss"),
                        sessionIds = activityList,
                        speakerActivity = childActivitieSpeakerList,
                        country = speaker.Country
                    });

            }
            return speakerModelList;
        }

        private string sLogFormat;
        private string sErrorTime;

        private void CreateLogFiles()
        {
            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
        }
        private void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName + sErrorTime, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();

        }
        // POST api/leaderboards
        public HttpResponseMessage Post(AttendeeList attendeeList)
        {
            try
            {
                RequestHttpModule request = new RequestHttpModule();
                request.context_BeginRequest(null, null);

                CreateLogFiles();

                foreach (var attendee in attendeeList.attendees)
                {
                    ErrorLog(HttpContext.Current.Server.MapPath("~"), serializeAttendeeModel(attendee));
                }

                foreach (var attendee in attendeeList.attendees)
                {
                    try
                    {
                        SavePersonalRallyAttendee(attendee);
                    }
                    catch (Exception ex)
                    {
                        string errormsg = ex.Message;
                        if (ex.InnerException != null)
                            errormsg = errormsg + ex.InnerException.Message;
                        CreateLogFiles();
                        ErrorLog(HttpContext.Current.Server.MapPath("~"), errormsg);
                    }
                }

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                if (ex.InnerException != null)
                    errormsg = errormsg + ex.InnerException.Message;
                CreateLogFiles();
                ErrorLog(HttpContext.Current.Server.MapPath("~"), errormsg);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        private string serializeAttendeeModel(AttendeeModel attendee)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("||");
            sb.Append("|FirstName|");
            sb.Append(attendee.FirstName);
            sb.Append("|LastName|");
            sb.Append(attendee.LastName);
            sb.Append("|Email|");
            sb.Append(attendee.Email);
            sb.Append("|Tablet|");
            sb.Append(attendee.Tablet);
            sb.Append("|PhoneNumberHome|");
            sb.Append(attendee.PhoneNumberHome);
            sb.Append("|UsesFacebook|");
            sb.Append(attendee.UsesFacebook);
            sb.Append("|UsesTwitter|");
            sb.Append(attendee.UsesTwitter);
            sb.Append("|UsesInstagram|");
            sb.Append(attendee.UsesInstagram);
            sb.Append("|DiaHoraDeRegistro|");
            sb.Append(attendee.DiaHoraDeRegistro);
            sb.Append("|DiaHoraDePremio|");
            sb.Append(attendee.DiaHoraDePremio);
            sb.Append("|Participo|");
            sb.Append(attendee.Participo);
            sb.Append("|PremioGanado|");
            sb.Append(attendee.PremioGanado);
            sb.Append("||");
            return sb.ToString();
        }

        // PUT api/leaderboards/5
        public HttpResponseMessage Put(AttendeeList attendeeList)
        {
            try
            {
                RequestHttpModule request = new RequestHttpModule();
                request.context_BeginRequest(null, null);

                foreach (var attendee in attendeeList.attendees)
                {
                    ErrorLog(HttpContext.Current.Server.MapPath("~"), serializeAttendeeModel(attendee));
                }

                //SaveLeaderBoard(attendeeList);

                //http://stackoverflow.com/questions/20928929/jquery-ajax-call-executes-error-on-200
                //HttpStatusCode.OK is defined as follows: Equivalent to HTTP status 200. OK indicates that the request succeeded and that the requested information is in the response. This is the most common status code to receive.
                //Because a 200 implies some content is sent with the response (even an empty JSON object literal would be fine), you can run into problems if jQuery's Ajax implementation assumes a non-zero length response but does not receive it, especially if it tries parsing JSON (and possibly XML) from it. This is why John S makes the suggestion of changing the dataType to text; doing so would allow you to take specific action when receving an empty response.
                //On the other hand, HttpStatusCode.NoContent is defined as (emphasis mine):
                //Equivalent to HTTP status 204. NoContent indicates that the request has been successfully processed and that the response is intentionally blank.
                //In your particular situation, it may make more sense to set the status code to HttpStatusCode.NoContent to ensure that jQuery Ajax understands that it does not need to attempt any parsing/processing of the response.
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        private static void SavePersonalRallyAttendee(AttendeeModel attendee)
        {
            PersonalRallyAttendee personalRallyAttendee = new PersonalRallyAttendee();

            if (string.IsNullOrEmpty(attendee.Tablet))
                attendee.Tablet = "Tableta no identificada";
            personalRallyAttendee.Tablet = attendee.Tablet;
            personalRallyAttendee.FirstName = attendee.FirstName;
            personalRallyAttendee.LastName = attendee.LastName;
            personalRallyAttendee.Email = attendee.Email;

            if (attendee.DiaHoraDePremio < (new DateTime(1753, 1, 1)))
                attendee.DiaHoraDePremio = DateTime.Now;
            personalRallyAttendee.DiaHoraDePremio = attendee.DiaHoraDePremio;

            if (attendee.DiaHoraDeRegistro < (new DateTime(1753, 1, 1)))
                attendee.DiaHoraDeRegistro = DateTime.Now;
            personalRallyAttendee.DiaHoraDeRegistro = attendee.DiaHoraDeRegistro;

            personalRallyAttendee.PhoneNumberHome = attendee.PhoneNumberHome;


            if (string.IsNullOrEmpty(attendee.PremioGanado))
                attendee.PremioGanado = "";
            personalRallyAttendee.PremioGanado = attendee.PremioGanado;

            personalRallyAttendee.UsesFacebook = (attendee.UsesFacebook == null? false : true);
            personalRallyAttendee.UsesInstagram = (attendee.UsesInstagram == null? false : true);
            personalRallyAttendee.UsesOtherSocialNetworks = (attendee.UsesOtherSocialNetworks == null? false : true);
            personalRallyAttendee.UsesTwitter = (attendee.UsesTwitter == null? false : true);
            personalRallyAttendee.Participo = (attendee.Participo == null ? false : true);

            bool encontrado = false;

            IList<PersonalRallyAttendee> praTempList = PersonalRallyAttendeesService.FindGamerByEmail(personalRallyAttendee.Email);
            if (praTempList != null)
            {
                //ya existe!, verifico si cambio algo?
                foreach (var praTemp in praTempList)
                {
                    if (string.IsNullOrEmpty(praTemp.PremioGanado))
                        praTemp.PremioGanado = "";//para que no falle el compare to

                    if (   praTemp.FirstName.CompareTo(personalRallyAttendee.FirstName) == 0
                        && praTemp.LastName.CompareTo(personalRallyAttendee.LastName) == 0
                        && praTemp.Tablet.CompareTo(personalRallyAttendee.Tablet) == 0
                        //&& praTemp.PremioGanado.CompareTo(personalRallyAttendee.PremioGanado) == 0
                        )
                    {
                        encontrado = true;
                    }
                }
            }
                
            if (!encontrado)
            { 
                PersonalRallyAttendeesService.SavePersonalRallyAttendee(personalRallyAttendee);
            }
        }

        // DELETE api/leaderboards/5
        public void Delete(int id)
        {
        }


        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    bool isCorsRequest = request.Headers.Contains(Origin);
        //    if (isCorsRequest)
        //    {
        //        return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
        //        {
        //            HttpResponseMessage resp = t.Result;
        //            resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
        //            return resp;
        //        });
        //    }
        //    else
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //}

        //protected void saraza()
        //{
        //    bool isPreflightRequest = request.Method == HttpMethod.Options;
        //    if (isCorsRequest)
        //    {
        //        if (isPreflightRequest)
        //        {
        //            return Task.Factory.StartNew<HttpResponseMessage>(() =>
        //            {
        //                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //                response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

        //                string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
        //                if (accessControlRequestMethod != null)
        //                {
        //                    response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
        //                }

        //                string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
        //                if (!string.IsNullOrEmpty(requestedHeaders))
        //                {
        //                    response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
        //                }

        //                return response;
        //            }, cancellationToken);
        //        }
        //    }
        //}
    }
}
