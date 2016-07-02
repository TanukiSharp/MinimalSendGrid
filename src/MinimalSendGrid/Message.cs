using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalSendGrid
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
        /// Gets the message bodies.
        /// </summary>
        public MessageBody[] Bodies { get; }

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
        /// <param name="bodies">The message bodies.</param>
        public Message(MessageEndPoint from, MessageEndPoint[] to, MessageEndPoint[] cc, MessageEndPoint[] bcc, string subject, MessageBody[] bodies)
        {
            if (from.IsValid == false)
                throw new ArgumentException("Invalid 'from' information.");

            if (to == null || to.Length == 0)
                throw new ArgumentException("Missing 'to' information.");
            else if (to.Any(x => x.IsValid == false))
                throw new ArgumentException("In 'to' argument, at least one instance is invalid.");

            if (bodies != null && bodies.Any(x => x.IsValid == false))
                throw new ArgumentException("In 'bodies' argument, at least one instance is invalid.");

            if (cc != null && cc.Length == 0)
                cc = null;

            if (bcc != null && bcc.Length == 0)
                bcc = null;

            if (string.IsNullOrWhiteSpace(subject))
                subject = null;

            if (bodies != null && bodies.Length == 0)
                bodies = null;

            From = from;
            To = to;
            Cc = cc;
            Bcc = bcc;
            Subject = subject;
            Bodies = bodies;
        }
    }
}
