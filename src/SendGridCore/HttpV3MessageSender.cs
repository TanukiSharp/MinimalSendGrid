using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SendGridCore
{
    /// <summary>
    /// Represents an implementation of the SendGrid email sending HTTP service using API version 3.
    /// </summary>
    public class HttpV3MessageSender : IMessageSender
    {
        /// <summary>
        /// Gets the SendGrid HTTP email sending service base URI.
        /// </summary>
        public const string BaseUri = "https://api.sendgrid.com";

        /// <summary>
        /// Gets the SendGrid HTTP email sending service path.
        /// </summary>
        public const string DefaultSendEndPointPath = "/v3/mail/send/beta";

        private readonly string sendEndPointPath;
        private readonly HttpClient client;

        /// <summary>
        /// Initializes the HttpV3MessageSender instance.
        /// </summary>
        /// <param name="apiKey">The API key used to send emails.</param>
        public HttpV3MessageSender(string apiKey)
            : this(apiKey, DefaultSendEndPointPath)
        {
        }

        /// <summary>
        /// Initializes the HttpV3MessageSender instance.
        /// </summary>
        /// <param name="apiKey">The API key used to send emails.</param>
        /// <param name="sendEndPointPath">A custom HTTP email sending service path.</param>
        public HttpV3MessageSender(string apiKey, string sendEndPointPath)
        {
            if (string.IsNullOrWhiteSpace(sendEndPointPath))
                throw new ArgumentException(nameof(sendEndPointPath), $"Invalid '{nameof(sendEndPointPath)}' argument.");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException(nameof(apiKey), $"Invalid '{nameof(apiKey)}' argument.");

            this.sendEndPointPath = sendEndPointPath;

            client = new HttpClient();

            client.BaseAddress = new Uri(BaseUri, UriKind.Absolute);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        /// <summary>
        /// Sends a message as an email through a HTTP request.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>Returns a simple representation of the HTTP response.</returns>
        public async Task<MessageSenderResult> Send(Message message)
        {
            if (message.IsValid == false)
                throw new InvalidOperationException("Invalid message.");

            object jsonRoot = FormatMessage(message);
            string jsonContent = JsonGenerator.Stringify(jsonRoot, true);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(sendEndPointPath, content);

            var statusCode = (int)response.StatusCode;

            if (statusCode >= 200 && statusCode <= 299)
                return new MessageSenderResult(true, statusCode, null);

            string errorMessage = string.Format(
                "Message: '{0}', Content: '{1}'",
                response.ReasonPhrase,
                await response.Content.ReadAsStringAsync());

            return new MessageSenderResult(false, statusCode, errorMessage);
        }

        private object FormatMessage(Message message)
        {
            var root = new Dictionary<string, object>();

            root["from"] = FormatMessageEndPoint(message.From);

            var personalizations = new Dictionary<string, object>();

            personalizations["to"] = message.To.Select(FormatMessageEndPoint).ToArray();
            if (message.Cc != null)
                personalizations["cc"] = message.Cc.Select(FormatMessageEndPoint).ToArray();
            if (message.Bcc != null)
                personalizations["bcc"] = message.Bcc.Select(FormatMessageEndPoint).ToArray();

            root["personalizations"] = new object[] { personalizations };

            if (message.Subject != null)
                root["subject"] = message.Subject;

            if (message.Body != null)
            {
                root["content"] = new object[]
                {
                    new Dictionary<string, object>
                    {
                        ["type"] = "text/plain",
                        ["value"] = message.Body
                    }
                };
            }

            return root;
        }

        private object FormatMessageEndPoint(MessageEndPoint endPoint)
        {
            var obj = new Dictionary<string, object>();

            if (endPoint.Name != null)
                obj["name"] = endPoint.Name;

            obj["email"] = endPoint.Address;

            return obj;
        }
    }
}
