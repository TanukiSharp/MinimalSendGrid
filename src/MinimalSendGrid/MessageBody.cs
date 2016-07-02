using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalSendGrid
{
    /// <summary>
    /// Contains the different available content types for a message body.
    /// </summary>
    public static class MessageBodyContentType
    {
        /// <summary>
        /// Plain text content type.
        /// </summary>
        public static readonly string Text = "text/plain";

        /// <summary>
        /// HTML content type.
        /// </summary>
        public static readonly string Html = "text/html";
    }

    public struct MessageBody
    {
        /// <summary>
        /// Gets the type of content of the message body.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the content of the message body.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Checks whether the current MessageBody instance is valid or not.
        /// </summary>
        public bool IsValid
        {
            get { return Type != null; }
        }

        /// <summary>
        /// Creates a plain text message body.
        /// </summary>
        /// <param name="content">The plain text message body content.</param>
        public MessageBody(string content)
            : this(MessageBodyContentType.Text, content)
        {
        }

        /// <summary>
        /// Creates a plain text message body.
        /// </summary>
        /// <param name="type">The type of message body content.</param>
        /// <param name="content">The message body content.</param>
        public MessageBody(string type, string content)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException(nameof(type), $"Invalid '{nameof(type)}' argument.");

            Type = type;
            Content = content ?? string.Empty;
        }

        /// <summary>
        /// Implicit cast operator to seemlessly create a MessageBody from a string.
        /// (message body type is decided by the constructor with no 'type' argument)
        /// </summary>
        /// <param name="content">The content of the message body.</param>
        public static implicit operator MessageBody(string content)
        {
            return new MessageBody(content);
        }

        /// <summary>
        /// Implicit cast operator to seemlessly transform a MessageBody to a string.
        /// (becomes the content of the message body instance)
        /// </summary>
        /// <param name="messageBody">The message body to transform to a string.</param>
        public static implicit operator string(MessageBody messageBody)
        {
            return messageBody.Content;
        }
    }

    /// <summary>
    /// Comparer used to sort message bodies, in the order 'plain text', then 'html', then others.
    /// </summary>
    public class MessageBodyComparer : IComparer<MessageBody>
    {
        public static readonly Lazy<MessageBodyComparer> Default = new Lazy<MessageBodyComparer>();

        /// <summary>
        /// Returns a value indicating how to arrange message bodies when sorting.
        /// </summary>
        /// <param name="x">A message body to compare.</param>
        /// <param name="y">Another message body to compare.</param>
        /// <returns>Returns a value indicating how to arrange message bodies when sorting.</returns>
        public int Compare(MessageBody x, MessageBody y)
        {
            if (x.IsValid == false || y.IsValid == false)
                return 0;

            int xWeight = 1;
            int yWeight = 1;

            if (string.Equals(x.Type, MessageBodyContentType.Text, StringComparison.OrdinalIgnoreCase))
                xWeight = -1;
            else if (string.Equals(x.Type, MessageBodyContentType.Html, StringComparison.OrdinalIgnoreCase))
                xWeight = 0;

            if (string.Equals(y.Type, MessageBodyContentType.Text, StringComparison.OrdinalIgnoreCase))
                yWeight = -1;
            else if (string.Equals(y.Type, MessageBodyContentType.Html, StringComparison.OrdinalIgnoreCase))
                yWeight = 0;

            return xWeight.CompareTo(yWeight);
        }
    }
}
