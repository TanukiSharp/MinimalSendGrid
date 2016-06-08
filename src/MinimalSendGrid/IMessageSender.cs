using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalSendGrid
{
    /// <summary>
    /// Represents the result of a message sending.
    /// </summary>
    public struct MessageSenderResult
    {
        /// <summary>
        /// Gets a preset success MessageSenderResult instance with a status code of zero.
        /// </summary>
        public static readonly MessageSenderResult SuccessMessageSenderResult = new MessageSenderResult(true, 0, null);

        /// <summary>
        /// Gets whether sending the message succeeded or not.
        /// </summary>
        public readonly bool IsSuccess;

        /// <summary>
        /// Gets the sending status code, if any.
        /// </summary>
        /// <remarks>This is not necessarily bound to the notion or error.</remarks>
        public readonly int StatusCode;

        /// <summary>
        /// Gets the sending error message, if any.
        /// </summary>
        public readonly string ErrorMessage;

        /// <summary>
        /// Initializes a MessageSenderResult instance.
        /// </summary>
        /// <param name="isSuccess">Flag telling whether sending the message succeeded or not.</param>
        /// <param name="statusCode">The sending status code.</param>
        /// <param name="errorMessage">The sending erro message.</param>
        public MessageSenderResult(bool isSuccess, int statusCode, string errorMessage)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Returns a string representation of the MessageSenderResult instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"IsSuccess: {IsSuccess}, StatusCode: {StatusCode}");

            if (string.IsNullOrWhiteSpace(ErrorMessage) == false)
                sb.Append($", ErrorMessage: {ErrorMessage}");

            return sb.ToString();
        }
    }

    /// <summary>
    /// Represents a message sending facade.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <returns>Returns a MessageSenderResult instance containing information about the sending process.</returns>
        Task<MessageSenderResult> Send(Message message);
    }
}
