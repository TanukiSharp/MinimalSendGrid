using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalSendGrid
{
    /// <summary>
    /// Represent a mutable Message.
    /// </summary>
    public class MessageBuilder
    {
        private MessageEndPoint from;
        private List<MessageEndPoint> to;
        private List<MessageEndPoint> cc;
        private List<MessageEndPoint> bcc;
        private string subject;
        private List<MessageBody> bodies;

        /// <summary>
        /// Sets the sender information of the message.
        /// </summary>
        /// <param name="endPoint">Sender information of the message.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder SetFrom(MessageEndPoint endPoint)
        {
            from = endPoint;
            return this;
        }

        /// <summary>
        /// Adds main recipient(s) information to the message.
        /// </summary>
        /// <param name="endPoints">Message main recipient(s) information.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddTo(params MessageEndPoint[] endPoints)
        {
            return AddTo((IEnumerable<MessageEndPoint>)endPoints);
        }

        /// <summary>
        /// Adds main recipient(s) information to the message.
        /// </summary>
        /// <param name="endPoints">Message main recipient(s) information.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddTo(IEnumerable<MessageEndPoint> endPoints)
        {
            if (to == null)
                to = new List<MessageEndPoint>();

            to.AddRange(endPoints);
            return this;
        }

        /// <summary>
        /// Adds carbon copy recipient(s) information to the message.
        /// </summary>
        /// <param name="endPoints">Message carbon copy recipient(s) information.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddCc(params MessageEndPoint[] endPoints)
        {
            return AddCc((IEnumerable<MessageEndPoint>)endPoints);
        }

        /// <summary>
        /// Adds carbon copy recipient(s) information to the message.
        /// </summary>
        /// <param name="endPoints">Message carbon copy recipient(s) information.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddCc(IEnumerable<MessageEndPoint> endPoints)
        {
            if (cc == null)
                cc = new List<MessageEndPoint>();

            cc.AddRange(endPoints);
            return this;
        }

        /// <summary>
        /// Adds blind carbon copy recipient(s) information to the message.
        /// </summary>
        /// <param name="endPoints">Message blind carbon copy recipient(s) information.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddBcc(params MessageEndPoint[] endPoints)
        {
            return AddBcc((IEnumerable<MessageEndPoint>)endPoints);
        }

        /// <summary>
        /// Adds blind carbon copy recipient(s) information to the message.
        /// </summary>
        /// <param name="endPoints">Message blind carbon copy recipient(s) information.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddBcc(IEnumerable<MessageEndPoint> endPoints)
        {
            if (bcc == null)
                bcc = new List<MessageEndPoint>();

            bcc.AddRange(endPoints);
            return this;
        }

        /// <summary>
        /// Sets the subject of the message.
        /// </summary>
        /// <param name="subject">Subject of the message.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder SetSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        /// <summary>
        /// Adds a body to the message.
        /// </summary>
        /// <param name="body">Body of the message.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddBody(params MessageBody[] bodies)
        {
            return AddBody((IEnumerable<MessageBody>)bodies);
        }

        /// <summary>
        /// Adds a body to the message.
        /// </summary>
        /// <param name="body">Body of the message.</param>
        /// <returns>Returns the current MessageBuilder for composition.</returns>
        public MessageBuilder AddBody(IEnumerable<MessageBody> bodies)
        {
            if (this.bodies == null)
                this.bodies = new List<MessageBody>();

            this.bodies.AddRange(bodies);
            return this;
        }

        /// <summary>
        /// Constructs an immutable message.
        /// </summary>
        /// <returns>Returns an immutable message.</returns>
        public Message Build()
        {
            return new Message(
                from,
                to != null ? to.ToArray() : null,
                cc != null ? cc.ToArray() : null,
                bcc != null ? bcc.ToArray() : null,
                subject,
                bodies != null ? bodies.ToArray() : null
            );
        }
    }
}
