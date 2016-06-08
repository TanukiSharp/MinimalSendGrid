using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalSendGrid
{
    /// <summary>
    /// Represents an immutable message end point information.
    /// </summary>
    public struct MessageEndPoint
    {
        /// <summary>
        /// Gets the name of the message end point.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets the address of the message end point.
        /// </summary>
        public readonly string Address;

        /// <summary>
        /// Gets whether the current MessageEndPoint instance is valid or not.
        /// </summary>
        /// <remarks>Valid if at least the address contains at least one non whitespace character.</remarks>
        public bool IsValid
        {
            get
            {
                return string.IsNullOrWhiteSpace(Address) == false;
            }
        }

        /// <summary>
        /// Initializes the MessageEndPoint instance.
        /// </summary>
        /// <param name="address">The address of the message end point.</param>
        public MessageEndPoint(string address) : this(null, address)
        {
        }

        /// <summary>
        /// Initializes the MessageEndPoint instance.
        /// </summary>
        /// <param name="name">The name of the message end point.</param>
        /// <param name="address">The address of the message end point.</param>
        public MessageEndPoint(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentNullException(nameof(address), $"Invalid '{nameof(address)}' argument");

            if (string.IsNullOrWhiteSpace(name))
                name = null;

            Name = name;
            Address = address;
        }

        /// <summary>
        /// Returns a string representation of the current MessageEndPoint instance.
        /// </summary>
        /// <returns>Returns a string representation of the current MessageEndPoint instance.</returns>
        public override string ToString()
        {
            if (Name == null)
                return Address;

            return string.Format("{0} <{1}>", Name, Address);
        }

        /// <summary>
        /// Implicit cast operator to seemlessly create a MessageEndPoint from a string. (used as address member)
        /// </summary>
        /// <param name="address">The address of the message end point.</param>
        public static implicit operator MessageEndPoint(string address)
        {
            return new MessageEndPoint(address);
        }

        /// <summary>
        /// Implicit cast operator to seemlessly transform a MessageEndPoint to a string. (becomes the value of the address memeber)
        /// </summary>
        /// <param name="endPoint">The message end point to transform to a string.</param>
        public static implicit operator string(MessageEndPoint endPoint)
        {
            return endPoint.Address;
        }
    }
}
