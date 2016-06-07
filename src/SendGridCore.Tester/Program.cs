using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGridCore;

namespace SendGridCore.Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program().Run().Wait();
        }

        private async Task Run()
        {
            var sender = new HttpV3MessageSender("<YOUR_API_KEY_HERE>");

            // construct and send a message
            Message message = new MessageBuilder()
                .SetFrom("you@domain.ext")
                .AddTo("alice@another.domain.ext")
                .AddCc(new MessageEndPoint("bob", "bob@domain.ext"), "charly@domain.ext")
                .SetSubject("test subject")
                .SetBody("test body")
                .Build();
            MessageSenderResult result = await sender.Send(message);
            Console.WriteLine(result);

            // same for another message
            message = new MessageBuilder()
                .SetFrom("you@domain.ext")
                .AddTo("you@domain.ext")
                .SetSubject("congrats!")
                .SetBody("congrats for sending email!")
                .Build();

            result = await sender.Send(message);
            Console.WriteLine(result);
        }
    }
}
