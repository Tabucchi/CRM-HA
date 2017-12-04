using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MobileServices.Handlers
{
    public class APILoggerHandler : DelegatingHandler
    {
        private string sLogFormat;
        private string sErrorTime;

        public void CreateLogFiles()
        {
            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString()+" "+DateTime.Now.ToLongTimeString().ToString()+" ==> ";
            
            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear    = DateTime.Now.Year.ToString();
            string sMonth    = DateTime.Now.Month.ToString();
            string sDay    = DateTime.Now.Day.ToString();
            sErrorTime = sYear+sMonth+sDay;
        }
        public void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName + sErrorTime, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
        public int id { get; set; }
        public string RequestMethod { get; set; }
        public DateTime Timestamp { get; set; }
        public string uri { get; set; }
        public string IP { get; set; }
        
        public string UsageType { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //stuff happens here
            //if (request != null)
            //{
            //    StringBuilder requestLogStringBuilder = new StringBuilder();

            //    UsageType = request.GetType().Name;
            //    requestLogStringBuilder.Append("|UsageType|");
            //    requestLogStringBuilder.Append(UsageType);
            //    RequestMethod = request.Method.Method;
            //    requestLogStringBuilder.Append("|RequestMethod|");
            //    requestLogStringBuilder.Append(RequestMethod);
            //    uri = request.RequestUri.ToString();
            //    requestLogStringBuilder.Append("|RequestUri|");
            //    requestLogStringBuilder.Append(uri);
            //    IP = ((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            //    requestLogStringBuilder.Append("|IP|");
            //    requestLogStringBuilder.Append(IP);
            //    Timestamp = DateTime.Now;
            //    requestLogStringBuilder.Append("|Timestamp|");
            //    requestLogStringBuilder.Append(Timestamp);
            //    this.extractHeaders(request.Headers);
            //    requestLogStringBuilder.Append("|Headers|");
            //    foreach (var header in Headers)
            //    {
            //        requestLogStringBuilder.Append(header);
            //        requestLogStringBuilder.Append(";");
            //    }

            //    HttpContext.Current.Request.InputStream.Position = 0;
            //    using (StreamReader inputStream = new StreamReader(HttpContext.Current.Request.InputStream))
            //    {
            //        requestLogStringBuilder.Append("|RequestPayload|");
            //        requestLogStringBuilder.Append(inputStream.ReadToEnd());
            //    }
            //    HttpContext.Current.Request.InputStream.Position = 0;
            //    CreateLogFiles();
            //    ErrorLog(HttpContext.Current.Server.MapPath(""), requestLogStringBuilder.ToString());
            //}

            return base.SendAsync(request, cancellationToken);
        }
        protected void extractHeaders(HttpHeaders h)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var i in h)
            {
                if (i.Value != null)
                {
                    string header = string.Empty;
                    foreach (var j in i.Value)
                    {
                        header += j + " ";
                    }
                    dict.Add(i.Key, header);
                }
            }
            Headers = dict;
        }
    }
}
