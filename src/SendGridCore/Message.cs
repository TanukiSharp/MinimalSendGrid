using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendGridCore
{
    /// <summary>
    /// Represents an immutable message.
    /// </summary>
    public struct Message
    {
        /// <summary>
        /// Gets the message sender information.
        /// </summary>
        public MessageEndPoint From { get; }

        /// <summary>
        /// Gets the message main recipients.
        /// </summary>
        public MessageEndPoint[] To { get; }

        /// <summary>
        /// Gets the message carbon copy recipients.
        /// </summary>
        public MessageEndPoint[] Cc { get; }

        /// <summary>
        /// Gets the message blind carbon copy recipients.
        /// </summary>
        public MessageEndPoint[] Bcc { get; }

        /// <summary>
        /// Gets the message subject.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Gets the message plain text body.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Checks whether the current Message instance is valid or not.
        /// </summary>
        /// <remarks>Valid if From member is valid and if there is at least one main recipient.</remarks>
        public bool IsValid
        {
            get
            {
                return From.IsValid && To != null && To.Length > 0;
            }
        }

        /// <summary>
        /// Initializes an instance of the Message structure.
        /// </summary>
        /// <param name="from">The message sender.</param>
        /// <param name="to">An array of message main recipients.</param>
        /// <param name="cc">An array of message carbon copy recipients.</param>
        /// <param name="bcc">An array of message blind carbon copy recipients.</param>
        /// <param name="subject">The message subject.</param>
        /// <param name="body">The message plain text body.</param>
        public Message(MessageEndPoint from, MessageEndPoint[] to, MessageEndPoint[] cc, MessageEndPoint[] bcc, string subject, string body)
        {
            if (from.IsValid == false)
                throw new InvalidOperationException("Invalid 'from' information.");
            if (to == null || to.Length == 0)
                throw new InvalidOperationException("Missing 'to' information.");

            if (cc != null && cc.Length == 0)
                cc = null;

            if (bcc != null && bcc.Length == 0)
                bcc = null;

            if (string.IsNullOrWhiteSpace(subject))
                subject = null;

            if (string.IsNullOrWhiteSpace(body))
                body = null;

            From = from;
            To = to;
            Cc = cc;
            Bcc = bcc;
            Subject = subject;
            Body = body;
        }
    }
}
